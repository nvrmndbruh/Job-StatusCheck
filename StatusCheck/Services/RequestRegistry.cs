using StatusCheck.Interfaces;
using StatusCheck.Models;
using System.Reflection;

namespace StatusCheck.Services
{
    public class RequestRegistry
    {
        // список всех найденных проверок
        private readonly Dictionary<string, (Type RequestType, RequestAttribute Metadata)> _registeredRequests = new();

        public RequestRegistry()
        {
            DiscoverHealthChecks();
        }

        private void DiscoverHealthChecks()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var checkTypes = assembly.GetTypes()
                .Where(t => typeof(IStatusCheck).IsAssignableFrom(t)    // ищем все классы, которые реализуют нужный интерфейс
                           && !t.IsInterface
                           && !t.IsAbstract);

            foreach (var type in checkTypes)
            {
                var attribute = type.GetCustomAttribute<RequestAttribute>();
                if (attribute != null)
                {
                    _registeredRequests[attribute.Name.ToLower()] = (type, attribute);
                    Console.WriteLine($"найдена проверка: {attribute.Name} ({type.Name}) | {attribute.ArgumentDescription}");
                }
            }
        }

        public IStatusCheck? CreateStatusCheck(string commandName)
        {
            if (_registeredRequests.TryGetValue(commandName.ToLower(), out var registration))
            {
                return Activator.CreateInstance(registration.RequestType) as IStatusCheck;
            }
            return null;
        }

        public bool IsRegistered(string commandName)
        {
            return _registeredRequests.ContainsKey(commandName.ToLower());
        }
    }
}