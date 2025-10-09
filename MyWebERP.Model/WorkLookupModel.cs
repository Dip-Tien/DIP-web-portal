using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class WorkLookupModel
    {
        public string work_id { get; set; }
        public string work_code { get; set; }
        public string work_name { get; set; }
    }

    public class WorkItemLookupModel
    {
        public string work_item_id { get; set; }
        public string work_item_code { get; set; }
        public string work_item_name { get; set; }
    }
}
