using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class VoucherHeaderBaseModel:DataHistoryModel
    {
        public System.String voucher_header_id { get; set; }
        public System.String company_id { get; set; }
        public System.String company_name { get; set; }
        public System.String voucher_code { get; set; }
        [Required]
        public System.String voucher_no { get; set; }
        [Required]
        public System.DateTime voucher_date { get; set; }
        public System.String comment { get; set; }
        public System.String who_trader { get; set; }
        [Required]
        public System.String currency_id { get; set; }
        public System.String currency_code { get; set; }
        public System.String currency_name { get; set; }
        public System.Decimal currency_rate { get; set; }
        [Required]
        public System.String customer_id { get; set; }
        public System.String customer_code { get; set; }
        public System.String customer_name { get; set; }
        public System.String customer_address { get; set; }
        public System.String customer_tel { get; set; }
        public System.String customer_fax { get; set; }
        public System.String customer_tax_code { get; set; }
        public System.Int16 post_gl_book { get; set; }
        public System.Int16 is_deleted { get; set; }
        public System.String created_by_display_name { get; set; }
        public System.DateTime created_on_date { get; set; }
        public System.String last_modified_by_display_name { get; set; }
        public System.DateTime last_modified_on_date { get; set; }
    }

    public class VoucherDetailBaseModel
    {
        public System.String voucher_detail_id { get; set; }
        public System.String voucher_header_id { get; set; }
        public System.String company_id { get; set; }
    }
}
