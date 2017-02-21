using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace PSP.Domain.Concrete
{

    public class PspContext : DbContext
    {
        public DbSet<auditoranalysis> auditoranalysis { get; set; }
        public DbSet<baseusers> baseusers { get; set; }
        public DbSet<clientcontacts> clientcontacts { get; set; }
        public DbSet<clients> clients { get; set; }
        public DbSet<companyanalysis> companyanalysis { get; set; }
        public DbSet<events> events { get; set; }
        public DbSet<groups> groups { get; set; }
        public DbSet<history> history { get; set; }
        public DbSet<mails> mails { get; set; }
        public DbSet<users> users { get; set; }
    }
}
