using StatusCheck.Models;

namespace StatusCheck.Interfaces
{
    public interface IStatusCheck
    {
        public string Name { get; }
        public Task<RequestResults> CheckAsync(CancellationToken cancellationToken = default);
    }
}
