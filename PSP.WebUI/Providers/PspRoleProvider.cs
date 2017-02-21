using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Providers.Entities;
using System.Web.Security;
using PSP.Domain;
using PSP.Domain.Concrete;

namespace PSP.WebUI.Providers
{
    public class PspRoleProvider : RoleProvider
    {
        // возвращает все роли пользователя
        public override string[] GetRolesForUser(string login)
        {
            string[] role = new string[] { };
            using (pspEntities _db = new pspEntities())
            {
                try
                {
                    // Получаем пользователя
                    var user = (from u in _db.baseusers
                                 where u.Login == login
                                 select u).FirstOrDefault();
                    if (user != null)
                    {
                        // получаем роль

                        if (user.Group != null)
                        {
                            role = new string[] { user.Group };
                        }
                    }
                }
                catch
                {
                    role = new string[] { };
                }
            }
            return role;
        }

        // показывает, связан ли пользователь с данной ролью
        public override bool IsUserInRole(string username, string roleName)
        {
            bool outputResult = false;
            // Находим пользователя
            using (pspEntities _db = new pspEntities())
            {
                try
                {
                    // Получаем пользователя
                    var user = (from u in _db.baseusers
                                 where u.Login == username
                                 select u).FirstOrDefault();
                    if (user != null)
                    {
                        // получаем роль
                        string userRole = UserRoles.RolesList.FirstOrDefault(r => r.Equals(user.Group));
                            
                        //сравниваем
                        if (userRole != null && userRole.Equals(roleName))
                        {
                            outputResult = true;
                        }
                    }
                }
                catch
                {
                    outputResult = false;
                }
            }
            return outputResult;
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }
        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}