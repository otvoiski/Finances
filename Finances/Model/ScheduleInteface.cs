using System;
using System.Collections;
using System.Collections.Generic;

namespace Finances.Model
{
    public class ScheduleInteface
    {
        public int Id { get; set; }

        public int BillId { get; set; }

        public string Description { get; set; }

        public int Installments { get; set; }

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        public bool IsActive { get; set; }
        public double Price { get; set; }
    }
}