using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace BOP3_Task_1_C_Sharp_Application_Development



{


       public class SharedDataHelp
        {
            private static int _currentUserId;
            private static string _currentUserName;
            TimeZone localZone = TimeZone.CurrentTimeZone;
            //public static string conString = "server=127.0.0.1 ;database=client_schedule;uid=sqlUser;pwd=Passw0rd!;";

            public static string uid = "root";
            public static string pwd = "test";
            public static string server = "127.0.0.1";
            public static string database = "u05jyp";
            public static string conString = $"server={server};database={database};uid={uid};pwd={pwd}";



        public static int getCurrentUserId()
            {
                return _currentUserId;
            }

            public static void setCurrentUserId(int currentUserId)
            {
                _currentUserId = currentUserId;
            }

            public static string getCurrentUserName()
            {
                return _currentUserName;
            }

            public static void setCurrentUserName(string currentUserName)
            {
                _currentUserName = currentUserName;
            }

            public static List<AppointmentByMonthDataClass> getAppointments()
            {
            List<AppointmentByMonthDataClass> apptByMonthList = new List<AppointmentByMonthDataClass>();


            string query = $@"select date_format(start, '%M %Y') as 'MonthYear', `type`, count(type)
                               from appointment
                               GROUP BY `type`,  date_format(start, '%M %Y')
                               ORDER BY start asc";
            MySqlConnection c = new MySqlConnection(SharedDataHelp.conString);
            c.Open();
            MySqlCommand cmd = new MySqlCommand(query, c);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                AppointmentByMonthDataClass apptByMonthReport = new AppointmentByMonthDataClass();
                apptByMonthReport.MonthYear = rdr[0].ToString();
                apptByMonthReport.Type = rdr[1].ToString();
                apptByMonthReport.Count = rdr[2].ToString();

                apptByMonthList.Add(apptByMonthReport);
            }


            return apptByMonthList;
            }


            public static int newID(List<int> idList)
            {
                int highestID = 0;
                foreach (int id in idList)
                {
                    if (id > highestID)
                        highestID = id;
                }
                return highestID + 1;
            }

            public static string createTimestamp()
            {
                return DateTime.Now.ToString();
            }

            public static int createID(string table)
            {
                MySqlConnection c = new MySqlConnection(conString);
                c.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT {table + "Id"} FROM {table}", c);
                MySqlDataReader rdr = cmd.ExecuteReader();
                List<int> idList = new List<int>();
                while (rdr.Read())
                {
                    idList.Add(Convert.ToInt32(rdr[0]));
                }
                rdr.Close();
                c.Close();
                return newID(idList);
            }


            static public int createRecord(Record record)
            {
                int recId = createID(record.table);
                string recInsert = "";
                try
                {

                    recInsert = $"INSERT INTO {record.table} (appointmentId, customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy)" +
                    $" VALUES ('{recId}', '{record.customerId}', " +
                    $"{getCurrentUserId()},'' ,'' ,'' ,'' ,'{record.type}','' ,STR_TO_DATE('{record.start}', '%m/%d/%Y %h:%i:%s %p'), STR_TO_DATE('{record.end}', '%m/%d/%Y %h:%i:%s %p'), " +
                    $"STR_TO_DATE('{record.timestamp}', '%m/%d/%Y %h:%i:%s %p')," +
                    $" '{record.userName}', STR_TO_DATE('{record.timestamp}', '%m/%d/%Y %h:%i:%s %p'), '{record.userName}')";
                    Debug.WriteLine(recInsert);


                }
                catch
                {

                }

                MySqlConnection c = new MySqlConnection(conString);
                c.Open();
                MySqlCommand cmd = new MySqlCommand(recInsert, c);
                cmd.ExecuteNonQuery();
                c.Close();

                return recId;
            }

            static public int findCustomer(string search)
            {
                int customerId;
                string query;
                if (int.TryParse(search, out customerId))
                {
                    query = $"SELECT customerId FROM customer WHERE customerId = '{search.ToString()}'";
                }
                else
                {
                    query = $"SELECT customerId FROM customer WHERE customerName LIKE '{search}'";
                }
                MySqlConnection c = new MySqlConnection(SharedDataHelp.conString);
                c.Open();
                MySqlCommand cmd = new MySqlCommand(query, c);
                MySqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    rdr.Read();
                    customerId = Convert.ToInt32(rdr[0]);
                    rdr.Close(); c.Close();
                    return customerId;
                }
                return 0;
            }

            static public CustomerDetailsDataClass getCustomerDetails(int customerId)
            {

                CustomerDetailsDataClass customerDetails = new CustomerDetailsDataClass();

                string query = $"SELECT * FROM customer WHERE customerId = '{customerId}'";
                MySqlConnection c = new MySqlConnection(SharedDataHelp.conString);
                c.Open();
                MySqlCommand cmd = new MySqlCommand(query, c);
                MySqlDataReader rdr = cmd.ExecuteReader();
                rdr.Read();

                Debug.WriteLine(rdr[3].ToString());

                // Customer Table Details
                customerDetails.customerName = rdr[1].ToString();
                customerDetails.addressId = Int32.Parse(rdr[2].ToString());
                customerDetails.active = rdr[3].ToString();
                rdr.Close();

                query = $"SELECT * FROM address WHERE addressId = '{customerDetails.addressId}'";
                cmd = new MySqlCommand(query, c);
                rdr = cmd.ExecuteReader();
                rdr.Read();

                // Address Table Details
                customerDetails.address = rdr[1].ToString();
                customerDetails.cityId = Int32.Parse(rdr[3].ToString());
                customerDetails.zip = rdr[4].ToString();
                customerDetails.phone = rdr[5].ToString();
                rdr.Close();

                query = $"SELECT * FROM city WHERE cityId = '{customerDetails.cityId}'";
                cmd = new MySqlCommand(query, c);
                rdr = cmd.ExecuteReader();
                rdr.Read();

                // City Table Details
                customerDetails.city = rdr[1].ToString();
                customerDetails.countryId = Int32.Parse(rdr[2].ToString());
                rdr.Close();

                query = $"SELECT * FROM country WHERE countryId = '{customerDetails.countryId}'";
                cmd = new MySqlCommand(query, c);
                rdr = cmd.ExecuteReader();
                rdr.Read();

                // Country Table Details
                customerDetails.country = rdr[1].ToString();
                rdr.Close();
                c.Close();

                return customerDetails;
            }

            public static bool CreateCustomer(CreateCustomerRecordDataClass record, int addressId)
            {

            int recId = createID("customer");

            string query = $@"INSERT INTO `customer`
                                (`customerId`,
                                `customerName`,
                                `addressId`,
                                `active`,
                                `createDate`,
                                `createdBy`,
                                `lastUpdate`,
                                `lastUpdateBy`)
                                VALUES
                                (
                                  '{recId}',
                                  '{record.customerName}',
                                  '{addressId}',
                                  '{(record.active ? 1 : 0)}',
                                  '{DateTime.Parse(DateTime.Now.ToString()).ToString("yyyy-M-dd HH:mm:ss", CultureInfo.CurrentCulture)}',
                                  '{getCurrentUserName()}',
                                  '{DateTime.Parse(DateTime.Now.ToString()).ToString("yyyy-M-dd HH:mm:ss", CultureInfo.CurrentCulture)}',
                                  '{getCurrentUserName()}'

                                 )";

            try
            {


                MySqlConnection c = new MySqlConnection(conString);
                c.Open();
                MySqlCommand cmd = new MySqlCommand(query, c);
                cmd.ExecuteNonQuery();
                c.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }



        public static bool UpdateCustomer(CustomerDetailsDataClass record)
        {

            string addressQuery = $@"UPDATE `address`
                                SET
                                `address` = '{record.address}',
                                `postalCode` = '{record.zip}',
                                `phone` = '{record.phone}',
                                `lastUpdate` = '{DateTime.Parse(DateTime.Now.ToString()).ToString("yyyy-M-dd HH:mm:ss", CultureInfo.CurrentCulture)}',
                                `lastUpdateBy` = '{getCurrentUserName()}'
                                WHERE addressId = '{record.addressId}'";
            Debug.WriteLine(addressQuery);

            try
            {
                MySqlConnection c = new MySqlConnection(SharedDataHelp.conString);
                c.Open();
                MySqlCommand cmd = new MySqlCommand(addressQuery, c);
                cmd.ExecuteReader();
                c.Close();
            }
            catch
            {
                Debug.WriteLine("Failure with Address");
                return false;
            }


            string cityQuery = $@"UPDATE `city`
                                    SET
                                    `city` = '{record.city}',
                                    `lastUpdate` = '{DateTime.Parse(DateTime.Now.ToString()).ToString("yyyy-M-dd HH:mm:ss", CultureInfo.CurrentCulture)}',
                                    `lastUpdateBy` = '{getCurrentUserName()}'
                                    WHERE cityId = '{record.cityId}'";
            Debug.WriteLine(cityQuery);

            try
            {
                MySqlConnection c = new MySqlConnection(conString);
                c.Open();
                MySqlCommand cityCmd = new MySqlCommand(cityQuery, c);
                cityCmd.ExecuteReader();
                c.Close();

            }
            catch
            {
                Debug.WriteLine("Failure with city");
                return false;
            }


            string countryQuery = $@"UPDATE `country`
                                    SET
                                    `country` = '{record.country}',
                                    `lastUpdate` = '{DateTime.Parse(DateTime.Now.ToString()).ToString("yyyy-M-dd HH:mm:ss", CultureInfo.CurrentCulture)}',
                                    `lastUpdateBy` = '{getCurrentUserName()}'

                                    WHERE countryId = '{record.countryId}'";
            Debug.WriteLine(countryQuery);

            try
            {
                MySqlConnection c = new MySqlConnection(conString);
                c.Open();
                MySqlCommand countryCmd = new MySqlCommand(countryQuery, c);
                countryCmd.ExecuteReader();
                c.Close();
            }
            catch
            {
                Debug.WriteLine("Failure with country");
                return false;
            }

            string customerQuery = $@"UPDATE `customer`
                                        SET
                                        `customerName` = '{record.customerName}',
                                        `active` = {record.active},
                                        `lastUpdate` = '{DateTime.Parse(DateTime.Now.ToString()).ToString("yyyy-M-dd HH:mm:ss", CultureInfo.CurrentCulture)}',
                                        `lastUpdateBy` = '{getCurrentUserName()}'

                                        WHERE customerId = '{record.customerId}'";
            Debug.WriteLine(customerQuery);
            try
            {
                MySqlConnection c = new MySqlConnection(conString);
                c.Open();
                MySqlCommand customerCmd = new MySqlCommand(customerQuery, c);
                customerCmd.ExecuteReader();
                c.Close();
            }
            catch
            {
                Debug.WriteLine("Failure with customer");
                return false;
            }

            return true;



        }


        public static bool DeleteCustomer(int customerId)
        {
            MySqlConnection c = new MySqlConnection(conString);
            c.Open();

            string addressIdquery = $@"SELECT `addressId` from customer where `customerId` = '{customerId}'";
            MySqlCommand cmd = new MySqlCommand(addressIdquery, c);
            MySqlDataReader rdr = cmd.ExecuteReader();
            rdr.Read();
            

            string addressId = rdr[0].ToString();
            rdr.Close();

            string cityIdquery = $@"SELECT `cityId` from address where `addressId` = '{addressId}'";
            cmd = new MySqlCommand(cityIdquery, c);
            rdr = cmd.ExecuteReader();
            rdr.Read();
            

            string cityId = rdr[0].ToString();
            rdr.Close();

            string countryIdquery = $@"SELECT `countryId` from city where `cityId` = '{cityId}'";
            cmd = new MySqlCommand(countryIdquery, c);
            rdr = cmd.ExecuteReader();
            rdr.Read();
            

            string countryId = rdr[0].ToString();
            rdr.Close();


            //Deleting Records
            try
            {
                DropRecordByID("address", addressId);
                DropRecordByID("city", cityId);
                DropRecordByID("country", countryId);
                DropRecordByID("customer", customerId.ToString());
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool DropRecordByID(string table, string id)
        {
            MySqlConnection c = new MySqlConnection(conString);
            c.Open();

            try
            {
                string query = $@"DELETE FROM {table} where `{table}Id` = '{id}'";
                MySqlCommand cmd = new MySqlCommand(query, c);
                cmd.ExecuteNonQuery();
                c.Close();
            }
            catch
            {
                Debug.WriteLine("Failed to delete the record.");
                return false;
            }


            return true;
        }




            static public int createRecordID(string timestamp, string userName, string table, string partOfQuery)
            {
                int recId = createID(table);
                string recInsert;

                recInsert = $"INSERT INTO {table}" +
                $" VALUES ('{recId}', {partOfQuery}, '{timestamp}', '{userName}', '{timestamp}', '{userName}')";



                MySqlConnection c = new MySqlConnection(conString);
                c.Open();
                MySqlCommand cmd = new MySqlCommand(recInsert, c);
                cmd.ExecuteNonQuery();
                c.Close();

                return recId;
            }

            static public UpdateAppointmentDataClass getAppointmentDetails(int appointmentId)
            {
                string query = $"SELECT * FROM appointment WHERE appointmentId = '{appointmentId}'";
                MySqlConnection c = new MySqlConnection(SharedDataHelp.conString);
                c.Open();
                MySqlCommand cmd = new MySqlCommand(query, c);
                MySqlDataReader rdr = cmd.ExecuteReader();
                rdr.Read();

                UpdateAppointmentDataClass appt = new UpdateAppointmentDataClass();
                // Customer Table Details
                appt.AppointmentID = appointmentId;
                appt.CustomerID = Int32.Parse(rdr[1].ToString());
                appt.type = rdr[7].ToString();
                appt.start = DateTime.Parse(convertToTimezone(rdr[9].ToString()));
                appt.end = DateTime.Parse(convertToTimezone(rdr[10].ToString()));
                rdr.Close();

                return appt;
            }

            static public void updateAppointment(UpdateAppointmentDataClass appointment)
            {
                string query = $@"UPDATE appointment
                              SET
                                `appointmentId` = {appointment.AppointmentID},
                                `customerId` = {appointment.CustomerID},
                                `type` = '{appointment.type}',
                                `start` = '{appointment.start.ToString("yyyy-M-dd HH:mm:ss")}',
                                `end` = '{appointment.end.ToString("yyyy-M-dd HH:mm:ss")}',
                                `lastUpdate` = '{DateTime.Now.ToString("yyyy-M-dd HH:mm:ss")}',
                                `lastUpdateBy` = '{getCurrentUserName()}'
                              WHERE appointmentId = '{appointment.AppointmentID}'";

                Debug.WriteLine(query);


                MySqlConnection c = new MySqlConnection(SharedDataHelp.conString);
                c.Open();
                MySqlCommand cmd = new MySqlCommand(query, c);
                MySqlDataReader rdr = cmd.ExecuteReader();


            }

            static public string convertToTimezone(string dateTime)
            {
                DateTime utcDateTime = DateTime.Parse(dateTime);
                DateTime localDateTime = utcDateTime.ToLocalTime();

                return localDateTime.ToString("MM/dd/yyyy hh:mm tt");
            }


            public static List<UserReportsDataClass> getUserReport()
            {

                List<UserReportsDataClass> userReportList = new List<UserReportsDataClass>();
                string query = $@"select u.userName, appt.`type`, appt.start, appt.end, cust.customerName
                                   from appointment appt
                                   join user u 
                                   on appt.userId = u.userId
                                   join customer cust
                                   on appt.customerId = cust.customerId
                                   Order By appt.userId, appt.start
                                   ";

                MySqlConnection c = new MySqlConnection(SharedDataHelp.conString);
                c.Open();
                MySqlCommand cmd = new MySqlCommand(query, c);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    UserReportsDataClass userReport = new UserReportsDataClass();
                    userReport.userName = rdr[0].ToString();
                    userReport.type = rdr[1].ToString();
                    userReport.startTime = DateTime.Parse(convertToTimezone(rdr[2].ToString()));
                    userReport.endTime = DateTime.Parse(convertToTimezone(rdr[3].ToString()));
                    userReport.customer = rdr[4].ToString();

                    userReportList.Add(userReport);

                }

            return userReportList;
            }

        public static List<TimingsReportDataClass> GetTimingsReport()
        {
            List<TimingsReportDataClass> timingReportList = new List<TimingsReportDataClass>();
            string query = $@"SELECT DATE_FORMAT(start, '%H:%i %p') as `time`, count(DATE_FORMAT(start, '%H:%i %p')) FROM appointment
                                GROUP BY DATE_FORMAT(start, '%H:%i %p')
                                ORDER BY DATE_FORMAT(start, '%H:%i %p') DESC";
            MySqlConnection c = new MySqlConnection(SharedDataHelp.conString);
            c.Open();
            MySqlCommand cmd = new MySqlCommand(query, c);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                TimingsReportDataClass timingReport = new TimingsReportDataClass();
                timingReport.time = DateTime.Parse(convertToTimezone(rdr[0].ToString())).ToString("h:mm tt");
                timingReport.count = rdr[1].ToString();

                timingReportList.Add(timingReport);

            }

            return timingReportList;
        }



        public static void LogToFile(string message)
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

            string path = String.Format("{0}\\{1}", projectDirectory, "Log\\Log.txt");
            using (StreamWriter sw = File.Exists(path) ? File.AppendText(path) : File.CreateText(path))
            {
                sw.WriteLine(message);
            }
        }

        public static IOrderedEnumerable<ViewAllCustomersDataCall> ViewAllCustomers()
        {
            List<ViewAllCustomersDataCall> viewAllCustomersList = new List<ViewAllCustomersDataCall>();


            string query = $@"SELECT customerId, customerName, active, createDate, createdBy FROM customer;";
            MySqlConnection c = new MySqlConnection(SharedDataHelp.conString);
            c.Open();
            MySqlCommand cmd = new MySqlCommand(query, c);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                ViewAllCustomersDataCall viewAllCustomerItem = new ViewAllCustomersDataCall();
                viewAllCustomerItem.customerId = Int32.Parse(rdr[0].ToString());
                viewAllCustomerItem.customerName = rdr[1].ToString();
                viewAllCustomerItem.active = rdr[2].ToString();
                viewAllCustomerItem.createDate = DateTime.Parse(convertToTimezone(rdr[3].ToString()));
                viewAllCustomerItem.createdBy = rdr[4].ToString();

                viewAllCustomersList.Add(viewAllCustomerItem);
            }

            var newDetails = viewAllCustomersList.OrderBy(x => x.customerName); //Lambda to order Customer name alphabetically
            return newDetails;
        }

        public static AppointmentReminderDataClass AppointmentReminders()
        {
            AppointmentReminderDataClass appointmentReminder = new AppointmentReminderDataClass();
            DateTime endDate = DateTime.Now.AddMinutes(15);
            DateTime startDate = DateTime.Now;

            string query = $@"SELECT c.customerName, a.start
                                FROM appointment a
                                JOIN customer c on a.customerId = c.customerId
                                WHERE DATE(start) = CURDATE()";
            MySqlConnection c = new MySqlConnection(SharedDataHelp.conString);
            c.Open();
            MySqlCommand cmd = new MySqlCommand(query, c);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                if (DateTime.Parse(rdr[1].ToString()) >= startDate && DateTime.Parse(rdr[1].ToString()) < endDate)
                {
                    appointmentReminder.trueFalse = true;
                    appointmentReminder.customerName = rdr[0].ToString();
                    appointmentReminder.start = DateTime.Parse(convertToTimezone(rdr[1].ToString()));

                    return appointmentReminder;
                }



            }
            appointmentReminder.trueFalse = false;
            return appointmentReminder;

        }

        public static bool AppointmentConflictCheck(DateTime start, DateTime end)
        {

            string query = $@"SELECT start, end FROM appointment;";
            MySqlConnection c = new MySqlConnection(SharedDataHelp.conString);
            c.Open();
            MySqlCommand cmd = new MySqlCommand(query, c);
            MySqlDataReader rdr = cmd.ExecuteReader();

            List<AppointmentDateCheckDataClass> appointmentCheckList = new List<AppointmentDateCheckDataClass>();

            while (rdr.Read())
            {
                AppointmentDateCheckDataClass appontmentCheck = new AppointmentDateCheckDataClass();
                appontmentCheck.start = DateTime.Parse(convertToTimezone(rdr[0].ToString()));
                appontmentCheck.end = DateTime.Parse(convertToTimezone(rdr[1].ToString()));
                appointmentCheckList.Add(appontmentCheck);
            }

            var ff = (from checklist in appointmentCheckList where checklist.start >= start && checklist.start <= end select checklist.start);
            if (ff.Any())
            {
                return true;
            }

            return false;
        }


        public static bool AppointmentBusinessHoursCheck(DateTime startTime, DateTime endTime)
        {

            DateTime businessStart = DateTime.Today.AddHours(8); // 8am
            DateTime businessEnd = DateTime.Today.AddHours(17); // 5pm
            if (startTime.TimeOfDay > businessStart.TimeOfDay && startTime.TimeOfDay < businessEnd.TimeOfDay &&
                endTime.TimeOfDay > businessStart.TimeOfDay && endTime.TimeOfDay < businessEnd.TimeOfDay)
                return false;
                Debug.WriteLine(string.Format("{0} or {1} is outside {2} or {3}", startTime, endTime, businessStart, businessEnd));

            return true;


        }


        public static bool DeleteAppointmentByID(int appointmentID)
        {
            MySqlConnection c = new MySqlConnection(conString);
            c.Open();

            try
            {
                string query = $@"DELETE FROM appointment where `appointmentId` = '{appointmentID}'";
                MySqlCommand cmd = new MySqlCommand(query, c);
                cmd.ExecuteNonQuery();
                c.Close();
            }
            catch
            {
                Debug.WriteLine("Failed to delete the appointment.");
                return false;
            }


            return true;
        }





    }

}
    