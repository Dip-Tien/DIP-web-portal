using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class APIResultModel
    {
        public APIResultModel()
        {
        }

        public APIResultModel(Int32 status, string details, Object data)
        {
            this.Status = status;
            this.StatusDetails = details;
            this.Data = data;
        }

        public Int32 Status { get; set; }
        public String StatusDetails { get; set; }
        public Object Data { get; set; }
    }

    public class APIResultDataModel
    {
        public int count_of_items { get; set; }
        public object data_details { get; set; }
    }

    public class HttpResponseMessageContent
    {
        public string Message { get; set; }
    }
}
