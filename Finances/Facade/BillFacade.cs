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
            return _sqlService
                .ToList<Bill>();
        }

        public bool Save(Bill bill)
        {
            if (bill.IsPaid)
                bill.Payment = DateTime.Today;

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

        public (bool IsSchedule, string Error) IsSchedule(Bill bill, bool delete = false)
        {
            #region Is a schedule?

            var haveScheduleWithDescription = _sqlService
                .ToList<Schedule>(x => x.Description == bill.Description)
                .Count > 0;

            if (!haveScheduleWithDescription)
            {
                #region you didn't edit the description, but is this a installment??

                var haveInstallment = _sqlService
                        .ToList<Installment>(x => x.BillId == bill.Id)?
                        .Count() > 0;

                if (haveInstallment)
                {
                    #region you cannot remove an invoice that is part of a installment.

                    return (true, "You cannot remove an invoice that is part of a installment!");

                    #endregion you cannot remove an invoice that is part of a installment.
                }
                else
                    return (false, null);

                #endregion you didn't edit the description, but is this a installment??
            }

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

                    return (true, "You cannot edit description if this is scheduled.");

                    #endregion you cannot edit description if this is scheduled.
                }
                else
                {
                    #region Check delete flag

                    if (delete)
                    {
                        return (true, "You cannot remove an invoice that is part of a schedule!");
                    }
                    else
                    {
                        return (false, null);
                    }

                    #endregion Check delete flag
                }

                #endregion ok, this is a edition, but can edit field?
            }
            else
            {
                #region Is not edinting, but exist on schedule table.

                return (true, "This invoice description already exists in on window of scheduling, change the description field!");

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

        (bool IsSchedule, string Error) IsSchedule(Bill bill, bool delete = false);
    }
}