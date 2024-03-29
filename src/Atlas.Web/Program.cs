using Atlas.Plans.Module;
using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Infrastructure.Integration.Bus;
using Atlas.Users.Module;
using Atlas.Web.ExecutionContext;
using Atlas.Web.Extensions;
using Atlas.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddConfigurations(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();
builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();

// Register Modules
builder.Services.AddSingleton<IUsersModule, UsersModule>();
builder.Services.AddSingleton<IPlansModule, PlansModule>();

var app = builder.Build();

var executionContextAccessor = app.Services.GetService<IExecutionContextAccessor>();
var eventBus = app.Services.GetRequiredService<IEventBus>();

await UsersModuleStartup.Start(executionContextAccessor, builder.Configuration, eventBus);
await PlansModuleStartup.Start(executionContextAccessor, builder.Configuration, eventBus);

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

app.Run();
