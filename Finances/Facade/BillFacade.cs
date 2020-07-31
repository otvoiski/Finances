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

        public IList<Bill> GetAllBills()
        {
            var i = new List<Bill>();
            var bills = _sqlService
                        .ToList<Bill>();
            foreach (var b in bills)
            {
                i.Add(new Bill
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

        public (bool isSchedule, string error) IsSchedule(Bill bill, bool delete = false)
        {
            #region Is a schedule?

            var haveScheduleWithDescription = _sqlService
                .ToList<Schedule>(x => x.Description == bill.Description)
                .Count > 0;

            if (!haveScheduleWithDescription) return (true, null);

            #endregion Is a schedule?

            #region if have, check this is edition or delete

            var oldBill = _sqlService
                .ToList<Bill>(x => x.Id == bill.Id)
                .FirstOrDefault();

            if (oldBill != null)
            {
                #region ok, this is a edition, but can edit field?

                if (bill.Description != oldBill.Description)
                {
                    #region you cannot edit description if this is scheduled.

                    return (false, "You cannot edit description if this is scheduled.");

                    #endregion you cannot edit description if this is scheduled.
                }
                else
                {
                    #region you didn't edit the description and passed, but is this a installment??

                    var haveInstallment = _sqlService
                            .ToList<Installment>(x => x.BillId == bill.Id)?
                            .Count() > 0;

                    if (haveInstallment)
                    {
                        #region you cannot remove an invoice that is part of a installment.

                        return (false, "You cannot remove an invoice that is part of a installment!");

                        #endregion you cannot remove an invoice that is part of a installment.
                    }
                    else
                    {
                        #region Check delete flag

                        if (delete)
                        {
                            return (false, "You cannot remove an invoice that is part of a schedule!");
                        }

                        #endregion Check delete flag

                        return (true, null);
                    }

                    #endregion you didn't edit the description and passed, but is this a installment??
                }

                #endregion ok, this is a edition, but can edit field?
            }
            else
            {
                #region Is not edinting, but exist on schedule table.

                return (false, "This invoice description already exists in on window of scheduling, change the description field!");

                #endregion Is not edinting, but exist on schedule table.
            }

            #endregion if have, check this is edition or delete
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
        IList<Bill> GetAllBills();

        bool Save(Bill bill);

        bool Delete(int id);

        Bill GetBill(int billId);

        IList<Bill> FindBills(string description);

        (bool isSchedule, string error) IsSchedule(Bill bill, bool delete = false);
    }
}