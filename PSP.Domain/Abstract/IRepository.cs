using System.Linq;

namespace PSP.Domain.Abstract
{
    public interface IRepository
    {
        pspEntities Context { get; set; }
        IQueryable<clients> Clients { get; }
        IQueryable<clientcontacts> ClientContacts { get; }
        IQueryable<groups> Groups { get; }
        IQueryable<users> Users { get; }
        IQueryable<baseusers> BaseUsers { get; }
        IQueryable<events> Events { get; }
        IQueryable<history> HistoryList { get; }

    }
}
