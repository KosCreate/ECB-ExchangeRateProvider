using AspNetCoreRateLimit;
using ECB_ExchangeRateProvider.AutoMapper_Profiles;
using ECB_ExchangeRateProvider.DBContexts;
using ECB_ExchangeRateProvider.Jobs;
using ECB_ExchangeRateProvider.Options;
using ECB_ExchangeRateProvider.Repositories;
using ExchangeRateGateway;
using ExchangeRateGateway.Interfaces;
using Microsoft.EntityFrameworkCore;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

var rateLimitOptions = new RateLimitOpt();
builder.Configuration.GetSection(RateLimitOpt.RateLimitSection).Bind(rateLimitOptions);

builder.Services.Configure<IpRateLimitOptions>(options => {
    options.GeneralRules = new List<RateLimitRule>() {
        new RateLimitRule() {
            Endpoint = rateLimitOptions.EndPoints,
            Limit = rateLimitOptions.Limit,
            Period = rateLimitOptions.Period
        }
    };
});

builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(AutoMapperSolutionProfile));

builder.Services.AddHttpClient<IExchangeRateService, ExchangeRateService>();

builder.Services.AddDbContext<ExchangeDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"))
);

builder.Services.AddScoped<IExchangeRateDbRepository, ExchangeRateDbRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();

var jobSettings = new JobSettings();
builder.Configuration.GetSection(JobSettings.JobSettingsSection).Bind(jobSettings);

builder.Services.AddQuartz(q => {
    var jobKey = new JobKey(jobSettings.JobKey);
    q.AddJob<UpdateExchangeRatesJob>(options => { options.WithIdentity(jobKey); });
    q.AddTrigger(options => {
        options.
        ForJob(jobKey).
        WithIdentity(jobSettings.JobTrigger).
        WithSimpleSchedule(sched => sched.WithIntervalInMinutes(1).RepeatForever());
    });
});

builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

builder.Services.AddMemoryCache();

var app = builder.Build();
app.UseIpRateLimiting();

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
