using Carter;
using Identity.API.Extensions;
using Notification.Api.Extensions;
using Notofication.Infrastructure.IoC.DI;


var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();


builder.Services.AddOpenApi();
builder.Services.AddTwilio();
builder.Services.AddTwilioConfig(builder.Configuration);
builder.Services.AddRepositories();
builder.Services
    .AddCarter()
    .AddVersion();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddMediatRSetup();
builder.Services.AddServices();
var connection = builder.Configuration.GetSection("ConnectionStrings:rabbitmq").Value;
builder.AddRabbitMQClient(connectionName: connection);


var app = builder.Build();

app.MapDefaultEndpoints();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapVersionedGroup()
    .MapCarter();

await app.ApplyMigrationsAsync();

app.Run();
