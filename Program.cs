using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BunnyMonitor;
using BlazorGeolocation;
using Blazored.LocalStorage;
using BunnyMonitor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<BlazorGeolocationService>();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<AccountService>();

var host = builder.Build();

var accountService = host.Services.GetRequiredService<AccountService>();
await accountService.Initialize();

await host.RunAsync();
