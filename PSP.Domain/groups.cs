//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSP.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class groups
    {
        public groups()
        {
            ID = Guid.NewGuid().ToString();
            this.users = new HashSet<users>();
        }
    
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string ID { get; set; }

        [Required]
        [Display(Name = "������������")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "�����")]
        public string Login { get; set; }

        [Required]
        [Display(Name = "������")]
        public string Password { get; set; }
        public virtual ICollection<users> users { get; set; }
    }
}
