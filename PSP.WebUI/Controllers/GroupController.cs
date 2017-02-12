using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PSP.Domain;

namespace PSP.WebUI.Controllers
{
    public class GroupController : Controller
    {
        private pspEntities db = new pspEntities();

        //
        // GET: /Group/

        public ActionResult Index()
        {
            return View(db.groups.ToList());
        }

        //
        // GET: /Group/Details/5

        public ActionResult Details(string id = null)
        {
            groups groups = db.groups.Find(id);
            if (groups == null)
            {
                return HttpNotFound();
            }
            return View(groups);
        }

        //
        // GET: /Group/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Group/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(groups groups)
        {
            if (ModelState.IsValid)
            {
                groups.ID = Guid.NewGuid().ToString();
                db.groups.Add(groups);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(groups);
        }

        //
        // GET: /Group/Edit/5

        public ActionResult Edit(string id = null)
        {
            groups groups = db.groups.Find(id);
            if (groups == null)
            {
                return HttpNotFound();
            }
            return View(groups);
        }

        //
        // POST: /Group/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(groups groups)
        {
            if (ModelState.IsValid)
            {
                db.Entry(groups).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(groups);
        }

        //
        // GET: /Group/Delete/5

        public ActionResult Delete(string id = null)
        {
            groups groups = db.groups.Find(id);
            if (groups == null)
            {
                return HttpNotFound();
            }
            return View(groups);
        }

        //
        // POST: /Group/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            groups groups = db.groups.Find(id);
            db.groups.Remove(groups);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}