namespace PSP.Domain.Entities
{
    public class ClientContact
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string PatrName { get; set; }
        public string Position { get; set; }
        public string Birth { get; set; }
        public string Telephone { get; set; }
        public string Mail { get; set; }
        public string Comments { get; set; }
        public bool Vip { get; set; }
        public string ClientID { get; set; }

        public virtual Client Client { get; set; }
    }
}
