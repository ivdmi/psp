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
        [Range(0, 6, ErrorMessage = "�������� �� 0 �� 6")]
        public int ActivityKey { get; set; }
        
        [Display(Name = "�����������")]
        public string Comment { get; set; }

        [Display(Name = "�����������")]
        public bool Check { get; set; }
    }
}