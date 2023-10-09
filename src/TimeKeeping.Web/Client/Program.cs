using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TimeKeeping.Web.Client;
using TimeKeeping.Web.Client.Clients;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<TimeKeepingClient>(x => new TimeKeepingClient(builder.HostEnvironment.BaseAddress, x.GetRequiredService<HttpClient>()));

builder.Services.AddScoped<AppStore>();
builder.Services.AddScoped<AppState>();

await builder.Build().RunAsync();
