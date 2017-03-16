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
        [Range(0, 6, ErrorMessage = "�������� �� 0 �� 6")]
        public int ActivityKey { get; set; }
        
        [Display(Name = "�����������")]
        public string Comment { get; set; }

        //[Required]
        //[Range(0, 23)]
        //[Display(Name = "����")]
        //int HourFrom { get; set; }

        //[Required]
        //[Range(0, 59)]
        //[Display(Name = "������")]
        //int MinuteFrom { get; set; }

        //[Required]
        //[Range(0, 23)]
        //[Display(Name = "����")]
        //int HourTo { get; set; }

        //[Required]
        //[Range(0, 59)]
        //[Display(Name = "������")]
        //private int MinuteTo { get; set; }

    }
}