using Finances.Model;
using Finances.Service;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

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
            return _sqlService
                .ToList<Bill>();
        }

        public bool Save(Bill bill)
        {
            //if (bill.IsPay)
            //    bill.Payment = DateTime.Now;

            return false;
            //return _sqlService.InsertOrReplace(bill) == 1;
        }

        public bool Delete(Bill bill)
        {
            return _sqlService
                .Delete(bill) == 1;
        }

        public Bill GetBill(int billId)
        {
            return _sqlService
                .ToList<Bill>(x => x.Id == billId)
                .FirstOrDefault();
        }

        public IList<Bill> FindBills(string description)
        {
            return _sqlService
                .Query<Bill>("select * from Bill where Description like ?", $"%{description}%");
        }
    }

    public interface IBillFacade
    {
        IList<Bill> GetAllBills();

        bool Save(Bill bill);

        bool Delete(Bill bill);

        Bill GetBill(int billId);

        IList<Bill> FindBills(string description);
    }
}