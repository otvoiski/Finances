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
        private int _billId;

        public BillManager(IBillFacade billFacade, IBillModule billModule)
        {
            _billFacade = billFacade;
            _billModule = billModule;
        }

        public BillManager Factory()
        {
            InitializeComponent();

            clickBotton.Content = "Add bill";

            installmentLabel.Visibility = Visibility.Hidden;
            installment.Visibility = Visibility.Hidden;

            date.SelectedDate = DateTime.Today;
            description.Text = "";
            installment.Text = string.Empty;
            value.Text = "-1";
            type.Text = "Debit Card";
            isPay.IsChecked = false;

            return this;
        }

        public BillManager Factory(Bill bill)
        {
            InitializeComponent();

            date.SelectedDate = bill.Date;
            description.Text = bill.Description;
            value.Text = bill.Price.ToString();
            installment.Text = bill.Installment?.ToString();

            _billId = bill.Id;

            if (bill.Type == "D")
            {
                installment.Text = string.Empty;
                type.Text = "Debit Card";
            }
            else
            {
                type.Text = "Credit Card";
            }

            installmentLabel.Visibility = Visibility.Hidden;
            installment.Visibility = Visibility.Hidden;

            isPay.IsChecked = bill.IsPaid;

            clickBotton.Content = "Edit bill";

            return this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (Bill bill, string error) = _billModule.ValidateBill(
                date.SelectedDate,
                description.Text,
                installment.Text,
                value.Text,
                type.Text,
                isPay.IsChecked.GetValueOrDefault());

            if (bill != null)
            {
                bill.Id = _billId;

                (bool isSchedule, string scheduleError) = _billFacade.IsSchedule(bill);

                if (isSchedule)
                {
                    MessageBox.Show(scheduleError, "Fail", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                else
                {
                    if (!_billFacade.Save(bill))
                    {
                        MessageBox.Show("Error on save bill!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    Error.Content = "";
                    Close();
                }
            }
            else
            {
                Error.Content = error;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _billId = 0;
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