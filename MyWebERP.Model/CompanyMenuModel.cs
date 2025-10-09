using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class CompanyMenuModel:DataHistoryModel
    {
        public System.String web_company_menu_id { get; set; }
        public System.String company_id { get; set; }
        public System.String web_menu_id { get; set; }
        [Required]
        public System.String web_company_menu_name { get; set; }
        public System.String parent_id { get; set; }
        public System.Boolean has_children { get; set; }
        public System.String title { get; set; }
        public System.String web_menu_name { get; set; }
        public System.String comment { get; set; }
        public System.String key_words { get; set; }
        public System.String icon { get; set; }

        public System.Int32 order { get; set; }

        public System.Int16 inactive { get; set; }
    }

    public class CompanyMainMenuHeaderModel
    {
        public CompanyMainMenuHeaderModel(string company_id, List<CompanyMainMenuModel> details)
        {
            this.company_id = company_id;
            this.details = details;
        }

        public string company_id { get; set; }

        public List<CompanyMainMenuModel> details { get; set; }
    }
    public class CompanyMainMenuModel:DataHistoryModel
    {
        public string web_company_main_chart_id { get; set; }

        public System.String web_company_menu_id { get; set; }
        public System.String web_company_menu_name { get; set; }
        public System.String title { get; set; }
        public int row { get; set; }
        public int col_width { get; set; }

        public System.Int32 order { get; set; }

        public System.Int16 inactive { get; set; }
    }
}
