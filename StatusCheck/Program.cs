using Microsoft.Extensions.Configuration;
using StatusCheck.Models;
using StatusCheck.Requests;
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
            config.Bind(appConfig); // Используем Bind вместо Get

            var registry = new RequestRegistry();
            
            if (args.Length == 0)
            {
                foreach (var check in appConfig.RequestsSettings)
                {
                    if (registry.IsRegistered(check.Key))
                    {
                        var request = registry.CreateStatusCheck(check.Key);

                        var result = await request.CheckAsync(check.Value.DefaultTarget);
                    }
                }
            }
        }
    }
}
