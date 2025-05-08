using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorTest;
using BlazorTest.Services;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register HttpClient for API calls
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register Services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<DataService>();
builder.Services.AddScoped<AppStateService>();
builder.Services.AddScoped<ChartJsInterop>();

// Add local storage support
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();