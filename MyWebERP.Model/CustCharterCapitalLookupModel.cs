using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class CustCharterCapitalLookupModel
    {
        public System.String cust_charter_capital_id { get; set; }
        public System.String company_id { get; set; }
        public System.String cust_charter_capital_code { get; set; }
        public System.String cust_charter_capital_name { get; set; }
        public System.Decimal charter_capital1 { get; set; }
        public System.Decimal charter_capital2 { get; set; }
        public System.String comment { get; set; }
        public System.Int32 order { get; set; }
    }
}
