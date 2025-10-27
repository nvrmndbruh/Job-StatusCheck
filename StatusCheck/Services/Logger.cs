using StatusCheck.Models;
using System.Text.Json;

namespace StatusCheck.Services
{
    public class Logger
    {
        private readonly string _filePath;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = false, // каждая запись на отдельной строке (JSONL)
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public Logger(string filePath)
        {
            _filePath = filePath;
        }

        public async Task WriteResultsAsync(List<RequestResults> results)
        {
            
            using var fileStream = new FileStream(_filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
            using var streamWriter = new StreamWriter(fileStream);

            foreach (var result in results)
            {
                var json = JsonSerializer.Serialize(new
                {
                    requestedTime = result.RequestedAt,
                    name = result.Name,
                    target = result.Target,
                    isSuccesful = result.IsSuccessful,
                    responseTime = result.ResponseTime
                }, _jsonOptions);

                await streamWriter.WriteLineAsync(json);
            }

            streamWriter.Close();
        }
    }
}
