using DASHMASTER.CORE.Helper;
using DASHMASTER.WEB.Helper;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
        }).AddRazorRuntimeCompilation();

        builder.Services.AddAuthentication(HelperClient.AUTHENTICATION_SCHEMA)
            .AddCookie(HelperClient.AUTHENTICATION_SCHEMA, opt =>
            {
                opt.Cookie.Name = HelperClient.AUTHENTICATION_SCHEMA;
            });

        var app = builder.Build();
        builder.Services.AddSingleton<ITokenHelper, TokenHelper>();
        builder.Services.AddSignalR();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
