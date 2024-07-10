using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using Newtonsoft.Json;
using System.Globalization;
using System.Reflection;
using System.Text;
using Vleko.Result;

namespace DASHMASTER.CORE.Helper
{
    public interface IGeneralHelper
    {
        string PasswordEncrypt(string text);
        string IncrementNumber(int totalRowCount);
        bool ValidateMail(string email);
        bool IsImage(string filename);
        string ConvertTitleFormat(string text);
        ObjectResponse<string> ResizeImage(byte[] img, int width, int height);
        ObjectResponse<string> ValidatePhoneNumber(string phone_number);
        Cell ConstructCell(string value, CellValues dataType, uint styleIndex = 0);
        Stylesheet GenerateStylesheet();
        Task<(bool IsSuccess, string ErrorMessage, T Result, Exception ex)> DoRequest<T>(
           HttpMethod httpMethod, string token,
           string url,
           object paramBody,
           bool isReturnJson = true,
           bool isContent = true,
           Dictionary<string, string> additionalHeaders = null
       ) where T : class;
    }

    public class GeneralHelper : IGeneralHelper
    {
        private readonly IMediator _mediator;

        public GeneralHelper(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region PasswordEncrypt
        public string PasswordEncrypt(string text)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(text);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes); // .NET 5 +
            }
        }
        #endregion

        #region Validate Email
        public bool ValidateMail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Validate Phone
        public ObjectResponse<string> ValidatePhoneNumber(string phone_number)
        {
            ObjectResponse<string> result = new ObjectResponse<string>();
            if (string.IsNullOrWhiteSpace(phone_number))
            {
                result.BadRequest($"Phone number cannot be empty!");
                return result;
            }
            phone_number = phone_number.Trim();
            phone_number = phone_number.Replace(".", "").Replace(",", "").Replace(" ", "");

            if (phone_number.Contains("+62"))
                phone_number = phone_number.Replace("+62", "0");

            bool valid = false;
            if (phone_number.Length >= 10 && phone_number.Length <= 13)
                valid = true;

            if (!double.TryParse(phone_number, out double n) || !valid)
            {
                result.BadRequest($"Phone number {phone_number} tidak valid!");
                return result;
            }
            result.OK();
            result.Data = phone_number;
            return result;
        }
        #endregion

        #region Image
        public bool IsImage(string filename)
        {
            List<string> ImageExtensions = new List<string> { ".JPG", ".JPEG", ".PNG" };
            return ImageExtensions.Contains(Path.GetExtension(filename).ToUpperInvariant());
        }
        public ObjectResponse<string> ResizeImage(byte[] img, int width, int height)
        {
            ObjectResponse<string> result = new ObjectResponse<string>();
            try
            {
                using (Image image = Image.Load(img))
                {
                    int new_width = image.Width;
                    int new_height = image.Height;
                    int size = width > height ? width : height;
                    int max = image.Height;
                    if (image.Width < max)
                        max = image.Width;
                    double pembagi = (double)max / (double)size;
                    if (pembagi >= 1)
                    {
                        new_width = (int)Math.Ceiling(image.Width / pembagi);
                        new_height = (int)Math.Ceiling(image.Height / pembagi);
                    }
                    else
                    {
                        pembagi = (double)size / (double)max;
                        new_width = (int)Math.Ceiling(image.Width * pembagi);
                        new_height = (int)Math.Ceiling(image.Height * pembagi);
                    }
                    int rect_x = (new_width / 2) - (width / 2);
                    int rect_y = (new_height / 2) - (height / 2);
                    image.Mutate(x => x
                    .Resize(new_width, new_height)
                         .Crop(new Rectangle(rect_x, rect_y, width, height)));

                    result.Data = image.ToBase64String(SixLabors.ImageSharp.Formats.Png.PngFormat.Instance);
                    result.OK();
                }
            }
            catch (Exception ex)
            {
                result.BadRequest(ex.Message);
            }
            return result;
        }
        #endregion

        #region Get Changed Properties
        public Dictionary<string, Tuple<object, object>> GetChangedProperties<T>(T a, T b, List<string> property_excluded) where T : class
        {
            List<string> list_property_excluded = new();
            if (property_excluded != null && property_excluded.Count > 0)
                list_property_excluded = property_excluded;

            if (a != null && b != null)
            {
                if (Object.Equals(a, b))
                {
                    return new Dictionary<string, Tuple<object, object>>();
                }
                var allProperties = a.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var result = new Dictionary<string, Tuple<object, object>>();
                foreach (var p in allProperties)
                {
                    if (!list_property_excluded.Contains(p.Name))
                    {
                        var v1 = p.GetValue(a);
                        var v2 = p.GetValue(b);
                        if (!Object.Equals(v1, v2))
                            result.Add(p.Name, new Tuple<object, object>(v1, v2));
                    }
                }
                return result;
            }
            else
            {
                var aText = $"{(a == null ? ("\"" + nameof(a) + "\"" + " was null") : "")}";
                var bText = $"{(b == null ? ("\"" + nameof(b) + "\"" + " was null") : "")}";
                var bothNull = !string.IsNullOrEmpty(aText) && !string.IsNullOrEmpty(bText);
                throw new ArgumentNullException(aText + (bothNull ? ", " : "") + bText);
            }
        }
        #endregion

        #region Export Excel Setting
        public Cell ConstructCell(string value, CellValues dataType, uint styleIndex = 0)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                StyleIndex = styleIndex
            };
        }
        public Stylesheet GenerateStylesheet()
        {
            Stylesheet stylesheet = new Stylesheet();
            Fonts fonts = new Fonts(
                new Font( // Index 0 - default
                    new FontSize() { Val = 12 }
                ),
                new Font( // Index 1 - header
                    new FontSize() { Val = 14 },
                    new Bold(),
                    new DocumentFormat.OpenXml.Office2010.Excel.Color() { Rgb = "FFFFFF" }
                ),
                new Font( // Index 2 - footer
                    new FontSize() { Val = 10 },
                    new Bold(),
                    new DocumentFormat.OpenXml.Office2010.Excel.Color() { Rgb = "000000" }
                ),
                new Font(
                    new FontSize() { Val = 14 },
                    new Bold(),
                    new DocumentFormat.OpenXml.Office2010.Excel.Color() { Rgb = "000000" }
                )
            );
            Fills fills = new Fills(
                 new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
                 new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }), // Index 1 - default
                 new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "66666666" } })
                 { PatternType = PatternValues.Solid }), // Index 2 - header
                 new Fill(new PatternFill(new ForegroundColor { Rgb = "FFFF00" })
                 { PatternType = PatternValues.Solid }),
                 new Fill(new PatternFill(new ForegroundColor { Rgb = "FAE521" })
                 { PatternType = PatternValues.Solid }),
                 new Fill(new PatternFill(new ForegroundColor { Rgb = "000000" }) // Index 5 = Black
                 { PatternType = PatternValues.Solid }),
                 new Fill(new PatternFill(new ForegroundColor { Rgb = "1A3562" }) // Index 6 = Blue Mantap
                 { PatternType = PatternValues.Solid }),
                 new Fill(new PatternFill(new ForegroundColor { Rgb = "F4B942" }) // Index 7 = Yellow Mantap
                 { PatternType = PatternValues.Solid }),
                 new Fill(new PatternFill(new ForegroundColor { Rgb = "FFE5B6" }) // Index 8 = Orange Mantap
                 { PatternType = PatternValues.Solid }),
                 new Fill(new PatternFill(new ForegroundColor { Rgb = "D8FFCE" }) // Index 9 = Green Mantap
                 { PatternType = PatternValues.Solid }),
                 new Fill(new PatternFill(new ForegroundColor { Rgb = "FF5A5A" }) // Index 10 = RED Mantap
                 { PatternType = PatternValues.Solid })
             );

            Borders borders = new Borders(
                new DocumentFormat.OpenXml.Spreadsheet.Border(), // index 0 default
                new DocumentFormat.OpenXml.Spreadsheet.Border( // index 1 black border
                    new LeftBorder(new DocumentFormat.OpenXml.Office2010.Excel.Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new RightBorder(new DocumentFormat.OpenXml.Office2010.Excel.Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new TopBorder(new DocumentFormat.OpenXml.Office2010.Excel.Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new BottomBorder(new DocumentFormat.OpenXml.Office2010.Excel.Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new DiagonalBorder())
            );

            CellFormats cellFormats = new CellFormats(
                new CellFormat(), // default
                new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }, // body
                new CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyFill = true }, // header
                new CellFormat { FontId = 2, FillId = 3, BorderId = 1, ApplyFill = true }, // header
                new CellFormat { FontId = 2, FillId = 4, BorderId = 0, ApplyFill = true },
                new CellFormat { FontId = 3, FillId = 4, BorderId = 0, ApplyFill = true },
                new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center })
                { FontId = 1, FillId = 5, BorderId = 1, ApplyFill = true, ApplyAlignment = true }, // Index 6 - Alignment + BACKGROUND BLACK
                new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center })
                { FontId = 1, FillId = 6, BorderId = 1, ApplyFill = true, ApplyAlignment = true }, // Index 7 - Alignment + BACKGROUND BLUE MANTAP
                new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center })
                { FontId = 3, FillId = 7, BorderId = 1, ApplyFill = true, ApplyAlignment = true },  // Index 8 - Alignment + BACKGROUND YELLOW MANTAP
                new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Center })
                { FontId = 0, FillId = 8, BorderId = 1, ApplyFill = true, ApplyAlignment = true },  // Index 9 - Alignment Right + BACKGROUND Orange MANTAP
                new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Center })
                { FontId = 0, FillId = 9, BorderId = 1, ApplyFill = true, ApplyAlignment = true },  // Index 10 - Alignment Right + BACKGROUND GREEN MANTAP
                new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Center })
                { FontId = 0, FillId = 10, BorderId = 1, ApplyFill = true, ApplyAlignment = true },  // Index 11 - Alignment Right + BACKGROUND Red MANTAP
                new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center })
                { FontId = 0, FillId = 9, BorderId = 1, ApplyFill = true, ApplyAlignment = true },  // Index 12 - Alignment Center + BACKGROUND GREEN MANTAP
                new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center })
                { FontId = 1, FillId = 10, BorderId = 1, ApplyFill = true, ApplyAlignment = true },  // Index 13 - Alignment Center + BACKGROUND Red MANTAP
                new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Center })
                { FontId = 3, FillId = 7, BorderId = 1, ApplyFill = true, ApplyAlignment = true },  // Index 14 - Alignment + BACKGROUND YELLOW MANTAP
                new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Center })
                { FontId = 0, FillId = 0, BorderId = 1, ApplyFill = true, ApplyAlignment = true }  // Index 15 - Alignment Right+ BACKGROUND POLOS MANTAP (UNTUK NUMERIC)
            );

            stylesheet = new Stylesheet(fonts, fills, borders, cellFormats);
            return stylesheet;
        }
        #endregion

        #region Do Request
        public async Task<(bool IsSuccess, string ErrorMessage, T Result, Exception ex)> DoRequest<T>(
           HttpMethod httpMethod, string token,
           string url,
           object paramBody,
           bool isReturnJson = true,
           bool isContent = true,
           Dictionary<string, string> additionalHeaders = null
       ) where T : class
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(30);
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    if (additionalHeaders != null)
                    {
                        foreach (var item in additionalHeaders)
                        {
                            client.DefaultRequestHeaders.Add(item.Key, item.Value);
                        }
                    }

                    if (!string.IsNullOrEmpty(token))
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    var request = new HttpRequestMessage(httpMethod, $"{url}");

                    if ((httpMethod == HttpMethod.Post || httpMethod == HttpMethod.Put) && paramBody != null)
                    {
                        string jsonString = JsonConvert.SerializeObject(paramBody);
                        request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    }

                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead);

                    var content = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(content) && isContent)
                    {
                        throw new Exception("Something Went Wrong!");
                    }

                    if (isReturnJson)
                    {
                        var result = JsonConvert.DeserializeObject<T>(content);
                        return (response.IsSuccessStatusCode, "", result, null);
                    }
                    else
                    {
                        return (response.IsSuccessStatusCode, "", (T)Convert.ChangeType(content, typeof(T)), null);
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null, ex);
            }
        }
        #endregion

        #region Convert String To Tilte Format (Huruf Awal Selalu Kapital)
        public string ConvertTitleFormat(string text)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string capitalizedText = textInfo.ToTitleCase(text.ToLower());
            return capitalizedText;
        }
        #endregion

        #region Helper Increment Number

        public string IncrementNumber(int totalRowCount)
        {
            string formattedNumber = (totalRowCount + 1).ToString("D8");
            return formattedNumber;
        }
        #endregion
    }
}
