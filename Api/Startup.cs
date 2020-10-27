using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.DotNet.Interactive.CSharp;
using Microsoft.DotNet.Interactive;

[assembly: FunctionsStartup(typeof(blazoract.Api.Startup))]

namespace blazoract.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<CompositeKernel>(new CompositeKernel() {
                new CSharpKernel().UseDefaultFormatting().UseDotNetVariableSharing()
            });
        }
    }
}