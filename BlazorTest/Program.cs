using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorTest;
using BlazorTest.Services;
using Blazored.LocalStorage;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure basic console logging
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Register HttpClient for API calls
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register Services in the correct order for dependency injection
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AppStateService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<DataService>();
builder.Services.AddScoped<ChartJsInterop>();

// Register FluentUI services
builder.Services.AddFluentUIComponents();

Console.WriteLine("Application services registered");

var host = builder.Build();

// Log application start
Console.WriteLine("Blazor WebAssembly application starting");

await host.RunAsync();