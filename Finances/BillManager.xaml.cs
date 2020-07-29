using Finances.Data;
using Finances.Facade;
using Finances.Model;
using Finances.Module;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Finances
{
    /// <summary>
    /// Interaction logic for AddBill.xaml
    /// </summary>
    public partial class BillManager : Window, IBillManager
    {
        private readonly IBillFacade _billFacade;
        private readonly IBillModule _billModule;

        private int _billId = 0;

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

            date.SelectedDate = DateTime.Now;
            description.Text = "";
            installment.Text = "1";
            value.Text = "-1";
            type.Text = "Debit Card";
            isPay.IsChecked = false;

            return this;
        }

        public BillManager Factory(BillInterface bill)
        {
            InitializeComponent();

            _billId = bill.Id;

            date.SelectedDate = bill.Date;
            description.Text = bill.Description;
            value.Text = bill.Price.ToString();
            installment.Text = bill.Installment.ToString();

            if (bill.Type == "D")
            {
                //installmentLabel.Visibility = Visibility.Hidden;
                //installment.Visibility = Visibility.Hidden;
                installment.Text = "1";
                type.Text = "Debit Card";
            }
            else
            {
                type.Text = "Credit Card";

                //installmentLabel.Visibility = Visibility.Visible;
                //installment.Visibility = Visibility.Visible;
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
                isPay.IsChecked.GetValueOrDefault(),
                1);

            if (bill != null)
            {
                bill.Id = _billId;

                _billFacade.Save(bill);

                Close();
            }
            else
            {
                Error.Content = error;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Visibility = Visibility.Hidden;
        }

        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selection = ((e.Source as ComboBox).SelectedItem as ComboBoxItem).Content as string;
            if (selection == "Debit Card")
            {
                //installmentLabel.Visibility = Visibility.Hidden;
                //installment.Visibility = Visibility.Hidden;
            }

            if (selection == "Credit Card")
            {
                //installmentLabel.Visibility = Visibility.Visible;
                //installment.Visibility = Visibility.Visible;
            }
        }
    }

    internal interface IBillManager
    {
        BillManager Factory();

        BillManager Factory(BillInterface bill);
    }
}