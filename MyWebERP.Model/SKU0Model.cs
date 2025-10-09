using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class SKU0Model:DataHistoryModel
    {
        public string sku_id { get; set; }
        public string data_code { get; set; }
        public string company_id { get; set; }
        [Required]
        public string sku { get; set; }
        [Required]
        public string sku_name { get; set; }
        public string sku_description { get; set; }
        public string prd_category_id { get; set; }
        public string prd_category_code { get; set; }
        public string prd_category_name { get; set; }
        [Required]
        public int cost_method_id { get; set; }
        public string cost_method_name { get; set; }
        [Required]
        public string unit_id { get; set; }
        [Required]
        public string unit_name { get; set; }
        public decimal sale_price { get; set; }

        public string image_url { get; set; }
        public Int16 inactive { get; set; }
    }

    public class SKU0LookupModel
    {
        public string sku_id { get; set; }
        public string company_id { get; set; }
        public string sku { get; set; }
        public string sku_name { get; set; }
        public string sku_description { get; set; }
        public string prd_category_id { get; set; }
        public string prd_category_code { get; set; }
        public string prd_category_name { get; set; }
        public int cost_method_id { get; set; }
        public string cost_method_name { get; set; }
        public string unit_id { get; set; }
        public string unit_name { get; set; }
        public decimal sale_price { get; set; }
    }
}
