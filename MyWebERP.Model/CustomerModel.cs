using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class CustomerLookupModel
    {
        public System.String customer_id { get; set; }
        public System.String company_id { get; set; }
        public System.String data_code { get; set; }
        public System.String other_data_code { get; set; }
        public System.String customer_code { get; set; }
        public System.String customer_name { get; set; }
        //public System.String customer_name2 { get; set; }
        //public System.String gender_id { get; set; }
        public System.String gender { get; set; }
        //public System.String vocative_id { get; set; }
        public System.String vocative { get; set; }
        public System.String tax_code { get; set; }
        //public System.String payment_terms_id { get; set; }
        //public System.String payment_terms_code { get; set; }
        //public System.String payment_terms_desc { get; set; }
        //public System.Int32 discount_term { get; set; }
        //public System.Decimal discount_rate { get; set; }
        //public System.Int32 payment_term { get; set; }
        //public System.Decimal overdue_interest_rate { get; set; }
        //public System.String cust_category_id { get; set; }
        public System.String cust_category_code { get; set; }
        public System.String cust_category_name { get; set; }
        //public System.String parent_category_id { get; set; }
        //public System.String parent_code { get; set; }
        //public System.String parent_name { get; set; }
        public System.String full_address { get; set; }
        public System.String address1 { get; set; }
        public System.String address2 { get; set; }
        //public System.String nation_id { get; set; }
        //public System.String nation_code { get; set; }
        //public System.String nation_name { get; set; }
        //public System.String province_id { get; set; }
        //public System.String province_code { get; set; }
        public System.String province_name { get; set; }
        //public System.String district_id { get; set; }
        //public System.String district_code { get; set; }
        public System.String district_name { get; set; }
        public System.String ward_id { get; set; }
        //public System.String ward_code { get; set; }
        //public System.String ward_name { get; set; }
        //public System.String region_id { get; set; }
        //public System.String region_code { get; set; }
        //public System.String region_name { get; set; }
        public System.String dealer { get; set; }
        public System.String tel { get; set; }
        public System.String tel2 { get; set; }
        public System.String email { get; set; }
        public System.String fax { get; set; }
        public System.String home_page { get; set; }
        public System.String facebook { get; set; }
        //public System.DateTime date_of_birth { get; set; }
        //public System.DateTime date_of_incorporation { get; set; }
        public System.String identification { get; set; }
        //public System.Int16 is_personal { get; set; }
        //public System.Int16 is_customer { get; set; }
        //public System.Int16 is_supplier { get; set; }
        //public System.Int16 is_employee { get; set; }
        public System.String comment { get; set; }
        //public System.Int32 order { get; set; }
        //public System.String desc { get; set; }
        //public System.String familiar_level_id { get; set; }
        //public System.String familiar_level_code { get; set; }
        //public System.String familiar_level_name { get; set; }
        //public System.String job_id { get; set; }
        //public System.String job_code { get; set; }
        //public System.String job_name { get; set; }
        //public System.String employee_id { get; set; }
        //public System.String employee_code { get; set; }
        //public System.String employee_name { get; set; }
        //public System.String field_of_activity_id { get; set; }
        //public System.String field_of_activity_code { get; set; }
        //public System.String field_of_activity_name { get; set; }
        //public System.String cust_business_id { get; set; }
        //public System.String cust_business_code { get; set; }
        //public System.String cust_business_name { get; set; }
        //public System.String cust_size_id { get; set; }
        //public System.String cust_size_code { get; set; }
        //public System.String cust_size_name { get; set; }
        //public System.String cust_charter_capital_id { get; set; }
        //public System.String cust_charter_capital_code { get; set; }
        //public System.String cust_charter_capital_name { get; set; }
        //public System.String cust_source_id { get; set; }
        //public System.String cust_source_code { get; set; }
        //public System.String cust_source_name { get; set; }
        //public System.Int32 extend_cust_source_id { get; set; }
        //public System.String cust_type_id { get; set; }
        //public System.String cust_type_code { get; set; }
        //public System.String cust_type_name { get; set; }
        //public System.String active_region_id { get; set; }
        //public System.String active_region_code { get; set; }
        //public System.String active_region_name { get; set; }
        //public System.Int32 extend_id { get; set; }
        //public System.Decimal member_card_amount { get; set; }
        //public System.String member_card_id { get; set; }
        //public System.String member_card_number { get; set; }
        //public System.String member_card_barcode { get; set; }

        
    }
    public class CustomerModel:DataHistoryModel
    {
        public System.String customer_id { get; set; }
        public System.String company_id { get; set; }
        public System.String data_code { get; set; }
        //public System.String other_data_code { get; set; }
        public System.String customer_code { get; set; }
        public System.String customer_name { get; set; }

        public System.String customer_name_full_address => $"{customer_name} {full_address}";
        public string tel_email => $"{tel} {email}";
        public System.String customer_name2 { get; set; }
        public System.String gender_id { get; set; }
        public System.String gender { get; set; }
        public System.String vocative_id { get; set; }
        public System.String vocative { get; set; }
        public System.String tax_code { get; set; }
        public System.String payment_terms_id { get; set; }
        public System.String payment_terms_code { get; set; }
        public System.String payment_terms_desc { get; set; } // Lỗi
        public System.Int32 discount_term { get; set; }
        public System.Decimal discount_rate { get; set; }
        public System.Int32 payment_term { get; set; }
        public System.Decimal overdue_interest_rate { get; set; }
        public System.String cust_category_id { get; set; }
        public System.String cust_category_code { get; set; }
        public System.String cust_category_name { get; set; }
        //public System.String parent_category_id { get; set; }
        //public System.String parent_code { get; set; }
        //public System.String parent_name { get; set; }
        public System.String full_address { get; set; }

        public System.String address1 { get; set; }
        //public System.String address2 { get; set; }
        //public System.String nation_id { get; set; }
        //public System.String nation_code { get; set; }
        //public System.String nation_name { get; set; }
        public System.String province_id { get; set; }
        public System.String province_code { get; set; }
        public System.String province_name { get; set; }
        public System.String district_id { get; set; }
        public System.String district_code { get; set; }
        public System.String district_name { get; set; }
        public System.String ward_id { get; set; }
        public System.String ward_code { get; set; }
        public System.String ward_name { get; set; }
        //public System.String region_id { get; set; }
        //public System.String region_code { get; set; }
        //public System.String region_name { get; set; }
        public System.String dealer { get; set; }
        public System.String tel { get; set; }
        public System.String tel2 { get; set; }
        public System.String email { get; set; }
        public System.String fax { get; set; }
        public System.String home_page { get; set; }
        public System.String facebook { get; set; }
        public System.DateTime date_of_birth { get; set; }
        public System.DateTime date_of_incorporation { get; set; }
        public System.String identification { get; set; }
        public System.Int16 is_personal { get; set; }
        public System.Int16 is_customer { get; set; }
        public System.Int16 is_supplier { get; set; }
        public System.Int16 is_employee { get; set; }
        public System.String comment { get; set; }
        public System.Int32 order { get; set; }

        public System.String desc { get; set; }// Có cột này thì gọi api insert/update bị lỗi, không hiểu tại sao
        public System.String familiar_level_id { get; set; }
        public System.String familiar_level_code { get; set; }
        public System.String familiar_level_name { get; set; }
        public System.String job_id { get; set; }
        public System.String job_code { get; set; }
        public System.String job_name { get; set; }
        public System.String employee_id { get; set; }
        public System.String employee_code { get; set; }
        public System.String employee_name { get; set; }
        public System.String field_of_activity_id { get; set; }
        public System.String field_of_activity_code { get; set; }
        public System.String field_of_activity_name { get; set; }
        public System.String cust_business_id { get; set; }
        public System.String cust_business_code { get; set; }
        public System.String cust_business_name { get; set; }
        public System.String cust_size_id { get; set; }
        public System.String cust_size_code { get; set; }
        public System.String cust_size_name { get; set; }
        public System.String cust_charter_capital_id { get; set; }
        public System.String cust_charter_capital_code { get; set; }
        public System.String cust_charter_capital_name { get; set; }
        public System.String cust_source_id { get; set; }
        public System.String cust_source_code { get; set; }
        public System.String cust_source_name { get; set; }
        public System.Int32 extend_cust_source_id { get; set; }
        public System.String cust_type_id { get; set; }
        public System.String cust_type_code { get; set; }
        public System.String cust_type_name { get; set; }
        public System.String active_region_id { get; set; }
        public System.String active_region_code { get; set; }
        public System.String active_region_name { get; set; }
        public System.String account_id { get; set; }
        public System.String account_code { get; set; }
        public System.String account_name { get; set; }
        //public System.String reference_url { get; set; }
        //public System.String partner_id { get; set; }
        //public System.String partner_secret_key { get; set; }
        //public System.String app_mobile_pass { get; set; }
        public System.Int16 inactive { get; set; }
        //public System.Int32 extend_id { get; set; }
        public System.Decimal member_card_amount { get; set; }
        public System.String member_card_id { get; set; }
        public System.String member_card_number { get; set; }
        public System.String member_card_barcode { get; set; }

        public List<CustContactModel> contacts { get; set; }
        public bool is_loading_contacts { get; set; }
        public bool contacts_loaded { get; set; }
    }

    public class CustomerImportModel:CustomerModel
    {
        public string import_result { get; set; }
        public string save_result { get; set; }
    }

    public class CustomerTypeModel
    {
        public Int16 customer_type_id { get; set; }
        public string customer_type_name { get; set; }
        public static List<CustomerTypeModel> GetDefaultList()
        {
            return new List<CustomerTypeModel>
            {
                new CustomerTypeModel { customer_type_id = 1, customer_type_name = "Cá nhân" },
                new CustomerTypeModel { customer_type_id = 0, customer_type_name = "Tổ chức" }
            };
        }
    }
}
