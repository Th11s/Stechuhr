using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TimeKeeping.Web.Client;
using TimeKeeping.Web.Client.Clients;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<TimeKeepingClient>(sp => new TimeKeepingClient(builder.HostEnvironment.BaseAddress, sp.GetRequiredService<HttpClient>()));
builder.Services.AddScoped<AppStore>();

builder.Services.AddLocalization();

await builder.Build().RunAsync();
