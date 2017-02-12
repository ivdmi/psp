using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PSP.Domain;
using PSP.Domain.Concrete;
using PSP.Domain.Service;

namespace PSP.WebUI.Controllers
{
    public class BaseUserController : Controller
    {
        private pspEntities db = new pspEntities();
        private BaseUsersService baseUsers = new BaseUsersService(new Repository());

        //
        // GET: /BaseUser/

        public ActionResult Index()
        {
            //                                               НУЖЕН ЛИ НАМ ВООБЩЕ СЕРВИС, если есть db - ?
            // return View(db.baseusers.ToList());
            return View(baseUsers.GetAllBaseUsers());
        }

        //
        // GET: /BaseUser/Details/5

        public ActionResult Details(string id = null)
        {
            //          baseusers baseusers = db.baseusers.Find(id);
            var baseUser = baseUsers.GetUserById(id);
            var bUser = baseUsers.GetUser(id);
            if (baseUser == null)
            {
                return HttpNotFound();
            }
            return View(baseUser);
        }

        //
        // GET: /BaseUser/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BaseUser/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(baseusers baseusers)
        {
            if (ModelState.IsValid)
            {
                baseusers.ID = Guid.NewGuid().ToString();
                db.baseusers.Add(baseusers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(baseusers);
        }

        //
        // GET: /BaseUser/Edit/5

        public ActionResult Edit(string id = null)
        {
            baseusers user = baseUsers.GetUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /BaseUser/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(baseusers user)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(baseusers).State = EntityState.Modified;
                //db.SaveChanges();
                baseUsers.UpdateUser(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        //
        // GET: /BaseUser/Delete/5

        public ActionResult Delete(string id = null)
        {
            baseusers user = baseUsers.GetUserById(id);

            //              db.baseusers.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /BaseUser/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            baseUsers.RemoveUser(id);
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}