using Microsoft.Win32;
using StatusCheck.Services;

namespace StatusCheck
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var registry = new RequestRegistry();
        }
    }
}
