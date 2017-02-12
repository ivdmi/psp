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
        private class SingleFactoryStats
        {
            public SingleFactoryStats() { Auditors = new List<Auditor>(); }

            public string Factory { set; get; }
            public int State { set; get; }

            public int Minutes { private set; get; }

            public class Auditor
            {
                public string Name { set; get; }
                public int Days { set; get; }
                public int Mins { set; get; }
            }

            public List<Auditor> Auditors { private set; get; }

            public void IncrementAuditorCounter(string N, int Mins)
            {
                Auditor Auditor = Auditors.Where(P => P.Name == N).Select(P => P).FirstOrDefault();
                if (null == Auditor)
                {
                    Auditors.Add(new Auditor { Name = N, Days = 1, Mins = Mins });
                }
                else
                {
                    Auditor.Days++;
                    Auditor.Mins += Mins;
                }
                Minutes += Mins;
            }
        }

        private static List<GridViewDataFactoryRowInfo> factoryStatisticsResult = new List<GridViewDataFactoryRowInfo>();

        private static int _rowCount = 0;

        public static void AddFactoryStatsLine(string Factory, string State, int Days, int TotalMinutes, string Auditors)
        {
            GridViewDataFactoryRowInfo factoryRow = new GridViewDataFactoryRowInfo();
            factoryRow.Count = ++_rowCount;
            factoryRow.Factory = Factory;
            factoryRow.State = State;
            factoryRow.Days = Days;
            factoryRow.TotalMinutes = EventHelper.FormatMinutes(TotalMinutes);
            factoryRow.Auditors = Auditors;
            factoryStatisticsResult.Add(factoryRow);
        }


        public static List<GridViewDataFactoryRowInfo> Get(DateTime startDate, DateTime endDate)
        {
            // Получить всех пользователей и все события за промежуток времени
            List<users> AllUsers = DataService.GetAllUsers();
            List<events> AllEvents = DataService.GetEventsByDate(startDate, endDate);

            var factoryStats = new List<SingleFactoryStats>();
            Func<string, int, string, int, SingleFactoryStats> AddDay = (F, S, A, Mins) =>
            {
                foreach (SingleFactoryStats stats in factoryStats)
                {
                    if (stats.Factory == F && stats.State == S)
                    {
                        stats.IncrementAuditorCounter(A, Mins);
                        return stats;
                    }
                }
                var NewItem = new SingleFactoryStats { Factory = F, State = S };
                NewItem.IncrementAuditorCounter(A, Mins);
                factoryStats.Add(NewItem);
                return NewItem;
            };

            // Получить имя аудитора по идентификатору
            Func<string, string> GetAuditorName = K => (from User in AllUsers where User.ID.ToLower() == K.ToLower() select User.Name).FirstOrDefault();

            // Собрать статистику
            foreach (events Event in AllEvents)
            {
                string[] Factories = Event.FactoryList.Split(';');
                foreach (string Factory in Factories)
                {

                    string S;
                    DateTime Beg;
                    DateTime End;
                    int Key;
                    string Comm;
                    if (EventHelper.UnpackEventFromString(Factory, out S, out Beg, out End, out Key, out Comm))
                    {
                        if (S.Length > 0)
                        {
                            AddDay(S, Key, GetAuditorName(Event.UserID), EventHelper.GetMinutes(Beg, End));
                        }
                    }
                }
            }

            // Отобразить статистику в таблице
            foreach (SingleFactoryStats Item in factoryStats)
            {
                var Auditors = new StringBuilder("");
                int TotalDays = 0;
                foreach (SingleFactoryStats.Auditor S in Item.Auditors)
                {
                    Auditors.AppendFormat("{0} ({1}), ", S.Name, EventHelper.FormatMinutes(S.Mins) /*S.Days*/);
                    TotalDays += S.Days;
                }
                Auditors.Remove(Auditors.Length - 2, 2);
                AddFactoryStatsLine(Item.Factory, EventHelper.States[Item.State].Name, TotalDays, Item.Minutes, Auditors.ToString());
            }

            return factoryStatisticsResult;
        }
    }
}
