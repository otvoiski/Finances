using System;
using System.Collections.Generic;
using System.Text;

namespace Finances.Model
{
    public class Wallet
    {
        public double Balance { get; set; }
        public IList<Bill> Bills { get; set; }
    }
}