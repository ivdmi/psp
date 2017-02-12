using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using PSP.Domain.Abstract;

namespace PSP.Domain.Service
{
    public class BaseUsersService
    {
        private readonly IRepository _entities;
        pspEntities db = new pspEntities();
        
        public BaseUsersService(IRepository paramRepository)
        {
            _entities = paramRepository;
        }
        
        /// <summary>
        /// Gets the group 
        /// </summary>
        /// <param name="id">The group GUID.</param>
        /// <returns></returns>
        public virtual baseusers GetUser(string id)
        {
            return _entities.BaseUsers.SingleOrDefault(g => g.ID.Contains(id));
        }

        // КАКОЙ ИЗ ЭТИХ 2 МЕТОДОВ ЛУЧШЕ                                                                        ??????
        public virtual baseusers GetUserById(string id = null)
        {
            return db.baseusers.Find(id);
        }

        /// <summary>
        /// Update the group 
        /// </summary>
        /// <param name="id">The group GUID.</param>
        /// <returns></returns>
        public virtual void RemoveUser(string id = null)
        {
            db.baseusers.Remove(GetUserById(id));
            db.SaveChanges();
        }

        /// <summary>
        /// Update the group 
        /// </summary>
        /// <param name="id">The group GUID.</param>
        /// <returns></returns>
        public virtual void UpdateUser(baseusers user)
        {
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        }

        /// <summary>
        /// Gets all groups 
        /// </summary>
        /// <returns>IList</returns>
        public virtual IList<baseusers> GetAllBaseUsers()
        {
            return _entities.BaseUsers.ToList();
        }
    }
}
