using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using StockControl.Data;
using StockControl.Services;
using StockControl.Models;
using Microsoft.EntityFrameworkCore;
using Blazored.LocalStorage;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
//using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<StockControlContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//        .AddRoles<IdentityRole>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ProduccionPolicy", policy =>
    policy.RequireRole("Producción"));

    options.AddPolicy("EmpleadoPolicy", policy =>
    policy.RequireRole("Empleado"));

    options.AddPolicy("PlannerPolicy", policy =>
    policy.RequireRole("Planner"));

    options.AddPolicy("AdminPolicy", policy =>
    policy.RequireRole("Admin"));
});

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<EntradaServices>();
builder.Services.AddScoped<SalidaServices>();
builder.Services.AddScoped<EntradasSalidasServices>();
builder.Services.AddScoped<PlannerServices>();
builder.Services.AddScoped<ShopOrderServices>();
builder.Services.AddScoped<HistorialServices>();
builder.Services.AddScoped<RegisterServices>();

var app = builder.Build();


builder.Services.AddLogging(builder => builder.AddDebug().AddConsole());


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
