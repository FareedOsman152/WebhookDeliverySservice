using Application;
using Infrastructure;
using Persistence;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApplication()
    .AddPersistence(builder.Configuration)
    .AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();