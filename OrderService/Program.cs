using Carter;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Database;
using OrderService.Extensions;
using OrderService.HttpClients.PedidosClient;
using OrderService.HttpClients.Policies;
using OrderService.HttpClients.Revenda;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("DefaultDataBase")));

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));

builder.Services.AddCarter();

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration["MessageBroker:Host"], h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"]);
            h.Password(builder.Configuration["MessageBroker:Password"]);
        });

        configurator.ConfigureEndpoints(context);
    });
});


builder.Services.AddHttpClient<PedidosClient>(client =>
{
    client.BaseAddress = new Uri("https://api.companyx.com/");
}).AddPolicyHandler(HttpPolicy.GetRetryPolicy());

builder.Services.AddHttpClient<RevendaClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7278");
}).AddPolicyHandler(HttpPolicy.GetRetryPolicy());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.MapCarter();

app.UseHttpsRedirection();

app.Run();
