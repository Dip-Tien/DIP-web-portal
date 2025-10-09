using MyWebERP.Model;
using Blazored.LocalStorage;

namespace MyWebERP.Data
{
    public class AppStateManager
    {
        private ILocalStorageService LocalStorageService;
        //public string? Me { get; set; } = null;

        //public event Action OnChange;

        //public void UserState() => OnChange?.Invoke();

        public LoginResultModel? UserInfo { get; set; }
        public int? Year { get; set; }
        public Model.CompanyItem? Company { get; set; }
        public LanguageModel? Language { get; set; }

        public MenuItem CurrentMenuItem { get; set; }

        public string CurentPageTitle
        {
            get
            {
                string _sTitle = "My BMS";

                if(this.Company != null)
                {
                    _sTitle += $" - {this.Company.CompanyName}";
                }

                if (this.CurrentMenuItem != null)
                {
                    _sTitle += $" / {this.CurrentMenuItem.MenuName}";
                }                

                return _sTitle;
            }
        }

        public AppStateManager(ILocalStorageService localStorageService)
        {
            LocalStorageService = localStorageService;
        }

        public event Action OnUserChange;
        public event Action OnYearChange;
        public event Action OnCompanyChange;
        public event Action OnLanguageChange;
        public event Action OnCurrentMenuItemChange;

        public void UserState() => OnUserChange?.Invoke();
        //public void WorkingYearState() => OnWorkingYearChange?.Invoke();
        public async Task YearState()
        {
            Year = await LocalStorageService.GetItemAsync<int?>(Data.LocalStorageName.YEAR);
            OnYearChange?.Invoke();
        }

        //public void CompanyState() => OnCompanyChange?.Invoke();
        public async Task CompanyState()
        {
            Company = await LocalStorageService.GetItemAsync<Model.CompanyItem>(Data.LocalStorageName.COMPANY);
            OnCompanyChange?.Invoke();
        }

        public void LanguageState() => OnLanguageChange?.Invoke();

        public async Task CurrentMenuItemChange()
        {
            OnCurrentMenuItemChange?.Invoke();
        }
    }
}
