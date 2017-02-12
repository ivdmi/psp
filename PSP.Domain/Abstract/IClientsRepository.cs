using System.Linq;
using PSP.Domain.Entities;

// НАВЕРНО, ИСПОЛЬЗОВАТЬ НЕ БУДУ. ВСЕ В ОДИН РЕПОЗИТОРИЙ

namespace PSP.Domain.Abstract
{
    public interface IClientsRepository
    {
        IQueryable<Client> Clients { get; }
    }
}
