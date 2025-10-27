using StatusCheck.Models;

namespace StatusCheck.Interfaces
{
    public interface IStatusCheck
    {
        public string Name { get; }
        public Task<RequestResults> CheckAsync(string target, CancellationToken cancellationToken = default);
    }
}