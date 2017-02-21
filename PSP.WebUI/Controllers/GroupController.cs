using System.Data;
using System.Linq;
using System.Web.Mvc;
using PSP.Domain;

namespace PSP.WebUI.Controllers
{
    public class GroupController : Controller
    {
        private pspEntities db = new pspEntities();


        // GET: /Group/
        public ActionResult Index()
        {
            return View(db.groups.ToList());
        }

        // GET: /Group/Details/
        public ActionResult Details(string id = null)
        {
            groups groups = db.groups.Find(id);
            if (groups == null)
            {
                return HttpNotFound();
            }
            return View(groups);
        }

        // GET: /Group/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Group/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(groups group)
        {
            if (ModelState.IsValid)
            {
                db.groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }

        // GET: /Group/Edit
        public ActionResult Edit(string id = null)
        {
            groups groups = db.groups.Find(id);
            if (groups == null)
            {
                return HttpNotFound();
            }
            return View(groups);
        }

        // POST: /Group/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(groups group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        // GET: /Group/Delete
        public ActionResult Delete(string id = null)
        {
            groups groups = db.groups.Find(id);
            if (groups == null)
            {
                return HttpNotFound();
            }
            return View(groups);
        }

        // POST: /Group/Delete
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