using System.Configuration;
using System.Globalization;
using GPNA.AlarmControlSystem;
using GPNA.AlarmControlSystem.Data;
using LocalDbStorage.Interfaces;
using LocalDbStorage.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

Int32 result;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services
    .AddScoped<IBufferAlarmService, BufferAlarmService>()
    .AddScoped<IActiveAlarmService, ActiveAlarmService>()
    .AddScoped<IIncomingAlarmService, IncomingAlarmService>()
    .AddScoped<ISuppressedAlarmService, SuppressedAlarmService>()
    .AddScoped<IBufferAlarmService, BufferAlarmService>()
    .AddScoped<ITagService, TagService>();

builder.Services.AddHttpClient<BufferAlarmService>(client =>
{
    client.BaseAddress = new Uri(configuration["HttpClientConfig:BaseAddress"]);
});

builder.Services.AddHttpClient<ActiveAlarmService>(client =>
{
    client.BaseAddress = new Uri(configuration["HttpClientConfig:BaseAddress"]);
});

builder.Services.AddHttpClient<IncomingAlarmService>(client =>
{
    client.BaseAddress = new Uri(configuration["HttpClientConfig:BaseAddress"]);
});

builder.Services.AddHttpClient<SuppressedAlarmService>(client =>
{
    client.BaseAddress = new Uri(configuration["HttpClientConfig:BaseAddress"]);
});

builder.Services.AddHttpClient<TagService>(client =>
{
    client.BaseAddress = new Uri(configuration["HttpClientConfig:BaseAddress"]);
});


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
