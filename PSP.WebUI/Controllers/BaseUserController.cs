using System;
using System.Web.Mvc;
using PSP.Domain;
using PSP.Domain.Abstract;
using PSP.Domain.Service;

namespace PSP.WebUI.Controllers
{
    public class BaseUserController : Controller
    {
        private BaseUsersService baseUsers;

        private IRepository repository;

        public BaseUserController(IRepository repositoryDi)
        {
            repository = repositoryDi;
            baseUsers = new BaseUsersService(repository);
        }

        // GET: /BaseUser/
        public ViewResult Index()
        {
            return View(baseUsers.GetAllBaseUsers());
        }

        // GET: /BaseUser/Details/5
        public ViewResult Details(string id)
        {
            var baseUser = baseUsers.GetUser(id);
            return View(baseUser);
        }

        // GET: /BaseUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /BaseUser/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(baseusers user)
        {
            if (ModelState.IsValid)
            {
                baseUsers.AddUser(user);
                baseUsers.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: /BaseUser/Edit
        public ActionResult Edit(string id = null)
        {
            baseusers user = baseUsers.GetUser(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: /BaseUser/Delete/5
        public ActionResult Delete(string id = null)
        {
            baseusers user = baseUsers.GetUser(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /BaseUser/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            baseUsers.RemoveUser(id);
            return RedirectToAction("Index");
        }

        // POST: /BaseUser/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(baseusers user)
        {
            if (ModelState.IsValid)
            {
                baseUsers.UpdateUser(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }
    }
}