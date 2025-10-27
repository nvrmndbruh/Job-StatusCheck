namespace StatusCheck.Models
{
    public class AppConfigurationModel
    {
        public Dictionary<string, RequestConfigurationModel> RequestsSettings { get; set; } = new();
        public string OutputFilePath { get; set; } = string.Empty;
    }
}
