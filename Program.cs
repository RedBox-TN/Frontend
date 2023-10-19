using Frontend;
using Frontend.Client_Utility;
using Frontend.Settings;
using Frontend.Token_Utility;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using RedBoxAuthentication;
using RedBoxServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var backEndSettings = builder.Configuration.GetSection("RedBoxBackEnd");
builder.Services.Configure<RedBoxBackEndSettings>(backEndSettings);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

var client = new ClientUtility(new OptionsWrapper<RedBoxBackEndSettings>(backEndSettings.Get<RedBoxBackEndSettings>()));

builder.Services.AddSingleton(client);

builder.Services.AddSingleton(new AuthenticationGrpcService.AuthenticationGrpcServiceClient(client.GetChannel()));
builder.Services.AddSingleton(new GrpcConversationServices.GrpcConversationServicesClient(client.GetChannel()));
builder.Services.AddSingleton(
    new GrpcSupervisedConversationService.GrpcSupervisedConversationServiceClient(client.GetChannel()));
builder.Services.AddSingleton(new GrpcAccountServices.GrpcAccountServicesClient(client.GetChannel()));
builder.Services.AddSingleton(new GrpcAdminServices.GrpcAdminServicesClient(client.GetChannel()));

builder.Services.AddSingleton<TokenRefresh>();

await builder.Build().RunAsync();