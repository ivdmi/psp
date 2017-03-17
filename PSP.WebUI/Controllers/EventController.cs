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
        
        public ActionResult BoardEvents(DateTime dateParam, string userId)
        {
            ViewBag.Month = DateTimeUtils.GetMonthName(startDate.Month);
            ViewBag.Year = startDate.Year;
            BoardEventsViewModel boardEvents = new BoardEventsViewModel()
            {
                EventsOfDay = eventService.GetUserEventsOfDay(dateParam, userId),
                Factories = dataService.GetFactoryList()
            };

            return View(boardEvents);
        }

        [HttpPost]
        public ActionResult BoardEvents(BoardEventsViewModel boardEventsParam, int action = 0)
        {
            var boardEvents = boardEventsParam;
            if (ModelState.IsValid)
            {
                
                if (action > 0)
                {
                    boardEvents.EventsOfDay.Activities.RemoveAt(action - 1);
                }
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
            boardEvents.Factories = dataService.GetFactoryList();
            if (action > 0)
                return RedirectToAction("BoardEvents", new { userId = boardEvents.EventsOfDay.UserId, dateParam = boardEvents.EventsOfDay.Date });
            else
                return RedirectToAction("Index");
        }

        public ActionResult AddActivity(DateTime date, string userId)
        {
            ViewBag.Date = date;
            ViewBag.UserId = userId;
            ViewBag.States = EventHelper.States;
            ViewBag.Factories = dataService.GetFactoryList();
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
     //       return RedirectToAction("Index");
            ViewBag.Date = date;
            ViewBag.UserId = userId;
            ViewBag.States = EventHelper.States;
            ViewBag.Factories = dataService.GetFactoryList();
            return View(activity);
        }

        //[HttpPost]
        //public ActionResult Delete(ElementaryActivity activity, DateTime date, string userId)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        events Event = eventService.GetEventsByDayAndUserId(date, userId);
        //        var eventsOfDay = eventService.GetUserEventsOfDay(date, userId);
        //        eventsOfDay.Activities.Remove(activity);
        //        Event = eventService.FillEvent(eventsOfDay, Event);
        //        if (Event.ID != null)
        //        {
        //            eventService.UpdateEvent(Event);
        //        }
        //        else
        //        {
        //            Event.ID = Guid.NewGuid().ToString();
        //            eventService.AddEvent(Event);
        //        }

        //        return RedirectToAction("BoardEvents", new { userId = userId, dateParam = date });
        //    }
        //    return RedirectToAction("Index");
        //}

    }
}
