using Microsoft.AspNetCore.Components;
using MyWebERP.Model;
using MyWebERP.Services;

namespace MyWebERP.Base.Components.BaseClasses
{
    public class ProvinceLookup<TValue> : LookupBase<TValue>
    {
        [Parameter] public EventCallback<ProvinceLookupModel> SelectedItemChanged { get; set; }

        protected List<ProvinceLookupModel> Data = new();
        protected ProvinceLookupModel? SelectedItem;


        protected override void OnSearchTextChanged(string? value)
        {
            SearchText = value ?? "";

            if (Data == null || Data.Count == 0)
            {
                ShouldLoadData = true; // Đánh dấu cần load
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            if (Value != null && string.IsNullOrEmpty(Value.ToString()) == false)
            {
                string valStr = Value.ToString() ?? "";
                if (Data.Count == 0 || SelectedItem == null || SelectedItem.province_id != valStr)
                {
                    await LoadDataAsync(); // Đảm bảo dữ liệu đã được load

                    var item = Data.FirstOrDefault(i => i.province_id == valStr);

                    if (item != null)
                    {
                        SelectedItem = item;
                    }
                    else
                    {
                        SelectedItem = new ProvinceLookupModel { province_id = valStr, province_name = "(Không tìm thấy)" };
                    }
                }
            }
            else
            {
                SelectedItem = null;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (ShouldLoadData)
            {
                ShouldLoadData = false;
                await LoadDataAsync(); // Gọi load sau khi popup đã render xong
            }
        }

        protected override async Task LoadDataAsync()
        {
            IsLoading = true;
            await InvokeAsync(StateHasChanged); // ép render trước khi await
                                                // Anh thay đoạn này bằng gọi service thực tế
            Data = await DataService.LookupCode<ProvinceLookupModel>(CompanyId, ActiveOnly, CompanyMenuId, "PROVINCE_LOOKUP");
            IsLoading = false;
            await InvokeAsync(StateHasChanged);
        }

        protected virtual IEnumerable<ProvinceLookupModel> FilteredList =>
            string.IsNullOrWhiteSpace(SearchText)
            ? Data
            : Data.Where(i =>
                i.province_name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                || i.comment.Contains(SearchText, StringComparison.OrdinalIgnoreCase));


        protected virtual async Task SelectItem(ProvinceLookupModel item)
        {
            IsPopupVisible = false;

            SelectedItem = item;
            await ValueChanged.InvokeAsync((TValue?)(object?)item.province_id);
            // Gọi callback lên component cha
            await SelectedItemChanged.InvokeAsync(SelectedItem);
        }
    }
}
