using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class ReportViewParamItem
    {
        //public System.Int32 web_company_menu_report_file_id { get; set; }
        public string report_file_id { get; set; }
        public string company_id { get; set; }
        public string company_menu_id { get; set; }
        public string report_title { get; set; }
        public string api_code { get; set; }
        /// <summary>
        /// Các param lấy dữ liệu
        /// </summary>
        public Dictionary<string, object> param { get; set; }
        /// <summary>
        /// Các param để đẩy lên view
        /// </summary>
        public Dictionary<string, object> view_param { get; set; }
        public bool is_voucher { get; set; }
    }
}
