using System;
using System.Collections.Generic;
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
    /// Interaction logic for DeleteAppointment.xaml
    /// </summary>
    public partial class DeleteAppointment : Window
    {
        public DeleteAppointment()
        {
            InitializeComponent();
        }

        public Dashboard dashboardObject;


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

        private void Delete_Clicked(object sender, RoutedEventArgs e)
        {

            try
            {
                SharedDataHelp.DeleteAppointmentByID(Int32.Parse(ApptIDSearchBox.Text));
                MessageBox.Show("Appointment Deleted");
                dashboardObject.updateCalendar();
                Close();
            }
            catch
            {
                MessageBox.Show("Could Not Delete Appointment..");
            }
        }
    }
}
