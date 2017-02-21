using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PSP.Domain.Entities
{
    public class BaseUser
    {
        public BaseUser()
        {
            ID = Guid.NewGuid().ToString();
        }

        [Required]
        [Display(Name = "Логин")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Login { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        public string Group { get; set; }
        public string ID { get; set; }
    }
}
