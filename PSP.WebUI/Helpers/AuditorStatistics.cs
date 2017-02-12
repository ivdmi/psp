using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSP.Domain;
using PSP.Domain.Service;
using PSP.WebUI.Models;

namespace PSP.WebUI.Helpers
{
    public static class AuditorStatistics
    {
//        public delegate void OnAddAuditorStatsElement(string Auditor, string State, int Days, int CalendarDays, int TotalMinutes, string Factories);

        private class SingleAuditorStats
        {
            public SingleAuditorStats()
            {
                Factories = new List<FactoryItem>();
                UniqueDates = new List<DateTime>();
            }

            // Имя аудитора
            public string Auditor { set; get; }
            // Состояние
            public int State { set; get; }

            // Подсчет количество посещенных предприятий
            public class FactoryItem
            {
                public string Name { set; get; }
                public int Count { set; get; }
                public int Minutes { set; get; }
            }

            public List<FactoryItem> Factories { private set; get; }

            public int TotalMinutes()
            {
                return Factories.Sum(Item => Item.Minutes);
            }

            public int GetDays()
            {
                return Factories.Sum(Item => Item.Count);
            }

            public void IncrementFactoryCounter(string Factory, int Minutes)
            {
                FactoryItem Item = Factories.Where(P => P.Name == Factory).Select(P => P).FirstOrDefault();
                if (null == Item)
                {
                    Factories.Add(new FactoryItem { Name = Factory, Count = 1, Minutes = Minutes });
                }
                else
                {
                    Item.Count++;
                    Item.Minutes += Minutes;
                }
            }

            // Подсчет совмещенных дней
            private List<DateTime> UniqueDates { set; get; }

            public void AddDate(DateTime Date)
            {
                if (UniqueDates.Any(UniqueDate => Date == UniqueDate))
                    return;
                UniqueDates.Add(Date);
            }

            public int GetRealDays()
            {
                return UniqueDates.Count;
            }
        }
        
        private static List<GridViewDataAuditorRowInfo> auditorStatisticsResult = new List<GridViewDataAuditorRowInfo>();

        private static int _rowCount = 0;
        public static void AddAuditorStatsLine(string Auditor, string State, int Days, int CalendarDays, int TotalMinutes, string Factories)
        {
            GridViewDataAuditorRowInfo auditorRow = new GridViewDataAuditorRowInfo();
            auditorRow.Count = ++_rowCount;
            auditorRow.Auditor = Auditor;
            auditorRow.State = State;
            auditorRow.Days = Days;
            auditorRow.CalendarDays = CalendarDays;
            auditorRow.TotalMinutes = EventHelper.FormatMinutes(TotalMinutes);
            auditorRow.Factories = Factories;
            auditorStatisticsResult.Add(auditorRow);
        }

        public static List<GridViewDataAuditorRowInfo> Get(DateTime startDate, DateTime endDate)
        {
            // Получить всех пользователей и все события за промежуток времени
            List<users> AllUsers = DataService.GetAllUsers();
            List<events> AllEvents = DataService.GetEventsByDate(startDate, endDate);

            // Получить имя аудитора по идентификатору
            Func<string, string> GetAuditorName = K => (from User in AllUsers where User.ID.ToLower() == K.ToLower() select User.Name).FirstOrDefault();

            Func<DateTime, bool> HasEventsAtDate = D => AllEvents.Any(Event => Event.Date == D);

            var auditorStats = new List<SingleAuditorStats>();
            Func<events, string, int, int, SingleAuditorStats> AddDay = (E, F, Minutes, State) =>
            {
                foreach (SingleAuditorStats stats in auditorStats)
                {
                    if (stats.Auditor == GetAuditorName(E.UserID) && stats.State == State)
                    {
                        stats.IncrementFactoryCounter(F, /*E.WorkTime*/Minutes);
                        if (HasEventsAtDate(E.Date))
                            stats.AddDate(E.Date);
                        return stats;
                    }
                }
                var NewItem = new SingleAuditorStats { Auditor = GetAuditorName(E.UserID), State = State };
                NewItem.IncrementFactoryCounter(F, /*E.WorkTime*/Minutes);
                if (HasEventsAtDate(E.Date))
                    NewItem.AddDate(E.Date);
                auditorStats.Add(NewItem);
                return NewItem;
            };

            // Собрать статистику
            int i = 0;
            foreach (events Event in AllEvents)
            {
                i++;
                string Buffer = Event.FactoryList;
                if (Buffer.EndsWith(";"))
                    Buffer = Buffer.Remove(Buffer.Length - 1);
                //string[] Factories = Event.FactoryList.Split(';');
                string[] Factories = Buffer.Split(';');
                foreach (string Factory in Factories)
                {
                    //if (Factory.Length > 0)
                    //string S = FactoryListBoxItem.GetFactoryName(Factory);
                    string S;
                    DateTime Beg;
                    DateTime End;
                    int Key;
                    string Comm;
                    if (EventHelper.UnpackEventFromString(Factory, out S, out Beg, out End, out Key, out Comm))
                    {
                        AddDay(Event, S, EventHelper.GetMinutes(Beg, End)/*Convert.ToInt32((End - Beg).TotalMinutes)*/, Key);
                    }
                }
            }

            // Отобразить статистику в таблице
            foreach (SingleAuditorStats Item in auditorStats)
            {
                var Factories = new StringBuilder("");
                foreach (var S in Item.Factories)
                    if (S.Name.Length > 0)
                        Factories.AppendFormat("{0} ({1}), ", S.Name, EventHelper.FormatMinutes(S.Minutes)/*S.Count*/);
                string Buffer = Factories.ToString();
                if (Buffer.EndsWith(", "))
                    Buffer = Buffer.Remove(Buffer.Length - 2, 2);
                AddAuditorStatsLine(Item.Auditor, EventHelper.States[Item.State].Name, Item.GetDays(), Item.GetRealDays(), Item.TotalMinutes(), Buffer);
            }
            
            return auditorStatisticsResult;
        }
    }
}
