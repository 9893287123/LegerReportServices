using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegerReportServices
{
    public class ErrorBO
    {
        public int Flag { get; set; }
        public string errorMsg { get; set; }
        public string warningMsg { get; set; }
        public string successMsg { get; set; }
        public string Title { get; set; }
        public string Msg { get; set; }
    }
}
