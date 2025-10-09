using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class ReportFileModel:DataHistoryModel
    {
        public string data_code { get; set; }
        public string report_file_id{ get; set; }
        public string web_menu_id { get; set; }
        public string web_menu_name { get; set; }
        public string report_file { get; set; }
        public string report_name { get; set; }
        public string report_file_desc { get; set; }
        public string report_title { get; set; }
        public string sp_name { get; set; }
        public string file_contents { get; set; }
        public Int16 invisible { get; set; }
    }
}
