using Blazored.LocalStorage;
using DevExpress.DataAccess.Json;
using DevExpress.XtraReports.UI;
using Microsoft.Extensions.Localization;
using MyWebERP.Data;
using MyWebERP.Lib;
using MyWebERP.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Dynamic;
using System.Globalization;
using System.Text;

namespace MyWebERP.Services
{
    public class DataService : IDataService
    {
        private readonly IStringLocalizer _localizer;
        private ILocalStorageService _localStorageService;
        private readonly IConfiguration _config;
        private HttpClient _httpClient;// { get; set; }
        private const string DETAIL_URL = @"/api/bms4";

        const string FileExtension = ".repx";
        readonly string ReportDirectory;

        //JsonSerializerOptions jsonInsensitiveOptions = new JsonSerializerOptions()
        //{
        //    // Bỏ qua tên hoa thường
        //    PropertyNameCaseInsensitive = true
        //};

        JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public DataService(IConfiguration config, 
            HttpClient httpClient, 
            ILocalStorageService localStorageService,
            IWebHostEnvironment env,
            IStringLocalizer localizer) 
        {
            _config = config;
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _localizer = localizer;

            ReportDirectory = Path.Combine(env.ContentRootPath, "Reports");
            if (!Directory.Exists(ReportDirectory))
            {
                Directory.CreateDirectory(ReportDirectory);
            }
        }

        #region Simple
        public async Task<HttpResponseMessage> CallMyApiSimple0(Object paramData, string apiCode)
        {
            var param = new APIParameterModel();
            param.Data = paramData;
            param.DataCode = apiCode;
            param.ConnectionName = _config.GetValue<string>("DbConnectionName");
            param.RootCompanyId = _config.GetValue<String>("DefaultCompanyId");
            param.CnnFromConfigFile = _config.GetValue<Int16>("ApiCnnFromConfigFile");

            //_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResult.access_token.access_token);

            if (_httpClient.DefaultRequestHeaders.Authorization == null)
            {
                //LoginResultModel user = await _localStorageService.GetItemAsync<LoginResultModel>(LocalStorageName.USER);                
                //_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.access_token.access_token);

                string _sToken = await _localStorageService.GetItemAsync<string?>(LocalStorageName.TOKEN);
                if (String.IsNullOrEmpty(_sToken))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _sToken);
                }
            }

            return await _httpClient.PostAsJsonAsync(DETAIL_URL, param);
        }

        public async Task<Model.APIResultModel> CallMyApiSimple(Object paramData, string apiCode)
        {
            //var param = new APIParameterModel();
            //param.Data = paramData;
            //param.DataCode = apiCode;
            //param.ConnectionName = _config.GetValue<string>("DbConnectionName");

            var result = await this.CallMyApiSimple0(paramData, apiCode);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await result.Content.ReadFromJsonAsync<APIResultModel>();
            }
            else
            {
                String _sMessage = await result.Content.ReadAsStringAsync();
                HttpResponseMessageContent _message = System.Text.Json.JsonSerializer.Deserialize<HttpResponseMessageContent>(_sMessage);

                return new APIResultModel(1, _message.Message, null);
            }
        }

        public async Task<APIResultDataModel?> GetDataSimple(Object paramData, string apiCode)
        {
            APIResultModel _result = await this.CallMyApiSimple(paramData, apiCode);

            if (_result.Status != 0)
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<APIResultDataModel>(_result.Data.ToString(), jsonSerializerSettings);

                // Lỗi
                //var serializer = JsonSerializer.Create(jsonSerializerSettings);
                //JToken token = _result.Data as JToken;
                //return token.ToObject<APIResultDataModel>(serializer);
            }
        }

        public async Task<List<TResult>> GetDataSimple<TResult>(Object paramData, string apiCode)
        {
            //APIResultModel _result = await this.CallMyApi(paramData, apiCode);

            //if (_result.Status != 0)
            //{
            //    return new List<TResult>();
            //}
            //else
            //{
            //    return System.Text.Json.JsonSerializer.Deserialize<APIResultDataModel>(_result.Data.ToString(), jsonInsensitiveOptions);
            //}

            APIResultDataModel _resultData = await this.GetDataSimple(paramData, apiCode);

            if (_resultData == null)
            {
                return new List<TResult>();
            }
            else
            {
                return JsonConvert.DeserializeObject<List<TResult>>(_resultData.data_details.ToString(), jsonSerializerSettings);

                //var serializer = JsonSerializer.Create(jsonSerializerSettings);
                //JToken token = _resultData.data_details as JToken;
                //return token.ToObject<List<TResult>>(serializer);
            }
        }
        #endregion Simple
        public async Task<HttpResponseMessage> CallMyApi0(Object paramData, string apiCode)
        {
            var param = new APIParameterModel();
            param.Data = paramData;
            param.DataCode = apiCode;
            param.ConnectionName = _config.GetValue<string>("DbConnectionName");
            param.RootCompanyId = _config.GetValue<String>("DefaultCompanyId");
            param.CnnFromConfigFile = _config.GetValue<Int16>("ApiCnnFromConfigFile");

            //_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResult.access_token.access_token);

            if (_httpClient.DefaultRequestHeaders.Authorization == null)
            {
                //LoginResultModel user = await _localStorageService.GetItemAsync<LoginResultModel>(LocalStorageName.USER);                
                //_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.access_token.access_token);

                string _sToken = await _localStorageService.GetItemAsync<string?>(LocalStorageName.TOKEN);
                if (String.IsNullOrEmpty(_sToken))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _sToken);
                }
            }

            // Bỏ lệnh này vì lưu khách hàng nhiều trường là bị lỗi
            // Giống như là có giới hạn data
            //return await _httpClient.PostAsJsonAsync("/api/bms2", param);

            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                Culture = new CultureInfo("en-US"),
            };

            string _sData = Newtonsoft.Json.JsonConvert.SerializeObject(param, jsonSerializerSettings);
            var content = new StringContent(_sData, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, _httpClient.BaseAddress + DETAIL_URL);

            request.Content = content;
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<Model.APIResultModel> CallMyApi(Object paramData, string apiCode)
        {
            //var param = new APIParameterModel();
            //param.Data = paramData;
            //param.DataCode = apiCode;
            //param.ConnectionName = _config.GetValue<string>("DbConnectionName");

            var result = await this.CallMyApi0(paramData, apiCode);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await result.Content.ReadFromJsonAsync<APIResultModel>();
            }
            else
            {
                String _sMessage = await result.Content.ReadAsStringAsync();
                HttpResponseMessageContent _message = System.Text.Json.JsonSerializer.Deserialize<HttpResponseMessageContent>(_sMessage);

                return new APIResultModel(1, _message.Message, null);
            }
        }

        public async Task<APIResultDataModel?> GetData(Object paramData, string apiCode)
        {
            APIResultModel _result = await this.CallMyApi(paramData, apiCode);

            if(_result.Status != 0)
            {
                return null;
            }
            else
            {
                //return System.Text.Json.JsonSerializer.Deserialize<APIResultDataModel>(_result.Data.ToString(), jsonInsensitiveOptions);
                return JsonConvert.DeserializeObject<APIResultDataModel>(_result.Data.ToString(), jsonSerializerSettings);
            }
        }

        public async Task<List<TResult>> GetData<TResult>(Object paramData, string apiCode)
        {
            //APIResultModel _result = await this.CallMyApi(paramData, apiCode);

            //if (_result.Status != 0)
            //{
            //    return new List<TResult>();
            //}
            //else
            //{
            //    return System.Text.Json.JsonSerializer.Deserialize<APIResultDataModel>(_result.Data.ToString(), jsonInsensitiveOptions);
            //}

            APIResultDataModel _resultData = await this.GetData(paramData, apiCode);

            if(_resultData == null)
            {
                return new List<TResult>();
            }
            else
            {
                //return JsonConvert.DeserializeObject<List<TResult>>(_resultData.data_details.ToString(), jsonSerializerSettings);

                var token = _resultData.data_details as JToken;
                var serializer = JsonSerializer.CreateDefault(jsonSerializerSettings);

                return token?.ToObject<List<TResult>>(serializer);
            }
        }

        public async Task<List<ExpandoObject>> GetDataX(Object paramData, string apiCode, List<DataColumn> dataColumns)
        {
            APIResultDataModel _resultData = await this.GetData(paramData, apiCode);
            List<ExpandoObject> _data = new List<ExpandoObject>();

            if (_resultData != null)
            {
                JArray array = JArray.Parse(_resultData.data_details.ToString());

                _data = MyLib.ConvertListJObjectToListExpando(array);
            }

            return _data;
        }

        public async Task<ExpandoObject> GetSingleData(Object paramData, string apiCode, List<DataColumn> dataColumns)
        {
            APIResultDataModel _resultData = await this.GetData(paramData, apiCode);

            if (_resultData == null || _resultData.data_details == null)
            {
                return null;
            }
            else
            {
                //return JsonConvert.DeserializeObject<List<TResult>>(_resultData.data_details.ToString(), jsonSerializerSettings);
                JObject o = JObject.Parse(_resultData.data_details.ToString());
                return MyLib.ConvertJObjectToExpando(o, dataColumns);
            }
        }

        #region Lookup

        private async Task<List<TResult>> LookupCode<TResult>(LookupParamModel param, string apiCode)
        {
            APIResultDataModel _resultData = await this.GetDataSimple(param, apiCode);

            if (_resultData == null)
            {
                return new List<TResult>();
            }
            else
            {
                //return JsonConvert.DeserializeObject<List<TResult>>(_resultData.data_details.ToString());
                var token = _resultData.data_details as JToken;

                return token?.ToObject<List<TResult>>();
            }
        }

        public async Task<List<TResult>> LookupCode<TResult>(string sCompanyId,
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
            string apiCode)
        {
            LookupParamModel _param = new LookupParamModel();
            _param.company_id = sCompanyId;
            _param.username = sUsername;
            _param.used_data_code = sUsedDataCode;
            _param.used_table_name = sUsedTableName;
            _param.date = dtmDate;
            _param.data_code = sDataCode;
            _param.active_only = iActiveOnly;
            _param.liabilities_account_only = iLiabilitiesAccountOnly;
            _param.detail_account_only = iDetailAccountOnly;
            _param.parent_only = iParentOnly;
            _param.itself_only = iItselfOnly;
            _param.company_menu_id = sCompanyMenuId;
            _param.parent = sParent;

            return await this.LookupCode<TResult>(_param, apiCode);
        }

        public async Task<List<TResult>> LookupCode<TResult>(string sCompanyId,
            DateTime dtmDate,
            string sDataCode,
            Int16 iActiveOnly,
            Int16 iLiabilitiesAccountOnly,
            Int16 iDetailAccountOnly,
            Int16 iItselfOnly,
            string sCompanyMenuId,
            string apiCode)
        {
            return await this.LookupCode<TResult>(sCompanyId,
                "",
                "",
                "",
                dtmDate,
                sDataCode,
                iActiveOnly,
                "",
                iLiabilitiesAccountOnly,
                iDetailAccountOnly,
                0,
                iItselfOnly,
                sCompanyMenuId,
                apiCode);
        }

        public async Task<List<TResult>> LookupCode<TResult>(string sCompanyId,
            Int16 iActiveOnly,
            Int16 iItselfOnly,
            string sCompanyMenuId,
            string apiCode)
        {
            return await this.LookupCode<TResult>(sCompanyId,
                DateTime.Now,
                "",
                iActiveOnly,
                0,
                0,
                iItselfOnly,
                sCompanyMenuId,
                apiCode);
        }

        public async Task<List<TResult>> LookupCode<TResult>(string sCompanyId,
            Int16 iActiveOnly,
            string sCompanyMenuId,
            string apiCode)
        {
            return await this.LookupCode<TResult>(sCompanyId,
                iActiveOnly,
                0,
                sCompanyMenuId,
                apiCode);
        }

        public async Task<List<TResult>> LookupCode<TResult>(string sCompanyId,
            Int16 iActiveOnly,
            string sParent,
            Int16 iItselfOnly,
            string sCompanyMenuId,
            string apiCode)
        {
            return await this.LookupCode<TResult>(sCompanyId,
                "",
                "",
                "",
                DateTime.Now,
                "",
                iActiveOnly,
                sParent,
                0,
                0,
                0,
                iItselfOnly,
                sCompanyMenuId,
                apiCode);
        }

        public async Task<List<TResult>> LookupCode<TResult>(string sCompanyId, string sCompanyMenuId, string apiCode)
        {
            return await this.LookupCode<TResult>(sCompanyId, 0, sCompanyMenuId, apiCode);
        }

        public async Task<List<TResult>> LookupAccountCode<TResult>(string sCompanyId,
            DateTime dtmDate,
            Int16 iActiveOnly,
            Int16 iLiabilitiesAccountOnly,
            Int16 iDetailAccountOnly,
            Int16 iItselfOnly,
            string sCompanyMenuId,
            string apiCode)
        {
            return await this.LookupCode<TResult>(sCompanyId,
                dtmDate,
                "",
                iActiveOnly,
                iLiabilitiesAccountOnly,
                iDetailAccountOnly,
                iItselfOnly,
                sCompanyMenuId,
                apiCode);
        }

        public async Task<List<TResult>> LookupAccountCode<TResult>(string sCompanyId,
            DateTime dtmDate,
            Int16 iActiveOnly,
            Int16 iLiabilitiesAccountOnly,
            Int16 iDetailAccountOnly,
            string sCompanyMenuId,
            string apiCode)
        {
            return await this.LookupAccountCode<TResult>(sCompanyId,
                dtmDate,
                iActiveOnly,
                iLiabilitiesAccountOnly,
                iDetailAccountOnly,
                0,
                sCompanyMenuId,
                apiCode);
        }

        public async Task<List<TResult>> LookupAccountCode<TResult>(string sCompanyId,
            Int16 iActiveOnly,
            Int16 iLiabilitiesAccountOnly,
            Int16 iDetailAccountOnly,
            Int16 iItselfOnly,
            string sCompanyMenuId,
            string apiCode)
        {
            return await this.LookupAccountCode<TResult>(sCompanyId,
                DateTime.Now,
                iActiveOnly,
                iLiabilitiesAccountOnly,
                iDetailAccountOnly,
                iItselfOnly,
                sCompanyMenuId,
                apiCode);
        }

        public async Task<List<TResult>> LookupAccountCode<TResult>(string sCompanyId,
            Int16 iActiveOnly,
            Int16 iLiabilitiesAccountOnly,
            Int16 iDetailAccountOnly,
            string sCompanyMenuId,
            string apiCode)
        {
            return await this.LookupAccountCode<TResult>(sCompanyId,
                DateTime.Now,
                iActiveOnly,
                iLiabilitiesAccountOnly,
                iDetailAccountOnly,
                0,
                sCompanyMenuId,
                apiCode);
        }

        //private async Task<List<dynamic>> LookupCode(LookupParamModel param, string apiCode)
        //{
        //    APIResultDataModel _resultData = await this.GetData(param, apiCode);

        //    if (_resultData == null)
        //    {
        //        return new List<dynamic>();
        //    }
        //    else
        //    {
        //        return JsonConvert.DeserializeObject<List<dynamic>>(_resultData.data_details.ToString());
        //    }
        //}

        //public async Task<List<dynamic>> LookupCode(string sCompanyId,
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
        //    string apiCode)
        //{
        //    LookupParamModel _param = new LookupParamModel();
        //    _param.company_id = sCompanyId;
        //    _param.username = sUsername;
        //    _param.used_data_code = sUsedDataCode;
        //    _param.used_table_name = sUsedTableName;
        //    _param.date = dtmDate;
        //    _param.data_code = sDataCode;
        //    _param.active_only = iActiveOnly;
        //    _param.liabilities_account_only = iLiabilitiesAccountOnly;
        //    _param.detail_account_only= iDetailAccountOnly;
        //    _param.parent_only = iParentOnly;
        //    _param.itself_only  = iItselfOnly;
        //    _param.company_menu_id = sCompanyMenuId;

        //    return await this.LookupCode(_param, apiCode);
        //}

        //public async Task<List<dynamic>> LookupCode(string sCompanyId,
        //    DateTime dtmDate,
        //    string sDataCode,
        //    Int16 iActiveOnly,
        //    Int16 iLiabilitiesAccountOnly,
        //    Int16 iDetailAccountOnly,
        //    Int16 iItselfOnly,
        //    string sCompanyMenuId,
        //    string apiCode)
        //{
        //    return await this.LookupCode(sCompanyId,
        //        "",
        //        "",
        //        "",
        //        dtmDate,
        //        sDataCode,
        //        iActiveOnly,
        //        iLiabilitiesAccountOnly,
        //        iDetailAccountOnly,
        //        0,
        //        iItselfOnly,
        //        sCompanyMenuId,
        //        apiCode);
        //}

        //public async Task<List<dynamic>> LookupCode(string sCompanyId,
        //    Int16 iActiveOnly,
        //    Int16 iItselfOnly,
        //    string sCompanyMenuId,
        //    string apiCode)
        //{
        //    return await this.LookupCode(sCompanyId,
        //        DateTime.Now,
        //        "",
        //        iActiveOnly,
        //        0,
        //        0,
        //        iItselfOnly,
        //        sCompanyMenuId,
        //        apiCode);
        //}

        //public async Task<List<dynamic>> LookupCode(string sCompanyId,
        //    Int16 iActiveOnly,
        //    string sCompanyMenuId,
        //    string apiCode)
        //{
        //    return await this.LookupCode(sCompanyId,
        //        iActiveOnly,
        //        0,
        //        sCompanyMenuId,
        //        apiCode);
        //}

        //public async Task<List<dynamic>> LookupCode(string sCompanyId, string sCompanyMenuId, string apiCode)
        //{
        //    return await this.LookupCode(sCompanyId, 0, sCompanyMenuId, apiCode);
        //}

        //public async Task<List<dynamic>> LookupAccountCode(string sCompanyId,
        //    DateTime dtmDate,
        //    Int16 iActiveOnly,
        //    Int16 iLiabilitiesAccountOnly,
        //    Int16 iDetailAccountOnly,
        //    Int16 iItselfOnly,
        //    string sCompanyMenuId,
        //    string apiCode)
        //{
        //    return await this.LookupCode(sCompanyId,
        //        dtmDate,
        //        "",
        //        iActiveOnly,
        //        iLiabilitiesAccountOnly,
        //        iDetailAccountOnly,
        //        iItselfOnly,
        //        sCompanyMenuId,
        //        apiCode);
        //}

        //public async Task<List<dynamic>> LookupAccountCode(string sCompanyId,
        //    DateTime dtmDate,
        //    Int16 iActiveOnly,
        //    Int16 iLiabilitiesAccountOnly,
        //    Int16 iDetailAccountOnly,
        //    string sCompanyMenuId,
        //    string apiCode)
        //{
        //    return await this.LookupAccountCode(sCompanyId,
        //        dtmDate,
        //        iActiveOnly,
        //        iLiabilitiesAccountOnly,
        //        iDetailAccountOnly,
        //        0,
        //        sCompanyMenuId,
        //        apiCode);
        //}

        //public async Task<List<dynamic>> LookupAccountCode(string sCompanyId,
        //    Int16 iActiveOnly,
        //    Int16 iLiabilitiesAccountOnly,
        //    Int16 iDetailAccountOnly,
        //    Int16 iItselfOnly,
        //    string sCompanyMenuId,
        //    string apiCode)
        //{
        //    return await this.LookupAccountCode(sCompanyId,
        //        DateTime.Now,
        //        iActiveOnly,
        //        iLiabilitiesAccountOnly,
        //        iDetailAccountOnly,
        //        iItselfOnly,
        //        sCompanyMenuId,
        //        apiCode);
        //}

        //public async Task<List<dynamic>> LookupAccountCode(string sCompanyId,
        //    Int16 iActiveOnly,
        //    Int16 iLiabilitiesAccountOnly,
        //    Int16 iDetailAccountOnly,
        //    string sCompanyMenuId,
        //    string apiCode)
        //{
        //    return await this.LookupAccountCode(sCompanyId,
        //        DateTime.Now,
        //        iActiveOnly,
        //        iLiabilitiesAccountOnly,
        //        iDetailAccountOnly,
        //        0,
        //        sCompanyMenuId,
        //        apiCode);
        //}

        #endregion Lookup

        #region Menus
        public async Task<List<MenuItem>> GetMenus()
        {
            return await this.GetMenus(false);
        }

        public async Task<List<MenuItem>> GetMenus(bool getMainChart)
        {
            string _sCompanyId = await _localStorageService.GetItemAsync<string?>(LocalStorageName.COMPANY_ID);

            if (String.IsNullOrEmpty(_sCompanyId))
            {
                return new List<MenuItem>();
            }

            String _sApiCode = "GET_WEB_MENU_BY_USER";

            dynamic dData = new ExpandoObject();
            dData.company_id = _sCompanyId;
            dData.main_chart = (getMainChart ? 1 : 0);

            APIResultModel _result = await this.CallMyApi(dData, _sApiCode);

            if (_result.Status != 0)
            {
                return new List<MenuItem>();
            }

            APIResultDataModel _resultData = JsonConvert.DeserializeObject<APIResultDataModel>(_result.Data.ToString(), jsonSerializerSettings);

            if (_resultData?.data_details != null)
            {
                List<MenuItem> _lstResult = JsonConvert.DeserializeObject<List<MenuItem>>(_resultData.data_details.ToString(), jsonSerializerSettings);

                if(getMainChart)
                {
                    return _lstResult;
                }

                return await this.CreateMyMenus(_lstResult);
            }
            else
            {
                return new List<MenuItem>();
            }
        }

        public async Task<List<MenuItem>> GetMenus(string lang)
        {
            return await this.GetMenus(false, lang);
        }

        public async Task<List<MenuItem>> GetMenus(bool getMainChart, string lang)
        {
            string _sCompanyId = await _localStorageService.GetItemAsync<string?>(LocalStorageName.COMPANY_ID);

            if (String.IsNullOrEmpty(_sCompanyId))
            {
                return new List<MenuItem>();
            }

            String _sApiCode = "GET_WEB_MENU_BY_USER";

            dynamic dData = new ExpandoObject();
            dData.company_id = _sCompanyId;
            dData.main_chart = (getMainChart ? 1 : 0);

            APIResultModel _result = await this.CallMyApi(dData, _sApiCode);

            if (_result.Status != 0)
            {
                return new List<MenuItem>();
            }

            APIResultDataModel _resultData = JsonConvert.DeserializeObject<APIResultDataModel>(_result.Data.ToString(), jsonSerializerSettings);

            if (_resultData.data_details != null)
            {
                List<MenuItem> _lstResult = JsonConvert.DeserializeObject<List<MenuItem>>(_resultData.data_details.ToString(), jsonSerializerSettings);

                if (getMainChart)
                {
                    return _lstResult;
                }

                return await this.CreateMyMenus(_lstResult, lang);
            }
            else
            {
                return new List<MenuItem>();
            }
        }

        public async Task<MenuItem> GetMenuSingle(string companyMenuId)
        {
            string _sCompanyId = await _localStorageService.GetItemAsync<string?>(LocalStorageName.COMPANY_ID);

            if (String.IsNullOrEmpty(_sCompanyId))
            {
                return new MenuItem();
            }

            return await GetMenuSingle(_sCompanyId, companyMenuId);

            //String _sApiCode = "GET_WEB_MENU_SINGLE";

            //dynamic dData = new ExpandoObject();
            //dData.company_id = _sCompanyId;
            //dData.company_menu_id = companyMenuId;

            //APIResultModel _result = await this.CallMyApiSimple(dData, _sApiCode);

            //if (_result.Status != 0)
            //{
            //    return new MenuItem();
            //}

            //MenuItem menuItem = JsonConvert.DeserializeObject<MenuItem>(_result.Data.ToString(), jsonSerializerSettings);

            //return menuItem;
        }

        public async Task<MenuItem> GetMenuSingle(string companyId, string companyMenuId)
        {
            String _sApiCode = "GET_WEB_MENU_SINGLE";

            dynamic dData = new ExpandoObject();
            dData.company_id = companyId;
            dData.company_menu_id = companyMenuId;

            APIResultModel _result = await this.CallMyApiSimple(dData, _sApiCode);

            if (_result.Status != 0)
            {
                return new MenuItem();
            }

            //APIResultDataModel _resultData = JsonSerializer.Deserialize<APIResultDataModel>(_result.Data.ToString());

            //MenuItem menuItem = JsonSerializer.Deserialize<MenuItem>(_result.Data.ToString(), jsonInsensitiveOptions);
            MenuItem menuItem = JsonConvert.DeserializeObject<MenuItem>(_result.Data.ToString(), jsonSerializerSettings);

            return menuItem;
        }

        private async Task<List<MenuItem>> CreateMyMenus(List<MenuItem> menuItems)
        {
            List<MenuItem> result = new List<MenuItem>();
            List<MenuItem> list = menuItems;
            List<MenuItem> roots = list.Where(x => x.ParentId == "").OrderBy(x => x.Order).ToList();

            foreach (MenuItem item in roots)
            {
                result.Add(item);
                await this.CreateMenuChildrent(item, list);
            }

            return result;
        }

        private async Task CreateMenuChildrent(MenuItem parent, List<MenuItem> menuItems)
        {
            parent.Children = menuItems.Where(x => x.ParentId == parent.CompanyMenuId).OrderBy(x => x.Order).ToList();

            foreach (MenuItem item in parent.Children)
            {
                await this.CreateMenuChildrent(item, menuItems);
            }
        }

        private async Task<List<MenuItem>> CreateMyMenus(List<MenuItem> menuItems, string lang)
        {
            List<MenuItem> result = new List<MenuItem>();
            List<MenuItem> list = menuItems;
            List<MenuItem> roots = list.Where(x => x.ParentId == "").OrderBy(x => x.Order).ToList();

            foreach (MenuItem item in roots)
            {
                item.Title = _localizer[item.Title];
                item.MenuName = _localizer[item.MenuName];

                result.Add(item);
                await this.CreateMenuChildrent(item, list, lang);
            }

            return result;
        }

        private async Task CreateMenuChildrent(MenuItem parent, List<MenuItem> menuItems, string lang)
        {
            parent.Children = menuItems.Where(x => x.ParentId == parent.CompanyMenuId).OrderBy(x => x.Order).ToList();

            foreach (MenuItem item in parent.Children)
            {
                item.Title = _localizer[item.Title];
                item.MenuName = _localizer[item.MenuName];
                await this.CreateMenuChildrent(item, menuItems);
            }
        }
        #endregion Menus

        public async Task<Model.APIResultModel> CheckToken(String token)
        {
            //var param = new APIParameterModel();
            //param.Data = "{" + String.Format("\"token_2_check\":\"{0}\"", token) + "}";
            //param.DataCode = "CHECK_TOKEN";
            //param.ConnectionName = _config.GetValue<string>("DbConnectionName");

            //var result = await _httpClient.PostAsJsonAsync("/api/bms2", param);

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            //var paramData = "{" + String.Format("\"token_2_check\":\"{0}\"", token) + "}";
            //var result = await this.CallMyApiSimple0(paramData, "CHECK_TOKEN");

            dynamic paramData = new ExpandoObject();
            paramData.token_2_check = token;
            var result = await this.CallMyApiSimple0(paramData, "CHECK_TOKEN");

            //System.IO.File.WriteAllText(@"D:\10.Downloads\__tmp.txt", await result.Content.ReadAsStringAsync());
                        
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var sResult = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Model.APIResultModel>(sResult);
                //return await result.Content.ReadFromJsonAsync<APIResultModel>();// Lỗi không chạy
            }
            else
            {
                return new APIResultModel(202, "", null);
            }
        }

        public async Task<List<PeriodItem>> PeriodLookup(String periodId)
        {
            string _sCompanyId = await _localStorageService.GetItemAsync<string?>(LocalStorageName.COMPANY_ID);
            String _sApiCode = "PERIOD_LOOKUP";
            //String _sData = "{\"company_id\":\"" + _sCompanyId + "\", \"period_id\":\"" + periodId + "\"}";

            //APIResultModel _result = await this.CallMyApi(_sData, _sApiCode);

            dynamic dData = new ExpandoObject();
            dData.company_id = _sCompanyId;
            dData.period_id = periodId;

            APIResultModel _result = await this.CallMyApiSimple(dData, _sApiCode);

            if (_result.Status != 0)
            {
                return new List<PeriodItem>();
            }

            APIResultDataModel _resultData = JsonConvert.DeserializeObject<APIResultDataModel>(_result.Data.ToString(), jsonSerializerSettings);

            return JsonConvert.DeserializeObject<List<PeriodItem>> (_resultData.data_details.ToString(), jsonSerializerSettings);
        }

        public async Task<List<LookupItem>> GetLookup(string sListLookupApiCode)
        {
            string _sCompanyId = await _localStorageService.GetItemAsync<string?>(LocalStorageName.COMPANY_ID);
            String _sApiCode = "API_FET_INFO";

            dynamic dData = new ExpandoObject();
            dData.company_id = _sCompanyId;
            dData.sp_code = sListLookupApiCode;

            APIResultModel _result = await this.CallMyApiSimple(dData, _sApiCode);

            if (_result.Status != 0)
            {
                return new List<LookupItem>();
            }

            APIResultDataModel _resultData = JsonConvert.DeserializeObject<APIResultDataModel>(_result.Data.ToString(), jsonSerializerSettings);

            return JsonConvert.DeserializeObject<List<LookupItem>>(_resultData.data_details.ToString(), jsonSerializerSettings);
        }

        public async Task<List<CompanyItem>> GetCompany4MenuFilter(string companyId, string companyMenuId)
        {
            String _sApiCode = "GET_COMPANY_4_MENU_FILTER_W";

            dynamic dData = new ExpandoObject();
            dData.company_id = companyId;
            dData.company_menu_id = companyMenuId;

            APIResultModel _result = await this.CallMyApiSimple(dData, _sApiCode);

            if (_result.Status != 0)
            {
                return new List<CompanyItem>();
            }

            APIResultDataModel _resultData = JsonConvert.DeserializeObject<APIResultDataModel>(_result.Data.ToString(), jsonSerializerSettings);

            return JsonConvert.DeserializeObject<List<CompanyItem>>(_resultData.data_details.ToString(), jsonSerializerSettings);
        }

        public async Task<String> CreateDataCode(string companyId, string dataCode, string prevCode)
        {
            dynamic dData = new ExpandoObject();
            dData.company_id = companyId; 
            dData.prev_code = prevCode;
            dData.data_code = dataCode;

            APIResultModel _result = await this.CallMyApiSimple(dData, "CREATE_DATA_CODE_W");

            if(_result.Status == 0)
            {
                return _result.Data.ToString();
            }
            else
            {
                return "";
            }

        }
        public async Task<String> CreateDataCodeByDate(string companyId, string dataCode, string prevCode, DateTime date, string dateColumn)
        {
            dynamic dData = new ExpandoObject();
            dData.company_id = companyId;
            dData.prev_code = prevCode;
            dData.date = date;
            dData.dateColumn = dateColumn;
            dData.data_code = dataCode;

            APIResultModel _result = await this.CallMyApiSimple(dData, "CREATE_DATA_CODE_BY_DATE_W");

            if (_result.Status == 0)
            {
                return _result.Data.ToString();
            }
            else
            {
                return "";
            }
        }

        public async Task<String> CreateVoucherNo(string companyId, string dataCode, DateTime date)
        {
            dynamic dData = new ExpandoObject();
            dData.company_id = companyId;
            dData.date = date.ToString("yyyy-MM-dd HH:mm:ss"); // Đối với thằng dynamic này thì ngày tháng phải làm thế này kẻo nó gen ra dạng vào sql không xử lý được
            dData.data_code = dataCode;

            APIResultModel _result = await this.CallMyApiSimple(dData, "CREATE_VOUCHER_NO_W");

            if (_result.Status == 0)
            {
                return _result.Data.ToString();
            }
            else
            {
                return "";
            }
        }

        public async Task<decimal> GetCurrencyRate(string companyId, string currencyCode, DateTime date)
        {
            dynamic dData = new ExpandoObject();
            dData.company_id = companyId;
            dData.date = date.ToString("yyyy-MM-dd HH:mm:ss");
            dData.currency_code = currencyCode;

            APIResultModel _result = await this.CallMyApiSimple(dData, "GET_CURRENCY_RATE");

            if (_result.Status == 0)
            {
                CurrencyRateLookupModel currencyRate = Newtonsoft.Json.JsonConvert.DeserializeObject< CurrencyRateLookupModel>(_result.Data.ToString());
                if (currencyRate != null)
                {
                    return currencyRate.currency_rate;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }

        public async Task<string> GetRowId(string companyId, string companyMenuId)
        {
            dynamic dData = new ExpandoObject();
            dData.company_id = companyId;
            dData.company_menu_id = companyMenuId;

            APIResultModel _result = await this.CallMyApiSimple(dData, "GET_ROW_ID");

            if (_result.Status == 0)
            {
                RowIdModel row = Newtonsoft.Json.JsonConvert.DeserializeObject<RowIdModel>(_result.Data.ToString());
                if (row != null)
                {
                    return row.row_id;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public async Task<Byte[]> GetReportFileContent(string companyId, string reportFileId)
        {
            dynamic _param = new ExpandoObject();
            _param.report_file_id = reportFileId;
            _param.company_id = companyId;

            APIResultModel resultModel = await this.CallMyApiSimple(_param, "WEB_REPORT_FILE_FET_CONTENT");

            if (resultModel != null)
            {
                if (resultModel.Status == 0)
                {
                    if (resultModel.Data == null)
                    {
                        using var ms = new MemoryStream();
                        using XtraReport report = new XtraReport();
                        report.SaveLayoutToXml(ms);
                        return ms.ToArray();
                    }
                    else
                    {
                        byte[] _byteArray = CM.StringToByteArray(resultModel.Data.ToString());

                        return _byteArray;
                    }
                }
            }

            return new byte[0];
        }
        public async Task SaveReportFileContent(string companyId, string reportFileId, XtraReport report)
        {
            using var stream = new MemoryStream();
            report.SaveLayoutToXml(stream);
            string sContent = CM.StreamToString(stream);

            dynamic _param = new ExpandoObject();
            _param.report_file_id = reportFileId;
            _param.file_contents = sContent;

            APIResultModel resultModel = await this.CallMyApi(_param, "WEB_REPORT_FILE_UPD_CONTENT");
        }

        public async Task<XtraReport> CreateReportFile(string companyId, string reportFileId)
        {
            byte[] bytes = await this.GetReportFileContent(companyId, reportFileId);
            Stream stream = CM.ByteArrayToStream(bytes);
            XtraReport _report = new XtraReport();
            _report.LoadLayout(stream);

            return _report;
        }

        public async Task<object> GetStringData4Report(ReportViewParamItem param)
        {
            Dictionary<string, object> dicParam = param.param;
            dynamic _param = new ExpandoObject();

            foreach (string _sParamName in dicParam.Keys)
            {
                MyLib.AddProperty(_param, _sParamName, dicParam[_sParamName]);
            }

            if(dicParam.ContainsKey("company_id") == false & string.IsNullOrEmpty(param.company_id))
            {
                // Nếu tham số chưa có company_id thì tự động add vào
                MyLib.AddProperty(_param, "company_id", param.company_id);
            }

            //var menuItem = await this.GetMenuSingle(param.company_id, param.company_menu_id);
            //var reportFileItem = menuItem.ReportFiles.FirstOrDefault(x => x.report_file_id == param.report_file_id);

            //string _sSpName = string.IsNullOrEmpty(reportFileItem.sp_name) ? menuItem.FetSingleDataCode : reportFileItem.sp_name;
            string _sSpName = param.api_code;

            APIResultModel _result = await this.CallMyApiSimple(_param, _sSpName);

            if(_result.Status == 0)
            {
                var jsonDataSource = new JsonDataSource();
                // Specify the object that retrieves JSON data.
                jsonDataSource.JsonSource = new CustomJsonSource(_result.Data.ToString());
                jsonDataSource.Fill();

                return jsonDataSource;
            }

            return null;
        }

        public async Task<List<DevExpress.XtraReports.Parameters.Parameter>> GetReportParameters(string companyId)
        {
            dynamic _param = new ExpandoObject();
            _param.company_id = companyId;

            APIResultModel _result = await this.CallMyApiSimple(_param, "REPORT_SYSTEM_PARAM_FET");

            if (_result.Status == 0)
            {
                APIResultDataModel _data = JsonConvert.DeserializeObject<APIResultDataModel>(_result.Data.ToString());
                return JsonConvert.DeserializeObject<List<DevExpress.XtraReports.Parameters.Parameter>>(_data.data_details.ToString());
            }

            return new List<DevExpress.XtraReports.Parameters.Parameter>();
        }

        public async Task SetReportDataSource(XtraReport report, ReportViewParamItem param)
        {
            ReportViewParamItem _param = param;

            List<DevExpress.XtraReports.Parameters.Parameter> _parameters = await this.GetReportParameters(_param.company_id);

            if (_param.view_param != null)
            {
                foreach (string _sKey in _param.view_param.Keys)
                {
                    if (_parameters.FirstOrDefault(x => x.Name == _sKey) == null)
                    {
                        _parameters.Add(new DevExpress.XtraReports.Parameters.Parameter()
                        {
                            Name = _sKey,
                            Value = _param.view_param[_sKey]
                        });
                    }
                }
            }

            report.DataSource = await this.GetStringData4Report(_param);

            foreach (DevExpress.XtraReports.Parameters.Parameter _p in _parameters)
            {
                if (report.Parameters[_p.Name] != null)
                {
                    report.Parameters[_p.Name].Value = _p.Value;
                }
                else
                {
                    report.Parameters.Add(_p);
                }
            }
        }

        public async Task<XtraReport> CreateReport(ReportViewParamItem param)
        {
            ReportViewParamItem _param = param;
            string path = Path.Combine(ReportDirectory, _param.report_file_id + FileExtension);
            XtraReport report = null;

            if (Directory.EnumerateFiles(ReportDirectory).
                    Select(Path.GetFileNameWithoutExtension).
                    Contains(_param.report_file_id))
            {
                report = XtraReport.FromXmlFile(path);
            }
            else
            {
                report = new XtraReport();
            }

            await this.SetReportDataSource(report, _param);

            report.RequestParameters = false;

            return report;
        }
    }
}
