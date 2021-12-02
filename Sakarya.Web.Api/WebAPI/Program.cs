using System.Threading;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Business.Internals.Handlers.OperationClaims;
using Core.Utilities.IoC;
using DataAccess.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static Business.Internals.Handlers.OperationClaims.CreateOperationClaimsInternalCommand;

namespace WebAPI
{
    /// <summary>
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// </summary>
        /// <param name="args"></param>
#pragma warning disable 1998
        public static async Task Main(string[] args)
#pragma warning restore 1998
        {
            var result = CreateHostBuilder(args).Build().RunAsync();
            var operationClaimRepository = ServiceTool.ServiceProvider.GetService<IOperationClaimRepository>();
            var createOcResult = new CreateOperationClaimsInternalCommandHandler(operationClaimRepository).Handle(
                new CreateOperationClaimsInternalCommand(), new CancellationToken());

            result.Wait();
            createOcResult.Wait();
        }

        /// <summary>
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options => options.AddServerHeader = false);
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                });
        }
    }
}