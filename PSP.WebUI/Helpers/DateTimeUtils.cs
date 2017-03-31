using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PSP.WebUI.Helpers
{
    public class DateTimeUtils
    {
        public static int DaysInMonth(int Year, int Month)
        {
            return DateTime.DaysInMonth(Year, Month);
        }

        public static bool IsWeekend(DateTime Date)
        {
            return Date.DayOfWeek == DayOfWeek.Sunday || Date.DayOfWeek == DayOfWeek.Saturday;
        }

        public static DateTime GetYesterday()
        {
            return DateTime.Today.AddDays(-1);
        }

        public static DateTime GetTomorrow()
        {
            return DateTime.Today.AddDays(1);
        }

        public static DateTime GetNextDay(DateTime Date)
        {
            return Date.AddDays(1);
        }

        public static DateTime GetNextMonth(DateTime Date)
        {
            return Date.AddMonths(1);
        }

        public static DateTime GetNextYear(DateTime Date)
        {
            return Date.AddYears(1);
        }

        public static DateTime GetPrevDay(DateTime Date)
        {
            return Date.AddDays(-1);
        }

        public static DateTime GetPrevMonth(DateTime Date)
        {
            return Date.AddMonths(-1);
        }

        public static DateTime GetPrevYear(DateTime Date)
        {
            return Date.AddYears(-1);
        }

        public static DateTime GetFirstDayOfThisYear(int Year)
        {
            return new DateTime(DateTime.Today.Year, 1, 1);
        }

        public static DateTime GetFirstDayOfThisMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime GetLastDayOfThisYear(int Year)
        {
            var Date = new DateTime(DateTime.Today.Year + 1, 1, 1);
            return GetPrevDay(Date);
        }

        public static string ToYearMonthString(DateTime Time)
        {
            return string.Format("{0}-{1}", Time.Year, Time.Month);
        }

        public static string GetMonthName(int Month)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month);
        }

        public static string DateRangeToString(DateTime Start, DateTime End)
        {
            bool Sameyear = Start.Year == End.Year;
            bool Samemonth = Start.Month == End.Month;
            string Startmonthname = GetMonthName(Start.Month);
            string Endmonthname = GetMonthName(End.Month);
            if (Sameyear)
            {
                if (Samemonth)
                    return string.Format("{0}, {1}", Startmonthname, Start.Year);
                return string.Format("{0} - {1}, {2}", Startmonthname, Endmonthname, Start.Year);
            }
            return string.Format("{0}, {1} - {2}, {3}", Startmonthname, Start.Year, Endmonthname, End.Year);
        }
        public static DateTime GetEndDateOfMonth(DateTime date)
        {
            int Year = date.Year;
            int Month = date.Month;
            var EndDate = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month));
            return EndDate;
        }

        public static bool CheckWeekendByColumn(int column, DateTime startDate)
        {
            return DateTimeUtils.IsWeekend(startDate.AddDays(column));
        }

    }
}