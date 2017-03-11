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
        private GroupService groupService;
        private EventService eventService;
        private EventsOfDay eventsOfDay;
        private DateTime startDate = DateTime.Now.AddDays(-64);

        public EventController(IRepository paramRepository)
        {
            repository = paramRepository;
            groupService = new GroupService(repository);
            eventService = new EventService(repository);
            eventsOfDay = new EventsOfDay();
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


        public ActionResult DayEvents(string userId, DateTime date)
        {
            ViewBag.Month = DateTimeUtils.GetMonthName(date.Month);
            ViewBag.Year = date.Year;
            SelectList eventsSelectedList = new SelectList(EventHelper.States, "Key", "Name");
            ViewBag.Events = eventsSelectedList;
            eventsOfDay = eventService.GetUserEventsOfDay(date, userId);
            return View(eventsOfDay);
        }

        [HttpPost]
        public ActionResult DayEvents(EventsOfDay eventsOfDay)
        {
            var events = eventService.GetEventsByDayAndUserId(eventsOfDay.Date, eventsOfDay.UserId);
            var factoryList = eventService.PackFactoryList(eventsOfDay);
            events.FactoryList = factoryList;
            eventService.UpdateEvent(events);

            ViewBag.Month = DateTimeUtils.GetMonthName(startDate.Month);
            ViewBag.Year = startDate.Year;
            SelectList eventsSelectedList = new SelectList(EventHelper.States, "Key", "Name");
            ViewBag.Events = eventsSelectedList;
            //            EventsOfDayViewModel eventsBoard = new EventsOfDayViewModel();
            eventsOfDay = eventService.GetUserEventsOfDay(eventsOfDay.Date, eventsOfDay.UserId);

            return View(eventsOfDay);
        }

        public ActionResult SaveEvent(EventsOfDay eventsOfDay)
        {
            var dayEvents = eventService.GetEventsByDayAndUserId(eventsOfDay.Date, eventsOfDay.UserId);
            var factoryList = eventService.PackFactoryList(eventsOfDay);
            dayEvents.FactoryList = factoryList;
            eventService.UpdateEvent(dayEvents);

            ViewBag.Month = DateTimeUtils.GetMonthName(startDate.Month);
            ViewBag.Year = startDate.Year;
            SelectList eventsSelectedList = new SelectList(EventHelper.States, "Key", "Name");
            ViewBag.Events = eventsSelectedList;
            //            EventsOfDayViewModel eventsBoard = new EventsOfDayViewModel();
            eventsOfDay = eventService.GetUserEventsOfDay(eventsOfDay.Date, eventsOfDay.UserId);
            return View("DayEvents", eventsOfDay);
        }

        public ActionResult BoardEvents(string userId, DateTime dateParam)
        {
            ViewBag.Month = DateTimeUtils.GetMonthName(startDate.Month);
            ViewBag.Year = startDate.Year;
            SelectList eventsSelectedList = new SelectList(EventHelper.States, "Key", "Name");
            ViewBag.Events = eventsSelectedList;
            eventsOfDay = eventService.GetUserEventsOfDay(dateParam, userId);
            return View(eventsOfDay);
        }

        [HttpPost]
        public ViewResult BoardEvents(EventsOfDay eventsOfDayParam)
        {
            eventsOfDay = eventsOfDayParam;
            events events = eventService.GetEventsByDayAndUserId(eventsOfDayParam.Date, eventsOfDay.UserId);
            string factoryList = eventService.PackFactoryList(eventsOfDay);
            events.FactoryList = factoryList;
            events.EventDesc = eventsOfDayParam.EventDesc;
            eventService.UpdateEvent(events);
            return View(eventsOfDay);
        }
    }
}
