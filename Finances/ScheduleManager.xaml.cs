using Finances.Facade;
using Finances.Model;
using Finances.Module;
using System;
using System.Windows;

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
        private Schedule _schedule;
        private Bill _bill;

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
                    _schedule.Id,
                    _bill.Id,
                    Installment.Text,
                    StartDate.SelectedDate,
                    Active.IsChecked.GetValueOrDefault());
            if (schedule != null)
            {
                _scheduleFacade.Save(schedule);
                Close();
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
                _bill = bill;
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

            ConfirmButton.Content = "Add new schedule";
            ConfirmButton.IsEnabled = false;

            return this;
        }

        public ScheduleManager Factory(Schedule schedule)
        {
            InitializeComponent();

            _schedule = schedule;

            EndDate.Content = DateTime.Today;

            ConfirmButton.Content = "Edit schedule";
            return this;
        }

        private void Installment_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (int.TryParse(Installment.Text, out int installmentNumber) && installmentNumber > 0)
            {
                EndDate.Content = DateTime.Today.AddMonths(installmentNumber);
            }
        }
    }

    public interface IScheduleManager
    {
        ScheduleManager Factory();

        ScheduleManager Factory(Schedule schedule);
    }
}