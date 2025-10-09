using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class WidgetConfig
    {
        public string ComponentFullName { get; set; } = string.Empty;
        public string Col { get; set; } = "col-md-12";
        public Dictionary<string, object>? Parameters { get; set; }
    }
}
