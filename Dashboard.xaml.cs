using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        public MainWindow loginForm;

        public class DashboardLayout
        {
            public string UserName { get; set; }
            public string AppointmentID { get; set; }
            public string CustomerID { get; set; }
            public string AppointmentType { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }

        }
        public Dashboard()
        {
            InitializeComponent();
            SharedDataHelp.LogToFile(String.Format("User {0} Logged in: {1}", SharedDataHelp.getCurrentUserName(), DateTime.Now.ToString()));
            WelcomeBlock.Text = "Welcome back, " + SharedDataHelp.getCurrentUserName() + "!";
            weekRadioButton.IsChecked = true;
            appointmentCalendar.DataContext = getCalendar("week").OrderByDescending(x => x.AppointmentID); // Lambda Function to sort Calendar results

            AppointmentReminderDataClass appointmentCheck = SharedDataHelp.AppointmentReminders();
            if (appointmentCheck.trueFalse)
            {
                MessageBox.Show(String.Format("REMINDER: You have an appointment with {0} at {1}", appointmentCheck.customerName, appointmentCheck.start.ToString()));
            }
            else
            {
                return;
            }
        }
        static public void reminderCheck(DataGrid calendar)
        {
            if (calendar.Items.Count == 0)
            {
                MessageBox.Show(calendar.ItemsSource.ToString());
                return;
            }
            else
            {
                foreach (DataGridRow row in calendar.ItemsSource)
                {
                    DateTime now = DateTime.UtcNow;
                    DateTime start = DateTime.Parse(row.Item.ToString()).ToUniversalTime();
                    //DateTime start = DateTime.Parse(row.Item.ToString()).ToUniversalTime();
                    TimeSpan nowUntilStartOfApp = now - start;
                    if (nowUntilStartOfApp.TotalMinutes >= -15 && nowUntilStartOfApp.TotalMinutes < 1)
                    {
                        //MessageBox.Show($"Reminder: You have a meeting with {row.Cells[4].Value} at {row.Cells[2].Value}");
                        MessageBox.Show($"Reminder: You have a meeting with");
                    }
                }
            }


        }


        static public List<DashboardLayout> getCalendar(string viewType)
        {

            string query = "";
            if (viewType.Equals("week"))
            {
                query = $@"SELECT a.customerId, a.type, a.start, a.end, a.appointmentId, u.userName 
                            FROM appointment a
                            JOIN user u on a.userId = u.userId
                            WHERE a.start >= date_sub(curdate(), interval weekday(curdate()) day) and
                                  a.start <= date_sub(curdate(), interval weekday(curdate()) - 7 day)";
            }
            if (viewType.Equals("month"))
            {
                query = $@"SELECT a.customerId, a.type, a.start, a.end, a.appointmentId, u.userName 
                            FROM appointment a
                            JOIN user u on a.userId = u.userId
                            WHERE u.userid = '{SharedDataHelp.getCurrentUserId()}'
                            AND MONTH(start) = MONTH(CURRENT_DATE())";
            }
            if (viewType.Equals("all"))
            {
                query = $@"SELECT a.customerId, a.type, a.start, a.end, a.appointmentId, u.userName 
                            FROM appointment a";
            }

            MySqlConnection c = new MySqlConnection(SharedDataHelp.conString);
            c.Open();
            MySqlCommand cmd = new MySqlCommand(query, c);
            MySqlDataReader rdr = cmd.ExecuteReader();

            List<DashboardLayout> layout = new List<DashboardLayout>();

            // Creates a dictionary of all the appointments
            while (rdr.Read())
            {
                DashboardLayout dashboardList = new DashboardLayout();

                dashboardList.CustomerID = rdr[0].ToString();
                dashboardList.AppointmentType = rdr[1].ToString();
                dashboardList.StartTime = SharedDataHelp.convertToTimezone(rdr[2].ToString());
                dashboardList.EndTime = SharedDataHelp.convertToTimezone(rdr[3].ToString());
                dashboardList.AppointmentID = rdr[4].ToString();
                dashboardList.UserName = rdr[5].ToString();

                layout.Add(dashboardList);


            }


            Debug.WriteLine(layout.Count);
            return layout;
        }

        public void updateCalendar()
        {

            appointmentCalendar.DataContext = getCalendar("week").OrderByDescending(x => x.AppointmentID); // Lambda Function to sort Calendar results
        }

        private void createCustomerButton_Click(object sender, EventArgs e)
        {
            CreateCustomer createCustomer = new CreateCustomer();
            createCustomer.Show();
        }

        private void updateCustomerButton_Click(object sender, EventArgs e)
        {
            UpdateCustomer updateCustomer = new UpdateCustomer();
            updateCustomer.Show();
        }

        private void deleteCustomerButton_Click(object sender, EventArgs e)
        {
            DeleteCustomer deleteCustomer = new DeleteCustomer();
            deleteCustomer.Show();
        }

        private void MainForm_FormClosing(object sender)
        {
            loginForm.Close();
        }

        private void weekRadioButton_Checked(object sender, EventArgs e)
        {
            appointmentCalendar.DataContext = getCalendar("week").OrderByDescending(x => x.StartTime);
        }

        private void addAppointment_Click(object sender, EventArgs e)
        {
            AddAppointment addAppointment = new AddAppointment();
            addAppointment.dashboardObject = this;
            addAppointment.Show();
        }

        private void updateAppointment_Click(object sender, EventArgs e)
        {
            UpdateAppointment updateAppointment = new UpdateAppointment();
            updateAppointment.dashboardObject = this;
            updateAppointment.Show();
        }

        private void deleteAppointment_Click(object sender, EventArgs e)
        {
            DeleteAppointment deleteAppointment = new DeleteAppointment();
            deleteAppointment.dashboardObject = this;
            deleteAppointment.Show();
        }

        private void numberOfApps_Click(object sender, EventArgs e)
        {
            NumberOfAppointmentsReport numberOfAppointments = new NumberOfAppointmentsReport();
            numberOfAppointments.Show();
        }

        private void userSchedules_Click(object sender, EventArgs e)
        {
            UserSchedules userSchedules = new UserSchedules();
            userSchedules.Show();
        }

        private void timingsReport_Click(object sender, EventArgs e)
        {
            TimingsReport timingsReport = new TimingsReport();
            timingsReport.Show();
        }
        private void MonthViewRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (monthViewRadioButton.IsChecked == true)
            {
                appointmentCalendar.DataContext = getCalendar("month");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void viewAllButton_Click(object sender, RoutedEventArgs e)
        {
            ViewAllCustomers viewAllCustomers = new ViewAllCustomers();
            viewAllCustomers.Show();
        }

        private void viewAllRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (viewAllRadioButton.IsChecked == true)
            {
                appointmentCalendar.DataContext = getCalendar("all").OrderByDescending(x => x.StartTime);
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            foreach (DashboardLayout part in getCalendar("week").OrderByDescending(x => x.AppointmentID))
            {
                if (part.AppointmentType.Contains(SearchBox.Text))
                {
                    //filterModeLisst.Add(part);
                }
            }

            //partsGrid.ItemsSource = filterModeLisst.ToList();


        }


    }
}
