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
        public List<RequestConfigurationModel> RequestSettings { get; set; } = new();
    }
}
