using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.WebPages;
using PSP.Domain;
using PSP.Domain.Abstract;
using PSP.Domain.Service;
using PSP.WebUI.Models;

namespace PSP.WebUI.Helpers
{
    public class EventStatistics
    {
        private DateTime StartDate = DateTime.Now;
        private DateTime EndDate = DateTime.Now;

        private DataService dataService;
        private GroupService groupService;
        public EventStatistics(IRepository repository)
        {
            dataService = new DataService(repository);
            groupService = new GroupService(repository);
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

            row.Columns.Add(String.Empty);
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
        public List<string> GetHeaderLine(DateTime startDate)
        {
            List<string> row = new List<string>();
            StartDate = startDate;
            EndDate = DateTimeUtils.GetEndDateOfMonth(startDate);
            int Year = StartDate.Year;
            int Month = StartDate.Month;
            int Day = StartDate.Day;

            row.Add("Ф.И.О.");
            for (int i = 0; i < (EndDate - StartDate).Days + 2; i++)
            {
                row.Add(Day.ToString());
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

        private RowMonth GetRow(string group, string user)
        {
            RowMonth row = new RowMonth();
            row.Columns.Add(group);
            row.Columns.Add(user);
            for (int i = 0; i < (EndDate - StartDate).Days + 2; i++)
            {
                row.Columns.Add(String.Empty);
            }
            return row;
        }

        //private IList<string> GetDays()
        //{
        //    IList<string> row = new List<string>();
        //    for (int i = 0; i < (EndDate - StartDate).Days + 2; i++)
        //    {
        //        row.Add(i.ToString());
        //    }
        //    return row;
        //}


        //public List<RowMonth> GetAuditorEventList(DateTime startDate)
        //{
        //    StartDate = startDate;
        //    EndDate = DateTimeUtils.GetEndDateOfMonth(StartDate);
        //    _auditorEvetsList.Add(GetHeader());

        //    foreach (var group in groupService.GetAllGroups())
        //    {
        //        foreach (var user in group.users.Where(u => u.Hidden == 0).OrderBy(u => u.Name).ToList())
        //        {
        //            _auditorEvetsList.Add(GetRow(group.Name, user.Name));
        //        }
        //    }

        //    return _auditorEvetsList;
        //}

        // получить события по ячейкам для всех пользователей 
        public List<EventGroupModel> GetGroupsEventList(DateTime startDate)
        {
            StartDate = startDate;
            EndDate = DateTimeUtils.GetEndDateOfMonth(StartDate);
            List<EventGroupModel> eventList = new List<EventGroupModel>();
            foreach (var gr in groupService.GetAllGroups())
            {
                var group = new EventGroupModel() { GroupName = gr.Name };

                foreach (var us in gr.users.Where(u => u.Hidden == 0).OrderBy(u => u.Name))
                {
                    //                    List<events> userEvents = dataService.GetEventsByDateAndUserId(StartDate, EndDate, us.ID);
                    var user = GetUserEventsInCells(us.Name, us.ID);
                    group.Users.Add(user);
                }
                eventList.Add(group);
            }

            return eventList;
        }

        // получить пользователя с событиями в ячейках
        private EventUser GetUserEventsInCells(string name, string userId)
        {
            EventUser user = new EventUser() { UserName = name };

            var list = dataService.GetEventsByDateAndUserId(StartDate.Date, EndDate.Date, userId);
            for (int i = 0; i <= (EndDate - StartDate).Days + 1; i++)
            {
                CellElement cell = new CellElement() { ColumnIndex = i };
                var userEv = list.FirstOrDefault(ev => ev.Date.Date == StartDate.AddDays(i).Date);
                if (userEv != null)
                    cell.DayEvent = userEv;
                else
                {
                    cell.DayEvent = new events();
                }
                cell = EventCellFormatting(cell);
                user.Cells.Add(cell);
            }
            return user;
        }

        public CellElement EventCellFormatting(CellElement cellElement)
        {
            var cell = cellElement;
            int column = cell.ColumnIndex;

            if (!cell.DayEvent.FactoryList.IsEmpty())       // Есть событие
            {
                var Event = cell.DayEvent;
                // Фон
                //                Color Color = EventHelper.States[Event.State].NetColor;
                //              cell.BackColor = ControlPaint.Light(Color);
                //              E.CellElement.BackColor2 = ControlPaint.LightLight(Color);
                cell.DrawFill = true;
                cell.BackColor = EventHelper.States[Event.State].NetColor;
                cell.Text = "";

                var Builder = new StringBuilder("Предприятия\n");
                string[] Splitted = Event.FactoryList.Split(';');
                foreach (string S in Splitted)
                {
                    string Fact;
                    DateTime Beg;
                    DateTime End;
                    int Key;
                    string Comm;
                    if (FactoryListBoxItem.UnpackFromString(S, out Fact, out Beg, out End, out Key, out Comm))
                        Builder.AppendFormat("{0} - {1}\n", Fact, EventHelper.States[Key].Name);
                }
                cell.ToolTipText = Builder.ToString();
            }

                // &#013
            else if (DateTimeUtils.CheckWeekendByColumn(column, StartDate))
            {
                cell.BackColor = Color.LightPink;
                cell.BackColor2 = Color.HotPink;
                cell.DrawFill = true;
                cell.ToolTipText = "";
            }
            else
            {
                //                E.CellElement.NumberOfColors = 2;
                cell.DrawFill = false;
                //                E.CellElement.ResetValue(VisualElement.BackColorProperty);
                //                E.CellElement.ResetValue(VisualElement.ForeColorProperty);
                cell.ToolTipText = "";
            }
            return cell;
        }

    }
}