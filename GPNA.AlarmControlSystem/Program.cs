using Blazored.Modal;
using Blazored.Toast;
using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Services;
using NLog.Web;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

double timeOut = 1000;
double.TryParse(configuration["ConnectionConfig:AlarmControlSystemWebApi:TimeOut"], out timeOut);

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLog();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services
    .AddBlazoredModal()
    .AddBlazoredToast()
    .AddScoped<ISpinnerService, SpinnerService>()
    .AddScoped<IBufferAlarmService, BufferAlarmService>()
    .AddScoped<IIncomingAlarmService, IncomingAlarmService>()
    .AddScoped<IActiveAlarmService, ActiveAlarmService>()
    .AddScoped<ISuppressedAlarmService, SuppressedAlarmService>()
    .AddScoped<IWorkStationService, WorkStationService>()
    .AddScoped<IUserService, UserService>()
    .AddScoped<ITagService, TagService>()
    .AddScoped<IEmailService, EmailService>()
    .AddScoped<IFieldService, FieldService>();

builder.Services.AddHttpClient<IAlarmControlSystemApiBroker, AlarmControlSystemApiBroker>(client =>
{
    client.BaseAddress = new Uri(configuration["ConnectionConfig:AlarmControlSystemWebApi:Uri"]);
    client.Timeout = TimeSpan.FromMilliseconds(timeOut);
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
