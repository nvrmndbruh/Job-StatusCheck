namespace StatusCheck.Models
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RequestAttribute : Attribute
    {
        public string Name { get; }                 // название для обращения через консоль
        public string ArgumentDescription { get; }  // описание необходимого аргумента

        public RequestAttribute(string name, string argument)
        {
            Name = name;
            ArgumentDescription = argument;
        }
    }
}
