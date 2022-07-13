using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for UpdateAppointment.xaml
    /// </summary>
    public partial class UpdateAppointment : Window
    {
        public UpdateAppointment()
        {
            InitializeComponent();
        }

        public Dashboard dashboardObject;

        private void StartTimeCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {

            startTimeTextBox.Text = startTimePick.SelectedDate.ToString();

        }

        private void EndTimeCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {

            endTimeTextBox.Text = endTimePick.SelectedDate.ToString();

        }


        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UpdateClick(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrWhiteSpace(CustomerID.Text) ||
                String.IsNullOrWhiteSpace(apptTypeTextBox.Text) ||
                String.IsNullOrWhiteSpace(startTimeTextBox.Text) ||
                String.IsNullOrWhiteSpace(endTimeTextBox.Text))
            {
                MessageBox.Show("Ensure all field are valid.");
            }

            DateTime startTime = DateTime.Parse(startTimeTextBox.Text).ToUniversalTime();
            DateTime endTime = DateTime.Parse(endTimeTextBox.Text).ToUniversalTime();


            UpdateAppointmentDataClass apptUpdate = new UpdateAppointmentDataClass();
            apptUpdate.CustomerID = Int32.Parse(CustomerID.Text);
            apptUpdate.AppointmentID = Int32.Parse(ApptIDSearchBox.Text);
            apptUpdate.type = apptTypeTextBox.Text;
            apptUpdate.start = startTime;
            apptUpdate.end = endTime;

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
                            SharedDataHelp.updateAppointment(apptUpdate);

                            MessageBox.Show("Record Updated");
                            dashboardObject.updateCalendar();
                            Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Could not update appointment");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not update appointment");
            }



        }

        private void AppointmentSearch(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateAppointmentDataClass apptSearch = SharedDataHelp.getAppointmentDetails(Int32.Parse(ApptIDSearchBox.Text));
                CustomerID.Text = apptSearch.CustomerID.ToString();
                apptTypeTextBox.Text = apptSearch.type;
                startTimeTextBox.Text = apptSearch.start.ToString();
                endTimeTextBox.Text = apptSearch.end.ToString();

            }
            catch
            {
                MessageBox.Show("Could not search the ID.");
            }
            

        }

    }
}
