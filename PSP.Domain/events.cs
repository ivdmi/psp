//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PSP.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class events
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Comments { get; set; }
        public System.DateTime Date { get; set; }
        public int State { get; set; }
        public string UserID { get; set; }
        public string FactoryList { get; set; }
        public int WorkTime { get; set; }
        public string EventDesc { get; set; }
    
        public virtual users users { get; set; }
    }
}
