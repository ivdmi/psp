using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSP.Domain.Abstract;

namespace PSP.Domain.Service
{
    public class GroupService
    {
        private readonly IRepository _entities;
        

        public GroupService(IRepository paramRepository)
        {
            _entities = paramRepository;
        }

        /// <summary>
        /// Gets the group 
        /// </summary>
        /// <param name="groupIdentifier">The group GUID.</param>
        /// <returns></returns>
        public virtual groups GetGroup(Guid groupIdentifier)
        {
            return _entities.Groups
                .Include(x=>x.Name)
                .Include(x=>x.Login)
                .Include(x=>x.Password)
                .Include(x=>x.users)
                .SingleOrDefault(g => g.ID == groupIdentifier.ToString());
        }

        /// <summary>
        /// Gets all groups 
        /// </summary>
        /// <param name="groupIdentifier">The group GUID.</param>
        /// <returns></returns>
        public virtual IList<groups> GetGroups()
        {
            return _entities.Groups.ToList();
        }



    }
}
