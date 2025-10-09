using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class ActiveStatusModel
    {
        public Int16 active_status_id { get; set; }
        public string active_status_name { get; set; }

        public static List<ActiveStatusModel> GetDefaultList()
        {
            return new List<ActiveStatusModel>
            {
                new ActiveStatusModel { active_status_id = 0, active_status_name = "Đang SD" },
                new ActiveStatusModel { active_status_id = 1, active_status_name = "Không SD" }
            };
        }

    }
}
