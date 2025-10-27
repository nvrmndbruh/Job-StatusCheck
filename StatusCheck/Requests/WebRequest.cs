using StatusCheck.Interfaces;
using StatusCheck.Models;
using System.Diagnostics;
using System.Threading;

namespace StatusCheck.Requests
{
    [Request(
        name:"web",
        argument:"url")]
    internal class WebRequest : IStatusCheck
    {
        private readonly HttpClient _httpClient = new();

        public string Name => "Web Request";

        public async Task<RequestResults> CheckAsync(string target, CancellationToken cancellationToken = default)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = new RequestResults
            {
                Name = this.Name,
                Address = target
            };
            try
            {
                // Добавляем схему, если отсутствует
                if (!target.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                    !target.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    target = "https://" + target;
                }

                var response = await _httpClient.GetAsync(target, cancellationToken);
                stopwatch.Stop();

                result.IsSuccessful = response.IsSuccessStatusCode;
                result.ResponseTime = stopwatch.ElapsedMilliseconds;
            }
            catch (HttpRequestException ex)
            {
                stopwatch.Stop();
                result.IsSuccessful = false;
                result.ResponseTime = stopwatch.ElapsedMilliseconds;
            }
            catch (TaskCanceledException)
            {
                stopwatch.Stop();
                result.IsSuccessful = false;
                result.ResponseTime = stopwatch.ElapsedMilliseconds;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                result.IsSuccessful = false;
            }

            return result;

            throw new NotImplementedException();
        }
    }
}
