using Microsoft.EntityFrameworkCore;
using PoSBackend.Context;
using PoSBackend.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddTransient<UnitRepository>();
builder.Services.AddTransient<ItemRepository>();
builder.Services.AddTransient<ReceiptRepository>();

builder.Services.AddCors(x =>
{
    x.AddPolicy("a", policy => {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

/*
var myKey = builder.Configuration.GetValue<int>("MyKey");
var myKey2 = builder.Configuration.GetSection("MyKey2");
var A = myKey2.GetValue<int>("A");
*/

var connectionString = builder.Configuration.GetConnectionString("PoSDbContext");

builder.Services.AddDbContext<PoSDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
}/*, ServiceLifetime.Singleton*/);


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.UseCors("a");

app.Run();
