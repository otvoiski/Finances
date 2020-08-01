using CsvHelper;
using Finances.Facade;
using Finances.Model;
using Finances.Module;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
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

        // module
        private readonly IBillModule _billModule;

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

            // module
            _billModule = services.GetRequiredService<IBillModule>();

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

                    if (isSchedule)
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

        private void Button_Click_Import(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog
            {
                DefaultExt = ".csv",
                Filter = "CSV files (*.csv)|*.csv"
            };

            bool? result = open.ShowDialog();
            if (result == true)
            {
                using var reader = new StreamReader(open.FileName);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv.GetRecords<Bill>().ToList();

                var load = MessageBox.Show($"Do you want to load { records.Count } invoice(s)?", "Exported", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (load == MessageBoxResult.Yes)
                {
                    foreach (var item in records)
                    {
                        (Bill bill, string error) = _billModule.ValidateBill(
                            item.Date,
                            item.Description,
                            item.Installment,
                            item.Price.ToString(),
                            item.Type == "D"
                                ? "Debit Card"
                                : "Credit Card",
                            item.IsPaid);

                        if (bill != null)
                        {
                            if (!_billFacade.IsSchedule(bill).IsSchedule)
                            {
                                _billFacade.Save(bill);
                            }
                        }
                    }
                }

                MessageBox.Show($"Datas imported of the file {open.FileName}", "Exported", MessageBoxButton.OK, MessageBoxImage.Information);

                LoadWallet();
            }
        }

        private void Button_Click_Export(object sender, RoutedEventArgs e)
        {
            try
            {
                var save = new SaveFileDialog()
                {
                    DefaultExt = ".csv",
                    Filter = "CSV files (*.csv)|*.csv"
                };

                bool? result = save.ShowDialog();
                if (result == true)
                {
                    using StreamWriter writer = new StreamWriter(save.FileName);
                    using CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                    csv.WriteRecords(_billFacade.GetAllBills());

                    MessageBox.Show($"Datas exported in {save.FileName}", "Exported", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
    }
}