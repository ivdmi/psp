using System.Collections.Generic;
using System.Linq;
using PSP.Domain.Abstract;

namespace PSP.Domain.Concrete
{
    public class Repository : IRepository
    {

        public Repository()
        {
            Context = new pspEntities();
        }

        public pspEntities Context {get; set;}

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

        public IQueryable<companyanalysis> CompanyAnalysis
        {
            get { return Context.companyanalysis; }
        }
    }
}
