using Blazored.SessionStorage;
using Frontend;
using Frontend.Channel_Utility;
using Frontend.Client_Utility;
using Frontend.Token_Utility;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RedBoxAuthentication;
using RedBoxServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var backEndSettings = builder.Configuration.GetSection("RedBoxBackEnd");

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

var channel = new ChannelUtility();

builder.Services.AddSingleton(channel);

builder.Services.AddSingleton(new AuthenticationGrpcService.AuthenticationGrpcServiceClient(channel.GetChannel()));
builder.Services.AddSingleton(new GrpcConversationServices.GrpcConversationServicesClient(channel.GetChannel()));
builder.Services.AddSingleton(
    new GrpcSupervisedConversationService.GrpcSupervisedConversationServiceClient(channel.GetChannel()));
builder.Services.AddSingleton(new GrpcAccountServices.GrpcAccountServicesClient(channel.GetChannel()));
builder.Services.AddSingleton(new GrpcAdminServices.GrpcAdminServicesClient(channel.GetChannel()));

builder.Services.AddSingleton<TokenUtility>();
builder.Services.AddSingleton<ClientUtility>();
builder.Services.AddBlazoredSessionStorageAsSingleton();

await builder.Build().RunAsync();