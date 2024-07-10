using DASHMASTER.CORE.Attributes;
using DASHMASTER.CORE.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DASHMASTER.CORE
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.Configure<ApplicationConfig>(options => configuration.Bind(nameof(ApplicationConfig), options));

            var type = typeof(DependencyInjection);
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddTransient<IGeneralHelper, GeneralHelper>();
            services.AddTransient<ITokenHelper, TokenHelper>();

            return services;
        }
    }
}
