using Finances.Model;
using Finances.Service;
using SQLite;
using System;
using System.Collections.Generic;

namespace Finances.Facade
{
    public class BillFacade : IBillFacade
    {
        private readonly ISqlService _sqlService;

        public BillFacade(ISqlService sqlService)
        {
            _sqlService = sqlService;
        }

        public bool Insert(Bill bill)
        {
            using SQLiteConnection connection = _sqlService.Factory();
            return connection.Insert(bill) > 0;
        }

        public IList<Bill> GetAllBills()
        {
            using SQLiteConnection connection = _sqlService.Factory();
            return connection.Table<Bill>().ToList();
        }

        public bool Update(Bill bill)
        {
            if (bill.IsPay)
                bill.Payment = DateTime.Now;

            using SQLiteConnection connection = _sqlService.Factory();
            return connection.Update(bill) > 0;
        }

        public bool Remove(Bill bill)
        {
            using SQLiteConnection connection = _sqlService.Factory();
            return connection.Delete(bill) > 0;
        }
    }

    public interface IBillFacade
    {
        IList<Bill> GetAllBills();

        bool Insert(Bill bill);

        bool Update(Bill bill);

        bool Remove(Bill bill);
    }
}