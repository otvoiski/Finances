using SQLite;
using System;

namespace Finances.Model
{
    public class Bill
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public DateTime? Payment { get; set; }
        public string Description { get; set; }
        public int Parcel { get; set; }
        public double Value { get; set; }
        public string Type { get; set; }
        public bool IsPay { get; set; }
    }
}