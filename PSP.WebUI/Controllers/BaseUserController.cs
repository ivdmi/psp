using System.Web.Mvc;
using PSP.Domain;
using PSP.Domain.Abstract;
using PSP.Domain.Concrete;
using PSP.Domain.Service;

namespace PSP.WebUI.Controllers
{
    //[Authorize(Roles = "admin")]
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
        [HttpGet]
        //[Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            SelectList roles = new SelectList(UserRoles.RolesList);
            ViewBag.Roles = roles;
            return View();
        }

        // POST: /BaseUser/Create
        [HttpPost]
        //[Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(baseusers user)
        {
            if (ModelState.IsValid)
            {
                baseUsers.AddUser(user);
                baseUsers.SaveChanges();
                return RedirectToAction("Index");
            }            
            SelectList roles = new SelectList(UserRoles.RolesList);
            ViewBag.Roles = roles;
            return View(user);
        }
        
        // GET: /BaseUser/Delete
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            baseUsers.RemoveUser(id);
            return RedirectToAction("Index");
        }

        // GET: /BaseUser/Edit
        [HttpGet]
        //[Authorize(Roles = "admin")]
        public ActionResult Edit(string id)
        {
            baseusers user = baseUsers.GetUser(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            SelectList roles = new SelectList(UserRoles.RolesList);
            ViewBag.Roles = roles;
            return View(user);
        }

        // POST: /BaseUser/Edit
        [HttpPost]
        //[Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(baseusers user)
        {
            if (ModelState.IsValid)
            {
                baseUsers.UpdateUser(user);
                return RedirectToAction("Index");
            }
            SelectList roles = new SelectList(UserRoles.RolesList);
            ViewBag.Roles = roles;
            return View(user);
        }
    }
}