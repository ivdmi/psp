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
    public class AuditorStatistics
    {
        private DataService dataService;
        public AuditorStatistics(IRepository repository)
        {
            dataService = new DataService(repository);
        }
        
        private class AuditorStatsItem
        {

            public AuditorStatsItem()
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

            public void IncrementFactoryCounter(string factory, int minutes)
            {
                FactoryItem Item = Factories.Where(P => P.Name == factory).Select(P => P).FirstOrDefault();
                if (null == Item)
                {
                    Factories.Add(new FactoryItem { Name = factory, Count = 1, Minutes = minutes });
                }
                else
                {
                    Item.Count++;
                    Item.Minutes += minutes;
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

        // Значительно дольше чем Func<string, string> GetAuditorName = K => (from User in AllUsers where User.ID.ToLower() == K.ToLower() select User.Name).FirstOrDefault();
        private string GetAuditorByIdName(string Id)
        {
            var user = dataService.GetAllUsers().FirstOrDefault(item => item.ID.Equals(Id));
            return user == null ? string.Empty : user.Name;
        }

        public List<GridViewDataAuditorRowInfo> Get(DateTime startDate, DateTime endDate)
        {
            // Получить всех пользователей и все события за промежуток времени
            List<users> AllUsers = dataService.GetAllUsers();
            List<events> AllEvents = dataService.GetEventsByDate(startDate, endDate);

            // Получить имя аудитора по идентификатору
            Func<string, string> GetAuditorName = K => (from User in AllUsers where User.ID.ToLower() == K.ToLower() select User.Name).FirstOrDefault();

            Func<DateTime, bool> HasEventsAtDate = D => AllEvents.Any(Event => Event.Date == D);

            var auditorStats = new List<AuditorStatsItem>();
            Func<events, string, int, int, AuditorStatsItem> AddDay = (E, F, Minutes, State) =>
            {
                foreach (AuditorStatsItem stats in auditorStats)
                {
                  if (stats.Auditor == GetAuditorName(E.UserID) && stats.State == State)
                  //                    if (stats.Auditor == GetAuditorByIdName(E.UserID) && stats.State == State)
//                    if (stats.Auditor == DataService.GetAuditorNameByIdService(E.UserID) && stats.State == State)
                    {
                        stats.IncrementFactoryCounter(F, /*E.WorkTime*/Minutes);
                        if (HasEventsAtDate(E.Date))
                            stats.AddDate(E.Date);
                        return stats;
                    }
                }
                var newItem = new AuditorStatsItem { Auditor = GetAuditorName(E.UserID), State = State };
//                var NewItem = new AuditorStatsItem { Auditor = GetAuditorByIdName(E.UserID), State = State };
//                var NewItem = new SingleAuditorStats { Auditor = DataService.GetAuditorNameByIdService(E.UserID), State = State };
                newItem.IncrementFactoryCounter(F, /*E.WorkTime*/Minutes);
                if (HasEventsAtDate(E.Date))
                    newItem.AddDate(E.Date);
                auditorStats.Add(newItem);
                return newItem;
            };

            // Собрать статистику
            int i = 0;
            foreach (events Event in AllEvents)
            {
                i++;
                string buffer = Event.FactoryList;
                if (buffer.EndsWith(";"))
                    buffer = buffer.Remove(buffer.Length - 1);
                //string[] Factories = Event.FactoryList.Split(';');
                string[] factories = buffer.Split(';');
                foreach (string factory in factories)
                {
                    //if (Factory.Length > 0)
                    //string S = FactoryListBoxItem.GetFactoryName(Factory);
                    string S;
                    DateTime Beg;
                    DateTime End;
                    int Key;
                    string Comm;
                    if (EventHelper.UnpackEventFromString(factory, out S, out Beg, out End, out Key, out Comm))
                    {
                        AddDay(Event, S, EventHelper.GetMinutes(Beg, End)/*Convert.ToInt32((End - Beg).TotalMinutes)*/, Key);
                    }
                }
            }

            // Отобразить статистику в таблице
            foreach (AuditorStatsItem item in auditorStats)
            {
                var factories = new StringBuilder("");
                foreach (var S in item.Factories)
                    if (S.Name.Length > 0)
                        factories.AppendFormat("{0} ({1}), ", S.Name, EventHelper.FormatMinutes(S.Minutes)/*S.Count*/);
                string buffer = factories.ToString();
                if (buffer.EndsWith(", "))
                    buffer = buffer.Remove(buffer.Length - 2, 2);
                AddAuditorStatsLine(item.Auditor, EventHelper.States[item.State].Name, item.GetDays(), item.GetRealDays(), item.TotalMinutes(), buffer);
            }
            
            return auditorStatisticsResult;
        }
    }
}
