using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Compression;

namespace Atlas.Shared.Presentation;

public static class SharedPresentationDependencyInjection
{
    /// <summary>
    /// Adds cookie-based authentication services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    public static IServiceCollection AddCookieAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        });

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.Strict;
        });

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = Utils.IsDevelopment() ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
            options.Cookie.Name = SharedPresentationConstants.CookieName;

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

        return services;
    }

    /// <summary>
    /// Configures response compression. Using Gzip.
    /// </summary>
    /// <param name="services">The IServiceCollection.</param>
    /// <returns>The updated IServiceCollection.</returns>
    public static IServiceCollection ConfigureResponseCompression(this IServiceCollection services)
    {
        services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Fastest;
        });
        services.AddResponseCompression(options =>
        {
            options.Providers.Add<GzipCompressionProvider>();
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json" });
            options.EnableForHttps = true;
        });
        return services;
    }

    /// <summary>
    /// Configures the behaviour for API versioning.
    /// </summary>
    public static IServiceCollection AddVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new ApiVersion(1, 0);
            opt.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader() // this method allow us to get the version number from the URL, eg: https://domain.com/api/v1/metod
            );
        });

        return services;
    }

    /// <summary>
    /// Adds presentation layer services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    public static IServiceCollection AddSharedPresentationDependencyInjection(this IServiceCollection services)
    {
        return services
            .AddCookieAuthentication()
            .AddVersioning()
            .ConfigureResponseCompression();
    }
}
