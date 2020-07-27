using Finances.Model;
using System;
using System.Windows.Controls;

namespace Finances.Module
{
    public class BillModule : IBillModule
    {
        public Bill ValidateBill(DatePicker date, TextBox description, TextBox parcel, TextBox value, ComboBox type, CheckBox isPay, Label error)
        {
            #region Empty Check

            if (string.IsNullOrWhiteSpace(description.Text))
            {
                error.Content = "Description can not is empty";
                return null;
            }
            if (string.IsNullOrWhiteSpace(parcel.Text))
            {
                error.Content = "Parcel can not is empty";
                return null;
            }
            if (string.IsNullOrWhiteSpace(value.Text))
            {
                error.Content = "Value can not is empty";
                return null;
            }

            #endregion Empty Check

            #region Value Check

            if (!int.TryParse(parcel.Text, out int parcelNumber))
            {
                error.Content = "Parcel is not a number";
                return null;
            }

            if (!double.TryParse(value.Text, out double valueNumber))
            {
                error.Content = "Value is not a number";
                return null;
            }

            string typeBox;

            switch ((type.SelectedItem as ComboBoxItem).Content)
            {
                case "Debit Card":
                    typeBox = "D";
                    break;

                case "Credit Card":
                    typeBox = "C";
                    break;

                default:
                    error.Content = "Type do not exist";
                    return null;
            }

            #endregion Value Check

            #region Date Check

            if (!DateTime.TryParse(date.Text, out DateTime dateTime))
            {
                error.Content = "Its not a date time";
                return null;
            }

            #endregion Date Check

            return new Bill
            {
                Date = dateTime,
                Description = description.Text,
                Parcel = parcelNumber,
                Value = valueNumber,
                Type = typeBox,
                IsPay = isPay.IsChecked.GetValueOrDefault(),
                Payment = isPay.IsChecked.GetValueOrDefault()
                ? DateTime.Now
                : default
            };
        }
    }

    public interface IBillModule
    {
        Bill ValidateBill(DatePicker date, TextBox description, TextBox parcel, TextBox value, ComboBox type, CheckBox isPay, Label error);
    }
}