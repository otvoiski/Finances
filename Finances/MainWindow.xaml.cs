using Finances.Facade;
using Finances.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Finances
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IBillFacade _billFacade;
        private readonly IBillManager _billManager;
        private Wallet _wallet;
        private DateTime _date;

        public MainWindow()
        {
            // Colors in Hex
            // #080705 black
            // #40434E charcoal
            // #702632 wine
            // #912F40 red violet
            // #fffffa white

            InitializeComponent();

            _billFacade = App.Services.GetRequiredService<IBillFacade>();
            _billManager = App.Services.GetRequiredService<IBillManager>();

            _date = DateTime.Today;
            date.Content = _date.ToString("MMMM, yyyy");

            LoadInterface();
        }

        private void LoadInterface()
        {
            _wallet = new Wallet
            {
                Bills = _billFacade.GetAllBills()
            };

            foreach (var bill in _wallet.Bills)
            {
                _wallet.Balance += bill.Value;

                _wallet.BillsToPay += !bill.IsPay
                    ? bill.Value
                    : 0;

                _wallet.TotalBillsMonth += !bill.IsPay && bill.Date.Month == DateTime.Now.Month
                    ? bill.Value
                    : 0;

                _wallet.TotalBillsYear += !bill.IsPay && bill.Date.Year == DateTime.Now.Year
                    ? bill.Value
                    : 0;

                _wallet.BillsPaidYear += bill.IsPay && bill.Date.Year == DateTime.Now.Year && bill.Value < 0
                    ? bill.Value
                    : 0;
            }

            balance.Foreground = _wallet.Balance >= 0
                ? new SolidColorBrush(Colors.Green)
                : balance.Foreground = new SolidColorBrush(Colors.Red);

            balance.Content = $"${_wallet.Balance}";
            bills_to_pay.Content = $"${_wallet.BillsToPay}";
            total_bills_on_month.Content = $"${_wallet.TotalBillsMonth}";
            total_bills_on_year.Content = $"${_wallet.TotalBillsYear}";
            bills_paid_in_the_year.Content = $"${_wallet.BillsPaidYear}";
            editButton.IsEnabled = false;
            deleteButton.IsEnabled = false;

            BillList.ItemsSource = _wallet.Bills;
            BillList.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            var bill = _billManager.Factory();
            bill.ShowDialog();
            LoadInterface();
        }

        private void Button_Click_Edit(object sender, RoutedEventArgs e)
        {
            if (BillList.SelectedIndex >= 0)
            {
                var bill = _billManager.Factory(_wallet.Bills[BillList.SelectedIndex]);
                bill.ShowDialog();
                LoadInterface();
            }
        }

        private void Button_Click_Remove(object sender, RoutedEventArgs e)
        {
            if (BillList.SelectedIndex >= 0)
            {
                var bill = _wallet.Bills[BillList.SelectedIndex];

                var result = MessageBox.Show($"Do you want remove {bill.Description} on value ${bill.Value}?", "Remove bill", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _billFacade.Remove(bill);
                    LoadInterface();
                }
            }
        }

        private void Button_Click_Schedule(object sender, RoutedEventArgs e)
        {
        }

        private void BillList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BillList.SelectedIndex >= 0)
            {
                editButton.IsEnabled = true;
                deleteButton.IsEnabled = true;
            }
            else
            {
                editButton.IsEnabled = false;
                deleteButton.IsEnabled = false;
            }
        }

        private void Arrow_right_Click(object sender, RoutedEventArgs e)
        {
            _date = new DateTime(_date.Ticks).AddMonths(1);
            date.Content = _date.ToString("MMMM, yyyy");
        }

        private void Arrow_left_Click(object sender, RoutedEventArgs e)
        {
            _date = new DateTime(_date.Ticks).AddMonths(-1);
            date.Content = _date.ToString("MMMM, yyyy");
        }
    }
}