using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class DistrictLookupModel
    {
        public System.String district_id { get; set; }
        public System.String company_id { get; set; }
        public System.String district_code { get; set; }
        public System.String district_name { get; set; }
        public System.String province_id { get; set; }
        public System.String province_code { get; set; }
        public System.String province_name { get; set; }
        public System.String nation_id { get; set; }
        public System.String nation_code { get; set; }
        public System.String nation_name { get; set; }
        public System.String extend_id { get; set; }
        public System.Int32 viettel_post_id { get; set; }
        public System.String comment { get; set; }
        public System.Int32 order { get; set; }
        public System.Int16 inactive { get; set; }
    }
}
