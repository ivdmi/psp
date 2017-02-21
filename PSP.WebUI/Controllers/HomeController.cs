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
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private IRepository repository;

        private GroupService groupService = new GroupService();

        public HomeController(IRepository paramRepository)
        {
            repository = paramRepository;
        }

        public ActionResult Index()
        {
            return View(groupService.GetGroups());
        }

        public ActionResult AuditorStat()
        {
            DateTime StartDate = DateTime.Parse("1/12/2016");
            DateTime EndDate = DateTime.Parse("31/12/2016");
            AuditorStatistics statistics = new AuditorStatistics(repository);
            var auditorStatisticsResult = statistics.Get(StartDate, EndDate);
            ViewBag.StartDate = StartDate.ToShortDateString();
            ViewBag.EndDate = EndDate.ToShortDateString();
            return View(auditorStatisticsResult);
        }

        public ActionResult FactoryStat()
        {
            DateTime StartDate = DateTime.Parse("1/12/2016");
            DateTime EndDate = DateTime.Parse("31/12/2016");
            FactoryStatistics statistics =  new FactoryStatistics(repository);
            var factoryStatisticResult = statistics.Get(StartDate, EndDate);
            ViewBag.StartDate = StartDate.ToShortDateString();
            ViewBag.EndDate = EndDate.ToShortDateString();
            return View(factoryStatisticResult);
        }

        public ActionResult About()
        {
            var usersTreeView = new UsersTreeView(repository);
            IList<UsersTreeViewModel> usersList = usersTreeView.GetTreeViewData();
            return View(usersList);
        }

        public ActionResult Contact()
        {
            var user = repository.Users.FirstOrDefault();
            return View(user);
        }

        public ActionResult Details(string id)
        {
            var count = repository.Users.Count(gr => gr.GroupID == id);
            var firstOrDefault = repository.Groups.FirstOrDefault(g => g.ID == id);
            string details = String.Empty;
            if (firstOrDefault != null)
            {
                details = "В группе " + firstOrDefault.Name + " " + count.ToString() + " элементов";
            }
            return PartialView(details);
        }

        
        public ActionResult UserInfo(string us)
        {
            var user = repository.Users.FirstOrDefault(u => u.ID.Contains(us));
            // ViewBag.User = user;
            return PartialView("_UserInfoPartialView", user);
        }


        [HttpPost]
        public ActionResult UserInfo()
        {
            return RedirectToAction("Index");
        }
    }
}