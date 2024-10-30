using Blazored.LocalStorage;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using MudBlazor;
using MudBlazor.Extensions;
using MudBlazor.Services;
using MudBlazor.Translations;
using OzyParkAdmin.Components;
using OzyParkAdmin.Components.Account;
using OzyParkAdmin.Infrastructure.Layout;
using OzyParkAdmin.Infrastructure.Middlewares;
using OzyParkAdmin.Infrastructure.Plantillas;
using OzyParkAdmin.Shared;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.AddSerilog();

builder.Services.Configure<HubOptions>(options =>
{
    options.MaximumReceiveMessageSize = null;
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddSingleton<ThemeOzyPark>();

builder.Services.AddSingleton(new DialogOptions
{
    CloseButton = true,
    Position = DialogPosition.Center,
    FullWidth = true,
    BackdropClick = false,
    MaxWidth = MaxWidth.ExtraLarge,
});

builder.Services.AddMudServices(options =>
{
    options.SnackbarConfiguration.RequireInteraction = true; //Espera eternamente hasta que lo cierre.
});
builder.Services.AddMudExtensions();
builder.Services.AddMudTranslations();
builder.Services.AddBlazoredLocalStorage();

builder.Services.Configure<TemplateOptions>(options => options.TemplatePath = builder.Environment.WebRootPath);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddScoped<IUserPreferencesService, UserPreferencesService>();
builder.Services.AddLocalization();

builder.AddOzyParkAdmin();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseClientIp();

app.UseMudExtensions();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseRequestLocalization(options =>
    options
        .SetDefaultCulture("es-CL")
        .AddSupportedCultures("es-CL")
        .AddSupportedUICultures("es-CL"));

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

await app.RunAsync();
