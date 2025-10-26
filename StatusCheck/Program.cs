using Microsoft.Extensions.Configuration;

namespace StatusCheck
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // загрузка конфигурации
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appconfig.json", true, false)
                .Build();
        }
    }
}
