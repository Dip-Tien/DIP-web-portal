using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebERP.Model
{
    public class YearModel
    {
        public YearModel()
        {

        }

        public YearModel(int year)
        {
            Year = year;
        }

        public int Year { get; set; }
    }
}
