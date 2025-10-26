using StatusCheck.Interfaces;
using StatusCheck.Models;
using StatusCheck.Services;
using System.Runtime.CompilerServices;

namespace StatusCheck.Requests
{
    [RequestAttribute(
        name:"web",
        argument:"url")]
    internal class WebRequest : IStatusCheck
    {
        private readonly HttpClient _httpClient;

        public string Name => "Web Request";

        public Task<RequestResults> CheckAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
