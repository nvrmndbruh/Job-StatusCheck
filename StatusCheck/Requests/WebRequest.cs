using StatusCheck.Interfaces;
using StatusCheck.Models;
using System.Diagnostics;

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
                Target = target
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
                result.Message = response.IsSuccessStatusCode
                    ? $"Website is accessible. Status: {(int)response.StatusCode}"
                    : $"Website returned error. Status: {(int)response.StatusCode}";
            }
            catch (HttpRequestException ex)
            {
                stopwatch.Stop();
                result.IsSuccessful = false;
                result.ResponseTime = stopwatch.ElapsedMilliseconds;
                result.Message = $"HTTP Error: {ex.Message}";
            }
            catch (TaskCanceledException)
            {
                stopwatch.Stop();
                result.IsSuccessful = false;
                result.ResponseTime = stopwatch.ElapsedMilliseconds;
                result.Message = $"Request timed out";
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                result.IsSuccessful = false;
                result.ResponseTime = stopwatch.ElapsedMilliseconds;
                result.Message = $"Error: {ex.Message}";
            }

            return result;

            throw new NotImplementedException();
        }
    }
}
