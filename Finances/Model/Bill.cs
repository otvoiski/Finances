using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finances.Model
{
    public class Bill
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public DateTime Date { get { return Date; } set { Date = DateTime.Now; } }

        public DateTime? Payment { get; set; }

        [NotNull]
        public string Description { get; set; }

        [NotNull]
        public int Parcel { get; set; }

        [NotNull]
        public double Value { get; set; }

        [NotNull]
        public string Type { get { return Type; } set { Type = "D"; } }

        [NotNull]
        public bool IsPay { get; set; }

        [Ignore]
        public IList<Parcel> Parcels { get; set; }
    }
}