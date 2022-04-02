using LettersAnalyzer.Client;
using LettersAnalyzer.Shared.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Text.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddTransient<JsonSerializerOptions>(conf =>
{
    var opts = new JsonSerializerOptions();
    opts.Converters.Add(new DateOnlyConverter());
    return opts;
});

await builder.Build().RunAsync();
