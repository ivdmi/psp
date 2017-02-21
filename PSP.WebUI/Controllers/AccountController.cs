using System.Linq;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using System.Web.Security;
using PSP.Domain;
using PSP.WebUI.Infrastructure.Abstract;
using PSP.WebUI.Models;

namespace PSP.WebUI.Controllers
{
    // Для разделения пользователей по ролям: администратор, менеджер, пользователь - 
    // смотри "Создание провайдера ролей" на https://metanit.com/sharp/helpdeskmvc/3.1.php

    [AllowAnonymous]
    public class AccountController : Controller
    {
        IAuthProvider authProvider;

        public AccountController(IAuthProvider auth)
        {
            authProvider = auth;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                if (ValidateUser(model.UserName, model.Password))
                {
//                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Request");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный пароль или логин");
                }
            }
            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Account");
        }



        //        }

        //        if (authProvider.Authenticate(model.UserName, model.Password))
        //        {
        //            return Redirect(returnUrl ?? Url.Action("Index", "Admin"));
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Incorrect username or password");
        //            return View();
        //        }
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}

        private bool ValidateUser(string login, string password)
        {
            bool isValid = false;

            using (pspEntities _db = new pspEntities())
            {
                try
                {
                    var user = (from u in _db.baseusers
                                 where u.Login == login && u.Password == password
                                 select u).FirstOrDefault();

                    if (user != null)
                    {
                        isValid = true;
                    }
                }
                catch
                {
                    isValid = false;
                }
            }
            return isValid;
        }
    }
}