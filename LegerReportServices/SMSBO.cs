using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegerReportServices
{
    public class SMSBO
    {
        public DateTime TransactioDate { get; set; }
        public Decimal TodaysRequestAccepted { get; set; }
        public Decimal TRMH { get; set; }
        public Decimal TRMP { get; set; }
        public Decimal TRCG { get; set; }
        public Decimal TROT { get; set; }
        public Decimal MonthlyRequestAccepted { get; set; }
        public Decimal MRMH { get; set; }
        public Decimal MRMP { get; set; }
        public Decimal MRCG { get; set; }
        public Decimal MROT { get; set; }
        public string SMSBodyForRequest { get; set; }
        public string SMSBodyForRecatisharge { get; set; }
        public string SMSBodyForRecharge { get; set; }

        public string SMSCombie { get; set; }
        public string MobileNo { get; set; }
        public string RequestString { get; set; }
        public string ResponseString { get; set; }
        public string SupportBy { get; set; }

        //for Topup and PVC
        public Decimal TotalTransaction { get; set; }
        public Decimal TodaysTopUpRechargeAccepted { get; set; }
        public Decimal TodaysTopUpPVCMH { get; set; }
        public Decimal TodaysTopUpPVCMP { get; set; }
        public Decimal TodaysTopUpPVCCG { get; set; }
        public Decimal TodaysTopUpPVCOT { get; set; }
        public Decimal MonthlyTopUpRechargeAccepted { get; set; }
        public Decimal MonthlyTopUpPVCMH { get; set; }
        public Decimal MonthlyTopUpPVCMP { get; set; }
        public Decimal MonthlyTopUpPVCCG { get; set; }
        public Decimal MonthlyTopUpPVCOT { get; set; }
        public Decimal TodaysPVCAccepted { get; set; }
        public Decimal MonthlyPVCAccepted { get; set; }
        public Decimal MonthlyTransaction { get; set; }


        public Decimal TodayMoneyTranTransaction { get; set; }
        public Decimal TodayMoneyTranCommission { get; set; }
        public Decimal MonthlyMoneyTranCommission { get; set; }
        public Decimal TodaysMoneyTranMH { get; set; }
        public Decimal TodaysMoneyTranMP { get; set; }
        public Decimal TodaysMoneyTranCG { get; set; }
        public Decimal TodaysMoneyTranOT { get; set; }
        public Decimal MonthlyMoneyTranTransaction { get; set; }
        public Decimal MonthlyMoneyTranMH { get; set; }
        public Decimal MonthlyMoneyTranMP { get; set; }
        public Decimal MonthlyMoneyTranCG { get; set; }
        public Decimal MonthlyMoneyTranOT { get; set; }
        public string SMSBodyForMoneyTran { get; set; }
        public string SMSBodyForMoneyTranwithComm { get; set; }
        public Decimal TodaysINTCount { get; set; }
        public Decimal TodaysINTVal { get; set; }
        public Decimal MonthlyINTVal { get; set; }

        public int IsSMSExecuted { get; set; }
        public DateTime SMSExecutionTime { get; set; }

    }

    public class SetupDetailsBO : ErrorBO
    {
        public int HourOfExicution { get; set; }
        public string FromMail { get; set; }
        public string Pwd { get; set; }
        public string SendToEmailID { get; set; }

        public int IsSMSExecuted { get; set; }
        public DateTime SMSExecutionTime { get; set; }
    }
}
