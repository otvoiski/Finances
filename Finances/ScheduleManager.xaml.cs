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
    /// Interaction logic for ScheduleManager.xaml
    /// </summary>
    public partial class ScheduleManager : Window, IScheduleManager
    {
        private readonly IFindBill _findBill;
        private readonly IScheduleFacade _scheduleFacade;
        private readonly IScheduleModule _scheduleModule;
        private int _scheduleId;
        private int _billID;

        public ScheduleManager(IScheduleFacade scheduleFacade, IFindBill findBill, IScheduleModule scheduleModule)
        {
            _findBill = findBill;
            _scheduleFacade = scheduleFacade;
            _scheduleModule = scheduleModule;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (Schedule schedule, string error) = _scheduleModule
                .Validate(
                    _scheduleId,
                    _billID,
                    Installment.Text,
                    StartDate.SelectedDate,
                    Active.IsChecked.GetValueOrDefault());
            if (schedule != null)
            {
                try
                {
                    _scheduleFacade.Save(schedule);

                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }
            }
            else
            {
                Error.Content = error;
            }
        }

        private void Button_Find(object sender, RoutedEventArgs e)
        {
            var findBill = _findBill.Factory();
            findBill.ShowDialog();

            var bill = findBill.Bill;
            if (bill != null)
            {
                _billID = bill.Id;

                Description.Text = bill.Description;
                Price.Text = bill.Price.ToString();

                ConfirmButton.IsEnabled = true;
            }
            else
            {
                ConfirmButton.IsEnabled = false;
            }
        }

        public ScheduleManager Factory()
        {
            InitializeComponent();

            Description.Text = "";
            _billID = 0;

            Description.Text = "";
            Price.Text = "";

            ConfirmButton.Content = "Add new schedule";
            ConfirmButton.IsEnabled = false;

            StartDate.SelectedDate = DateTime.Today;

            Installment.Text = "0";
            EndDate.Content = "∞";

            return this;
        }

        public ScheduleManager Factory(ScheduleInteface schedule)
        {
            InitializeComponent();

            _scheduleId = schedule.Id;

            Description.Text = schedule.Description;
            Price.Text = schedule.Price.ToString();

            Installment.Text = schedule.Installments.ToString();

            if (schedule.Installments == 0)
                EndDate.Content = "∞";
            else
                EndDate.Content = DateTime.Today.ToShortDateString();

            StartDate.SelectedDate = schedule.Start;

            Active.IsChecked = schedule.IsActive;

            ConfirmButton.Content = "Edit schedule";
            ConfirmButton.IsEnabled = true;
            return this;
        }

        private void Installment_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var text = e.Source as TextBox;

            if (int.TryParse(text.Text, out int installmentNumber) && installmentNumber > 0)
            {
                EndDate.Content = StartDate
                    .SelectedDate
                    .GetValueOrDefault()
                    .AddMonths(installmentNumber).ToShortDateString();
            }

            if (installmentNumber == 0 && EndDate != null)
            {
                EndDate.Content = "∞";
            }
        }
    }

    public interface IScheduleManager
    {
        ScheduleManager Factory();

        ScheduleManager Factory(ScheduleInteface schedule);
    }
}