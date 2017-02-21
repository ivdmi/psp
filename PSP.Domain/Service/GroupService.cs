using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PSP.Domain.Abstract;
using PSP.Domain.Concrete;

namespace PSP.Domain.Service
{
    public class GroupService
    {
        private readonly IRepository _entities = new Repository();
        
        /// <summary>
        /// Gets the group 
        /// </summary>
        /// <param name="id">The group GUID.</param>
        /// <returns></returns>
        public groups GetGroup(Guid id)
        {
            return _entities.Groups
                .Include(x=>x.Name)
                .Include(x=>x.Login)
                .Include(x=>x.Password)
                .Include(x=>x.users)
                .FirstOrDefault(g => g.ID == id.ToString());
        }

        /// <summary>
        /// Gets all groups 
        /// </summary>
        /// <returns></returns>
        public IList<groups> GetGroups()
        {
            return _entities.Groups.ToList();
        }
    }
}
