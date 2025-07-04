using BackTransaccionesLogicStudio.Models;
using BackTransaccionesLogicStudio.Services;
using BackTransaccionesLogicStudio.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
var productsApiBaseUrl = builder.Configuration["Services:ProductsApi:BaseUrl"]
                       ?? throw new InvalidOperationException(
                              "ProductsApi url no está configurado");

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DbtestLogicStudioContext>(opts =>
    opts.UseSqlServer(connection));
builder.Services.AddHttpClient("ProductsApi", client =>
{
    client.BaseAddress = new Uri(productsApiBaseUrl);
});
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
