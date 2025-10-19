using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyWebERP.Data;
using MyWebERP.Model;

namespace MyWebERP.Base.Components.Lookup
{
    public partial class MyComboBox<TItem, TValue> : ComponentBase
    {
        /// <summary>
        /// Không cần dùng đến, chỉ để Blazor không báo lỗi.
        /// DynamicComponent luôn cố gắng truyền thêm ChildContent (mặc định nếu đang nằm trong RenderFragment nào đó), 
        /// hoặc Blazor engine kỳ vọng ChildContent tồn tại trong component được render.
        /// → Kết quả: nếu component(ví dụ MyComboBox) không có[Parameter] public RenderFragment ChildContent { get; set; }, 
        /// thì lỗi “does not have a property matching the name 'ChildContent'” sẽ xảy ra.
        /// </summary>
        [Parameter] public RenderFragment? ChildContent { get; set; }

        [Parameter] public RenderFragment? ExtraButtons { get; set; }
        [Parameter] public List<TItem>? Data { get; set; }
        [Parameter] public string? ValueFieldName { get; set; }
        [Parameter] public string? TextFieldName { get; set; }
        [Parameter] public bool ReadOnly { get; set; } = false;
        [Parameter] public TValue? Value { get; set; }
        [Parameter] public EventCallback<TValue?> ValueChanged { get; set; }
        [Parameter] public Func<TItem, string>? DisplayText { get; set; }
        [Parameter] public Func<TItem, string, bool>? CustomFilter { get; set; } // thêm SearchText để lọc theo nhiều trường
        [Parameter] public string NullText { get; set; } = "Chọn...";
        [Parameter] public RenderFragment<TItem>? ItemTemplate { get; set; }
        [Parameter] public string EmptyText { get; set; } = "Không tìm thấy dữ liệu";
        [Parameter] public string CssClass { get; set; } = "";

        [Parameter] public Microsoft.Extensions.Localization.IStringLocalizer Language{ get; set; }
        [Parameter] public Blazored.LocalStorage.ILocalStorageService LocalStorageService { get; set; } = default!;

        [Inject] IJSRuntime JS { get; set; } = default!;
        private bool IsDropdownVisible = false;
        private string DropDownWidth { get; set; } = "350px";
        private string InputId = $"input_{Guid.NewGuid():N}";
        private string DropdownId = $"dropdown_{Guid.NewGuid():N}";

        // Dùng để scroll đến item đã chọn khi mở dropdown
        bool _prevDropdownVisible;
        ElementReference _scrollContainer;
        private string ScrollContainerId = $"list_{Guid.NewGuid():N}";

        //private TItem? SelectedItem;
        public TItem? SelectedItem { get; private set; }
        [Parameter] public EventCallback<TItem> OnSelectedItem { get; set; }

        bool SkipFirstFilter = false; // 👈 cờ kiểm soát
        string _searchText = string.Empty;

        string CurrentText => IsDropdownVisible ? SearchText : (SelectedItem == null ? string.Empty : GetDisplayText(SelectedItem));

        protected string SearchText
        {
            get => IsDropdownVisible ? _searchText : (SelectedItem == null ? string.Empty : GetDisplayText(SelectedItem));
            set
            {
                // Nếu vừa mở dropdown mà người dùng chưa gõ gì => bỏ qua filter
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

        private async Task ToggleDropdown()
        {
            IsDropdownVisible = !IsDropdownVisible;

            if (IsDropdownVisible)
            {
                // Lần đầu mở: hiển thị giá trị đã chọn
                _searchText = (SelectedItem == null ? string.Empty : GetDisplayText(SelectedItem));
                SkipFirstFilter = true; // 👈 set cờ để không filter ngay
            }
            else
            {
                SkipFirstFilter = false;
            }
        }

        private void OnSearchTextChanged(string value)
        {

            if (IsDropdownVisible == false)
            {
                IsDropdownVisible = true;
            }

            SkipFirstFilter = false; // 👈 tắt cờ, từ đây trở đi filter hoạt động bình thường
            _searchText = value;

        }

        private IEnumerable<TItem> FilteredList =>
        (string.IsNullOrWhiteSpace(SearchText) | SkipFirstFilter)
            ? Data
            : Data?.Where(x =>
                (CustomFilter != null && CustomFilter(x, SearchText))
                || (DisplayText != null && DisplayText(x)
                    .Contains(SearchText, StringComparison.OrdinalIgnoreCase)));


        protected async Task SelectItem(TItem item)
        {
            SelectedItem = item;
            SearchText = GetDisplayText(item);
            IsDropdownVisible = false;

            var val = GetValue(item);
            Value = val;
            await ValueChanged.InvokeAsync(val);
            await OnSelectedItem.InvokeAsync(SelectedItem);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // tự động lấy độ rộng của TextBox
                var width = await JS.InvokeAsync<double>("getElementWidthBySelector", $"#{InputId}");
                //DropDownWidth = $"{width}px";
                DropDownWidth = $"{(int)Math.Round(width)}px"; // 👈 làm tròn và ép int luôn
                StateHasChanged();
            }

            // khi dropdown vừa chuyển từ false -> true thì scroll đến item đã chọn
            if (IsDropdownVisible && !_prevDropdownVisible)
            {
                _prevDropdownVisible = true;

                await MyLib.ScrollToRow(JS, ScrollContainerId);

                // optional small delay để đảm bảo DOM đã render hết (tránh trường hợp virtualization / render async)
                //await Task.Delay(50);
                //try
                //{
                //    await JS.InvokeVoidAsync("MyComboBox_ScrollToSelected", _scrollContainer);
                //}
                //catch(Exception ex)
                //{
                //    await JS.InvokeVoidAsync("console.log", ex.Message);

                //    Console.WriteLine("⚠️ JS function not found: " + ex.Message);
                //}
            }
            else if (!IsDropdownVisible && _prevDropdownVisible)
            {
                _prevDropdownVisible = false;
            }
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

            IsDropdownVisible = false;
            await ValueChanged.InvokeAsync(Value);
            await OnSelectedItem.InvokeAsync(SelectedItem);
            StateHasChanged();
        }
    }
}
