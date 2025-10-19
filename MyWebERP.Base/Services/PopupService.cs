using Microsoft.AspNetCore.Components;
using MyWebERP.Base.Components;

public class PopupService
{
    private readonly List<PopupItem> _activePopups = new();
    private readonly Dictionary<string, Func<object?, Task>> _onClosedHandlers = new();
    public event Action<bool>? OnLoadingChanged;
    
    public IReadOnlyList<PopupItem> ActivePopups => _activePopups;

    public event Action? OnChange;

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            if (_isLoading != value)
            {
                _isLoading = value;
                OnLoadingChanged?.Invoke(value);
            }
        }
    }

    // Quản lý z-index động cho popup chồng nhau
    private int _zIndexBase = 1500;
    private int _nextLayer = 0; 
    private int GetNextLayer() => _zIndexBase + (_nextLayer++ * 100);
    private void ReleaseLayer()
    {
        _nextLayer = Math.Max(0, _nextLayer - 1);
    }

    // -------------------------------
    // Hiển thị popup
    // -------------------------------
    public async Task ShowAsync(string key, Dictionary<string, object?>? extraParameters = null)
    {
        IsLoading = true; // ⚡ Bắt đầu loading
        await Task.Yield(); // Cho phép UI cập nhật spinner

        var type = FindPopupTypeByKey(key);
        if (type == null)
        {
            Console.WriteLine($"❌ Popup '{key}' not found in loaded assemblies.");
            return;
        }

        var parameters = new Dictionary<string, object?>
        {
            ["Visible"] = true,
            ["PopupLayerIndex"] = GetNextLayer(), // ✅ cấp layer tự động
            ["OnClose"] = EventCallback.Factory.Create<object?>(this, async (result) => await CloseAsync(key, result))
        };

        // Gộp thêm các param tùy ý
        if (extraParameters != null)
        {
            foreach (var kv in extraParameters)
                parameters[kv.Key] = kv.Value;
        }

        var item = new PopupItem
        {
            Key = key,
            ComponentType = type,
            Parameters = parameters
        };

        _activePopups.Add(item);
        OnChange?.Invoke();

        // chờ 1 nhịp để render hoàn tất (có thể tùy chỉnh)
        await Task.Delay(100);
        IsLoading = false; // ✅ Kết thúc loading
    }

    // -------------------------------
    // Đóng popup
    // -------------------------------
    public async Task CloseAsync(string key, object? result = null)
    {
        var item = _activePopups.FirstOrDefault(x => x.Key == key);
        if (item != null)
        {
            _activePopups.Remove(item);
            ReleaseLayer(); // ✅ hạ layer khi popup đóng
            OnChange?.Invoke();
        }

        // 🔹 Gọi callback nếu có đăng ký
        if (_onClosedHandlers.TryGetValue(key, out var callback))
        {
            await callback.Invoke(result);
            _onClosedHandlers.Remove(key); // chỉ gọi 1 lần
        }
    }

    // -------------------------------
    // Đăng ký callback khi popup đóng
    // -------------------------------
    public void OnPopupClosed(string key, Func<object?, Task> callback)
    {
        _onClosedHandlers[key] = callback;
    }

    // -------------------------------
    // Tìm loại component theo tên
    // -------------------------------
    private Type? FindPopupTypeByKey(string key)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t =>
                typeof(ComponentBase).IsAssignableFrom(t) &&
                t.Name.Equals(key, StringComparison.OrdinalIgnoreCase));
    }

    // -------------------------------
    // PopupItem để render trong PopupHost
    // -------------------------------
    public class PopupItem
    {
        public string Key { get; set; } = "";
        public Type ComponentType { get; set; } = default!;
        public Dictionary<string, object?> Parameters { get; set; } = new();
    }
}
