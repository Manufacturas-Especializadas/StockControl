using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using StockControl.Data;
using StockControl.Services;
using StockControl.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StockControlContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<EntradaServices>();
builder.Services.AddScoped<SalidaServices>();
builder.Services.AddScoped<EntradasSalidasServices>();
builder.Services.AddScoped<PlannerServices>();
builder.Services.AddScoped<ShopOrderServices>();

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
