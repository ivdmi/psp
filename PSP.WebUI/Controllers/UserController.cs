using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using PSP.Domain;
using PSP.Domain.Abstract;
using PSP.Domain.Concrete;
using PSP.Domain.Service;

namespace PSP.WebUI.Controllers
{
    [Authorize(Roles = "admin, manager, user")]
    public class UserController : Controller
    {
        private IRepository _repository;

        private UsersService _users;

        public UserController(IRepository repositoryDi)
        {
            _repository = repositoryDi;
            _users = new UsersService(_repository);
        }

        // GET: 
        public ViewResult Index()
        {
            return View(_users.GetActiveUsers());
        }

        // GET: 
        public ViewResult Details(string id)
        {
            var user = _users.GetUser(id);
            return View(user);
        }

        // GET: 
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.GroupID = SelectList();
            return View();
        }

        // POST: 
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(users user)
        {
            if (ModelState.IsValid)
            {
                _users.AddUser(user);
                _users.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GroupID = SelectList();
            return View(user);
        }

        // GET: Delete
        [Authorize(Roles = "admin")]
        public ActionResult Delete(string id = null)
        {
            users user = _users.GetUser(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            _users.RemoveUser(id);
            return RedirectToAction("Index");
        }

        // GET: 
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(string id)
        {
            users user = _users.GetUser(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.GroupID = SelectList();
            return View(user);
        }

        // POST: 
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(users user)
        {
            if (ModelState.IsValid)
            {
                _users.UpdateUser(user);
                return RedirectToAction("Index");
            }
            ViewBag.GroupID = SelectList();
            return View(user);
        }

        private IEnumerable<SelectListItem> SelectList()
        {
            var _groupService = new GroupService(_repository);
            IEnumerable<SelectListItem> selectList =
            from s in _groupService.GetAllGroups()
            select new SelectListItem { Text = s.Name, Value = s.ID };
            return selectList;
        }

    }
}