using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSP.Domain;
using PSP.Domain.Service;
using PSP.WebUI.Models;

namespace PSP.WebUI.Helpers
{
    public static class FactoryStatistics
    {
        private class FactoryStatsItem
        {
            public FactoryStatsItem() { Auditors = new List<Auditor>(); }

            public string Factory { set; get; }
            public int State { set; get; }

            public int Minutes { private set; get; }

            public class Auditor
            {
                public string Name { set; get; }
                public int Days { set; get; }
                public int Minutes { set; get; }
            }

            public List<Auditor> Auditors { private set; get; }

            public void IncrementAuditorCounter(string name, int minutes)
            {
                Auditor auditor = Auditors.FirstOrDefault(p => p.Name.Equals(name));
                if (null == auditor)
                {
                    Auditors.Add(new Auditor { Name = name, Days = 1, Minutes = minutes });
                }
                else
                {
                    auditor.Days++;
                    auditor.Minutes += minutes;
                }
                Minutes += minutes;
            }
        }

        private static List<GridViewDataFactoryRowInfo> factoryStatisticsResult = new List<GridViewDataFactoryRowInfo>();

        private static int _rowCount = 0;

        public static void AddFactoryStatsLine(string factory, string state, int days, int totalMinutes, string auditors)
        {
            GridViewDataFactoryRowInfo factoryRow = new GridViewDataFactoryRowInfo();
            factoryRow.Count = ++_rowCount;
            factoryRow.Factory = factory;
            factoryRow.State = state;
            factoryRow.Days = days;
            factoryRow.TotalMinutes = EventHelper.FormatMinutes(totalMinutes);
            factoryRow.Auditors = auditors;
            factoryStatisticsResult.Add(factoryRow);
        }


        public static List<GridViewDataFactoryRowInfo> Get(DateTime startDate, DateTime endDate)
        {
            // Получить всех пользователей и все события за промежуток времени
            List<users> AllUsers = DataService.GetAllUsers();
            List<events> AllEvents = DataService.GetEventsByDate(startDate, endDate);

            var factoryStats = new List<FactoryStatsItem>();
            Func<string, int, string, int, FactoryStatsItem> AddDay = (F, S, A, Mins) =>
            {
                foreach (FactoryStatsItem stats in factoryStats)
                {
                    if (stats.Factory == F && stats.State == S)
                    {
                        stats.IncrementAuditorCounter(A, Mins);
                        return stats;
                    }
                }
                var NewItem = new FactoryStatsItem { Factory = F, State = S };
                NewItem.IncrementAuditorCounter(A, Mins);
                factoryStats.Add(NewItem);
                return NewItem;
            };

            // Получить имя аудитора по идентификатору
            Func<string, string> GetAuditorName = K => (from user in AllUsers where user.ID.ToLower() == K.ToLower() select user.Name).FirstOrDefault();

            // Собрать статистику
            foreach (events Event in AllEvents)
            {
                string[] Factories = Event.FactoryList.Split(';');
                foreach (string Factory in Factories)
                {

                    string S;
                    DateTime begin;
                    DateTime End;
                    int key;
                    string Comm;
                    if (EventHelper.UnpackEventFromString(Factory, out S, out begin, out End, out key, out Comm))
                    {
                        if (S.Length > 0)
                        {
                            AddDay(S, key, GetAuditorName(Event.UserID), EventHelper.GetMinutes(begin, End));
                        }
                    }
                }
            }

            // Отобразить статистику в таблице
            foreach (FactoryStatsItem Item in factoryStats)
            {
                var Auditors = new StringBuilder("");
                int TotalDays = 0;
                foreach (FactoryStatsItem.Auditor S in Item.Auditors)
                {
                    Auditors.AppendFormat("{0} ({1}), ", S.Name, EventHelper.FormatMinutes(S.Minutes) /*S.Days*/);
                    TotalDays += S.Days;
                }
                Auditors.Remove(Auditors.Length - 2, 2);
                AddFactoryStatsLine(Item.Factory, EventHelper.States[Item.State].Name, TotalDays, Item.Minutes, Auditors.ToString());
            }

            return factoryStatisticsResult;
        }
    }
}
