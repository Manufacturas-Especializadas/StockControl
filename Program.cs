using StockControl;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StockControl.Services;
using Microsoft.EntityFrameworkCore;
using StockControl.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddDbContext<StockControlContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//Add services
builder.Services.AddScoped<EntradaServices>();


await builder.Build().RunAsync();

