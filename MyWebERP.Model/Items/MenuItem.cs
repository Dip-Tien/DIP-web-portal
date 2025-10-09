
using MyWebERP.Model;
using System.Text.Json.Serialization;

namespace MyWebERP.Model
{    
    public class MenuItem
    {
        public MenuItem()
        {
        }
        public MenuItem(string name)
        {
            MenuName = name;
        }

        public MenuItem(string name, string url)
        {
            MenuName = name;
            Url = url;
        }

        public MenuItem(string name, string title, string url)
        {
            MenuName = name;
            Url = url;
            Title = title;
        }

        public MenuItem(string name, string title, string iconCss, string url)
        {
            IconCss = iconCss;
            MenuName = name;
            Url = url;
            Title = title;
        }

        public MenuItem(string name, string title, string iconCss, bool beginGroup)
        {
            IconCss = iconCss;
            MenuName = name;
            Title = title;
            BeginGroup = beginGroup;
        }

        /// <summary>
        /// Tạo menu từ lookup menu để truyền sang các form lookup cho phép thêm/sửa ở lookup
        /// </summary>
        /// <param name="lookupItem"></param>
        public MenuItem(LookupItem lookupItem)
        {
            CompanyMenuId = lookupItem.company_menu_id;
            DataCode = lookupItem.data_code;
            IdColumnName = lookupItem.id_column_name;
            DataCode = lookupItem.data_code;
            InsDataCode = lookupItem.ins_data_code;
            UpdDataCode = lookupItem.upd_data_code;
            FetSingleDataCode = lookupItem.fet_single_data_code;
        }

        public MenuItem(string name, List<MenuItem> children)
        {
            MenuName = name;
            Children = children;
        }

        public List<MenuItem> Children { get; set; } = new List<MenuItem>();

        public string CompanyMenuId { get; set; }

        public string MenuId { get; set; }

        public string MenuName { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string SmallUrl { get; set; }

        /// <summary>
        /// Nếu trống thì thêm/sửa = popup
        /// </summary>
        public string EditUrl { get; set; }

        public string SmallEditUrl { get; set; }

        public string Assembly { get; set; }

        public string Target { get; set; }

        public bool BeginGroup { get; set; }

        /// <summary>
        /// Mặc định icon để đặt kích thước chung.
        /// </summary>
        public string IconCss { get; set; } = "icon";
        public string IconUrl { get; set; }// = @"images/wheather/storm.svg";
        public string CssClass { get; set; } = "menu-item";
        public string Controller { get; set; }
        public string IndexAction { get; set; }
        public string EditAction { get; set; }
        public string DeleteAction { get; set; }
        public string DataCode { get; set; }
        public string FetDataCode { get; set; }
        public string FetSingleDataCode { get; set; }
        public string InsDataCode { get; set; }
        public string UpdDataCode { get; set; }
        public string DelDataCode { get; set; }
        public string ParentId { get; set; }

        public string IdColumnName { get; set; }

        public string PathTitle { get; set; }

        public virtual bool Enabled => true;

        /// <summary>
        /// Thứ tự dòng - dùng cho main chart
        /// </summary>
        public int Row { get; set; }
        /// <summary>
        /// Độ rộng tính theo col của grid web (tối đa 12). dùng cho main chart
        /// </summary>
        public int ColWidth { get; set; }

        public int Order { get; set; }
        public Boolean NewPermission
        {
            get
            {
                return this.CheckPerission("NEW") && String.IsNullOrEmpty(InsDataCode) == false;
            }
        }

        public Boolean EditPermission
        {
            get
            {
                return this.CheckPerission("EDIT") && String.IsNullOrEmpty(UpdDataCode) == false;
            }
        }

        public Boolean DeletePermission
        {
            get
            {
                return (this.CheckPerission("DELETE") && String.IsNullOrEmpty(DelDataCode) == false);
            }
        }

        public Boolean CheckPerission(string permissionKey)
        {
            if(this.Permissions.FirstOrDefault(x => x.PermissionKey == permissionKey & x.AllowAccess) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Các menu là các tab
        /// Lưu ý có lệnh tạo mới này để trường hợp api trả về null nó tự tạo mới
        /// Khỏi cần kiểm tra if null ở code nữa, chi cần kiểm tra count > 0
        /// </summary>
        public List<MenuItem> Tabs { get; set; } = new List<MenuItem>();
        public List<MenuPermissionItem> Permissions { get; set; } = new List<MenuPermissionItem>();

        public List<MenuReportFileItem> ReportFiles { get; set; } = new List<MenuReportFileItem>();

    }

    //public class MenuTab
    //{
    //    public string Id { get; set; }
    //    public string CompanyMenuId { get; set; }

    //    public string MenuId { get; set; }

    //    public string MenuName { get; set; }

    //    public string Title { get; set; }

    //    public string Url { get; set; }
    //    //public string Controller { get; set; }
    //    //public string IndexAction { get; set; }
    //    //public string EditAction { get; set; }
    //    //public string DeleteAction { get; set; }
    //    public string DataCode { get; set; }
    //    public string FetDataCode { get; set; }
    //    public string FetSingleDataCode { get; set; }
    //    public string InsDataCode { get; set; }
    //    public string UpdDataCode { get; set; }
    //    public string DelDataCode { get; set; }

    //    public string IdColumnName { get; set; }

    //    public int Order { get; set; }
    //}
}