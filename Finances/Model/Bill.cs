﻿using SQLite;
using System;
using System.Collections.Generic;

namespace Finances.Model
{
    public class Bill
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public DateTime Date { get; set; }

        public DateTime? Payment { get; set; }

        [NotNull]
        public string Description { get; set; }

        [NotNull]
        public int Installment { get; set; }

        [NotNull]
        public double Value { get; set; }

        [NotNull]
        public string Type { get; set; }

        [NotNull]
        public bool IsPay { get; set; }

        [Ignore]
        public IList<Installment> Installments { get; set; }
    }
}