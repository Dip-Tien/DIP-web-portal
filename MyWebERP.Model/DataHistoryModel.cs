using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class DataHistoryModel
    {
        public System.Int32 created_by_user_id { get; set; }
        public string created_by_display_name { get; set; }
        public System.DateTime created_on_date { get; set; }
        public System.Int32 last_modified_by_user_id { get; set; }
        public string last_modified_by_display_name { get; set; }
        public System.DateTime last_modified_on_date { get; set; }
    }
}
