using Plitkarka.Application;
using Plitkarka.Application.Hubs;
using Plitkarka.Domain.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .ConfigureServices((hostContext, services) =>
    {
        services
            .AddDefaultAWSOptions(hostContext.Configuration.GetAWSOptions())
            .AddServices()
            .AddConfiguration()
            .AddMySql()
            .AddMapping()
            .AddSwagger()
            .AddSignaR()
            .AddMyHealthChecks(hostContext.Configuration);
    });

var app = builder.Build();

app.UseRouting();

app.UseCors(builder => builder.AllowAnyOrigin());

app.MapHealthChecks("/api/health");

app.UseAuthentication();
app.UseAuthorization();

app.UseWhen(
    context => context.Request.Path.StartsWithSegments("/api"),
    app =>
    {
        app.UseMiddleware<ExceptionMiddleware>();
    });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseMiddleware<DevAuthorizationMiddleware>();
}
else
{
    app.UseMiddleware<AuthorizationMiddleware>();
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/signalr/chat");
    endpoints.MapControllers();
});

app.Run();
