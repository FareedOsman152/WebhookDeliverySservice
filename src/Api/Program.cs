using Application;
using Infrastructure;
using Persistence;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddApplication()
    .AddPersistence(builder.Configuration)
    .AddInfrastructure()
    .AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    // app.MapOpenApi();
}
app.MapSwagger();
app.UseHttpsRedirection();


app.Run();