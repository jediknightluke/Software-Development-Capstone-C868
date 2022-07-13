using System;
using System.Collections.Generic;
using System.Text;

namespace BOP3_Task_1_C_Sharp_Application_Development
{
    class DataModels
    {
    }

    class TimingsReportDataClass
    {
        public string time { get; set; }
        public string count { get; set; }
    }

    class Record
    {

        public DateTime timestamp { get; set; }
        public string userName { get; set; }
        public string table { get; set; }

        public string customerId { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string type { get; set; }
        public int userId { get; set; }

    }

    class UpdateAppointmentDataClass
    {
        public int AppointmentID { get; set; }
        public int CustomerID { get; set; }
        public string type { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }

    }

    class AppointmentByMonthDataClass
    {
        public string MonthYear { get; set; }
        public string Type { get; set; }
        public string Count { get; set; }

    }

    class CreateCustomerRecordDataClass
    {
        public int customerID { get; set; }
        public string customerName { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string zipCode { get; set; }
        public string country { get; set; }
        public bool active { get; set; }

    }

    class CustomerDetailsDataClass
    {
        public string customerName { get; set; }
        public int customerId { get; set; }
        public int addressId { get; set; }
        public string active { get; set; }
        public string address { get; set; }
        public int cityId { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
        public string city { get; set; }
        public int countryId { get; set; }
        public string country { get; set; }
    }

    class UserReportsDataClass
    {
        public string userName { get; set; }
        public string type { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string customer { get; set; }

    }

    class ViewAllCustomersDataCall
    {
        public int customerId { get; set; }
        public string customerName { get; set; }
        public string active { get; set; }
        public DateTime createDate { get; set; }
        public string createdBy { get; set; }
    }

    class AppointmentReminderDataClass
    {
        public bool trueFalse { get; set; }
        public string customerName { get; set; }
        public DateTime start { get; set; }
    }

    class AppointmentDateCheckDataClass
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
    }
}
