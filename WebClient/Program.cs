using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebClient.Clients;
using Polly;
using System.Net.Http;
using Polly.Timeout;
using WebClient.UI;
using WebClient.RandomCustomerGeneration;

namespace WebClient
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            IHost host = AppStartup();
            DialogRunner dialogRunner = host.Services.GetRequiredService<DialogRunner>();
            await dialogRunner.Run();
        }

        private static IHost AppStartup()
        {
            var builder = new ConfigurationBuilder();

            var host = Host.CreateDefaultBuilder()
                        .ConfigureServices((context, services) => {
                            Random jitterer = new Random();
                            services.AddHttpClient<CustomerClient>(client =>
                            {
                                client.BaseAddress = new System.Uri("https://localhost:5001");
                            })
                            .AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>()
                                .WaitAndRetryAsync(5, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp) + jitterer.NextDouble())))
                            .AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().CircuitBreakerAsync(
                                3,
                                TimeSpan.FromSeconds(15)
                            ))
                            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));

                            services.AddTransient<DialogRunner>()
                            .AddTransient<IRandomCustomerGenerator, RandomCustomerGenerator>();
                        })
                        .Build();

            return host;
        }
    }
}