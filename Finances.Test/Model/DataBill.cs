using System;

namespace Finances.Test.Model
{
    internal class DataBill
    {
        public bool IsPay { get; internal set; }
        public string Type { get; internal set; }
        public string Value { get; internal set; }
        public string Installment { get; internal set; }
        public string Description { get; internal set; }
        public DateTime? Date { get; internal set; }
    }
}