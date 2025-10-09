using DevExpress.DataAccess.Json;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using MyWebERP.Model;
using MyWebERP.Services;
using System.ComponentModel.Design;
using System.IO;
using System.Threading.Tasks;

namespace MyWebERP.Data
{
    /// <summary>
    /// Hiện tại không cần dùng, vì hệ thống chỉ gọi cái MyReportStorageWebExtension thôi
    /// </summary>
    public class MyWebDocumentViewerReportResolver: IWebDocumentViewerReportResolver
    {
        readonly IDataService dataService;
        readonly ReportStorageWebExtension reportStorageWebExtension;
        MenuReportFileItem reportFileItem;
        MenuItem menuItem;

        public MyWebDocumentViewerReportResolver(ReportStorageWebExtension reportStorageWebExtension, IDataService dataService)
        {
            this.reportStorageWebExtension = reportStorageWebExtension;
            this.dataService = dataService;
        }
        public XtraReport Resolve(string reportEntry)
        {
            //string reportName = reportEntry.Substring(0, reportEntry.IndexOf("?") == -1 ? reportEntry.Length : reportEntry.IndexOf("?"));

            //var reportLayout = reportStorageWebExtension.GetData(reportEntry);
            //if (reportLayout == null)
            //    return new XtraReport();
            //using (var ms = new MemoryStream(reportLayout))
            //{
            //    var report = XtraReport.FromXmlStream(ms);
            //    report.DataSource = CreateObjectDataSource(reportName);
            //    return report;
            //}

            ReportViewParamItem _param = Newtonsoft.Json.JsonConvert.DeserializeObject<ReportViewParamItem>(reportEntry);

            //var task = GetDataAsync(url);
            var _menuItemTask = dataService.GetMenuSingle(_param.company_id, _param.company_menu_id);
            menuItem = _menuItemTask.Result;
            reportFileItem = menuItem.ReportFiles.FirstOrDefault(x => x.report_file_id == _param.report_file_id);

            var reportLayout = reportStorageWebExtension.GetData(_param.company_menu_id);
            if (reportLayout == null)
                return new XtraReport();
            using (var ms = new MemoryStream(reportLayout))
            {
                var report = XtraReport.FromXmlStream(ms);
                report.DataSource = CreateObjectDataSource(_param);
                return report;
            }
        }

        private object CreateObjectDataSource(ReportViewParamItem param)
        {
            //string _sSpName = string.IsNullOrEmpty(reportFileItem.sp_name) ? menuItem.FetSingleDataCode : reportFileItem.sp_name;
            ////string _sFetSingleParam = await Data.MyLib.CreateFetSingleParam(menuItem.IdColumnName, FocusedDataId);
            ////Model.APIResultModel _resultFetSingle = await DataService.CallMyApiSimple(_sFetSingleParam, _sSpName);

            ////if (_resultFetSingle.Status == 0)
            ////{
            ////    return _resultFetSingle.Data.ToString();
            ////}

            //string _sFetDataParam = "{" + $"\"{menuItem.IdColumnName}\"=\"{param.list_id}\",\"company_id\"=\"{param.company_id}\"" + "}";
            //var _fetDataResult = dataService.CallMyApiSimple(_sFetDataParam, _sSpName);
            //APIResultModel _fetDataResultModel = _fetDataResult.Result;

            //if (_fetDataResultModel.Status == 0)
            //{
            //    var jsonDataSource = new JsonDataSource();
            //    // Specify the object that retrieves JSON data.
            //    jsonDataSource.JsonSource = new CustomJsonSource(_fetDataResultModel.Data.ToString());
            //    jsonDataSource.Fill();

            //    return jsonDataSource;
            //}

            return null;
        }
    }
}
