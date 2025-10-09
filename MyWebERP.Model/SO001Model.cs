using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class SO001Model:VoucherHeaderBaseModel
    {
        public SO001Model()
        {
            details = new List<SO001DetailModel>();
        }
        public System.String src_company_id { get; set; }
        public System.String src_company_name { get; set; }
        public System.String src_voucher_header_id { get; set; }
        
        public System.String member_card_id { get; set; }
        public System.Decimal member_card_amount { get; set; }
        public System.String member_card_number { get; set; }
        public System.String member_card_barcode { get; set; }
                
        public System.String other_requirements { get; set; }
        public System.String order_content { get; set; }
        public System.String delivery_content { get; set; }
        public System.DateTime delivery_date { get; set; }
        public System.Int32 warning_delivery_day { get; set; }
        public System.String generated { get; set; }
        public System.String payment_method_id { get; set; }
        public System.String payment_method_name { get; set; }
        public System.String payment_terms_id { get; set; }
        public System.String payment_terms_desc { get; set; }
        public System.Int32 discount_term { get; set; }
        public System.Decimal discount_rate { get; set; }
        public System.Int32 payment_term { get; set; }
        public System.Decimal overdue_interest_rate { get; set; }
        public System.Decimal amount_fc { get; set; }
        public System.Decimal amount { get; set; }
        public System.Decimal discount_amount_fc { get; set; }
        public System.Decimal discount_amount { get; set; }
        public System.Decimal export_tax_fc { get; set; }
        public System.Decimal export_tax { get; set; }
        public System.Decimal vat_fc { get; set; }
        public System.Decimal vat { get; set; }
        public System.Decimal total_amount_fc { get; set; }
        public System.Decimal total_amount { get; set; }
        public System.Decimal prepay_amount_fc { get; set; }
        public System.Decimal prepay_amount { get; set; }
        public System.Decimal cash_discount_amount_fc { get; set; }
        public System.Decimal shipping_amount_fc { get; set; }
        public System.Decimal receivable_amount_fc { get; set; }
        public System.Decimal receivable_amount { get; set; }
        public System.Decimal member_card_paid_amount_fc { get; set; }
        public System.Decimal member_card_paid_amount { get; set; }
        public System.Decimal voucher_paid_amount_fc { get; set; }
        public System.Decimal voucher_paid_amount { get; set; }
        public System.Decimal promotion_gift_amount_fc { get; set; }
        public System.Decimal promotion_gift_amount { get; set; }
        public System.Decimal remain_amount_fc { get; set; }
        public System.Decimal remain_amount { get; set; }
        public System.String employee_id { get; set; }
        public System.String employee_code { get; set; }
        public System.String employee_name { get; set; }
        public System.String status_id { get; set; }
        public System.String status { get; set; }
        public System.String status_name { get; set; }
        public System.String status_desc { get; set; }
        public System.Int32 color { get; set; }
        public System.String so_order_type_id { get; set; }
        public System.String so_order_type_name { get; set; }
        public System.Int16 ship_complated { get; set; }
        public System.String ship_status { get; set; }
        
        public List<SO001DetailModel> details { get; set; }
    }

    public class SO001DetailModel:VoucherDetailBaseModel
    {
        public System.String src_voucher_detail_id { get; set; }
        
        public System.String sku_id { get; set; }
        [Required]
        public System.String sku { get; set; }
        public System.String product_id { get; set; }
        public System.String product_code { get; set; }
        public System.String product_name { get; set; }
        public System.DateTime expiry_date { get; set; }
        [Required]
        public System.Decimal quantity { get; set; }
        public System.Decimal quantity_base { get; set; }
        public System.String unit_id { get; set; }
        public System.String unit_code { get; set; }
        public System.String unit_name { get; set; }
        public System.Decimal coefficient { get; set; }
        public System.Int16 attach { get; set; }
        public System.Int16 promotions { get; set; }
        public System.Int16 discounting_goods { get; set; }
        public System.Decimal price_fc { get; set; }
        public System.Decimal price { get; set; }
        public System.Decimal amount_fc { get; set; }
        public System.Decimal amount { get; set; }
        public System.Decimal export_tax_rate { get; set; }
        public System.Decimal export_tax_fc { get; set; }
        public System.Decimal export_tax { get; set; }
        public System.Decimal vat_rate { get; set; }
        public System.Decimal vat_fc { get; set; }
        public System.Decimal vat { get; set; }
        public System.Decimal discount_rate { get; set; }
        public System.Decimal discount_amount_fc { get; set; }
        public System.Decimal discount_amount { get; set; }
        public System.Decimal total_amount_fc { get; set; }
        public System.Decimal total_amount { get; set; }
        public System.String delivery_time { get; set; }
        public System.String delivery_location { get; set; }
        public System.String comment { get; set; }
        public System.String employee_id { get; set; }
        public System.String employee_code { get; set; }
        public System.String employee_name { get; set; }
        public System.Int16 ship_complated { get; set; }
        public System.String ship_status { get; set; }
        public System.Int16 invoice_complated { get; set; }
        public System.String src_data_code { get; set; }

    }
}
