using Blazored.LocalStorage;
using DevExpress.DirectX.Common.Direct2D;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
//using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
using MyWebERP.Data;
using MyWebERP.Model;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;

namespace MyWebERP.Authentication
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;// { get; set; }
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        //private readonly Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage.ProtectedSessionStorage _protectedSessionStore;

        public LoginService(IConfiguration config, 
            HttpClient httpClient, 
            AuthenticationStateProvider authenticationStateProvider, 
            ILocalStorageService localStorageService
            )
        {
            _config = config;
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorageService = localStorageService;
        }

        public async Task<LoginResultModel> Login(string username, string password, LanguageModel lang, int year)
        {
            String _sCnnName = _config.GetValue<string>("DbConnectionName");
            LoginParamModel _loginParam = new LoginParamModel(_sCnnName, username, password);
            _loginParam.RootCompanyId = _config.GetValue<String>("DefaultCompanyId");
            _loginParam.CnnFromConfigFile = _config.GetValue<Int16>("ApiCnnFromConfigFile");

            var result = await _httpClient.PostAsJsonAsync("/api/weblogin", _loginParam);

            var sXXX = await result.Content.ReadAsStringAsync();

            var resultModel = await result.Content.ReadFromJsonAsync<APIResultModel>();

            if (resultModel.Status!= 0)
            {
                throw new Exception(resultModel.StatusDetails);
            }

            //var loginResult = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResultModel>(resultModel.Data.ToString());
            var loginResult = JsonSerializer.Deserialize<LoginResultModel>(resultModel.Data.ToString(),
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

            await _localStorageService.SetItemAsync<LoginResultModel>(LocalStorageName.USER, loginResult);
            await _localStorageService.SetItemAsync<string>(LocalStorageName.TOKEN, loginResult.access_token.access_token);
            await _localStorageService.SetItemAsync<LanguageModel>(LocalStorageName.LANGUAGE, lang);
            await _localStorageService.SetItemAsync<int>(LocalStorageName.YEAR, year);

            //var culture = new CultureInfo(lang.language_code);
            //Thread.CurrentThread.CurrentCulture = culture;
            //Thread.CurrentThread.CurrentUICulture = culture;

            ((MyAuthenticationStateProvider)_authenticationStateProvider).AuthenticateUser(loginResult.display_name);
            // Lưu ý để chữ B hoa để trên api xử lý đưa vào sp
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResult.access_token.access_token);

            return loginResult;
        }

        public async Task Logout()
        {
            // Xóa thông tin cũ
            await MyLib.ClearAllLocalStorage(_localStorageService);

            ((MyAuthenticationStateProvider)_authenticationStateProvider).UnauthenticateUser();

            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
