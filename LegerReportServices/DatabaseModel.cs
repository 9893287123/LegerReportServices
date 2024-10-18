using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegerReportServices
{
    public class DatabaseModel
    {
    }
    public class UserModel
    {
        public string AtishayVendorID { get; set; }
        public string PersonEmailID { get; set; }
        public string NameOnPANCard { get; set; }
        public string MobileNo { get; set; }


    }
    public class UserTransactionModel
    {
        public string OpeningBalance { get; set; }
        public string Credit { get; set; }
        public string Debit { get; set; }
        public string ClosingBalance { get; set; }
        public string TransactionDate { get; set; }
        public string Narration { get; set; }
    }
}
