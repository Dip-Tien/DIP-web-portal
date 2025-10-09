using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MyWebERP.Data;
using MyWebERP.Model;
using MyWebERP.Services;
using System.Net.NetworkInformation;
using System.Security.Claims;

namespace MyWebERP.Authentication
{
    //public class MyAuthenticationService
    //{
    //    public event Action<ClaimsPrincipal>? UserChanged;
    //    private ClaimsPrincipal? currentUser;

    //    public ClaimsPrincipal CurrentUser
    //    {
    //        get { return currentUser ?? new(); }
    //        set
    //        {
    //            currentUser = value;

    //            if (UserChanged is not null)
    //            {
    //                UserChanged(currentUser);
    //            }
    //        }
    //    }
    //}
    //public class MyAuthStateProvider : AuthenticationStateProvider
    //{
    //    private AuthenticationState authenticationState;

    //    public MyAuthStateProvider(MyAuthenticationService service)
    //    {
    //        authenticationState = new AuthenticationState(service.CurrentUser);

    //        service.UserChanged += (newUser) =>
    //        {
    //            authenticationState = new AuthenticationState(newUser);
    //            NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
    //        };
    //    }

    //    public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
    //        Task.FromResult(authenticationState);
    //}

    public class MyAuthenticationStateProvider : AuthenticationStateProvider
    {
        
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly IDataService _dataService;
        private readonly AppStateManager _appState;

        public MyAuthenticationStateProvider(IDataService dataService,
            HttpClient httpClient,
            ILocalStorageService localStorageService,
            AppStateManager appState)
        {
            _dataService = dataService;
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _appState = appState;
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var _loginInfo = await _localStorageService.GetItemAsync<LoginResultModel>(LocalStorageName.USER);

            var _company = await _localStorageService.GetItemAsync<CompanyItem>(LocalStorageName.COMPANY);

            if (_loginInfo == null || _loginInfo.access_token == null)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            else
            {
                APIResultModel _checkToken = await _dataService.CheckToken(_loginInfo.access_token.access_token);//.CallMyApi(String.Format("\"token_2_check\":\"{0}\"", _loginInfo.access_token.access_token), "CHECK_TOKEN");

                if (_checkToken == null || _checkToken.Status != 0)
                {
                    await MyLib.ClearAllLocalStorage(_localStorageService);
                    this.UnauthenticateUser();
                    _httpClient.DefaultRequestHeaders.Authorization = null;

                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }
                else
                {
                    //var identity = new ClaimsIdentity("Admin", "Admin", "Admin");
                    var identity = new ClaimsIdentity(
                    [
                        new Claim(ClaimTypes.Name, _loginInfo.display_name),
                    ], "Custom Authentication");

                    _appState.Company = _company;

                    var user = new ClaimsPrincipal(identity);

                    return new AuthenticationState(user);
                }

                //var identity = new ClaimsIdentity(
                //    [
                //        new Claim(ClaimTypes.Name, _loginInfo.display_name),
                //    ], "Custom Authentication");
                //var user = new ClaimsPrincipal(identity);

                //return new AuthenticationState(user);
            }
        }

        public void AuthenticateUser(string userIdentifier)
        {
            var identity = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, userIdentifier),
            ], "Custom Authentication");

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void UnauthenticateUser()
        {
            var auth = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            NotifyAuthenticationStateChanged(Task.FromResult(auth));
        }
    }
}
