using Atlas.Plans.Module;
using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Infrastructure.Integration.Bus;
using Atlas.Users.Module;
using Atlas.Web.ExecutionContext;
using Atlas.Web.Extensions;
using Atlas.Web.Middleware;

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
}

async Task InitialiseModules(IServiceProvider serviceProvider, IConfiguration config)
{
    var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
    var executionContextAccessor = new ExecutionContextAccessor(httpContextAccessor!);

    var eventBus = serviceProvider.GetRequiredService<IEventBus>();
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

    await UsersModuleStartup.Start(executionContextAccessor, config, eventBus, loggerFactory);
    await PlansModuleStartup.Start(executionContextAccessor, config, eventBus, loggerFactory);
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

// Build app after services have been registered
var app = builder.Build();

// Startup the modules
await InitialiseModules(app.Services, builder.Configuration);

// Add all middleware
AddMiddleware(app);

// Run the API
app.Run();
