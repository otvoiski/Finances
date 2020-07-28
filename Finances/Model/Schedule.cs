using SQLite;
using System;

namespace Finances.Model
{
    public class Schedule
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int BillId { get; set; }

        [NotNull]
        public int Installment { get; set; }

        [NotNull]
        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        [NotNull]
        public bool IsActive { get; set; }
    }
}