using TZ_AdvertisingPlatform;
using TZ_AdvertisingPlatform.Interfaces;
using TZ_AdvertisingPlatform.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IAdvertisingRepository, InMemoryAdvertisingRepository>();
builder.Services.AddScoped<IAdRecordValidator, AdRecordValidator>();
builder.Services.AddScoped<IAdvertisingService, AdvertisingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
