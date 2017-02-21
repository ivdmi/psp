using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PSP.Domain.Abstract;

namespace PSP.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IRepository _repository;

        public AdminController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

    }
}
