using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyWebERP.Data;


namespace MyWebERP.Base.Components.Lookup
{
    public partial class MyPopupComboBox<TItem, TValue> : ComponentBase
    {
        [Parameter] public RenderFragment? ExtraButtons { get; set; }
        [Parameter] public RenderFragment? FooterTemplate { get; set; }
        [Parameter] public List<TItem>? Data { get; set; }
        [Parameter] public string? ValueFieldName { get; set; }
        [Parameter] public string? TextFieldName { get; set; }
        [Parameter] public bool ReadOnly { get; set; } = false;
        [Parameter] public TValue? Value { get; set; }
        [Parameter] public EventCallback<TValue?> ValueChanged { get; set; }
        [Parameter] public Func<TItem, string>? DisplayText { get; set; }
        [Parameter] public Func<TItem, string, bool>? CustomFilter { get; set; }
        [Parameter] public string NullText { get; set; } = "Chọn...";
        [Parameter] public RenderFragment<TItem>? ItemTemplate { get; set; }
        [Parameter] public string EmptyText { get; set; } = "Không tìm thấy dữ liệu";
        [Parameter] public string CssClass { get; set; } = "";

        [Parameter] public Microsoft.Extensions.Localization.IStringLocalizer Language { get; set; }
        [Parameter] public Blazored.LocalStorage.ILocalStorageService LocalStorageService { get; set; } = default!;
        [Inject] IJSRuntime JS { get; set; } = default!;
        [Parameter]
        public bool ShowByMyOwn{ get; set; } = true;
        [Parameter]
        public EventCallback<bool> ShowByMyOwnChanged { get; set; }

        protected bool IsPopupVisible = false;
        private string InputId = $"input_{Guid.NewGuid():N}";
        // Dùng để scroll đến item đã chọn khi mở popup
        ElementReference _scrollContainer;
        private string ScrollContainerId = $"list_{Guid.NewGuid():N}";
        private ElementReference _popupContainer;

        public TItem? SelectedItem { get; private set; }

        [Parameter] public EventCallback<TItem> OnSelectedItem { get; set; }

        bool SkipFirstFilter = false;
        string _searchText = string.Empty;

        string CurrentText => IsPopupVisible ? SearchText : (SelectedItem == null ? string.Empty : GetDisplayText(SelectedItem));

        protected string SearchText
        {
            get => IsPopupVisible ? _searchText : (SelectedItem == null ? string.Empty : GetDisplayText(SelectedItem));
            set
            {
                if (SkipFirstFilter && value == (SelectedItem == null ? string.Empty : GetDisplayText(SelectedItem)))
                    return;

                _searchText = value;
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            if (Data != null && Value != null)
            {
                SelectedItem = Data.FirstOrDefault(i => Equals(GetValue(i), Value));
                if (SelectedItem != null)
                {
                    _searchText = GetDisplayText(SelectedItem);
                    StateHasChanged();
                }
            }
        }

        public virtual async Task TogglePopup()
        {
            IsPopupVisible = !IsPopupVisible;

            if (IsPopupVisible)
            {
                // 💾 Lưu scroll cha trước khi hiện popup
                await JS.InvokeVoidAsync("MyScrollHelper.saveParentScroll", _popupContainer);

                ShowByMyOwn = true;
                await ShowByMyOwnChanged.InvokeAsync(ShowByMyOwn); // 👈 Thêm dòng này
                _searchText = string.Empty;// cứ mở thì clear (SelectedItem == null ? string.Empty : GetDisplayText(SelectedItem));
                SkipFirstFilter = true;

                //await Task.Delay(50);
                //await JS.InvokeVoidAsync("DataContainer_ScrollToSelected", _scrollContainer);
                await MyLib.ScrollToRow(JS, ScrollContainerId);

            }
            else
            {
                SkipFirstFilter = false;
                // 🔁 Khôi phục scroll cha sau khi đóng popup
                await JS.InvokeVoidAsync("MyScrollHelper.restoreParentScroll", _popupContainer);

            }

            await InvokeAsync(StateHasChanged);
        }

        public async Task ShowPopupByParent()
        {
            if (!IsPopupVisible)
            {
                IsPopupVisible = true;
                ShowByMyOwn = false;
                await ShowByMyOwnChanged.InvokeAsync(ShowByMyOwn); // 👈 Thêm dòng này
                _searchText = string.Empty;// SelectedItem == null ? string.Empty : GetDisplayText(SelectedItem);
                SkipFirstFilter = true;
                await InvokeAsync(StateHasChanged);
            }
        }

        private void OnSearchTextChanged(string value)
        {
            if (!IsPopupVisible)
                IsPopupVisible = true;

            SkipFirstFilter = false;
            _searchText = value;
        }

        private IEnumerable<TItem> FilteredList =>
            (string.IsNullOrWhiteSpace(SearchText) || SkipFirstFilter)
                ? Data
                : Data?.Where(x =>
                    (CustomFilter != null && CustomFilter(x, SearchText))
                    || (DisplayText != null && DisplayText(x).Contains(SearchText, StringComparison.OrdinalIgnoreCase)));

        protected async Task SelectItem(TItem item)
        {
            SelectedItem = item;
            SearchText = GetDisplayText(item);
            IsPopupVisible = false;

            var val = GetValue(item);
            Value = val;
            await ValueChanged.InvokeAsync(val);
            await OnSelectedItem.InvokeAsync(SelectedItem);
        }

        private TValue? GetValue(TItem item)
        {
            if (item == null || string.IsNullOrEmpty(ValueFieldName))
                return default;

            var prop = typeof(TItem).GetProperty(ValueFieldName);
            if (prop == null) return default;

            var val = prop.GetValue(item);
            return (TValue?)Convert.ChangeType(val, typeof(TValue));
        }

        private string GetDisplayText(TItem item)
        {
            if (item == null || string.IsNullOrEmpty(TextFieldName))
                return string.Empty;

            var prop = typeof(TItem).GetProperty(TextFieldName);
            var val = prop?.GetValue(item);
            return val?.ToString() ?? "";
        }

        private async Task OnClearClick()
        {
            _searchText = string.Empty;
            Value = default;
            SelectedItem = default;
            IsPopupVisible = false;
            await ValueChanged.InvokeAsync(Value);
            await OnSelectedItem.InvokeAsync(SelectedItem);
            StateHasChanged();
        }

        //protected virtual async Task OnBackClicked()
        //{
        //    IsPopupVisible = false; // Đóng popup
        //}

        [Parameter] public EventCallback OnBackClicked { get; set; }

        private async Task HandleBackClicked()
        {
            IsPopupVisible = false;

            if (OnBackClicked.HasDelegate)
                await OnBackClicked.InvokeAsync();
        }

    }

}
