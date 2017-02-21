using System.Collections.Generic;
using System.Linq;
using PSP.Domain.Abstract;
using PSP.Domain.Entities;

namespace PSP.Domain.Concrete
{
    public class Repository : IRepository
    {

        public Repository()
        {
            Context = new PspEntities();
        }

        public PspEntities Context {get; set;}

        public IQueryable<clients> Clients
        {
            get { return Context.clients; }
        }

        public IQueryable<clientcontacts> ClientContacts
        {
            get { return Context.clientcontacts; }
        }

        public IQueryable<users> Users
        {
            get { return Context.users; }
        }

        public IQueryable<groups> Groups
        {
            get { return Context.groups; }
        }

        public IQueryable<baseusers> BaseUsers
        {
            get { return Context.baseusers; }
        }

        public IQueryable<events> Events
        {
            get { return Context.events; }
        }
        public IQueryable<history> HistoryList
        {
            get { return Context.history; }
        }
    }
}
