using DevExpress.XtraReports.UI;
using MyWebERP.Data;
using MyWebERP.Model;
using System.Dynamic;

namespace MyWebERP.Services
{
    public interface IDataService
    {
        /// <summary>
        /// Đơn giản, dùng để gọi cho những việc data nhẹ
        /// </summary>
        /// <param name="paramData"></param>
        /// <param name="apiCode"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> CallMyApiSimple0(Object paramData, string apiCode);

        Task<Model.APIResultModel> CallMyApiSimple(Object paramData, string apiCode);

        Task<APIResultDataModel?> GetDataSimple(Object paramData, string apiCode);

        Task<List<TResult>> GetDataSimple<TResult>(Object paramData, string apiCode);

        Task<HttpResponseMessage> CallMyApi0(Object paramData, string apiCode);
        Task<Model.APIResultModel> CallMyApi(Object paramData, string apiCode);

        Task<APIResultDataModel?> GetData(Object paramData, string apiCode);

        Task<List<TResult>> GetData<TResult>(Object paramData, string apiCode);

        Task<List<ExpandoObject>> GetDataX(Object paramData, string apiCode, List<DataColumn> dataColumns);

        Task<ExpandoObject> GetSingleData(Object paramData, string apiCode, List<DataColumn> dataColumns);

        #region Lookup

        Task<List<TResult>> LookupCode<TResult>(string sCompanyId,
            string sUsername,
            string sUsedDataCode,
            string sUsedTableName,
            DateTime dtmDate,
            string sDataCode,
            Int16 iActiveOnly,
            string sParent,
            Int16 iLiabilitiesAccountOnly,
            Int16 iDetailAccountOnly,
            Int16 iParentOnly,
            Int16 iItselfOnly,
            string sCompanyMenuId,
            string apiCode);

        Task<List<TResult>> LookupCode<TResult>(string sCompanyId,
            DateTime dtmDate,
            string sDataCode,
            Int16 iActiveOnly,
            Int16 iLiabilitiesAccountOnly,
            Int16 iDetailAccountOnly,
            Int16 iItselfOnly,
            string sCompanyMenuId,
            string apiCode);

        Task<List<TResult>> LookupCode<TResult>(string sCompanyId,
            Int16 iActiveOnly,
            Int16 iItselfOnly,
            string sCompanyMenuId,
            string apiCode);
        Task<List<TResult>> LookupCode<TResult>(string sCompanyId,
            Int16 iActiveOnly,
            string sParent,
            Int16 iItselfOnly,
            string sCompanyMenuId,
            string apiCode);

        Task<List<TResult>> LookupCode<TResult>(string sCompanyId,
            Int16 iActiveOnly,
            string sCompanyMenuId,
            string apiCode);

        Task<List<TResult>> LookupCode<TResult>(string sCompanyId, string sCompanyMenuId, string apiCode);

        Task<List<TResult>> LookupAccountCode<TResult>(string sCompanyId,
            DateTime dtmDate,
            Int16 iActiveOnly,
            Int16 iLiabilitiesAccountOnly,
            Int16 iDetailAccountOnly,
            Int16 iItselfOnly,
            string sCompanyMenuId,
            string apiCode);

        Task<List<TResult>> LookupAccountCode<TResult>(string sCompanyId,
            DateTime dtmDate,
            Int16 iActiveOnly,
            Int16 iLiabilitiesAccountOnly,
            Int16 iDetailAccountOnly,
            string sCompanyMenuId,
            string apiCode);

        Task<List<TResult>> LookupAccountCode<TResult>(string sCompanyId,
            Int16 iActiveOnly,
            Int16 iLiabilitiesAccountOnly,
            Int16 iDetailAccountOnly,
            Int16 iItselfOnly,
            string sCompanyMenuId,
            string apiCode);
        Task<List<TResult>> LookupAccountCode<TResult>(string sCompanyId,
            Int16 iActiveOnly,
            Int16 iLiabilitiesAccountOnly,
            Int16 iDetailAccountOnly,
            string sCompanyMenuId,
            string apiCode);

        //Task<List<dynamic>> LookupCode(string sCompanyId, 
        //    string sUsername,
        //    string sUsedDataCode,
        //    string sUsedTableName,
        //    DateTime dtmDate,
        //    string sDataCode,
        //    Int16 iActiveOnly,
        //    Int16 iLiabilitiesAccountOnly,
        //    Int16 iDetailAccountOnly,
        //    Int16 iParentOnly,
        //    Int16 iItselfOnly,
        //    string sCompanyMenuId, 
        //    string apiCode);

        //Task<List<dynamic>> LookupCode(string sCompanyId,
        //    DateTime dtmDate,
        //    string sDataCode,
        //    Int16 iActiveOnly,
        //    Int16 iLiabilitiesAccountOnly,
        //    Int16 iDetailAccountOnly,
        //    Int16 iItselfOnly,
        //    string sCompanyMenuId,
        //    string apiCode);

        //Task<List<dynamic>> LookupCode(string sCompanyId,
        //    Int16 iActiveOnly,
        //    Int16 iItselfOnly,
        //    string sCompanyMenuId,
        //    string apiCode);

        //Task<List<dynamic>> LookupCode(string sCompanyId,
        //    Int16 iActiveOnly,
        //    string sCompanyMenuId,
        //    string apiCode);

        //Task<List<dynamic>> LookupCode(string sCompanyId, string sCompanyMenuId, string apiCode);

        //Task<List<dynamic>> LookupAccountCode(string sCompanyId,
        //    DateTime dtmDate,
        //    Int16 iActiveOnly,
        //    Int16 iLiabilitiesAccountOnly,
        //    Int16 iDetailAccountOnly,
        //    Int16 iItselfOnly,
        //    string sCompanyMenuId,
        //    string apiCode);

        //Task<List<dynamic>> LookupAccountCode(string sCompanyId,
        //    DateTime dtmDate,
        //    Int16 iActiveOnly,
        //    Int16 iLiabilitiesAccountOnly,
        //    Int16 iDetailAccountOnly,
        //    string sCompanyMenuId,
        //    string apiCode);

        //Task<List<dynamic>> LookupAccountCode(string sCompanyId,
        //    Int16 iActiveOnly,
        //    Int16 iLiabilitiesAccountOnly,
        //    Int16 iDetailAccountOnly,
        //    Int16 iItselfOnly,
        //    string sCompanyMenuId,
        //    string apiCode);
        //Task<List<dynamic>> LookupAccountCode(string sCompanyId,
        //    Int16 iActiveOnly,
        //    Int16 iLiabilitiesAccountOnly,
        //    Int16 iDetailAccountOnly,
        //    string sCompanyMenuId,
        //    string apiCode);

        #endregion Lookup
        Task<List<MenuItem>> GetMenus();

        Task<List<MenuItem>> GetMenus(string lang);

        Task<List<MenuItem>> GetMenus(bool getMainChart);

        Task<List<MenuItem>> GetMenus(bool getMainChart, string lang);
        Task<MenuItem> GetMenuSingle(string companyMenuId);

        Task<MenuItem> GetMenuSingle(string companyId, string companyMenuId);
        Task<Model.APIResultModel> CheckToken(String token);

        Task<List<PeriodItem>> PeriodLookup(String periodId);
        Task<List<LookupItem>> GetLookup(string sListLookupApiCode);

        Task<List<CompanyItem>> GetCompany4MenuFilter(string companyId, string companyMenuId);

        Task<String> CreateDataCode(string companyId, string dataCode, string prevCode);
        Task<String> CreateDataCodeByDate(string companyId, string dataCode, string prevCode, DateTime date, string dateColumn);

        Task<String> CreateVoucherNo(string companyId, string dataCode, DateTime date);

        Task<decimal> GetCurrencyRate(string companyId, string currencyCode, DateTime date);

        Task<string> GetRowId(string companyId, string companyMenuId);

        Task<Byte[]> GetReportFileContent(string companyId, string reportFileId);
        Task SaveReportFileContent(string companyId, string reportFileId, XtraReport report);

        Task<XtraReport> CreateReportFile(string companyId, string reportFileId);

        Task<object> GetStringData4Report(ReportViewParamItem param);

        Task<List<DevExpress.XtraReports.Parameters.Parameter>> GetReportParameters(string companyId);

        Task SetReportDataSource(XtraReport report, ReportViewParamItem param);

        Task<XtraReport> CreateReport(ReportViewParamItem param);
    }
}
