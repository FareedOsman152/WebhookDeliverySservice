using Api;
using Api.Endpoints;
using Application;
using Infrastructure;
using Persistence;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddApi()
    .AddApplication()
    .AddPersistence(builder.Configuration)
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    // app.MapOpenApi();
}
app.MapSwagger();
app.UseHttpsRedirection();
OrderFromMarketplaceEndpoints.Map(app);


app.Run();