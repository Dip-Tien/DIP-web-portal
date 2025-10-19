namespace MyWebERP.Base.Components
{
    public interface IMyPopup
    {
        /// <summary>
        /// Hiển thị popup (có thể truyền dữ liệu đầu vào, ví dụ model hoặc id)
        /// </summary>
        Task ShowAsync(object? parameter = null);

        /// <summary>
        /// Đóng popup (nếu đang hiển thị)
        /// </summary>
        Task CloseAsync(object? parameter = null);

        /// <summary>
        /// Sự kiện xảy ra khi popup đóng, có thể trả dữ liệu kết quả
        /// </summary>
        event Func<object?, Task>? PopupClosed;

        /// <summary>
        /// Trạng thái hiển thị hiện tại
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// (Tùy chọn) Tiêu đề popup, để service có thể hiển thị log/trace/debug
        /// </summary>
        string Title { get; }
    }
}
