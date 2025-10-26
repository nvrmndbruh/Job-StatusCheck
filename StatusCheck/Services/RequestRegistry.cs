using StatusCheck.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StatusCheck.Services
{
    public class RequestRegistry
    {
        private readonly Dictionary<string, (Type RequestType, RequestAttribute Metadata)> _registeredRequests = new();

        public RequestRegistry()
        {
            DiscoverHealthChecks();
        }

        private void DiscoverHealthChecks()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var checkTypes = assembly.GetTypes()
                .Where(t => typeof(IStatusCheck).IsAssignableFrom(t)
                           && !t.IsInterface
                           && !t.IsAbstract);

            foreach (var type in checkTypes)
            {
                var attribute = type.GetCustomAttribute<RequestAttribute>();
                if (attribute != null)
                {
                    _registeredRequests[attribute.Name.ToLower()] = (type, attribute);
                    Console.WriteLine($"найдена проверка: {attribute.Name} ({type.Name})");
                }
            }
        }
    }
}
