using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PSP.WebUI.Models
{
    // Модель для окна ввода событий
    public class EventsOfDay
    {
        public EventsOfDay()
        {
            Activities = new List<ElementaryActivity>();
            Date = new DateTime();
        }
        public string UserId { get; set; }
        
        [Display(Name = "Сотрудник ")]
        public string UserName { get; set; }
        
        [Display(Name = "Дата: ")]
        [DisplayFormat(DataFormatString = "{0:dd:MM:yyyy}")]
        public DateTime Date { get; set; }
        [Display(Name = "Комментарии: ")]
        public string Comments { get; set; }
        [Display(Name = "Суть консультации: ")]
        public string EventDesc { get; set; }
        public IList<ElementaryActivity> Activities { get; set; }
    }
}