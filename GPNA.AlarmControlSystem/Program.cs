using System.Net;
using System.Security.Principal;
using Blazored.Modal;
using Blazored.Toast;
using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Options;
using GPNA.AlarmControlSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

double timeOut = 1000;
double.TryParse(configuration["ConnectionConfig:AlarmControlSystemWebApi:TimeOut"], out timeOut);

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLog();

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate(options =>
    {
        // options.Events = new NegotiateEvents
        // {
        //     OnAuthenticated = context =>
        //     {
        //         if (context.Principal.Identity is WindowsIdentity windowsIdentity)
        //         {
        //             string loginName = windowsIdentity.Name;
        //         }
        //
        //         return Task.CompletedTask;
        //     }
        // };
    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // укзывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = JwtOptions.ISSUER,

            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = JwtOptions.AUDIENCE,
            // будет ли валидироваться время существования
            ValidateLifetime = true,

            // установка ключа безопасности
            IssuerSigningKey = JwtOptions.GetSymmetricSecurityKey(),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("console", policy => policy.RequireRole("BUILTIN\\Пользователи журналов производительности"));
});

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
    .AddScoped<IEmailService, EmailService>();

builder.Services.AddHttpClient<IAlarmControlSystemApiBroker, AlarmControlSystemApiBroker>(client =>
{
    client.BaseAddress = new Uri(configuration["ConnectionConfig:AlarmControlSystemWebApi:Uri"]);
    client.Timeout = TimeSpan.FromMilliseconds(timeOut);
});

builder.Services.AddScoped<AuthorizationService>();

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

app.MapBlazorHub()
    .RequireAuthorization(new AuthorizeAttribute()
    {
        AuthenticationSchemes = NegotiateDefaults.AuthenticationScheme,
        // Roles = "superAdmin"
    });

app.MapFallbackToPage("/_Host");

app.Run();