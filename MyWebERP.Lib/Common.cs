using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Dynamic;
using System.IO;

namespace MyWebERP.Lib
{
    public class CM:Common
    {

    }
    public class Common
    {
        public static string StreamToString(Stream stream)
        {
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }        

        public static byte[] StringToByteArray(string src)
        {
            return Encoding.UTF8.GetBytes(src);
        }

        public static Stream StringToStream(string src)
        {
            byte[] byteArray = CM.StringToByteArray(src);
            //return new MemoryStream(byteArray);
            return CM.ByteArrayToStream(byteArray);
        }

        public static Stream ByteArrayToStream(Byte[] bytes)
        {
            return new MemoryStream(bytes);
        }

        public static System.IO.MemoryStream CreateMomoryStreamFromString(string p_sData)
        {
            byte[] _byteArray = new byte[p_sData.Length];
            //System.Text.ASCIIEncoding _encoding = new ASCIIEncoding();
            System.Text.UTF8Encoding _unicodeEncoding = new UTF8Encoding();

            _byteArray = _unicodeEncoding.GetBytes(p_sData);

            //System.IO.MemoryStream _memoryStreamResult = new System.IO.MemoryStream(_byteArray);
            //_memoryStreamResult.Seek(0, System.IO.SeekOrigin.Begin);

            //return _memoryStreamResult;
            return CM.CreateMomoryStreamFromByteArray(_byteArray);
        }

        public static System.IO.MemoryStream CreateMomoryStreamFromByteArray(Byte[] p_bytes)
        {
            byte[] _byteArray = p_bytes;

            System.IO.MemoryStream _memoryStreamResult = new System.IO.MemoryStream(_byteArray);
            _memoryStreamResult.Seek(0, System.IO.SeekOrigin.Begin);

            return _memoryStreamResult;
        }

        public static string IntColor2HtmlColor(int p_iColor)
        {
            if (p_iColor == 0)
                return "#FFFFFF"; // màu mặc định nếu không có giá trị

            return $"#{p_iColor & 0xFFFFFF:X6}";
        }

        /// <summary>
        /// Hàm bỏ dấu Tiếng Việt
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var ch in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        public static bool ContainsIgnoreDiacritics(string? source, string search)
        {
            if (string.IsNullOrWhiteSpace(source)) return false;
            return RemoveDiacritics(source)
                .Contains(RemoveDiacritics(search), StringComparison.OrdinalIgnoreCase);
        }

        #region Excel

        public async Task<List<ExpandoObject>> ReadExcelFileAsync(Stream fileStream)
        {
            // Bắt buộc để EPPlus dùng license miễn phí
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var result = new List<ExpandoObject>();

            using (var package = new ExcelPackage(fileStream))
            {
                var worksheet = package.Workbook.Worksheets[0]; // đọc sheet đầu tiên
                if (worksheet == null) return result;

                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                // Giả sử dòng 1 là header
                var headers = new List<string>();
                for (int col = 1; col <= colCount; col++)
                {
                    headers.Add(worksheet.Cells[1, col].Text);
                }

                // Đọc dữ liệu từ dòng 2
                for (int row = 2; row <= rowCount; row++)
                {
                    dynamic obj = new ExpandoObject();
                    var dict = (IDictionary<string, object>)obj;

                    for (int col = 1; col <= colCount; col++)
                    {
                        string header = headers[col - 1];
                        string value = worksheet.Cells[row, col].Text;
                        dict[header] = value;
                    }

                    result.Add(obj);
                }
            }

            return result;
        }

        #endregion Excel
    }
}
