using System.Collections.Generic;
using System.Data;
using System.Linq;
using PSP.Domain.Abstract;

namespace PSP.Domain.Service
{
    public class BaseUsersService
    {
        private readonly IRepository _entities;
        
        public BaseUsersService(IRepository repository)
        {
            _entities = repository;
        }

        /// <summary>
        /// Gets the group 
        /// </summary>
        /// <param name="id">The group GUID.</param>
        /// <returns></returns>
        public baseusers GetUser(string id)
        {
            return _entities.BaseUsers.FirstOrDefault(g => g.ID.Equals(id));
        }

        /// <summary>
        /// Update the group 
        /// </summary>
        /// <param name="id">The group GUID.</param>
        /// <returns></returns>
        public void RemoveUser(string id)
        {
//            var usr = _entities.Context.baseusers.Find(id);
            var usr = _entities.Context.baseusers.FirstOrDefault(u => u.ID == id);
            if (usr != null) _entities.Context.baseusers.Remove(usr);
            SaveChanges();
        }

        /// <summary>
        /// Update the group 
        /// </summary>
        /// <param name="user">The group GUID.</param>
        /// <returns></returns>
        public void AddUser(baseusers user)
        {
            _entities.Context.baseusers.Add(user);
            SaveChanges();
        }

        /// <summary>
        /// Update the group 
        /// </summary>
        /// <param name="user">The group GUID.</param>
        /// <returns></returns>
        public void UpdateUser(baseusers user)
        {
            _entities.Context.Entry(user).State = EntityState.Modified;
            SaveChanges();
        }

        public void SaveChanges()
        {
            _entities.Context.SaveChanges();
        }

        /// <summary>
        /// Gets all groups 
        /// </summary>
        /// <returns>IList</returns>
        public IList<baseusers> GetAllBaseUsers()
        {
            return _entities.BaseUsers.ToList();
        }
    }
}
