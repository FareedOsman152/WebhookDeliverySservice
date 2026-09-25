using Application;
using Infrastructure;
using Persistence;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApplication()
    .AddPersistence()
    .AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();