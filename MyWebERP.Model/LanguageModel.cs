using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class LanguageModel
    {
        public LanguageModel() { }

        public LanguageModel(string p_sLanguage_code, string p_Language_name)
        {
            this.language_code = p_sLanguage_code;
            this.language_name = p_Language_name;
        }

        public string language_code { get; set; }
        public string language_name { get; set; }
    }
}
