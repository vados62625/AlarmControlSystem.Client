using System.Configuration;
using GPNA.AlarmControlSystem;
using GPNA.AlarmControlSystem.Data;
using LocalDbStorage.Interfaces;
using LocalDbStorage.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddTransient<IDataService, DataService>();
builder.Services.AddHttpClient<DataService>(c =>
{
    var uri = configuration["HttpClientConfig:BaseAddress"];
    var timeOut = configuration["HttpClientConfig:Timeout"];

    c.BaseAddress = new System.Uri(uri);
    c.Timeout = TimeSpan.FromMilliseconds(10000);
});
builder.Services.AddTransient<IUiService, UiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
