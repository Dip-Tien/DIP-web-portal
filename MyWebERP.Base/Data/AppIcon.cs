using Microsoft.Extensions.Localization;

namespace MyWebERP.Base.Data
{
    // AppIcon.cs

    public enum AppIcon
    {
        Home,
        Search,
        User,
        Menu,
        Bell,
        Plus,
        Check,
        Edit,
        Trash,
        ArrowLeft,
        ArrowRight,
        LogIn,
        LogOut,
        Settings,
        Eye
    }

// AppIconExtensions.cs
    public static class AppIconExtensions
    {
        public static string ToIconName(this AppIcon icon)
        {
            return icon switch
            {
                AppIcon.Home => "home",
                AppIcon.Search => "search",
                AppIcon.User => "user",
                AppIcon.Menu => "menu",
                AppIcon.Bell => "bell",
                AppIcon.Plus => "plus",
                AppIcon.Check => "check",
                AppIcon.Edit => "edit",
                AppIcon.Trash => "trash",
                AppIcon.ArrowLeft => "arrow-left",
                AppIcon.ArrowRight => "arrow-right",
                AppIcon.LogIn => "log-in",
                AppIcon.LogOut => "log-out",
                AppIcon.Settings => "settings",
                AppIcon.Eye => "eye",
                _ => "question"
            };
        }

        public static string ToCssClass(this AppIcon icon, string prefix = "bi bi-")
        {
            return prefix + icon.ToIconName();
        }

        public static string ToDisplayText(this AppIcon icon, IStringLocalizer? localizer = null)
        {
            var key = "Icon." + icon.ToString();
            return localizer?[key] ?? icon switch
            {
                AppIcon.Home => "Trang chủ",
                AppIcon.Search => "Tìm kiếm",
                AppIcon.User => "Người dùng",
                AppIcon.Menu => "Menu",
                AppIcon.Bell => "Thông báo",
                AppIcon.Plus => "Thêm",
                AppIcon.Check => "Xác nhận",
                AppIcon.Edit => "Chỉnh sửa",
                AppIcon.Trash => "Xoá",
                AppIcon.ArrowLeft => "Trái",
                AppIcon.ArrowRight => "Phải",
                AppIcon.LogIn => "Đăng nhập",
                AppIcon.LogOut => "Đăng xuất",
                AppIcon.Settings => "Cài đặt",
                AppIcon.Eye => "Xem",
                _ => icon.ToString()
            };
        }
    }

    // Gợi ý sử dụng:
    // <DxButton StartIconCssClass="@AppIcon.Bell.ToCssClass()" Text="@AppIcon.Bell.ToDisplayText()" />
    // @AppIcon.Settings.ToDisplayText(localizer) // nếu inject IStringLocalizer<AppIcon> localizer

}
