using System.Net;
using System.Net.Http;
using Genbox.WolframAlpha.Abstract;
using Genbox.WolframAlpha.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Xunit.Abstractions;

namespace Genbox.WolframAlpha.Tests
{
    public abstract class TestBase
    {
        protected TestBase(ITestOutputHelper outputHelper)
        {
            IConfiguration configFile = new ConfigurationBuilder()
                                        .AddJsonFile("Config.json", false)
                                        .Build();

            ServiceCollection services = new ServiceCollection();
            services.AddSingleton(x =>
            {
                HttpClientHandler handler = new HttpClientHandler();

                if (bool.Parse(configFile["UseProxy"]))
                    handler.Proxy = new WebProxy(configFile["Proxy"]);

                return new HttpClient(handler);

            });

            services.AddLogging(x =>
            {
                x.SetMinimumLevel(LogLevel.Debug);
                x.AddXUnit(outputHelper);
            });

            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IXmlSerializer, ReflectedXmlSerializer>();
            services.AddSingleton<WolframAlphaClient>();
            services.AddSingleton(x =>
            {
                WolframAlphaConfig wolframConfig = new WolframAlphaConfig();
                configFile.Bind(wolframConfig);
                return wolframConfig;
            });

            ServiceProvider provider = services.BuildServiceProvider();
            Client = provider.GetRequiredService<WolframAlphaClient>();
            Config = provider.GetRequiredService<WolframAlphaConfig>();
        }

        protected WolframAlphaClient Client { get; }
        protected WolframAlphaConfig Config { get; }
    }
}