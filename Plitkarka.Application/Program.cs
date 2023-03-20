using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Plitkarka.Application;
using Plitkarka.Domain.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDefaultAWSOptions(hostContext.Configuration.GetAWSOptions());
        services.AddServices();
        services.AddConfiguration();
        services.AddMySql();
        services.AddMapping();
    });

var app = builder.Build();

app.UseCors(builder => builder.AllowAnyOrigin());

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<AuthorizationMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();     

app.Run();
