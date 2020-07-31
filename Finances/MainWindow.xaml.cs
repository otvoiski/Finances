using Finances.Facade;
using Finances.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
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
        // facades
        private readonly IBillFacade _billFacade;

        private readonly IScheduleFacade _scheduleFacade;

        // windows
        private readonly IBillManager _billManager;

        private readonly IScheduleBill _scheduleBill;

        private DateTime _date;

        public MainWindow()
        {
            InitializeComponent();

            App.Init();

            IServiceProvider services = Dependencies
                .GetDependencies()
                .BuildServiceProvider();

            // windows
            _billManager = services.GetRequiredService<IBillManager>();
            _scheduleBill = services.GetRequiredService<IScheduleBill>();

            // facades
            _billFacade = services.GetRequiredService<IBillFacade>();
            _scheduleFacade = services.GetRequiredService<IScheduleFacade>();

            _date = DateTime.Today;
            date.Content = _date.ToString("MMMM, yyyy");

            LoadWallet();
        }

        private void LoadWallet()
        {
            _scheduleFacade.LoadSchedule();

            var bills = _billFacade.GetAllBills();

            double Balance = 0,
                BillsToPay = 0,
                TotalBillsMonth = 0,
                TotalBillsYear = 0,
                BillsPaidYear = 0,
                BillsCreditCardYear = 0,
                BillsPaidMonth = 0;

            foreach (var bill in bills)
            {
                Balance += bill.IsPaid && bill.Date.Month <= DateTime.Today.Month && bill.Date.Year <= DateTime.Today.Year
                    ? bill.Price
                    : 0;

                BillsToPay += !bill.IsPaid
                    ? bill.Price
                    : 0;

                TotalBillsMonth += !bill.IsPaid && bill.Date.Month == DateTime.Today.Month
                    ? bill.Price
                    : 0;

                TotalBillsYear += !bill.IsPaid && bill.Date.Year == DateTime.Today.Year
                    ? bill.Price
                    : 0;

                BillsPaidYear += bill.IsPaid && bill.Payment?.Year == DateTime.Today.Year && bill.Price < 0
                    ? bill.Price
                    : 0;

                BillsCreditCardYear += !bill.IsPaid && bill.Date.Year == DateTime.Today.Year && bill.Price < 0 && bill.Type == "C"
                    ? bill.Price
                    : 0;

                BillsPaidMonth += bill.IsPaid && bill.Date.Month == DateTime.Today.Month
                    ? bill.Price
                    : 0;
            }

            balance.Foreground = Balance >= 0
                ? new SolidColorBrush(Colors.Green)
                : balance.Foreground = new SolidColorBrush(Colors.Red);

            balance.Content = Balance.ToString();
            bills_to_pay.Content = BillsToPay.ToString();
            total_bills_on_month.Content = TotalBillsMonth.ToString();
            total_bills_on_year.Content = TotalBillsYear.ToString();
            bills_paid_in_the_year.Content = BillsPaidYear.ToString();
            bills_on_credit_card_year.Content = BillsCreditCardYear.ToString();

            editButton.IsEnabled = false;
            deleteButton.IsEnabled = false;

            BillList.ItemsSource = bills.Where(x =>
                x.Date.Month == _date.Month &&
                x.Date.Year == _date.Year);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            _billManager
                .Factory()
                .ShowDialog();
            LoadWallet();
        }

        private void Button_Click_Edit(object sender, RoutedEventArgs e)
        {
            if (BillList.SelectedItem is Bill bill)
            {
                _billManager
                    .Factory(bill)
                    .ShowDialog();
                LoadWallet();
            }
        }

        private void Button_Click_Remove(object sender, RoutedEventArgs e)
        {
            if (BillList.SelectedItem is Bill bill)
            {
                var result = MessageBox.Show($"Do you want remove {bill.Description} on price {bill.Price}$ ?", "Remove bill", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    (bool isSchedule, string error) = _billFacade.IsSchedule(bill, true);

                    if (!isSchedule)
                    {
                        MessageBox.Show(error, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        _billFacade.Delete(bill.Id);
                    }

                    LoadWallet();
                }
            }
        }

        private void Button_Click_Schedule(object sender, RoutedEventArgs e)
        {
            _scheduleBill
                .Factory()
                .ShowDialog();
            LoadWallet();
        }

        private void BillList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selection = e.Source as DataGrid;

            if (selection.SelectedItem is Bill)
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
            LoadWallet();
        }

        private void Arrow_left_Click(object sender, RoutedEventArgs e)
        {
            _date = new DateTime(_date.Ticks).AddMonths(-1);
            date.Content = _date.ToString("MMMM, yyyy");
            LoadWallet();
        }
    }
}