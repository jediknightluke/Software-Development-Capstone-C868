using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;



namespace BOP3_Task_1_C_Sharp_Application_Development
{
    /// <summary>
    /// Interaction logic for AddAppointment.xaml
    /// </summary>
    public partial class AddAppointment : Window
    {
        public AddAppointment()
        {
            InitializeComponent();
        }

        private void StartTimeCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e) {

            startTimeTextBox.Text = startTimePick.SelectedDate.ToString();
            endTimeTextBox.Text = startTimePick.SelectedDate.ToString();

        }

        private void EndTimeCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {

            endTimeTextBox.Text = endTimePick.SelectedDate.ToString();

        }


        private void MonthlyCalendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e) { }
        private void MonthlyCalendar_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e) { }


        public Dashboard dashboardObject;

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }



        public static bool appIsOutsideBusinessHours(DateTime startTime, DateTime endTime)
        {
            startTime = startTime.ToLocalTime();
            endTime = endTime.ToLocalTime();
            DateTime businessStart = DateTime.Today.AddHours(8); // 8am
            DateTime businessEnd = DateTime.Today.AddHours(17); // 5pm
            if (startTime.TimeOfDay > businessStart.TimeOfDay && startTime.TimeOfDay < businessEnd.TimeOfDay &&
                endTime.TimeOfDay > businessStart.TimeOfDay && endTime.TimeOfDay < businessEnd.TimeOfDay)
                return false;

            return true;
        }

        private void addButton_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrWhiteSpace(customerIdTextBox.Text) ||
                String.IsNullOrWhiteSpace(typeTextBox.Text) ||
                String.IsNullOrWhiteSpace(startTimeTextBox.Text) ||
                String.IsNullOrWhiteSpace(endTimeTextBox.Text))
            {
                MessageBox.Show("Ensure all field are valid.");
            }




            DateTime timestamp = DateTime.Now;
            int userId = SharedDataHelp.getCurrentUserId();
            string username = SharedDataHelp.getCurrentUserName();

            DateTime startTime = DateTime.Parse(startTimeTextBox.Text).ToUniversalTime();
            DateTime endTime = DateTime.Parse(endTimeTextBox.Text).ToUniversalTime();

            Record record = new Record();
            record.customerId = customerIdTextBox.Text;
            record.end = endTime;
            record.start = startTime;
            record.table = "appointment";
            record.timestamp = timestamp;
            record.userName = username;
            record.type = typeTextBox.Text;
            record.userId = userId;



            try
            {
                if (SharedDataHelp.AppointmentConflictCheck(DateTime.Parse(SharedDataHelp.convertToTimezone(startTime.ToString())), DateTime.Parse(SharedDataHelp.convertToTimezone(endTime.ToString()))))
                    MessageBox.Show("Exception occured, appointment time conflics with another appointment");
                else
                {
                    try
                    {
                        if (SharedDataHelp.AppointmentBusinessHoursCheck(DateTime.Parse(SharedDataHelp.convertToTimezone(startTime.ToString())), DateTime.Parse(SharedDataHelp.convertToTimezone(endTime.ToString()))))
                            MessageBox.Show("Exception occured, appointments need to be within normal business hours");
                        else
                        {
                            SharedDataHelp.createRecord(record);
                            dashboardObject.updateCalendar();
                            Close();
                        }
                    }
                    catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddAppointment_Load(object sender, EventArgs e)
        {

        }

        public void businessHours()
        {
            MessageBox.Show("Exception occured, appointments need to be within normal business hours");
        }

        public void appOverlap()
        {
            MessageBox.Show("Exception occured, your appointment time is conflicting with an already present appointment");
        }


       
    }
}
