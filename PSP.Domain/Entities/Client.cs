using System;
using System.Collections.Generic;

namespace PSP.Domain.Entities
{
    public class Client
    {
        public Client()
        {
            this.ClientContacts = new HashSet<ClientContact>();
        }
    
        public string ID { get; set; }
        public string Company { get; set; }
        public string Activity { get; set; }
        public string Idx { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Office { get; set; }
        public string AddrComments { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Mail { get; set; }
        public string Comments { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Own { get; set; }
        public string Auditor { get; set; }
        public string Works { get; set; }
        public string Establishing { get; set; }
        public bool Special { get; set; }
    
        public virtual ICollection<ClientContact> ClientContacts { get; set; }
    }
}
