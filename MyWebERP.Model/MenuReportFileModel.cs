using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class MenuReportFileModel : DataHistoryModel
    {
        public System.Int32 web_company_menu_report_file_id { get; set; }
        public System.String company_id { get; set; }
        public System.String web_company_menu_id { get; set; }
        public System.String report_file_id { get; set; }
        public System.String report_name { get; set; }
        public System.String report_title { get; set; }
        public string report_desc { get; set; }
        public System.Int16 invisible { get; set; }
        public System.Int32 order { get; set; }
        public System.Int16 inactive { get; set; }
    }
}
