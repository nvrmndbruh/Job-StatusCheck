namespace StatusCheck.Models
{
    public class RequestConfigurationModel
    {
        public string DefaultTarget { get; set; } = string.Empty;   // цель по-умолчанию
        public int Timeout { get; set; }                            // время ожидания
    }
}
