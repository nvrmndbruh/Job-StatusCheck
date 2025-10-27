using Microsoft.Extensions.Configuration;
using StatusCheck.Models;
using StatusCheck.Services;
using System.Text.Json;

namespace StatusCheck
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // загрузка конфигурации
            var config = new ConfigurationBuilder()
                .AddJsonFile("appconfig.json", optional: true, reloadOnChange:false)
                .Build();

            // переводим в модели
            var appConfig = new AppConfigurationModel();
            config.Bind(appConfig);

            var registry = new RequestRegistry();
            var logger = new Logger(appConfig.OutputFilePath);

            List<RequestResults> results = new();
            
            if (args.Length == 0)
            {
                foreach (var check in appConfig.RequestsSettings)
                {
                    if (registry.IsRegistered(check.Key))
                    {
                        var request = registry.CreateStatusCheck(check.Key);

                        var result = await request.CheckAsync(check.Value.DefaultTarget);
                        results.Add(result);
                    }
                }
            }

            try
            {
                await logger.WriteResultsAsync(results);
                Console.WriteLine($"\nРезультаты проверки сохранены в файл {Path.GetFullPath(appConfig.OutputFilePath)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nНе удалось сохранить результаты проверок:\n{ex.Message}");
            }
        }
    }
}
