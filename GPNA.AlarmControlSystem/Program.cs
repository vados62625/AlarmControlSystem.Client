using GPNA.AlarmControlSystem;
using GPNA.AlarmControlSystem.Config;
using GPNA.AlarmControlSystem.Data;
using GPNA.AlarmControlSystem.Data.AlarmControlSystem;
using GPNA.AlarmControlSystem.LocalDbStorage.Data.Interfaces;
using GPNA.AlarmControlSystem.LocalDbStorage.Data.Requests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Configure<ConnectionConfig>(Configuration.GetSection("ConnectionConfig"));

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();



builder.Services.AddTransient<IRestClient, RestClient>();
builder.Services.AddTransient<IRestService, RestService>();
builder.Services.AddHttpClient<RestClient>(c => c.BaseAddress = new System.Uri("https://192.168.15.227:7010/"));
builder.Services.AddHttpClient<RestClient>(c => c.Timeout = TimeSpan.FromMilliseconds(10000));

builder.Services.AddSingleton<AlarmControlSystemContext>();
builder.Services.AddTransient<IUiRestService, UiRestService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
