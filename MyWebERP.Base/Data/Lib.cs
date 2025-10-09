using Blazored.LocalStorage;
using System.Reflection;
//using System.Runtime.CompilerServices;
//using System.Text.Json;
//using System.Text.Json.Nodes;
//using System.Xml.Linq;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using MyWebERP.Model;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;

namespace MyWebERP.Data
{
    public enum ChartType
    {
        Bar,
        Line,
        Area
    }
    public enum EditFormStatus
    {
        VIEW,
        NEW,
        EDIT,
        /// <summary>
        /// Trạng thái đang lưu
        /// </summary>
        SAVING
    }

    public enum DotAlign
    {
        None,
        Left,
        Right
    }
    public enum DataChangeType { Modification, Addition, Delete }

    public record DataChange(DataChangeType Type, HashSet<string> ChangedFields);

    public static class LocalStorageName
    {
        public const string USER = "user";
        public const string TOKEN = "token";
        public const string YEAR = "year";
        public const string COMPANY = "company";
        public const string COMPANY_ID = "company_id";
        public const string LANGUAGE = "lang";
    }

    public class MyLib
    {
        public static async Task ClearAllLocalStorage(ILocalStorageService localStorageService)
        {
            ILocalStorageService _localStorageService = localStorageService;

            await _localStorageService.RemoveItemAsync(LocalStorageName.USER);
            await _localStorageService.RemoveItemAsync(LocalStorageName.TOKEN);
            await _localStorageService.RemoveItemAsync(LocalStorageName.LANGUAGE);
            await _localStorageService.RemoveItemAsync(LocalStorageName.YEAR);
            await _localStorageService.RemoveItemAsync(LocalStorageName.COMPANY);
            await _localStorageService.RemoveItemAsync(LocalStorageName.COMPANY_ID);
        }

        public static DateTime GetLastDateOfMonth(Int32 p_iMonth, Int32 p_iYear)
        {
            Int32 _iMonth = p_iMonth;
            Int32 _iYear = p_iYear;

            Int32 _iNextMonth = _iMonth + 1;

            if (_iMonth == 12)
            {
                _iNextMonth = 1;
                _iYear += 1;
            }

            DateTime _dtmResult = new DateTime(_iYear, _iNextMonth, 1).AddDays(-1);

            return _dtmResult;
        }

        public static int FirstMonthOfQuarterByMonth(Int32 p_iMonth)
        {
            if (p_iMonth <= 3)
                return 1;
            else if (p_iMonth <= 6)
                return 4;
            else if (p_iMonth <= 9)
                return 7;
            else
                return 10;
        }

        public static void GetDataFromPeriod(PeriodItem periodItem, int workingYear, DateTime workingDate, ref DateTime date1, ref DateTime date2)
        {
            date1 = workingDate;
            date2 = workingDate;

            PeriodItem _periodItem = periodItem;
            String _sPeriodCode = _periodItem.PeriodCode;
            int _iThisMonth = workingDate.Month;

            switch (_sPeriodCode)
            {
                case "NGAY0":
                case "NGAY":
                    date1 = workingDate;
                    date2 = workingDate;
                    break;
                case "T2":
                    date2 = new DateTime(workingYear, 3, 1).AddDays(-1);
                    break;
                case "TN": // Tháng này
                    date2 = MyLib.GetLastDateOfMonth(_iThisMonth, workingYear);
                    date1 = DateTime.ParseExact("01/" + _iThisMonth.ToString().PadLeft(2, '0') + "/" + workingYear.ToString(), "dd/MM/yyyy", System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                    break;
                case "TT": // Tháng trước
                    if (_iThisMonth == 1)
                    {
                        // Tháng trước là tháng 12
                        date2 = new DateTime(workingYear, 1, 1).AddDays(-1);
                        //date1 = DateTime.ParseExact("01/12/" + workingYear.ToString(), "dd/MM/yyyy", System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                    }
                    else
                    {
                        date2 = MyLib.GetLastDateOfMonth(_iThisMonth - 1, workingYear);
                        //date1 = DateTime.ParseExact("01/" + (_iThisMonth - 1).ToString().PadLeft(2, '0') + "/" + workingYear.ToString(), "dd/MM/yyyy", System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                    }
                    date1 = new DateTime(workingYear, date2.Month, 1);
                    break;
                case "TS": // Tháng sau
                    if (_iThisMonth == 12)
                    {
                        date2 = MyLib.GetLastDateOfMonth(1, workingYear);
                        //date1 = DateTime.ParseExact("01/01/" + (workingYear + 1).ToString(), "dd/MM/yyyy", System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                    }
                    else
                    {
                        date2 = MyLib.GetLastDateOfMonth(_iThisMonth + 1, workingYear);
                        //date1 = DateTime.ParseExact("01/" + (_iThisMonth + 1).ToString().PadLeft(2, '0') + "/" + workingYear.ToString(), "dd/MM/yyyy", System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                    }
                    date1 = new DateTime(workingYear, date2.Month, 1);
                    break;
                case "QN":
                    date1 = new DateTime(workingYear, MyLib.FirstMonthOfQuarterByMonth(workingDate.Month), 1);
                    date2 = new DateTime(workingYear, MyLib.FirstMonthOfQuarterByMonth(workingDate.Month) + 2, 1).AddMonths(1).AddDays(-1);
                    break;
                case "QT":
                    date1 = new DateTime(workingYear, MyLib.FirstMonthOfQuarterByMonth(workingDate.Month), 1).AddMonths(-3);
                    date2 = new DateTime(workingYear, MyLib.FirstMonthOfQuarterByMonth(workingDate.Month), 1).AddDays(-1);
                    break;
                case "QS":
                    DateTime _dtmQuySau = new DateTime(workingYear, MyLib.FirstMonthOfQuarterByMonth(workingDate.Month) + 3, 1);
                    date1 = _dtmQuySau;
                    date2 = _dtmQuySau.AddMonths(3).AddDays(-1);
                    break;
                case "NAMT":
                    date1 = new DateTime(workingDate.Year - 1, 1, 1);
                    date2 = new DateTime(workingDate.Year - 1, 12, 31);
                    break;
                case "NAMS":
                    date1 = new DateTime(workingDate.Year + 1, 1, 1);
                    date2 = new DateTime(workingDate.Year + 1, 12, 31);
                    break;
                case "TUANN":
                    date1 = workingDate.AddDays(1 - (int)workingDate.DayOfWeek);
                    date2 = workingDate.AddDays(7 - (int)workingDate.DayOfWeek);
                    break;
                case "TUANT":
                    date1 = workingDate.AddDays(-(int)workingDate.DayOfWeek - 6);
                    date2 = workingDate.AddDays(-(int)workingDate.DayOfWeek);
                    break;
                case "TUANS":
                    date1 = workingDate.AddDays(8 - (int)workingDate.DayOfWeek);
                    date2 = workingDate.AddDays(14 - (int)workingDate.DayOfWeek);
                    break;
                default:
                    String _d2 = (_periodItem.Date1.Substring(_periodItem.Date2.Length - 1) == "/" ? _periodItem.Date2 : _periodItem.Date2 + "/") + workingYear.ToString();
                    date2 = DateTime.ParseExact(_d2, "dd/MM/yyyy", System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                    String _d1 = (_periodItem.Date1.Substring(_periodItem.Date1.Length - 1) == "/" ? _periodItem.Date1 : _periodItem.Date1 + "/") + workingYear.ToString();
                    date1 = DateTime.ParseExact(_d1, "dd/MM/yyyy", System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                    break;
            }
        }

        public static void GetDataFromPeriod(PeriodItem periodItem, int workingYear, DateTime workingDate, ref DateTime date1, ref DateTime date2, ref string periodName)
        {
            date1 = workingDate;
            date2 = workingDate;

            PeriodItem _periodItem = periodItem;
            String _sPeriodCode = _periodItem.PeriodCode;
            int _iThisMonth = workingDate.Month;

            switch (_sPeriodCode)
            {
                case "NGAY0":
                case "NGAY":
                    date1 = workingDate;
                    date2 = workingDate;
                    periodName = "Hôm nay";
                    break;
                case "T2":
                    date2 = new DateTime(workingYear, 3, 1).AddDays(-1);
                    periodName = $"Từ {date1.ToString("dd/MM/yyyy")} -> {date2.ToString("dd/MM/yyyy")}";
                    break;
                case "TN": // Tháng này
                    date2 = MyLib.GetLastDateOfMonth(_iThisMonth, workingYear);
                    date1 = DateTime.ParseExact("01/" + _iThisMonth.ToString().PadLeft(2, '0') + "/" + workingYear.ToString(), "dd/MM/yyyy", System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                    if (workingYear == DateTime.Now.Year)
                    {
                        periodName = "Tháng này";
                    }
                    else
                    {
                        periodName = $"Tháng {date1.Month.ToString()}/{workingYear.ToString()}";
                    }
                    break;
                case "TT": // Tháng trước
                    if (_iThisMonth == 1)
                    {
                        // Tháng trước là tháng 12
                        date2 = new DateTime(workingYear, 1, 1).AddDays(-1);
                        //date1 = DateTime.ParseExact("01/12/" + workingYear.ToString(), "dd/MM/yyyy", System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                    }
                    else
                    {
                        date2 = MyLib.GetLastDateOfMonth(_iThisMonth - 1, workingYear);
                        //date1 = DateTime.ParseExact("01/" + (_iThisMonth - 1).ToString().PadLeft(2, '0') + "/" + workingYear.ToString(), "dd/MM/yyyy", System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                    }
                    date1 = new DateTime(workingYear, date2.Month, 1);
                    periodName = $"Tháng {date1.Month.ToString()}/{workingYear.ToString()}";
                    break;
                case "TS": // Tháng sau
                    if (_iThisMonth == 12)
                    {
                        date2 = MyLib.GetLastDateOfMonth(1, workingYear);
                        //date1 = DateTime.ParseExact("01/01/" + (workingYear + 1).ToString(), "dd/MM/yyyy", System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                    }
                    else
                    {
                        date2 = MyLib.GetLastDateOfMonth(_iThisMonth + 1, workingYear);
                        //date1 = DateTime.ParseExact("01/" + (_iThisMonth + 1).ToString().PadLeft(2, '0') + "/" + workingYear.ToString(), "dd/MM/yyyy", System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                    }
                    date1 = new DateTime(workingYear, date2.Month, 1);
                    periodName = $"Tháng {date1.Month.ToString()}/{workingYear.ToString()}";
                    break;
                case "QN":
                    date1 = new DateTime(workingYear, MyLib.FirstMonthOfQuarterByMonth(workingDate.Month), 1);
                    date2 = new DateTime(workingYear, MyLib.FirstMonthOfQuarterByMonth(workingDate.Month) + 2, 1).AddMonths(1).AddDays(-1);
                    periodName = $"Từ {date1.ToString("dd/MM/yyyy")} -> {date2.ToString("dd/MM/yyyy")}";
                    break;
                case "QT":
                    date1 = new DateTime(workingYear, MyLib.FirstMonthOfQuarterByMonth(workingDate.Month), 1).AddMonths(-3);
                    date2 = new DateTime(workingYear, MyLib.FirstMonthOfQuarterByMonth(workingDate.Month), 1).AddDays(-1);
                    periodName = $"Từ {date1.ToString("dd/MM/yyyy")} -> {date2.ToString("dd/MM/yyyy")}";
                    break;
                case "QS":
                    DateTime _dtmQuySau = new DateTime(workingYear, MyLib.FirstMonthOfQuarterByMonth(workingDate.Month) + 3, 1);
                    date1 = _dtmQuySau;
                    date2 = _dtmQuySau.AddMonths(3).AddDays(-1);
                    periodName = $"Từ {date1.ToString("dd/MM/yyyy")} -> {date2.ToString("dd/MM/yyyy")}";
                    break;
                case "NAMT":
                    date1 = new DateTime(workingDate.Year - 1, 1, 1);
                    date2 = new DateTime(workingDate.Year - 1, 12, 31);
                    periodName = $"Năm {date1.Year.ToString()}";
                    break;
                case "NAMS":
                    date1 = new DateTime(workingDate.Year + 1, 1, 1);
                    date2 = new DateTime(workingDate.Year + 1, 12, 31);
                    periodName = $"Năm {date1.Year.ToString()}";
                    break;
                case "TUANN":
                    date1 = workingDate.AddDays(1 - (int)workingDate.DayOfWeek);
                    date2 = workingDate.AddDays(7 - (int)workingDate.DayOfWeek);
                    periodName = $"Từ {date1.ToString("dd/MM/yyyy")} -> {date2.ToString("dd/MM/yyyy")}";
                    break;
                case "TUANT":
                    date1 = workingDate.AddDays(-(int)workingDate.DayOfWeek - 6);
                    date2 = workingDate.AddDays(-(int)workingDate.DayOfWeek);
                    periodName = $"Từ {date1.ToString("dd/MM/yyyy")} -> {date2.ToString("dd/MM/yyyy")}";
                    break;
                case "TUANS":
                    date1 = workingDate.AddDays(8 - (int)workingDate.DayOfWeek);
                    date2 = workingDate.AddDays(14 - (int)workingDate.DayOfWeek);
                    periodName = $"Từ {date1.ToString("dd/MM/yyyy")} -> {date2.ToString("dd/MM/yyyy")}";
                    break;
                default:
                    String _d2 = (_periodItem.Date1.Substring(_periodItem.Date2.Length - 1) == "/" ? _periodItem.Date2 : _periodItem.Date2 + "/") + workingYear.ToString();
                    date2 = DateTime.ParseExact(_d2, "dd/MM/yyyy", System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                    String _d1 = (_periodItem.Date1.Substring(_periodItem.Date1.Length - 1) == "/" ? _periodItem.Date1 : _periodItem.Date1 + "/") + workingYear.ToString();
                    date1 = DateTime.ParseExact(_d1, "dd/MM/yyyy", System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                    periodName = $"Từ {date1.ToString("dd/MM/yyyy")} -> {date2.ToString("dd/MM/yyyy")}";
                    break;
            }
        }

        public static async Task<string> CreateFetSingleParam(ILocalStorageService localStorageService, string sColumnName, string sId)
        {
            string _sCompanyId = await localStorageService.GetItemAsync<string?>(LocalStorageName.COMPANY_ID);
            return await CreateFetSingleParam(_sCompanyId, sColumnName, sId);
        }

        public static async Task<string> CreateFetSingleParam(string sCompanyId, string sColumnName, string sId)
        {
            return "{" + $"\"company_id:\":\" {sCompanyId} \", \"{sColumnName}\":\"{sId}\"" + "}";
            //return "{\"company_id\":\"" + _sCompanyId + "\", \"" + sColumnName + "\":\"" + sId + "\"}";
        }

        public static async Task<string> CreateDeleteDataParam(ILocalStorageService localStorageService, string sColumnName, string sId)
        {
            // Lệnh này trả về kiểu có 2 dấu nháy
            //string _sCompanyId = await localStorageService.GetItemAsStringAsync(LocalStorageName.COMPANY_ID);
            string _sCompanyId = await localStorageService.GetItemAsync<string?>(LocalStorageName.COMPANY_ID);

            return await CreateDeleteDataParam(_sCompanyId, sColumnName, sId);
        }

        public static async Task<string> CreateDeleteDataParam(String sCompanyId, string sColumnName, string sId)
        {
            return "{" + $"\"company_id:\":\" {sCompanyId} \", \"{sColumnName}\":\"{sId}\"" + "}";
        }

        public static async Task<string> CreateDeleteVoucherDataParam(ILocalStorageService localStorageService, string sColumnName, string sId, Int16 iForceDelete)
        {
            string _sCompanyId = await localStorageService.GetItemAsync<string?>(LocalStorageName.COMPANY_ID);
            return await CreateDeleteVoucherDataParam(_sCompanyId, sColumnName, sId, iForceDelete);
        }

        public static async Task<string> CreateDeleteVoucherDataParam(String sCompanyId, string sColumnName, string sId, Int16 iForceDelete)
        {
            return "{" + $"\"company_id:\":\" {sCompanyId} \", \"{sColumnName}\":\"{sId}\"" + ", \"force_delete\":" + iForceDelete + "}";
            //return "{\"company_id\":\"" + _sCompanyId + "\", \"" + sColumnName + "\":\"" + sId + "\"}";
        }

        public static async Task MapObject<TData>(TData des, TData src)
        {
            //typeof(TData)
            //        .GetProperties()
            //        .Select((p, index) =>
            //            new { Index = index, PropertyInfo = p })
            //        .ToList()
            //        .ForEach(p =>
            //        {
            //            typeof(TData)
            //                .GetProperties()
            //                .Skip(p.Index)
            //                .FirstOrDefault()
            //                .SetValue(des,
            //                    p.PropertyInfo.GetValue(src));
            //        });

            if (des == null || src == null)
                return;

            var desProps = typeof(TData).GetProperties()
                                        .Where(p => p.CanWrite)
                                        .ToDictionary(p => p.Name);

            var srcProps = typeof(TData).GetProperties()
                                        .Where(p => p.CanRead)
                                        .ToDictionary(p => p.Name);

            foreach (var srcProp in srcProps.Values)
            {
                if (desProps.ContainsKey(srcProp.Name))
                {
                    var targetProp = desProps[srcProp.Name];
                    var value = srcProp.GetValue(src);

                    if (value == null)
                    {
                        targetProp.SetValue(des, null);
                        continue;
                    }

                    try
                    {
                        var targetType = Nullable.GetUnderlyingType(targetProp.PropertyType) ?? targetProp.PropertyType;
                        var convertedValue = Convert.ChangeType(value, targetType);
                        targetProp.SetValue(des, convertedValue);
                    }
                    catch
                    {
                        // Nếu convert không được, bỏ qua, không gây lỗi
                    }
                }
            }
        }

        public static string GetPropertyValueOfObject(Object obj, string sPropertyName)
        {
            if(obj.GetType().GetProperty(sPropertyName) == null)
            {
                return "";
            }

            return obj.GetType().GetProperty(sPropertyName).GetValue(obj).ToString();
        }

        public static decimal GetDecimalPropertyValueOfObject(Object obj, string sPropertyName)
        {
            PropertyInfo _prop = obj.GetType().GetProperty(sPropertyName);
            if (_prop == null)
            {
                return 0;
            }

            return (decimal)_prop.GetValue(obj);
        }

        public static decimal GetDecimalPropertyValueOfDynamicObject(ExpandoObject obj, string sPropertyName)
        {
            var _value = obj.FirstOrDefault(x => x.Key == sPropertyName).Value;

            if (_value == null)
            {
                return 0;
            }
            else
            {
                return decimal.Parse(_value.ToString());
            }
        }

        public static void MapObject(object p_objSrc, object p_objDes)
        {
            object _objSrc = p_objSrc;
            object _objDes = p_objDes;

            PropertyInfo[] _desProps = _objDes.GetType().GetProperties();
            PropertyInfo[] _srcProps = _objSrc.GetType().GetProperties();

            foreach (PropertyInfo _desProp in _desProps)
            {
                PropertyInfo _srcProp = _srcProps.FirstOrDefault(x => x.Name == _desProp.Name & x.PropertyType == _desProp.PropertyType);

                if (_srcProp != null)
                {
                    _srcProp.SetValue(_objDes, _srcProp.GetValue(_objSrc));
                }
            }
        }

        //public static dynamic ConvertToDynamic(JsonElement element)
        //{
        //    var expando = new System.Dynamic.ExpandoObject() as System.Collections.Generic.IDictionary<string, object>;

        //    foreach (var property in element.EnumerateObject())
        //    {
        //        // Recursively convert nested objects to dynamic as well
        //        if (property.Value.ValueKind == JsonValueKind.Object)
        //        {
        //            expando[property.Name] = ConvertToDynamic(property.Value);
        //        }
        //        else
        //        {
        //            expando[property.Name] = property.Value.ToString();
        //        }
        //    }

        //    return expando;
        //}

        //public static List<dynamic> ConvertToListDynamic(JsonArray array)
        //{
        //    JsonArray jsonArray = array;

        //    // Convert JsonArray to a list of dynamic objects
        //    List<dynamic> lstDyn = new List<dynamic>();
                       

        //    foreach (JsonElement element in jsonArray)
        //    {
        //        var person = ConvertToDynamic(element);
        //        lstDyn.Add(person);
        //    }

        //    return lstDyn;
        //}

        public static Type DataType(String sDataType)
        {
            switch(sDataType.ToLower())
            {
                case "int":
                case "int32":
                    return typeof(int);
                case "smallint":
                case "int16":
                    return typeof(int);
                default:
                    return typeof(String);
            }
        }

        public static ExpandoObject ConvertToExpando(object obj)
        {
            //Get Properties Using Reflections
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            PropertyInfo[] properties = obj.GetType().GetProperties(flags);

            //Add Them to a new Expando
            ExpandoObject expando = new ExpandoObject();
            foreach (PropertyInfo property in properties)
            {
                AddProperty(expando, property.Name, property.GetValue(obj));
            }

            return expando;
        }

        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            //Take use of the IDictionary implementation
            var expandoDict = expando as IDictionary<String, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        public static List<ExpandoObject> ConvertListJObjectToListExpando(JArray array)
        {
            //return array
            //    .Select(j =>
            //    {
            //        IDictionary<string, object> expando = new ExpandoObject();

            //        foreach (var prop in ((Newtonsoft.Json.Linq.JObject)j))
            //        {
            //            expando[prop.Key] = prop.Value?.ToObject<object>();
            //        }

            //        return (ExpandoObject)expando;
            //    })
            //    .ToList();

            // Lấy danh sách key đầy đủ từ tất cả các JObject
            var allKeys = array
                .SelectMany(j => ((JObject)j).Properties().Select(p => p.Name))
                .Distinct()
                .ToList();

            return array
                .Select(j =>
                {
                    var expando = new ExpandoObject() as IDictionary<string, object>;
                    var jobj = (JObject)j;

                    // Gán các key có trong JObject
                    foreach (var key in allKeys)
                    {
                        expando[key] = jobj.TryGetValue(key, out var value)
                            ? value?.ToObject<object>()
                            : 0; // hoặc null tùy bạn
                    }

                    return (ExpandoObject)expando;
                })
                .ToList();
        }

        public static List<ExpandoObject> ConvertListJObjectToListExpando(JArray array, List<string> expectedKeys)
        {
            return array
                .Select(j =>
                {
                    var expando = new ExpandoObject() as IDictionary<string, object>;
                    var jobj = (JObject)j;

                    foreach (var prop in jobj)
                    {
                        expando[prop.Key] = prop.Value?.ToObject<object>();
                    }

                    // Ép các key còn thiếu về null hoặc 0
                    foreach (var key in expectedKeys)
                    {
                        if (!expando.ContainsKey(key))
                            expando[key] = 0; // hoặc null tùy loại dữ liệu
                    }

                    return (ExpandoObject)expando;
                })
                .ToList();
        }

        //public static List<ExpandoObject> ConvertListJObjectToListExpando(JArray array, string expectedKeys)
        //{
        //    List<String> _lstExpectedKeys = expectedKeys.Split(",").ToList();
        //    return array
        //        .Select(j =>
        //        {
        //            var expando = new ExpandoObject() as IDictionary<string, object>;
        //            var jobj = (JObject)j;

        //            foreach (var prop in jobj)
        //            {
        //                expando[prop.Key] = prop.Value?.ToObject<object>();
        //            }

        //            // Ép các key còn thiếu về null hoặc 0
        //            foreach (var key in _lstExpectedKeys)
        //            {
        //                if (!expando.ContainsKey(key))
        //                    expando[key] = 0; // hoặc null tùy loại dữ liệu
        //            }

        //            return (ExpandoObject)expando;
        //        })
        //        .ToList();
        //}

        public static List<ExpandoObject> ConvertListJObjectToListExpando(JArray dataArray, string listColumnCsv = null)
        {
            List<ExpandoObject> result = new List<ExpandoObject>();

            // Nếu có list_column: chỉ lấy các field được chỉ định
            HashSet<string> selectedFields = null;
            if (!string.IsNullOrWhiteSpace(listColumnCsv))
            {
                selectedFields = new HashSet<string>(
                    listColumnCsv.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(s => s.Trim().ToLower())  // lower để đồng bộ
                );
            }

            foreach (JObject item in dataArray)
            {
                IDictionary<string, object> expando = new ExpandoObject();

                foreach (JProperty prop in item.Properties())
                {
                    string key = prop.Name;
                    object value = ((JValue)prop.Value).Value;

                    // Nếu có danh sách field cần chọn thì chỉ thêm nếu nằm trong danh sách
                    if (selectedFields == null || selectedFields.Contains(key.ToLower()))
                    {
                        expando[key] = value ?? "";  // Gán giá trị, nếu null thì cho chuỗi rỗng
                    }
                }

                result.Add((ExpandoObject)expando);
            }

            return result;
        }



        public static ExpandoObject ConvertJObjectToExpando(JObject obj, List<Model.DataColumn> dataColumns)
        {
            ExpandoObject expando = new ExpandoObject();
            foreach (JProperty jProperty in obj.Properties())
            {
                AddProperty(expando, jProperty.Name, obj.Property(jProperty.Name).Value);
            }

            foreach (Model.DataColumn dataColumn in dataColumns)
            {
                JProperty property = obj.Properties().FirstOrDefault(x => x.Name == dataColumn.column_name);

                if (property != null)
                {
                    MyLib.AddProperty(expando, property.Name, property.Value, dataColumn.data_type);
                }
            }

            return expando;

        }

        public static void AddProperty(ExpandoObject expando, string propertyName, JToken propertyValue, string typeName)
        {
            //Take use of the IDictionary implementation
            var value = MyLib.ConvertJValue2NetType(propertyValue, typeName);

            var expandoDict = expando as IDictionary<String, object>;

            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = value;
            else
                expandoDict.Add(propertyName, value);
        }

        public static object ConvertJValue2NetType(JToken value, string typeName)
        {
            if(value == null)
            {
                return null;
            }

            switch (typeName.ToLower())
            {
                case "int":
                case "int32":
                    return ((int)value);
                case "int64":
                case "long":
                    return ((Int64)value);
                case "float":
                case "decimal":
                    return ((decimal)value);
                case "smallint":
                case "short":
                case "int16":
                    return ((short)value);
                case "datetime":
                    return DateTime.Parse(value.ToString());
                case "bool":
                case "boolean":
                    return (bool)value;
                case "string":
                    return value.ToString();
                default:
                    return value;
            }
        }

        public static HashSet<string> ApplyModifiedFields<T>(T from, T to, IEnumerable<string> fieldNamesToWatch = null) where T : class
        {
            HashSet<string> modifiedFields = new();
            ForEachFieldOf<T>(fieldProperty => {
                var sourceValue = fieldProperty.GetValue(to);
                var value = fieldProperty.GetValue(from);
                if (!Equals(value, sourceValue))
                {
                    fieldProperty.SetValue(to, value);
                    modifiedFields.Add(fieldProperty.Name);
                }
            }, fieldNamesToWatch);
            return modifiedFields;
        }
        static void ForEachFieldOf<T>(Action<PropertyDescriptor> func, IEnumerable<string> fieldNames = null)
        {
            var properties = TypeDescriptor.GetProperties(typeof(T));
            if (fieldNames == null)
                fieldNames = properties.OfType<PropertyDescriptor>().Select(x => x.Name);
            foreach (string field in fieldNames)
            {
                var fieldProperty = properties[field];
                func(fieldProperty);
            }
        }

        public static async Task<String> Object2Base64(object obj)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                Culture = new CultureInfo("en-US"),
            };
            string _sJson = Newtonsoft.Json.JsonConvert.SerializeObject(obj, jsonSerializerSettings);

            byte[] bytes = Encoding.UTF8.GetBytes(_sJson);

            return System.Convert.ToBase64String(bytes);
        }

        public static async Task<TData> Base642Object<TData>(string base64)
        {
            byte[] bytes = System.Convert.FromBase64String(base64);

            string _sJson = Encoding.UTF8.GetString(bytes);

            return JsonConvert.DeserializeObject<TData>(_sJson);
        }

        public static System.Reflection.Assembly[] GetAdditionalAssemblies(string assemblyNames)
        {
            List<System.Reflection.Assembly> _assemblies = new List<System.Reflection.Assembly>();

            if (string.IsNullOrEmpty(assemblyNames) == false)
            {
                foreach (string assemblyName in assemblyNames.Split(","))
                {
                    string assemblyPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, assemblyName.Trim()); // or whatever your dll is named
                    byte[] dynamicBytes = System.IO.File.ReadAllBytes(assemblyPath);

                    var assembly = System.Reflection.Assembly.Load(dynamicBytes);
                    if (assembly != null)
                    {
                        _assemblies.Add(assembly);
                    }
                }
            }

            return _assemblies.ToArray();
        }
    }
}
