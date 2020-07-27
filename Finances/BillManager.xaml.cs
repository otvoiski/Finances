using Finances.Facade;
using Finances.Model;
using Finances.Module;
using System;
using System.Windows;

namespace Finances
{
    /// <summary>
    /// Interaction logic for AddBill.xaml
    /// </summary>
    public partial class BillManager : Window, IBillManager
    {
        private readonly IBillFacade _billFacade;
        private readonly IBillModule _billModule;

        private Bill _bill;

        public BillManager(IBillFacade billFacade, IBillModule billModule)
        {
            _billFacade = billFacade;
            _billModule = billModule;
        }

        public BillManager Factory()
        {
            InitializeComponent();

            clickBotton.Content = "Add bill";

            date.SelectedDate = DateTime.Now;
            description.Text = "";
            parcel.Text = "0";
            value.Text = "-1";
            type.Text = "Debit Card";
            isPay.IsChecked = false;

            return this;
        }

        public BillManager Factory(Bill bill)
        {
            InitializeComponent();

            _bill = bill;

            date.SelectedDate = _bill.Date;
            description.Text = _bill.Description;
            parcel.Text = _bill.Parcel.ToString();
            value.Text = _bill.Value.ToString();
            type.Text = _bill.Type == "D"
                ? "Debit Card"
                : "Credit Card";
            isPay.IsChecked = _bill.IsPay;

            clickBotton.Content = "Edit bill";

            return this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var bill = _billModule.ValidateBill(date, description, parcel, value, type, isPay, error);
            if (bill != null)
            {
                if (_bill.Id > 0)
                {
                    bill.Id = _bill.Id;
                    _billFacade.Update(bill);
                }
                else
                {
                    _billFacade.Insert(bill);
                }

                Visibility = Visibility.Hidden;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Visibility = Visibility.Hidden;
        }
    }

    internal interface IBillManager
    {
        BillManager Factory();

        BillManager Factory(Bill bill);
    }
}