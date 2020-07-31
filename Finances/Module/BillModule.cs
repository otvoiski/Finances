using Finances.Data;
using Finances.Model;
using System;

namespace Finances.Module
{
    public class BillModule : IBillModule
    {
        public (Bill bill, string error) ValidateBill(DateTime? date, string description, string installment, string priceString, string type, bool isPaid)
        {
            #region Empty Check

            if (string.IsNullOrWhiteSpace(description)) return (null, "Description can not is empty");

            if (string.IsNullOrWhiteSpace(priceString)) return (null, "Price can not is empty");

            if (string.IsNullOrWhiteSpace(priceString)) return (null, "Price can not is empty");

            #endregion Empty Check

            #region Value Check

            if (!double.TryParse(priceString, out double priceNumber)) return (null, "Value is not a number");

            string typeBox;

            switch (type)
            {
                case "Debit Card":
                    typeBox = "D";
                    break;

                case "Credit Card":
                    typeBox = "C";
                    break;

                default:
                    return (null, "Type do not exist");
            }

            #endregion Value Check

            #region Date Check

            if (date == null) return (null, "Its not a date time");

            #endregion Date Check

            return (new Bill
            {
                Date = date.GetValueOrDefault(),
                Description = description,
                Price = priceNumber,
                Installment = installment,
                Type = typeBox,
                IsPaid = isPaid,
                Payment = isPaid
                ? DateTime.Now
                : default
            }, null);
        }
    }

    public interface IBillModule
    {
        (Bill bill, string error) ValidateBill(DateTime? date, string description, string installment, string value, string type, bool isPay);
    }
}