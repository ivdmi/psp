using System.Data;
using System.Linq;
using System.Web.Mvc;
using PSP.Domain;
using PSP.Domain.Abstract;
using PSP.Domain.Service;

namespace PSP.WebUI.Controllers
{
    [Authorize(Roles = "admin")]
    public class GroupController : Controller
    {
        private GroupService _groups;

        private IRepository repository;

        public GroupController(IRepository repositoryDi)
        {
            repository = repositoryDi;
            _groups = new GroupService(repository);
        }

        // GET: /Group/
        public ActionResult Index()
        {
            return View(_groups.GetAllGroups());
        }

        // GET: /Group/Details/
        public ActionResult Details(string id)
        {
            var group = _groups.GetGroupbyId(id);
            return View(group);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(groups group)
        {
            if (ModelState.IsValid)
            {
                _groups.AddGroup(group);
                _groups.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }


        [Authorize(Roles = "admin")]
        public ActionResult Delete(string id = null)
        {
            groups group = _groups.GetGroupbyId(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: /BaseUser/Delete
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            _groups.RemoveGroup(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(string id)
        {
            groups group = _groups.GetGroupbyId(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: /BaseUser/Edit
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(groups group)
        {
            if (ModelState.IsValid)
            {
                _groups.UpdateGroup(group);
                return RedirectToAction("Index");
            }
            return View(group);
        }
    }
}