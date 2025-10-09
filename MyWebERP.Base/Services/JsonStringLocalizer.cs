using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace MyWebERP.Services
{
    using Microsoft.Extensions.Localization;
    using System.Globalization;

    public class JsonStringLocalizer<T> : IStringLocalizer<T>
    {
        
        private readonly JsonLocalizationProvider _provider;

        public JsonStringLocalizer(JsonLocalizationProvider provider)
        {
            _provider = provider;
        }

        public LocalizedString this[string name]
        {
            get
            {
                var culture = CultureInfo.CurrentUICulture.Name;
                var dic = _provider.Load(culture);

                if (dic.TryGetValue(name, out var value))
                    return new LocalizedString(name, value, false);

                return new LocalizedString(name, name, true);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = this[name];
                var formatted = string.Format(format.Value, arguments);
                return new LocalizedString(name, formatted, format.ResourceNotFound);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var culture = CultureInfo.CurrentUICulture.Name;
            var dic = _provider.Load(culture);

            return dic.Select(kvp =>
                new LocalizedString(kvp.Key, kvp.Value ?? kvp.Key, false));
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            // Optional: Blazor không gọi cái này
            return this;
        }
    }

}
