using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MyWebERP.Services
{
    public class JsonLocalizationProvider
    {
        private readonly string _baseFolder;
        private readonly List<string> _fallbackChain;   // Ða tầng
        private readonly SemaphoreSlim _ioLock = new(1, 1);

        // cache: culture → dict
        private readonly ConcurrentDictionary<string, Dictionary<string, string>> _cache
            = new();

        public JsonLocalizationProvider(string baseFolder, IEnumerable<string>? fallbackCultures = null)
        {
            _baseFolder = baseFolder;
            if (!Directory.Exists(_baseFolder))
                Directory.CreateDirectory(_baseFolder);

            _fallbackChain = new List<string>(fallbackCultures ?? new[] { "en-US" });
        }

        private string PathOf(string culture) => System.IO.Path.Combine(_baseFolder, $"{culture}.json");

        /* ---------- public API ---------- */
        public Dictionary<string, string> Load(string culture)
        {
            var path = PathOf(culture);
            Console.WriteLine($"📄 Đọc JSON: {path}");

            if (_cache.TryGetValue(culture, out var cached))
                return cached;

            if (!File.Exists(path))
            {
                Console.WriteLine($"⚠️ File không tồn tại: {path}");
                return new();
            }

            try
            {
                var json = File.ReadAllText(path, Encoding.UTF8);
                var dic = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new();
                _cache[culture] = dic;
                return dic;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi đọc JSON file '{path}': {ex.Message}");
                return new();
            }
        }


        public async Task<Dictionary<string, string>> LoadAsync(string culture)
        {
            var path = PathOf(culture);
            if (_cache.TryGetValue(culture, out var cached))
                return cached;

            if (!File.Exists(path))
            {
                Console.WriteLine($"⚠️ Không tìm thấy file: {path}");
                return new();
            }

            try
            {
                // DÙNG CÁCH AN TOÀN
                var json = await Task.Run(() => File.ReadAllText(path, Encoding.UTF8));
                var dic = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new();
                _cache[culture] = dic;
                return dic;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi đọc JSON file '{path}': {ex.Message}");
                return new();
            }
        }

        public async Task<bool> SaveAsync(string culture, Dictionary<string, string> data)
        {
            var path = PathOf(culture);

            await _ioLock.WaitAsync();
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);

                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });

                await File.WriteAllTextAsync(path, json);
                _cache[culture] = data;

                Console.WriteLine($"✅ Đã lưu file: {path}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi ghi file JSON ({culture}): {ex.Message}");
                return false;
            }
            finally
            {
                _ioLock.Release();
            }
        }

        public void ClearCache(string culture) => _cache.TryRemove(culture, out _);

        public async Task<string?> GetStringAsync(string culture, string key)
        {
            // thử culture hiện tại
            var dic = await LoadAsync(culture);
            if (dic.TryGetValue(key, out var val)) return val;

            // thử fallback chain theo thứ tự
            foreach (var fb in _fallbackChain)
            {
                if (string.Equals(fb, culture, System.StringComparison.OrdinalIgnoreCase)) continue;
                var fbdic = await LoadAsync(fb);
                if (fbdic.TryGetValue(key, out val)) return val;
            }
            return null;
        }

        public string? GetString(string culture, string key)
        {
            // thử culture hiện tại
            var dic = Load(culture);
            if (dic.TryGetValue(key, out var val)) return val;

            // thử fallback chain theo thứ tự
            foreach (var fb in _fallbackChain)
            {
                if (string.Equals(fb, culture, System.StringComparison.OrdinalIgnoreCase)) continue;
                var fbdic = Load(fb);
                if (fbdic.TryGetValue(key, out val)) return val;
            }
            return null;
        }

        /* upload helper */
        public async Task<bool> ImportFromStreamAsync(string culture, Stream stream, bool overwrite)
        {
            // đọc file JSON upload
            var uploaded = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream)
                           ?? new();
            var current = overwrite ? new Dictionary<string, string>()
                                    : new Dictionary<string, string>(await LoadAsync(culture));

            foreach (var kv in uploaded) current[kv.Key] = kv.Value;
            return await SaveAsync(culture, current);
        }

    }
}
