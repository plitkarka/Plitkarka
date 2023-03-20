using Microsoft.AspNetCore.Mvc.Filters;
using Plitkarka.Application;
using Plitkarka.Domain.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .ConfigureServices((hostContext, services) =>
    {
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
