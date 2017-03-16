﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.WebPages;
using Microsoft.Ajax.Utilities;
using PSP.Domain;
using PSP.Domain.Abstract;
using PSP.Domain.Service;
using PSP.WebUI.Models;

namespace PSP.WebUI.Helpers
{
    public class EventService
    {
        private DateTime StartDate = DateTime.Now;
        private DateTime EndDate = DateTime.Now;

        private DataService dataService;
        private GroupService groupService;
        public EventService(IRepository repository)
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
                    var user = GetUserEventsInCells(us.Name, us.ID);
                    group.Users.Add(user);
                }
                eventList.Add(group);
            }

            return eventList;
        }

        // получить события 1 дня для пользователя
        public EventsOfDay GetUserEventsOfDay(DateTime date, string userId)
        {
            EventsOfDay eventsOfDay = new EventsOfDay() { Date = date.Date, UserId = userId, UserName = groupService.GetUserNameById(userId) };
            var dayEvents = dataService.GetEventsByDayAndUserId(date.Date, userId);
            if (dayEvents != null)
            {
                eventsOfDay.Activities = ParseEvents(dayEvents.FactoryList);
                eventsOfDay.EventDesc = dayEvents.EventDesc;
                eventsOfDay.Comments = dayEvents.Comments;
            }
            return eventsOfDay;
        }

        public events GetEventsByDayAndUserId(DateTime date, string userId)
        {
            var dayEvents = dataService.GetEventsByDayAndUserId(date.Date, userId);
            return dayEvents;
        }

        // получить пользователя с событиями в ячейках
        private EventsUserOfPeriod GetUserEventsInCells(string name, string userId)
        {
            EventsUserOfPeriod user = new EventsUserOfPeriod() { UserName = name, UserId = userId };

            var list = dataService.GetEventsByDateAndUserId(StartDate.Date, EndDate.Date, userId);
            for (int i = 0; i <= (EndDate - StartDate).Days + 1; i++)
            {
                CellElement cell = new CellElement() { ColumnIndex = i, Date = StartDate.AddDays(i).Date };
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

        public List<ElementaryActivity> ParseEvents(string factoryList)
        {
            List<ElementaryActivity> eventsList = new List<ElementaryActivity>();
            if (!String.IsNullOrEmpty(factoryList)) // Есть событие
            {
                string[] Splitted = factoryList.Split(';');
                foreach (string item in Splitted)
                {
                    string factory;
                    DateTime timeFrom;
                    DateTime timeTo;
                    int activityKey;
                    string comment;
                    if (FactoryListBoxItem.UnpackFromString(item, out factory, out timeFrom, out timeTo, out activityKey,
                        out comment))
                    {
                        eventsList.Add(new ElementaryActivity() { Factory = factory, TimeFrom = timeFrom, TimeTo = timeTo, ActivityKey = activityKey, Comment = comment });
                    }
                }
            }
            return eventsList;
        }

        public CellElement EventCellFormatting(CellElement cellElement)
        {
            var cell = cellElement;
            int column = cell.ColumnIndex;

            var eventsList = ParseEvents(cell.DayEvent.FactoryList);
            if (eventsList.Count > 0)
            {
                var Event = cell.DayEvent;
                cell.DrawFill = true;
                cell.BackColor = EventHelper.States[Event.State].NetColor;
                cell.Text = "";
                var Builder = new StringBuilder("Предприятия\n");
                foreach (var item in eventsList)
                {
                    Builder.AppendFormat("{0} - {1}\n", item.Factory, EventHelper.States[item.ActivityKey].Name);
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
                cell.DrawFill = false;
                cell.ToolTipText = "";
            }
            return cell;
        }

        // Упаковка списка предприятий с событиями
        public string PackFactoryList(EventsOfDay eventsOfDay)
        {
            var Builder = new StringBuilder("");
            foreach (ElementaryActivity item in eventsOfDay.Activities)
            {
                if (String.IsNullOrEmpty(item.Comment))
                {
                    item.Comment = String.Empty;
                }
                if (String.IsNullOrEmpty(item.Factory))
                {
                    item.Factory = String.Empty;
                }
                Builder.AppendFormat("{0},{1},{2},{3},{4};", item.Factory, item.TimeFrom.ToShortTimeString(), item.TimeTo.ToShortTimeString(), item.ActivityKey.ToString(), item.Comment.Replace(';', ','));
            }
            return Builder.ToString();
        }

        public void UpdateEvent(events _event)
        {
            dataService.UpdateEvent(_event);
        }

        public void AddEvent(events _event)
        {
            dataService.AddEvent(_event);
        }

        public events CreateFilledEvent(EventsOfDay eventsOfDay)
        {
            var Event = new events {ID = new Guid().ToString()};
            Event = FillEvent(eventsOfDay, Event);
            dataService.AddEvent(Event);
            return Event;
        }

        public events AddElementaryActivity(events _event)
        {
            var Event = _event;
//            if (Event != null)
            {
                //Event.UserID = eventsOfDay.UserId;
                //Event.Comments = eventsOfDay.Comments;
                //Event.EventDesc = eventsOfDay.EventDesc;
                //Event.Date = eventsOfDay.Date;
                //Event.FactoryList = PackFactoryList(eventsOfDay);

                //if (eventsOfDay.Activities.Count > 0)
                //{
                //    Event.State = eventsOfDay.Activities[0].ActivityKey;
                //}
            }
            dataService.AddEvent(Event);
            return Event;
        }

        public events FillEvent(EventsOfDay eventsOfDay, events _event)
        {
            events Event;
            if (_event == null)
                Event = new events();
            else
            {
                Event = _event;
            }
            {
                Event.UserID = eventsOfDay.UserId;
                Event.Comments = eventsOfDay.Comments;
                Event.EventDesc = eventsOfDay.EventDesc;
                Event.Date = eventsOfDay.Date;
                Event.FactoryList = PackFactoryList(eventsOfDay);

                if (eventsOfDay.Activities.Count > 0)
                {
                    Event.State = eventsOfDay.Activities[0].ActivityKey;
                }
            }
            return Event;
        }

    }
}