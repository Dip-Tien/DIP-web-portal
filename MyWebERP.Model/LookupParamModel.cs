using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class LookupParamModel
    {
		/// <summary>
		/// Mã cty hiện hành
		/// </summary>
		public string company_id { get; set; }
		public string username { get; set; }
		/// <summary>
		/// data_code/voucher_code hiện hành
		/// </summary>
		public string used_data_code { get; set; }
		/// <summary>
		/// table_name hiện hành
		/// </summary>
		public string used_table_name { get; set; }
		/// <summary>
		/// Ngày hiện hành
		/// </summary>
		public DateTime date { get; set; }
		/// <summary>
		/// data_code lấy dữ liệu, dùng cho lookup có nhiều loại dữ liệu khác nhau
		/// </summary>
		public string data_code { get; set; }
		public Int16 active_only { get; set; }
		public Int16 liabilities_account_only { get; set; }
        public Int16 detail_account_only { get; set; }
		/// <summary>
		/// Chỉ lấy cha, dùng cho dữ liệu cây
		/// </summary>
        public Int16 parent_only { get; set; }
        public Int16 itself_only { get; set; }
		public string parent { get; set; }
		/// <summary>
		/// web_company_menu_id hiện hành, có thể dùng để phân quyền.
		/// </summary>
        public string company_menu_id { get; set; }
    }
}
