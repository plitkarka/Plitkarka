using Plitkarka.Application;
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
            .AddMyHealthChecks(hostContext.Configuration);
    });

var app = builder.Build();

app.UseCors(builder => builder.AllowAnyOrigin());

app.MapHealthChecks("/api/health");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

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

app.MapControllers();     

app.Run();
