using StatusCheck.Interfaces;
using StatusCheck.Models;

namespace StatusCheck.Requests
{
    [Request(
        name:"web",
        argument:"url")]
    internal class WebRequest : IStatusCheck
    {
        private readonly HttpClient _httpClient = new();

        public string Name => "Web Request";

        public Task<RequestResults> CheckAsync(string target, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
