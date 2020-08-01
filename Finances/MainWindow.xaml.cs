using Finances.Facade;
using Finances.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
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

            BillList.ItemsSource = bills.Where(x =>
                x.Date.Month == _date.Month &&
                x.Date.Year == _date.Year);

            double balance = 0,
                sumNegativePrice = 0,
                sumPositivePrice = 0;

            foreach (var bill in bills)
            {
                balance += bill.IsPaid && bill.Date.Month <= DateTime.Today.Month && bill.Date.Year <= DateTime.Today.Year
                    ? bill.Price
                    : 0;

                sumNegativePrice += bill.Date.Month == _date.Month && bill.Date.Year <= _date.Year && bill.Price < 0
                    ? bill.Price
                    : 0;

                sumPositivePrice += bill.Date.Month == _date.Month && bill.Date.Year <= _date.Year && bill.Price >= 0
                    ? bill.Price
                    : 0;
            }

            SetPriceOfText(Balance, balance);
            SetPriceOfText(SumNegativePrice, sumNegativePrice);
            SetPriceOfText(SumPositivePrice, sumPositivePrice);

            editButton.IsEnabled = false;
            deleteButton.IsEnabled = false;

            return;

            static void SetPriceOfText(Label text, double price)
            {
                text.Content = price.ToString("C");
                text.Foreground = price >= 0
                    ? new SolidColorBrush(Colors.Green)
                    : text.Foreground = new SolidColorBrush(Colors.Red);
            }
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