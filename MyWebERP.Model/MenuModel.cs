using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class MenuModel:DataHistoryModel
    {
        public System.String web_menu_id { get; set; }
        [Required]
        public System.String web_menu_name { get; set; }
        [Required]
        public System.String title { get; set; }
        public System.String url { get; set; }
        public System.String comment { get; set; }
        public System.String key_words { get; set; }
        public System.String icon { get; set; }
        public System.String web_assembly_id { get; set; }
        public System.String controller { get; set; }
        public System.String index_action { get; set; }
        public System.String edit_action { get; set; }
        public System.String delete_action { get; set; }
        public System.String data_code { get; set; }
        public System.Int32 order { get; set; }
        public System.Int16 is_super_user { get; set; }
        public System.Int16 inactive { get; set; }
        public System.String parent_id { get; set; }
        public System.String fet_data_code { get; set; }
        public System.String fet_single_data_code { get; set; }
        public System.String ins_data_code { get; set; }
        public System.String upd_data_code { get; set; }
        public System.String del_data_code { get; set; }
    }
}
