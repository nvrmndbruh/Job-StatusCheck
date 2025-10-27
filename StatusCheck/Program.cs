using Microsoft.Extensions.Configuration;
using StatusCheck.Models;
using StatusCheck.Services;
using System;

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

            List<RequestResults> results = [];

            Console.WriteLine("--[ STATUS CHECK by Knyazev Anton ]--");
            Console.WriteLine("\nСписок доступных проверок:");
            foreach (var check in registry.RegisteredRequests)
            {
                Console.WriteLine($"    {check.Key} ({check.Value.RequestType.Name}), аргумент: {check.Value.Metadata.ArgumentDescription}");
            }

            if (args.Length > 2)
            {
                Console.WriteLine("\nОШИБКА: Неверное число аргументов");
                return;
            }

            if (args.Length == 0)
            {
                Console.WriteLine("\nЗапуск проверок по-умолчанию\n");
                foreach (var check in appConfig.RequestsSettings)
                {
                    if (registry.IsRegistered(check.Key))
                    {
                        var request = registry.CreateStatusCheck(check.Key);

                        Console.Write($"{request.Name} ({check.Value.DefaultTarget}) | ");
                        var result = await request.CheckAsync(check.Value.DefaultTarget);
                        Console.WriteLine($"{(result.IsSuccessful ? "OK" : "FAIL")}");
                        results.Add(result);
                    }
                }
            }
            else
            {
                var requestName = args[0];
                if (registry.IsRegistered(requestName))
                {
                    Console.WriteLine($"\nЗапуск проверки {requestName}\n");

                    var target = args.Length == 2 ? args[1] : appConfig.RequestsSettings[requestName].DefaultTarget;
                    Console.WriteLine($"{requestName} ({target})");
                    var request = registry.CreateStatusCheck(requestName);

                    var result = await request.CheckAsync(target);
                    Console.WriteLine($"--> {(result.IsSuccessful ? "OK" : "FAIL")}, MESSAGE: {result.Message}");
                    results.Add(result);
                }
                else
                {
                    Console.WriteLine($"\nОШИБКА: Проверка с названием \"{requestName}\" не найдена");
                    return;
                }
            }

            try
            {
                await logger.WriteResultsAsync(results);
                Console.WriteLine($"\n\nРезультаты проверки сохранены в файл {Path.GetFullPath(appConfig.OutputFilePath)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n\nНе удалось сохранить результаты проверок:\n{ex.Message}");
            }
        }
    }
}
