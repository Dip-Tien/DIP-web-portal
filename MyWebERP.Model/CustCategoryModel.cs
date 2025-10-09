using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class CustCategoryModel
    {
        public System.String cust_category_id { get; set; }
        public System.String data_code { get; set; }
        public System.String company_id { get; set; }
        [Required]
        public System.String cust_category_code { get; set; }
        [Required]
        public System.String cust_category_name { get; set; }
        public System.String parent_id { get; set; }
        public System.String comment { get; set; }
        public System.Int16 inactive { get; set; }
        public System.String created_by_display_name { get; set; }
        public System.DateTime created_on_date { get; set; }
        public System.String last_modified_by_display_name { get; set; }
        public System.DateTime last_modified_on_date { get; set; }
    }

    public class CustCategoryLookupModel
    {
        public System.String cust_category_id { get; set; }
        public System.String data_code { get; set; }
        public System.String company_id { get; set; }
        [Required]
        public System.String cust_category_code { get; set; }
        [Required]
        public System.String cust_category_name { get; set; }
        public System.String parent_id { get; set; }
        public System.String comment { get; set; }
    }
}
