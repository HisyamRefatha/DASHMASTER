using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASHMASTER.CORE.Attributes
{
    public class ApplicationConfig
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public int ExpiredToken { get; set; }
        public string AppUrl { get; set; }
        public string ApiUrl { get; set; }
    }

    public class EmailConfig
    {
        public string Smtp { get; set; }
        public int SmtpPort { get; set; }
        public string SenderMail { get; set; }
        public string Password { get; set; }
    }

    public class AttachmentMail
    {
        public Stream File { get; set; }
        public string Name { get; set; }
    }
}
