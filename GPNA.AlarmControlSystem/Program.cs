using Blazored.Modal;
using Blazored.Toast;
using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Options;
using GPNA.AlarmControlSystem.Services;
using NLog.Web;
using IAuthorizationService = GPNA.AlarmControlSystem.Interfaces.IAuthorizationService;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, true)
    //.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, true)
    .Build();

double timeOut = 1000;
double.TryParse(configuration["ConnectionConfig:AlarmControlSystemWebApi:TimeOut"], out timeOut);

builder.Host.UseNLog();

// builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
//     .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("console", policy => policy.RequireRole("BUILTIN\\Пользователи журналов производительности"));
});

builder.Services.Configure<AcsModuleOptions>(configuration.GetSection("AcsModuleConfig"));
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
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
    .AddScoped<IExportService, ExportService>()
    .AddScoped<IFieldService, FieldService>();

builder.Services.AddHttpClient<IAlarmControlSystemApiBroker, AlarmControlSystemApiBroker>(client =>
{
    client.BaseAddress = new Uri(configuration["ConnectionConfig:AlarmControlSystemWebApi:Uri"]);
    client.Timeout = TimeSpan.FromMilliseconds(timeOut);
});

builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
    // .RequireAuthorization(new AuthorizeAttribute()
    // {
    //     AuthenticationSchemes = NegotiateDefaults.AuthenticationScheme,
    // });

app.MapFallbackToPage("/_Host");

app.Run();