using DASHMASTER.API.Handler;
using DASHMASTER.CORE;
using DASHMASTER.DATA;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

namespace DASHMASTER.API
{
    public class Program
    {
        private const string _defaultCorsPolicyName = "localhost";
        private IConfiguration _configuration { get; }
        private IWebHostEnvironment _environment { get; set; }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            IWebHostEnvironment _environment = builder.Environment;
            string _environtmentName = "Development";
            if (builder.Environment.IsDevelopment())
                _environtmentName = "Development";
            else if (builder.Environment.IsStaging())
                _environtmentName = "Staging";
            else if (builder.Environment.IsProduction())
                _environtmentName = "Production";

            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                      .AddJsonFile("appsettings.json")
                                                      .AddJsonFile($"appsettings.{_environtmentName}.json", optional: true)
                                                      .AddEnvironmentVariables()
                                                      .Build();
            IConfiguration _configuration = configuration;

            #region Logger
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(builder.Configuration)
                //.Filter.ByExcluding((le) => le.Level == Serilog.Events.LogEventLevel.Information)
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    path: Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log", "Application_.log"),
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                    fileSizeLimitBytes: 524288000,
                    rollingInterval: RollingInterval.Day
                )
                .CreateLogger();

            builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(logger, true));
            #endregion
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.RegisterData(_configuration);
            builder.Services.RegisterCore(_configuration);

            builder.Services.AddHsts(options =>
            {
                options.IncludeSubDomains = true;
            });
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("ApplicationConfig")["SecretKey"]);
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).
            AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = _configuration.GetSection("ApplicationConfig")["Issuer"],
                    ValidAudience = _configuration.GetSection("ApplicationConfig")["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                x.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddAuthorization(options =>
            {

            });

            builder.Services.AddAntiforgery(options =>
            {
                options.SuppressXFrameOptionsHeader = true;
            });

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(AuthorizeFilter));
            });
            builder.Services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

            builder.Services.AddRazorPages().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;

            });
            builder.Services.AddHealthChecks();

            if (_environment.IsDevelopment())
            {
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DASHMASTER.API", Version = "v1" });
                    c.AddSecurityDefinition("Bearer",
                        new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.Http,
                            BearerFormat = "JWT",
                            Scheme = "Bearer",
                            In = ParameterLocation.Header,
                            Name = Microsoft.Net.Http.Headers.HeaderNames.Authorization,
                            Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')"
                        }
                    );
                    c.AddSecurityRequirement(
                        new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    },
                                },
                                Array.Empty<string>()
                            }
                        });
                    c.EnableAnnotations();
                    c.MapType<TimeSpan>(() => new OpenApiSchema
                    {
                        Type = "string",
                        Example = new OpenApiString("00:00:00")
                    });
                });
                builder.Services.AddSwaggerGen();
            }

            builder.Services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            _configuration.GetValue<string>("AllowedHosts").Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                )
            );

            //Signal R
            builder.Services.AddSignalR();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.DocumentTitle = "DASHMASTER.API";
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                });
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();
            app.UseHealthChecks("/health");
            app.MapHealthChecks("/database_health");
            app.UseCors(_defaultCorsPolicyName);
            app.MapControllers();

            app.UseHttpsRedirection();

            app.Run();
        }
    }
}
