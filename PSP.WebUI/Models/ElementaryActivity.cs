using System;
using System.ComponentModel.DataAnnotations;

namespace PSP.WebUI.Models
{
    public class ElementaryActivity
    {
        [Required]
        public string Factory { get; set; }
        [Required]
        public DateTime TimeFrom { get; set; }
        [Required]
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