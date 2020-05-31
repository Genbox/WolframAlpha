using System.Net;
using System.Net.Http;
using Genbox.WolframAlpha.Abstract;
using Genbox.WolframAlpha.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Genbox.WolframAlpha.Tests
{
    public abstract class TestBase
    {
        protected TestBase()
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