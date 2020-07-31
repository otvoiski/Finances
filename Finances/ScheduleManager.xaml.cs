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
        private readonly IScheduleFacade _scheduleFacade;
        private readonly IScheduleModule _scheduleModule;
        private int _scheduleId;

        public ScheduleManager(IScheduleFacade scheduleFacade, IScheduleModule scheduleModule)
        {
            _scheduleFacade = scheduleFacade;
            _scheduleModule = scheduleModule;
        }

        public ScheduleManager Factory()
        {
            InitializeComponent();

            Description.Text = "";
            Price.Text = "-1";
            StartDate.SelectedDate = DateTime.Today;
            Installment.Text = "0";
            EndDate.Content = "∞";
            ConfirmButton.Content = "Add new schedule";
            Active.IsChecked = true;
            ConfirmButton.IsEnabled = false;
            return this;
        }

        public ScheduleManager Factory(ScheduleInteface schedule)
        {
            InitializeComponent();

            _scheduleId = schedule.Id;
            Description.Text = schedule.Description;
            Price.Text = schedule.Price.ToString();
            Installment.Text = schedule.Installments.ToString();
            StartDate.SelectedDate = schedule.Start;

            if (schedule.Installments == 0)
                EndDate.Content = "∞";
            else
                EndDate.Content = schedule
                    .Start
                    .AddMonths(schedule.Installments)
                    .ToShortDateString();
            Active.IsChecked = schedule.IsActive;
            ConfirmButton.Content = "Edit schedule";
            return this;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _scheduleId = 0;
            e.Cancel = true;
            Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (Schedule schedule, string error) = _scheduleModule
                .Validate(
                    _scheduleId,
                    Description.Text,
                    Price.Text,
                    Installment.Text,
                    StartDate.SelectedDate,
                    Active.IsChecked.GetValueOrDefault());
            if (schedule != null)
            {
                try
                {
                    schedule.Id = _scheduleId;

                    if (!_scheduleFacade.Save(schedule))
                    {
                        MessageBox.Show("Error on save schedule!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    Error.Content = "";

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

        private void Installment_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeEndDate((e.Source as TextBox).Text);
        }

        private void ChangeEndDate(string installment)
        {
            if (int.TryParse(installment, out int installmentNumber) && installmentNumber > 0)
            {
                EndDate.Content = StartDate
                    .SelectedDate
                    .GetValueOrDefault()
                    .AddMonths(installmentNumber - 1).ToShortDateString();
            }

            if (installmentNumber == 0 && EndDate != null)
            {
                EndDate.Content = "∞";
            }
        }

        private void StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeEndDate(Installment?.Text);
        }

        private void Description_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace((e.Source as TextBox)?.Text))
            {
                ConfirmButton.IsEnabled = false;
            }
            else
            {
                ConfirmButton.IsEnabled = true;
            }
        }
    }

    public interface IScheduleManager
    {
        ScheduleManager Factory();

        ScheduleManager Factory(ScheduleInteface schedule);
    }
}