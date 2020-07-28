using System;

namespace Finances.Test.Model
{
    internal class DataSchedule
    {
        public int ScheduleId { get; set; }
        public int BillId { get; set; }
        public string Installment { get; set; }
        public DateTime? Start { get; set; }
    }
}