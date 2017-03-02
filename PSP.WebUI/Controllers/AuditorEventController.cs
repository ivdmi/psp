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
    public class AuditorEventController : Controller
    {

        private IRepository repository;

        private GroupService groupService;
        private AuditorEvents auditorEvents;

        public AuditorEventController(IRepository paramRepository)
        {
            repository = paramRepository;
            groupService = new GroupService(repository);
            auditorEvents = new AuditorEvents(repository);
        }

        public ActionResult Index()
        {
            var eventList = auditorEvents.GetAuditorEventList(DateTime.Now);
            return View(groupService.GetAllGroups());
        }

    }
}
