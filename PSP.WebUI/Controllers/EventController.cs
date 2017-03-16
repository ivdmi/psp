using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PSP.Domain;
using PSP.Domain.Abstract;
using PSP.Domain.Service;
using PSP.WebUI.Helpers;
using PSP.WebUI.Models;

namespace PSP.WebUI.Controllers
{
    public class EventController : Controller
    {
        private IRepository repository;
        private EventService eventService;
        //       private EventsOfDay eventsOfDay;
        private DataService dataService;
        private DateTime startDate = DateTime.Now.AddDays(-64);

        public EventController(IRepository paramRepository)
        {
            repository = paramRepository;
            eventService = new EventService(repository);
            //           eventsOfDay = new EventsOfDay();
            dataService = new DataService(repository);
        }

        // Разноцветная таблица-мозаика
        public ActionResult Index()
        {
            List<string> header = eventService.GetHeaderLine(startDate);
            ViewBag.Header = header;
            ViewBag.Month = DateTimeUtils.GetMonthName(startDate.Month);
            ViewBag.Year = startDate.Year;
            ViewBag.Count = header.Count;
            List<EventGroupModel> eventList = eventService.GetGroupsEventList(startDate);
            return View(eventList);
        }


        //public ActionResult DayEvents(string userId, DateTime date)
        //{
        //    ViewBag.Month = DateTimeUtils.GetMonthName(date.Month);
        //    ViewBag.Year = date.Year;
        //    SelectList eventsSelectedList = new SelectList(EventHelper.States, "Key", "Name");
        //    ViewBag.Events = eventsSelectedList;
        //    eventsOfDay = eventService.GetUserEventsOfDay(date, userId);
        //    return View(eventsOfDay);
        //}

        //[HttpPost]
        //public ActionResult DayEvents(EventsOfDay eventsOfDay)
        //{
        //    var events = eventService.GetEventsByDayAndUserId(eventsOfDay.Date, eventsOfDay.UserId);
        //    var factoryList = eventService.PackFactoryList(eventsOfDay);
        //    events.FactoryList = factoryList;
        //    eventService.UpdateEvent(events);

        //    ViewBag.Month = DateTimeUtils.GetMonthName(startDate.Month);
        //    ViewBag.Year = startDate.Year;
        //    SelectList eventsSelectedList = new SelectList(EventHelper.States, "Key", "Name");
        //    ViewBag.Events = eventsSelectedList;
        //    //            EventsOfDayViewModel eventsBoard = new EventsOfDayViewModel();
        //    eventsOfDay = eventService.GetUserEventsOfDay(eventsOfDay.Date, eventsOfDay.UserId);

        //    return View(eventsOfDay);
        //}

        //public ActionResult SaveEvent(EventsOfDay eventsOfDay)
        //{
        //    var dayEvents = eventService.GetEventsByDayAndUserId(eventsOfDay.Date, eventsOfDay.UserId);
        //    var factoryList = eventService.PackFactoryList(eventsOfDay);
        //    dayEvents.FactoryList = factoryList;
        //    eventService.UpdateEvent(dayEvents);

        //    ViewBag.Month = DateTimeUtils.GetMonthName(startDate.Month);
        //    ViewBag.Year = startDate.Year;
        //    SelectList eventsSelectedList = new SelectList(EventHelper.States, "Key", "Name");
        //    ViewBag.Events = eventsSelectedList;

        //    //            EventsOfDayViewModel eventsBoard = new EventsOfDayViewModel();
        //    eventsOfDay = eventService.GetUserEventsOfDay(eventsOfDay.Date, eventsOfDay.UserId);
        //    return View("DayEvents", eventsOfDay);
        //}

        public ActionResult BoardEvents(DateTime dateParam, string userId)
        {
            ViewBag.Month = DateTimeUtils.GetMonthName(startDate.Month);
            ViewBag.Year = startDate.Year;
//            ViewBag.States = EventHelper.States;
            BoardEventsViewModel boardEvents = new BoardEventsViewModel()
            {
                EventsOfDay = eventService.GetUserEventsOfDay(dateParam, userId),
                Factories = dataService.GetFactoryList()
            };

            return View(boardEvents);
        }

        [HttpPost]
        public ActionResult BoardEvents(BoardEventsViewModel boardEventsParam)
        {
            var boardEvents = boardEventsParam;
            if (ModelState.IsValid)
            {
                events Event = eventService.GetEventsByDayAndUserId(boardEvents.EventsOfDay.Date, boardEvents.EventsOfDay.UserId);
                if (Event == null)
                {
                    // TODO -                                       ПРЕДУСМОТРЕТЬ ПРЕДУПРЕЖДЕНИЕ / МОДАЛЬНОЕ ОКНО? копирования событий с одного дня на другой
                    // TODO -                                       Разблокировать Model.Date @readonly="readonly"
                    Event = eventService.CreateFilledEvent(boardEvents.EventsOfDay);
                }
                else
                {
                    Event = eventService.FillEvent(boardEvents.EventsOfDay, Event);
                }
                eventService.UpdateEvent(Event);
            }
            ViewBag.Month = DateTimeUtils.GetMonthName(startDate.Month);
            ViewBag.Year = startDate.Year;

//            return View(boardEvents);
            return RedirectToAction("Index");
        }

        public ActionResult AddActivity(DateTime date, string userId)
        {
            ViewBag.Date = date;
            ViewBag.UserId = userId;
            ViewBag.States = EventHelper.States;
            return View();
        }

        [HttpPost]
        public ActionResult AddActivity(ElementaryActivity activity, DateTime date, string userId)
        {
            if (ModelState.IsValid)
            {
                events Event = eventService.GetEventsByDayAndUserId(date, userId);
                var eventsOfDay = eventService.GetUserEventsOfDay(date, userId);
                eventsOfDay.Activities.Add(activity);
                Event = eventService.FillEvent(eventsOfDay, Event);
                if (Event.ID != null)
                {
                    eventService.UpdateEvent(Event);
                }
                else
                {
                    Event.ID = Guid.NewGuid().ToString();
                    eventService.AddEvent(Event);
                }

                return RedirectToAction("BoardEvents", new { userId = userId, dateParam = date });
            }
            return RedirectToAction("Index");
        }
    }
}
