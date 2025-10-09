using DevExpress.XtraReports.UI;
using System.Dynamic;
using MyWebERP.Model;
//using System.Text;
using MyWebERP.Lib;
//using DevExpress.DataAccess.ObjectBinding;
using DevExpress.DataAccess.Json;
using DevExpress.CodeParser;
using MyWebERP.Services;
using System.ComponentModel.Design;
using DevExpress.DataAccess.ObjectBinding;
//using System.Xml;
//using System.Text.Json;
//using System;

namespace MyWebERP.Data
{
    public class MyReportStorageWebExtension: DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension
    {
        const string FileExtension = ".repx";
        readonly string ReportDirectory;
        protected MyWebERP.Services.IDataService DataService { get; set; }

        public MyReportStorageWebExtension(MyWebERP.Services.IDataService dataService, IWebHostEnvironment env)
        {
            DataService = dataService;

            ReportDirectory = Path.Combine(env.ContentRootPath, "Reports");
            if (!Directory.Exists(ReportDirectory))
            {
                Directory.CreateDirectory(ReportDirectory);
            }
        }
        public override bool CanSetData(string url)
        {
            // Determines whether a report with the specified URL can be saved.
            // Add custom logic that returns **false** for reports that should be read-only.
            // Return **true** if no valdation is required.
            // This method is called only for valid URLs (if the **IsValidUrl** method returns **true**).

            return true;
        }

        public override bool IsValidUrl(string url)
        {
            // Determines whether the URL passed to the current report storage is valid.
            // Implement your own logic to prohibit URLs that contain spaces or other specific characters.
            // Return **true** if no validation is required.

            return true;
        }

        public override async Task<byte[]> GetDataAsync(string url)
        {
            ReportViewParamItem _param = Newtonsoft.Json.JsonConvert.DeserializeObject<ReportViewParamItem>(url);

            //return await DataService.GetReportFileContent("", _param.report_file_id);

            try
            {
                //string path = Path.Combine(ReportDirectory, _param.report_file_id + FileExtension);
                //XtraReport report = null;

                //if (Directory.EnumerateFiles(ReportDirectory).
                //        Select(Path.GetFileNameWithoutExtension).
                //        Contains(_param.report_file_id))
                //{
                //    report = XtraReport.FromXmlFile(path);
                //}
                //else
                //{
                //    report = new XtraReport();
                //}

                //await DataService.SetReportDataSource(report, _param);

                //report.RequestParameters = false;

                XtraReport report = await DataService.CreateReport(_param);

                using (MemoryStream ms = new MemoryStream())
                {
                    report.SaveLayoutToXml(ms);
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new DevExpress.XtraReports.Web.ClientControls.FaultException("Could not get report data.", ex);
            }

            throw new DevExpress.XtraReports.Web.ClientControls.FaultException(string.Format("Could not find report '{0}'.", url));
        }

        public override byte[] GetData(string url)
        {
            var task = GetDataAsync(url);
            return task.Result;
        }

        public override Dictionary<string, string> GetUrls()
        {
            // Returns a dictionary of the existing report URLs and display names. 
            // This method is called when running the Report Designer, 
            // before the Open Report and Save Report dialogs are shown and after a new report is saved to storage.
            return Directory.GetFiles(ReportDirectory, "*" + FileExtension)
                                     .Select(Path.GetFileNameWithoutExtension)
                                     .ToDictionary<string, string>(x => x);
        }

        public override async Task SetDataAsync(XtraReport report, string url)
        {
            ReportViewParamItem _param = Newtonsoft.Json.JsonConvert.DeserializeObject<ReportViewParamItem>(url);
            //await DataService.SaveReportFileContent("", _param.report_file_id, report);

            // Stores the specified report to a Report Storage using the specified URL. 
            // This method is called only after the IsValidUrl and CanSetData methods are called.
            if (!IsWithinReportsFolder(_param.report_file_id, ReportDirectory))
                throw new DevExpress.XtraReports.Web.ClientControls.FaultException("Invalid report name.");


            // Xử lý lại datasource để chỉ lưu schema thôi
            if (report.DataSource != null && report.DataSource.GetType().Name == "JsonDataSource")
            {
                var jsonDataSource = report.DataSource as JsonDataSource;

                var jsonDataSource2 = new JsonDataSource();
                jsonDataSource2.Schema = jsonDataSource.Schema;

                report.DataSource = jsonDataSource2;
            }

            report.SaveLayoutToXml(Path.Combine(ReportDirectory, _param.report_file_id + FileExtension));
        }

        public override async Task<string> SetNewDataAsync(XtraReport report, string defaultUrl)
        {
            await SetDataAsync(report, defaultUrl);
            return defaultUrl;
        }

        private bool IsWithinReportsFolder(string url, string folder)
        {
            var rootDirectory = new DirectoryInfo(folder);
            var fileInfo = new FileInfo(Path.Combine(folder, url));
            return fileInfo.Directory.FullName.ToLower().StartsWith(rootDirectory.FullName.ToLower());
        }
    }
}
