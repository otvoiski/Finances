using SQLite;
using System;

namespace Finances.Data
{
    public class Schedule
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int Installment { get; set; }

        [NotNull]
        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        [NotNull]
        public bool IsActive { get; set; }

        [NotNull]
        public string Description { get; set; }

        [NotNull]
        public double Price { get; set; }
    }
}