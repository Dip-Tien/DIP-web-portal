using Microsoft.AspNetCore.Components;
using MyWebERP.Model;
using MyWebERP.Services;

namespace MyWebERP.Base.Components.BaseClasses
{
    public class LookupBase<TValue> : ComponentBase
    {
        [Inject]
        protected IDataService DataService { get; set; } = default!;

        [Parameter] public string CompanyId { get; set; }
        [Parameter] public string CompanyMenuId { get; set; }
        [Parameter] public Int16 ActiveOnly { get; set; }
        [Parameter] public string Parent { get; set; }
        [Parameter] public string? CssClass { get; set; }

        [Parameter] public TValue? Value { get; set; }
        [Parameter] public EventCallback<TValue?> ValueChanged { get; set; }

        protected string InputId = $"input_{Guid.NewGuid():N}";
        protected string DropdownId = $"dropdown_{Guid.NewGuid():N}";

        private bool _isPopupVisible;
        protected bool IsPopupVisible
        {
            get => _isPopupVisible;
            set
            {
                if (_isPopupVisible != value)
                {
                    _isPopupVisible = value;

                    if (value)
                    {
                        ShouldLoadData = true; // Đánh dấu cần load
                    }
                }
            }
        }

        protected bool ShouldLoadData;

        protected bool IsLoading;
        protected virtual string SearchText { get; set; } = string.Empty;

        protected virtual void OnSearchTextChanged(string? value)
        {
            SearchText = value ?? "";
        }

        protected virtual void OnClearClick()
        {
            SearchText = string.Empty;
            IsPopupVisible = false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (ShouldLoadData)
            {
                ShouldLoadData = false;
                await LoadDataAsync(); // Gọi load sau khi popup đã render xong

            }
        }

        protected virtual async Task LoadDataAsync()
        {
        }
    }
}
