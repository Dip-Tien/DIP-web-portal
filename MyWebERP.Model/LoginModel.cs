using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class LoginParamModel
    {
        public LoginParamModel()
        {
            //this.username = "1";
            //this.password = "sysadmin";
        }

        public LoginParamModel(string p_sConnectionName, string p_sUsername, string p_sPassword)
        {
            this.ConnectionName = p_sConnectionName;
            this.Username = p_sUsername;
            this.Password = p_sPassword;
        }

        public string ConnectionName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string RootCompanyId { get; set; }

        /// <summary>
        /// 1: lấy chuỗi kết nối từ file web.config
        /// 0: Lấy chuỗi kết nối qua api trung gian
        /// </summary>
        public Int16 CnnFromConfigFile { get; set; }
    }

    public class LoginResultModel
    {
        public int status { get; set; }
        public string message { get; set; }
        public string username { get; set; }
        public string display_name { get; set; }

        public string culture_code { get; set; }

        public WebAPIToken access_token { get; set; }
    }

    public class WebAPIToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public DateTime start_at { get; set; }
        public DateTime expires_at { get; set; }
        public int expires_in { get; set; }
        /// <summary>
        /// Hết hạn?
        /// </summary>
        public bool expired
        {
            get
            {
                if (this.expires_at < DateTime.Now)
                {
                    return true;
                }

                return false;
            }
        }
        public int error { get; set; }
        public string error_description { get; set; }
    }
}
