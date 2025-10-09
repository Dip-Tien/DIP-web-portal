using Microsoft.AspNetCore.Components;
using MyWebERP.Model;
using MyWebERP.Services;

namespace MyWebERP.Base.Components.BaseClasses
{
    public class DistrictLookup<TValue> : LookupBase<TValue>
    {
        [Parameter] public EventCallback<DistrictLookupModel> SelectedItemChanged { get; set; }

        protected List<DistrictLookupModel> Data = new();
        protected DistrictLookupModel? SelectedItem;

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
                if (Data.Count == 0 || SelectedItem == null || SelectedItem.district_id != valStr)
                {
                    await LoadDataAsync(); // Đảm bảo dữ liệu đã được load

                    var item = Data.FirstOrDefault(i => i.district_id == valStr);

                    if (item != null)
                    {
                        SelectedItem = item;
                    }
                    else
                    {
                        SelectedItem = new DistrictLookupModel { district_id = valStr, district_name = "(Không tìm thấy)" };
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

        protected async Task LoadDataAsync()
        {
            IsLoading = true;
            await InvokeAsync(StateHasChanged); // ép render trước khi await
                                                // Anh thay đoạn này bằng gọi service thực tế
            Data = await DataService.LookupCode<DistrictLookupModel>(CompanyId, ActiveOnly, Parent, 0, CompanyMenuId, "DISTRICT_LOOKUP");
            IsLoading = false;
            await InvokeAsync(StateHasChanged);
        }

        protected virtual IEnumerable<DistrictLookupModel> FilteredList =>
            string.IsNullOrWhiteSpace(SearchText)
            ? Data
            : Data.Where(i =>
                i.district_name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                || i.comment.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        protected virtual async Task SelectItem(DistrictLookupModel item)
        {
            IsPopupVisible = false;

            SelectedItem = item;
            await ValueChanged.InvokeAsync((TValue?)(object?)item.district_id);
            // Gọi callback lên component cha
            await SelectedItemChanged.InvokeAsync(SelectedItem);
        }
    }
}
