namespace StatusCheck.Services
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RequestAttribute : Attribute
    {
        public string Name { get; }         // название для обращения через консоль
        public string ArgumentDescription { get; }     

        public RequestAttribute(string name, string argument)
        {
            Name = name;
            ArgumentDescription = argument;
        }
    }
}
