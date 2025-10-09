using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class SKU001Model
    {
        public string sku_id { get; set; }

        public string company_id { get; set; }

        public string data_code { get; set; }

        public int row { get; set; }
        public int col { get; set; }
        public string sku { get; set; }
        public int color { get; set; }
        public string barcode { get; set; }

        public string display_barcode { get; set; }

        public string sku_name { get; set; }

        public string sku_description { get; set; }

        public string image_url { get; set; }
        //public string json_info { get; set; }

        public string floor_name { get; set; }
        public string work_item_name { get; set; }

        public List<Sku001TooltipItem> tooltips { get; set; } = new List<Sku001TooltipItem>();
    }

    public class Sku001TooltipItem
    {
        public string item_name { get; set; }
        public string item_value { get; set; }
        public int order { get; set; }
        public int is_image { get; set; }
    }
}
