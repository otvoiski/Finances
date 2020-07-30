using System.Collections.Generic;

namespace Finances.Model
{
    public class Wallet
    {
        public double Balance { get; set; }
        public IList<BillInterface> Bills { get; set; }
        public double BillsToPay { get; set; }
        public double TotalBillsMonth { get; set; }
        public double TotalBillsYear { get; set; }
        public double BillsPaidYear { get; set; }
        public double BillsCreditCardYear { get; set; }
        public double BillsPaidMonth { get; internal set; }
    }
}