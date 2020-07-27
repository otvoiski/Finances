using Finances.Facade;
using Finances.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Finances
{
    /// <summary>
    /// Interaction logic for AddBill.xaml
    /// </summary>
    public partial class AddBill : Window
    {
        private readonly IBillFacade _billFacade;

        public AddBill(IBillFacade billFacade)
        {
            InitializeComponent();

            _billFacade = billFacade;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            #region Empty Check

            if (string.IsNullOrWhiteSpace(description.Text))
            {
                error.Content = "Description can not is empty";
                return;
            }
            if (string.IsNullOrWhiteSpace(parcel.Text))
            {
                error.Content = "Parcel can not is empty";
                return;
            }
            if (string.IsNullOrWhiteSpace(value.Text))
            {
                error.Content = "Value can not is empty";
                return;
            }

            #endregion Empty Check

            #region Value Check

            if (!int.TryParse(parcel.Text, out int parcelNumber))
            {
                error.Content = "Parcel is not a number";
                return;
            }

            if (!double.TryParse(value.Text, out double valueNumber))
            {
                error.Content = "Value is not a number";
                return;
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
                    return;
            }

            #endregion Value Check

            #region Date Check

            if (!DateTime.TryParse(date.Text, out DateTime dateTime))
            {
                error.Content = "Its not a date time";
                return;
            }

            #endregion Date Check

            _billFacade.Insert(new Bill
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
            });
        }
    }
}