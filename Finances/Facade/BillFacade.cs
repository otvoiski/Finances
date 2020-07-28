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

        public IList<Bill> GetAllBills()
        {
            using SQLiteConnection connection = _sqlService.Factory();
            return connection
                .Table<Bill>().ToList();
        }

        public bool Save(Bill bill)
        {
            if (bill.IsPay)
                bill.Payment = DateTime.Now;

            using SQLiteConnection connection = _sqlService.Factory();
            return connection
                .InsertOrReplace(bill) > 0;
        }

        public bool Remove(Bill bill)
        {
            using SQLiteConnection connection = _sqlService.Factory();
            return connection
                .Delete(bill) > 0;
        }

        public Bill GetBill(int billId)
        {
            using SQLiteConnection connection = _sqlService.Factory();
            return connection
                .Table<Bill>()
                .Where(x => x.Id == billId)
                .FirstOrDefault();
        }

        public IList<Bill> FindBills(string description)
        {
            using SQLiteConnection connection = _sqlService.Factory();
            return connection
                .Query<Bill>("select * from Bill where Description like ?", $"%{description}%");
        }
    }

    public interface IBillFacade
    {
        IList<Bill> GetAllBills();

        bool Save(Bill bill);

        bool Remove(Bill bill);

        Bill GetBill(int billId);

        IList<Bill> FindBills(string description);
    }
}