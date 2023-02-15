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

app.UseMiddleware(typeof(ExceptionMiddleware));

app.UseSwagger();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
