using ECB_ExchangeRateProvider.AutoMapper_Profiles;
using ECB_ExchangeRateProvider.DBContexts;
using ECB_ExchangeRateProvider.Repositories;
using ExchangeRateGateway;
using ExchangeRateGateway.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(AutoMapperSolutionProfile));
builder.Services.AddHttpClient<IExchangeRateService, ExchangeRateService>();
builder.Services.AddDbContext<ExchangeRateDBContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"))
);

builder.Services.AddScoped<IExchangeRateDbRepository, ExchangeRateDbRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();

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
