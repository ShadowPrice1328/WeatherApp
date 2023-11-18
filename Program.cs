using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using WeatherApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IApiService, ApiService>();

var app = builder.Build();

app.UseStaticFiles();
app.MapDefaultControllerRoute();

app.Run();