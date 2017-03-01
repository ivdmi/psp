using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSP.Domain;
using PSP.Domain.Abstract;
using PSP.Domain.Service;
using PSP.WebUI.Models;

namespace PSP.WebUI.Helpers
{
    public class AuditorEvents
    {
        private DateTime StartDate = DateTime.Now;
        private DateTime EndDate = DateTime.Now;
        
        private DataService dataService;
        public AuditorEvents(IRepository repository)
        {
            dataService = new DataService(repository);
        }


        public class RowMonth
        {
            public List<string> Columns { get; set; }
            public RowMonth()
            {
                Columns = new List<string>();
            }
        }

        private List<RowMonth> _auditorEvetsList = new List<RowMonth>();

        private RowMonth GetHeader()
        {
            RowMonth row = new RowMonth();
            int Year = StartDate.Year;
            int Month = StartDate.Month;
            int Day = StartDate.Day;

            row.Columns.Add("Ф.И.О.");
            for (int i = 0; i < (EndDate - StartDate).Days + 2; i++)
            {
                row.Columns.Add(Day.ToString());
                if (Day >= DateTimeUtils.DaysInMonth(Year, Month))
                {
                    Day = 1;
                    DateTime Next = DateTimeUtils.GetNextMonth(new DateTime(Year, Month, 1));
                    Year = Next.Year;
                    Month = Next.Month;
                }
                else
                    Day++;
            }
            return row;
        }

        public List<RowMonth> GetAuditorEventList(DateTime startDate)
        {
            StartDate = startDate;
            EndDate = DateTimeUtils.GetEndDateOfMonth(StartDate);
            _auditorEvetsList.Add(GetHeader());

            return _auditorEvetsList;
        }

    }
}