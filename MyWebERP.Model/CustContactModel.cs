using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class CustContactModel:DataHistoryModel
    {
        public System.String cust_contact_id { get; set; }
        public System.String company_id { get; set; }
        public System.String customer_id { get; set; }
        public System.String contact_name { get; set; }
        public System.DateTime date_of_birth { get; set; }
        public System.String vocative_id { get; set; }
        public System.String vocative { get; set; }
        public System.String gender_id { get; set; }
        public System.String gender { get; set; }
        public System.String department { get; set; }
        public System.String address { get; set; }
        public System.String employee_id { get; set; }
        public System.String contact_position { get; set; }
        public System.String tel { get; set; }
        public System.String tel2 { get; set; }
        public System.String email { get; set; }
        public System.String comment { get; set; }
        public System.Int32 order { get; set; }
        public System.Int16 inactive { get; set; }
    }
}
