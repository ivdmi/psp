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
    
    public partial class baseusers
    {
        public baseusers()
        {
            ID = Guid.NewGuid().ToString();
        }

        public string Login { get; set; }
        public string Password { get; set; }
        public string Group { get; set; }
        public string ID { get; set; }
    }
}
