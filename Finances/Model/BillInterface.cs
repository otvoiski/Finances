using System;
using System.Collections.Generic;
using System.Text;

namespace Finances.Model
{
    public class BillInterface
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public double Price { get; set; }

        public bool IsPaid { get; set; }

        public DateTime? Payment { get; set; }

        public string Installment { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }
    }
}