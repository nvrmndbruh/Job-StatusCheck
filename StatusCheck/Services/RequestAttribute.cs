namespace StatusCheck.Services
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RequestAttribute : Attribute
    {
        public string Name { get; }
        public string Argument { get; }

        public RequestAttribute(string name, string argument)
        {
            Name = name;
            Argument = argument;
        }
    }
}
