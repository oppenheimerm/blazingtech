using Blazored.LocalStorage;
using BT.Admin.AuthState;
using BT.Admin.Components;
using BT.Admin.Helpers;
using BT.Admin.Services;
using BT.Admin.Services.Files;
using BT.Shared.Services.AuthService;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

var GoogleAppCredentialsFilePath = builder.Configuration["ConnectionStrings:GoogleAppCredentialsFilePath"];
// use the SetEnvironmentVariable() method to set up our google authentication.
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", GoogleAppCredentialsFilePath);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();

//  DI
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5203") });

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IJWTUtilities, JWTUtilities>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IPhotoService , PhotoService>();

builder.Services.AddBlazoredLocalStorage();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

//  Map controllers / user authentication / authorization
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

//  middleware would only called once for the blazor app at setup.
//  blazor is a component based system. component re-render is triggered by state changes, not navigation. 
//app.UseMiddleware<AuthApplicationUseMiddleware>();

app.Run();
