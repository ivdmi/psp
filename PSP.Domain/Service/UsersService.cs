using System.Collections.Generic;
using System.Data;
using System.Linq;
using PSP.Domain.Abstract;

namespace PSP.Domain.Service
{
    public class UsersService
    {
        private readonly IRepository _entities;
        
        public UsersService(IRepository repository)
        {
            _entities = repository;
        }

        public users GetUser(string id)
        {
            return _entities.Users.FirstOrDefault(g => g.ID.Equals(id));
        }

        public void RemoveUser(string id)
        {
            var usr = _entities.Context.users.Find(id);
            _entities.Context.users.Remove(usr);
            SaveChanges();
        }

        public void AddUser(users user)
        {
            _entities.Context.users.Add(user);
            SaveChanges();
        }

        public void UpdateUser(users user)
        {
            _entities.Context.Entry(user).State = EntityState.Modified;
            SaveChanges();
        }

        public void SaveChanges()
        {
            _entities.Context.SaveChanges();
        }

        public IList<users> GetAllusers()
        {
            return _entities.Users.ToList();
        }
    }
}
