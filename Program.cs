using Frontend;
using Frontend.Client_Utility;
using Frontend.Token_Utility;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RedBoxAuthentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

var client = new ClientUtility();

builder.Services.AddSingleton(client);

builder.Services.AddSingleton(new AuthenticationGrpcService.AuthenticationGrpcServiceClient(client.GetChannel()));

builder.Services.AddSingleton<TokenRefresh>();

await builder.Build().RunAsync();