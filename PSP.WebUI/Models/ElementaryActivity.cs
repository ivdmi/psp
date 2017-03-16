using System;
using System.ComponentModel.DataAnnotations;

namespace PSP.WebUI.Models
{
    public class ElementaryActivity
    {
        public string Factory { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public DateTime TimeFrom { get; set; }
        
        public DateTime TimeTo { get; set; }
        [Required]
        [Range(0, 6, ErrorMessage = "Интервал от 0 до 6")]
        public int ActivityKey { get; set; }
        
        [Display(Name = "Комментарии")]
        public string Comment { get; set; }

        //[Required]
        //[Range(0, 23)]
        //[Display(Name = "Часы")]
        //int HourFrom { get; set; }

        //[Required]
        //[Range(0, 59)]
        //[Display(Name = "Минуты")]
        //int MinuteFrom { get; set; }

        //[Required]
        //[Range(0, 23)]
        //[Display(Name = "Часы")]
        //int HourTo { get; set; }

        //[Required]
        //[Range(0, 59)]
        //[Display(Name = "Минуты")]
        //private int MinuteTo { get; set; }

    }
}