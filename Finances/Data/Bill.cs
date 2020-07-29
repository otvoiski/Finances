using SQLite;
using System;
using System.Collections.Generic;

namespace Finances.Data
{
    public class Bill
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public DateTime Date { get; set; }

        [NotNull]
        public double Price { get; set; }

        [NotNull]
        public bool IsPaid { get; set; }

        public DateTime? Payment { get; set; }

        [NotNull]
        public int Installment { get; set; }

        [NotNull]
        public string Description { get; set; }

        [NotNull]
        public string Type { get; set; }
    }
}