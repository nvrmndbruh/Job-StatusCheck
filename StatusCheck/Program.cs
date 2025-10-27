using Microsoft.Extensions.Configuration;
using StatusCheck.Models;
using StatusCheck.Services;

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
            try
            {
                config.Bind(appConfig);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nERROR: Failed to load configuration file");
                return;
            }
            

            var registry = new RequestRegistry();
            var logger = new Logger(appConfig.OutputFilePath);

            List<RequestResults> results = [];

            // стартовое сообщение
            Console.WriteLine("--[ STATUS CHECK by Knyazev Anton ]--");
            Console.WriteLine("\nAvailable checks:");
            foreach (var check in registry.RegisteredRequests)
            {
                Console.WriteLine($"    {check.Key} ({check.Value.RequestType.Name}), аргумент: {check.Value.Metadata.ArgumentDescription}");
            }

            if (args.Length > 2)    // неверное число аргументов
            {
                Console.WriteLine("\nERROR: Incorrect number of arguments");
                return;
            }

            if (args.Length == 0)   // стандартный сценарий
            {
                Console.WriteLine("\nRunning default checks...\n");
                foreach (var check in appConfig.RequestsSettings)
                {
                    if (registry.IsRegistered(check.Key))
                    {
                        var request = registry.CreateStatusCheck(check.Key);

                        Console.WriteLine($"{request.Name} ({check.Value.DefaultTarget})");

                        var result = await request.CheckAsync(check.Value.DefaultTarget);

                        Console.WriteLine($"--> {(result.IsSuccessful ? "OK" : "FAIL")}, MESSAGE: {result.Message}");
                        results.Add(result);
                    }
                }
            }
            else
            {
                var requestName = args[0];
                if (registry.IsRegistered(requestName))
                {
                    Console.WriteLine($"\nRunning \"{requestName}\"...\n");

                    // если не дана ссылка - берем из конфига
                    var target = args.Length == 2 ? args[1] : appConfig.RequestsSettings[requestName].DefaultTarget;
                    var request = registry.CreateStatusCheck(requestName);

                    Console.WriteLine($"{request.Name} ({target})");

                    var result = await request.CheckAsync(target);

                    Console.WriteLine($"--> {(result.IsSuccessful ? "OK" : "FAIL")}, MESSAGE: {result.Message}");
                    results.Add(result);
                }
                else
                {
                    Console.WriteLine($"\nERROR: \"{requestName}\" check not found");
                    return;
                }
            }

            try
            {
                // записываем результат
                await logger.WriteResultsAsync(results);
                Console.WriteLine($"\n\nAll results saved to {Path.GetFullPath(appConfig.OutputFilePath)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n\nERROR: can't save results\n{ex.Message}");
            }
        }
    }
}
