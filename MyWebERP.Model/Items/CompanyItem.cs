namespace MyWebERP.Model
{
    public class CompanyItem
    {
        public System.String CompanyId { get; set; }

        public System.String CompanyCode { get; set; }

        // Mã cty tổng.
        public System.String RootId { get; set; }

        public System.String RootCodeId { get; set; }

        public System.String RootName { get; set; }

        public System.String RootName2 { get; set; }

        public System.String RootRportName { get; set; }

        public System.String Alias { get; set; }

        public System.String Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String CompanyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String CompanyName2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String CompanyReportName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String ParentId { get; set; }

        public System.String ParentCodeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String ParentName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String ParentName2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String ParentCompanyReportName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String Address { get; set; }

        public System.String TaxCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String Tel { get; set; }

        public System.String HotLine { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String Fax { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String Email { get; set; }

        public System.String Website { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String Director { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String ChiefAccountant { get; set; }

        public System.String Treasurer { get; set; }

        public System.String ReportCreatedBy { get; set; }

        public System.String Facebook { get; set; }
        public System.String RepresentativeOffice { get; set; }
        public System.String LegalRepresentative { get; set; }
        public System.String LegalRepresentativePosition { get; set; }

        public String Languages { get; set; }

        public DateTime DateExpired { get; set; }

        public Int32 Users { get; set; }

        public Boolean EnableWeb { get; set; }

        public String License { get; set; }

        public System.String SmallLogo { get; set; }

        public System.String LargeLogo { get; set; }

        public System.String Slogan { get; set; }

        public Int32 MainType { get; set; }

        public System.Int32 Level { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Byte Inactive { get; set; }

        /// <summary>
        /// Để xử lý expand tree view
        /// </summary>
        public bool IsExpanded { get; set; } = false;

    }
}
