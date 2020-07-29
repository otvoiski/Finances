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
                int total = 1;

                var schedule = _sqlService
                    .ToList<Schedule>(x => x.BillId == b.Id)?
                    .FirstOrDefault();

                if (schedule != null)
                {
                    total = schedule.End.GetValueOrDefault().Month - DateTime.Today.Month;
                }

                i.Add(new BillInterface
                {
                    Date = b.Date,
                    Id = b.Id,
                    Description = b.Description,
                    Installment = $"{b.Installment}/{total}",
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

            return bill.Id > 0
                ? _sqlService.Update(bill) == 1
                : _sqlService.Insert(bill) == 1;
        }

        public bool Delete(int id)
        {
            return _sqlService
                .Delete(id) == 1;
        }

        public Data.Bill GetBill(int billId)
        {
            return _sqlService
                .ToList<Data.Bill>(x => x.Id == billId)
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
    }

    public interface IBillFacade
    {
        IList<Model.BillInterface> GetAllBills();

        bool Save(Data.Bill bill);

        bool Delete(int id);

        Data.Bill GetBill(int billId);

        IList<Data.Bill> FindBills(string description);
    }
}