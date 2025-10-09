using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class APIParameterModel
    {
        public String ConnectionName { get; set; }
        public String DataCode { get; set; }
        public String Token { get; set; }
        public string RootCompanyId { get; set; }
        public Object Data { get; set; }

        /// <summary>
        /// 1: lấy chuỗi kết nối từ file web.config
        /// 0: Lấy chuỗi kết nối qua api trung gian
        /// </summary>
        public Int16 CnnFromConfigFile { get; set; }
        public String LanguageCode { get; set; }
    }
}
