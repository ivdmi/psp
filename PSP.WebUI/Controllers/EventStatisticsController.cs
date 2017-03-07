using System;
using System.Collections.Generic;
using System.Data.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Web.Mvc;
using PSP.Domain;
using PSP.Domain.Abstract;
using PSP.Domain.Concrete;
using PSP.Domain.Service;
using PSP.WebUI.Helpers;
using PSP.WebUI.Models;

namespace PSP.WebUI.Controllers
{
    public class EventStatisticsController : Controller
    {

        private IRepository repository;

        private GroupService groupService;
        private EventStatistics auditorEvents;

        public EventStatisticsController(IRepository paramRepository)
        {
            repository = paramRepository;
            groupService = new GroupService(repository);
            auditorEvents = new EventStatistics(repository);
        }

        public ActionResult Index()
        {
            DateTime date = DateTime.Now.AddDays(-64);
   //         DateTime date = DateTime.Now;
            List<string> header = auditorEvents.GetHeaderLine(date);
            ViewBag.Header = header;
            ViewBag.Month = DateTimeUtils.GetMonthName(date.Month);
            ViewBag.Year = date.Year;
            ViewBag.Count = header.Count;
            var eventList = auditorEvents.GetGroupsEventList(date);
            return View(eventList);
        }
    }
}
