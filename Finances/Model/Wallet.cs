﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Finances.Model
{
    public class Wallet
    {
        public double Balance { get; set; }
        public IList<Bill> Bills { get; set; }
        public double BillsToPay { get; set; }
        public double TotalBillsMonth { get; set; }
        public double TotalBillsYear { get; set; }
        public double BillsPaidYear { get; set; }
    }
}