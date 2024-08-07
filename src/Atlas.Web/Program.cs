using Atlas.Law.Infrastructure.Module;
using Atlas.Plans.Infrastructure.Module;
using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Application.Abstractions.Services.EmailService;
using Atlas.Shared.Application.ModuleBridge;
using Atlas.Shared.Infrastructure.Builders;
using Atlas.Shared.Infrastructure.Integration.Bus;
using Atlas.Shared.Infrastructure.Options.OptonsSetup;
using Atlas.Shared.Infrastructure.Services;
using Atlas.Users.Infrastructure.Module;
using Atlas.Web.ExecutionContext;
using Atlas.Web.Extensions;
using Atlas.Web.Middleware;
using Atlas.Web.Modules.Shared;

void ConfigureServices(IServiceCollection services, ConfigureHostBuilder hostBuilder)
{
    // Add services to the container.
    services.AddConfigurations();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();
    services.AddSingleton<IEventBus, InMemoryEventBus>();

    // Register Modules
    services.AddSingleton<IUsersModule, UsersModule>();
    services.AddSingleton<IPlansModule, PlansModule>();
    services.AddSingleton<ILawModule, LawModule>();

    // Module Bridge (for synchronous communication between modules)
    services.AddSingleton<IModuleBridge, ModuleBridge>();

    // Contact email endpoint isn't specific to any one module, so we need to register those dependencies here.
    services.ConfigureOptions<EmailOptionsSetup>();
    services.AddMvcCore().AddRazorViewEngine(); // Enables MVC for potentially rendering email templates
    services.AddScoped<IEmailService, EmailService>(); // Registers the concrete implementation for sending emails
    services.AddScoped<EmailContentBuilder>(); // Registers a service for building email content
}

async Task InitialiseModules(IServiceProvider serviceProvider, IConfiguration config)
{
    var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
    var executionContextAccessor = new ExecutionContextAccessor(httpContextAccessor!);

    var eventBus = serviceProvider.GetRequiredService<IEventBus>();
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
    var moduleBridge = serviceProvider.GetRequiredService<IModuleBridge>();

    await UsersModuleStartup.Start(moduleBridge, executionContextAccessor, config, eventBus, loggerFactory);
    await PlansModuleStartup.Start(moduleBridge, executionContextAccessor, config, eventBus, loggerFactory);
    await LawModuleStartup.Start(moduleBridge, executionContextAccessor, config, eventBus, loggerFactory);
}

void AddMiddleware(WebApplication app)
{
    // Register the exception handling middleware, which will catch any unhandled exceptions and return a 500 response
    app.ConfigureExceptionHander();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        // If we're in production, redirect all traffic to HTTPS
        app.UseHttpsRedirection();
    }

    // Register the CORS middleware
    app.UseCors(Atlas.Web.Constants.CorsPolicyName);

    app.UseRouting();

    // Register the auth middlewares
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
}

// Create the app builder
var builder = WebApplication.CreateBuilder(args);

// Configure all global services
ConfigureServices(builder.Services, builder.Host);

builder.Services.AddHealthChecks();

// Build app after services have been registered
var app = builder.Build();

app.MapHealthChecks("/health");

// Startup the modules
await InitialiseModules(app.Services, builder.Configuration);

// Add all middleware
AddMiddleware(app);

// Run the API
app.Run();
