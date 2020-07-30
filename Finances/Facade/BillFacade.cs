using Finances.Data;
using Finances.Model;
using Finances.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Finances.Facade
{
    public class BillFacade : IBillFacade
    {
        private readonly ISqlService _sqlService;

        public BillFacade(ISqlService sqlService)
        {
            _sqlService = sqlService;
        }

        public IList<BillInterface> GetAllBills()
        {
            var i = new List<BillInterface>();
            var bills = _sqlService
                        .ToList<Bill>();
            foreach (var b in bills)
            {
                i.Add(new BillInterface
                {
                    Date = b.Date,
                    Id = b.Id,
                    Description = b.Description,
                    Installment = b.Installment,
                    IsPaid = b.IsPaid,
                    Payment = b.Payment,
                    Price = b.Price,
                    Type = b.Type
                });
            }
            return i;
        }

        public bool Save(Bill bill)
        {
            if (bill.IsPaid)
                bill.Payment = DateTime.Now;

            if (bill.Type == "C" && string.IsNullOrEmpty(bill.Installment))
            {
                bill.Installment = "1/1";
            }
            else
            {
                bill.Installment = null;
            }

            return bill.Id > 0
                ? _sqlService.Update(bill) == 1
                : _sqlService.Insert(bill) == 1;
        }

        public Bill GetBill(int billId)
        {
            return _sqlService
                .ToList<Bill>(x => x.Id == billId)
                .FirstOrDefault();
        }

        public IList<Bill> FindBills(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return _sqlService
                    .ToList<Bill>();
            }
            else
            {
                return _sqlService
                    .Query<Bill>("select * from Bill where Description like ?", $"%{description}%");
            }
        }

        public bool IsSchedule(int id)
        {
            // You cannot remove the invoice with part of the installment, so please remove the schedule first!
            return _sqlService
                .ToList<Installment>(x => x.BillId == id)?
                .Count() > 0;
        }

        public bool Delete(int id)
        {
            return _sqlService
                .Delete(new Bill { Id = id }) == 1;
        }

        public bool Delete(Bill bill)
        {
            return _sqlService
                .Delete(bill) == 1;
        }
    }

    public interface IBillFacade
    {
        IList<BillInterface> GetAllBills();

        bool Save(Bill bill);

        bool Delete(int id);

        Bill GetBill(int billId);

        IList<Bill> FindBills(string description);

        bool IsSchedule(int id);
    }
}