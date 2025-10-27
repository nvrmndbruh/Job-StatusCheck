using StatusCheck.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusCheck.Models
{
    public class AppConfigurationModel
    {
        public Dictionary<string, RequestConfigurationModel> RequestsSettings { get; set; } = new();
        public string OutputFilePath { get; set; } = string.Empty;
    }
}
