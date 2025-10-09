using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyWebERP.Model
{
    public class ProvinceLookupModel
    {
        public System.String province_id { get; set; }
        public System.String company_id { get; set; }
        public System.String province_code { get; set; }
        public System.String province_name { get; set; }
        public System.String nation_id { get; set; }
        public System.String comment { get; set; }

        public System.Int32 order { get; set; }
        public System.String region_id { get; set; }
        public System.String extend_id { get; set; }
        public System.Int32 viettel_post_id { get; set; }
    }
}
