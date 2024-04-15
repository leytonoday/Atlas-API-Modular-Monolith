using Atlas.Shared;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Atlas.Plans.Infrastructure.Options.OptionSetup;

namespace Atlas.Web.Extensions
{
    /// <summary>
    /// Extension methods for IServiceCollection to configure various services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures response compression using Gzip.
        /// </summary>
        /// <param name="services">The IServiceCollection to configure.</param>
        /// <returns>The updated IServiceCollection.</returns>
        public static IServiceCollection ConfigureResponseCompression(this IServiceCollection services)
        {
            // Configure Gzip compression provider options
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
            // Add response compression services
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json" });
                options.EnableForHttps = true;
            });
            return services;
        }

        /// <summary>
        /// Configures cookie-based authentication.
        /// </summary>
        /// <param name="services">The IServiceCollection to configure.</param>
        /// <returns>The updated IServiceCollection.</returns>
        public static IServiceCollection ConfigureCookieAuthentication(this IServiceCollection services)
        {
            // Add cookie authentication services
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = Utils.IsDevelopment() ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
                options.Cookie.Name = Constants.CookieName;

                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
            });

            // Configure cookie policy options
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });

            return services;
        }

        /// <summary>
        /// Configures API versioning behavior.
        /// </summary>
        /// <param name="services">The IServiceCollection to configure.</param>
        /// <returns>The updated IServiceCollection.</returns>
        public static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            // Add API versioning services
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader() // This method allows us to get the version number from the URL, e.g., https://domain.com/api/v1/method
                );
            });

            return services;
        }

        /// <summary>
        /// Configures Cross-Origin Resource Sharing (CORS) policy.
        /// </summary>
        /// <param name="services">The IServiceCollection to configure.</param>
        /// <returns>The updated IServiceCollection.</returns>
        public static IServiceCollection ConfigureCors(this IServiceCollection services)
        {
            return services.AddCors(options =>
            {
                options.AddPolicy(Constants.CorsPolicyName, builder => builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin()
                );
            });
        }

        /// <summary>
        /// Adds presentation-related services.
        /// </summary>
        /// <param name="services">The IServiceCollection to configure.</param>
        /// <returns>The updated IServiceCollection.</returns>
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            // Add controllers with configuration
            services.AddControllers(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
            });

            services.AddVersioning(); // Configure API versioning
            services.ConfigureResponseCompression(); // Configure response compression

            return services;
        }

        /// <summary>
        /// Adds configurations for various services.
        /// </summary>
        /// <param name="services">The IServiceCollection to configure.</param>
        /// <returns>The updated IServiceCollection.</returns>
        public static IServiceCollection AddConfigurations(this IServiceCollection services)
        {
            services.ConfigureOptions<StripeOptionsSetup>(); // Configure options for Stripe

            return services
                .AddPresentation() // Add presentation-related services
                .ConfigureCookieAuthentication() // Configure cookie-based authentication
                .ConfigureCors(); // Configure CORS policy
        }
    }
}
