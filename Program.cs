using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;
using WeatherApp.ActionFilters;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using WeatherApp;

var builder = WebApplication.CreateBuilder(args);

//string vaultUri = builder.Configuration.GetValue<string>("");
string? vaultUri = Environment.GetEnvironmentVariable("KEYVAULT_URI");

if (string.IsNullOrEmpty(vaultUri))
{
    Console.WriteLine("KEYVAULT_URI is not set or is empty.");
    vaultUri = Config.Uri;
}
else
{
    Console.WriteLine($"KEYVAULT_URI: {vaultUri}");
}

if (string.IsNullOrEmpty(vaultUri))
{
    throw new ArgumentNullException(nameof(vaultUri), "VaultUri cannot be null or empty.");
}

var keyVaultEndpoint = new Uri(vaultUri);

builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
builder.Services.AddSingleton(new SecretClient(new Uri(vaultUri), new DefaultAzureCredential()));

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IWeatherService, WeatherService>();
builder.Services.AddTransient<IApiKeyValidation, ApiKeyValidation>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ApiKeyFilter>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.MapDefaultControllerRoute();

app.Run();