using System;
using System.ComponentModel.DataAnnotations;

namespace PSP.WebUI.Models
{
    public class ElementaryActivity
    {
        [Required]
        public string Factory { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        //[DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public DateTime TimeFrom { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        //[DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public DateTime TimeTo { get; set; }
        [Required]
        [Range(0, 6, ErrorMessage = "Интервал от 0 до 6")]
        public int ActivityKey { get; set; }
        
        [Display(Name = "Комментарии")]
        public string Comment { get; set; }

        [Display(Name = "Комментарии")]
        public bool Check { get; set; }
    }
}