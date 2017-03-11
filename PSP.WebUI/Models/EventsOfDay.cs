using System;
using System.Collections.Generic;

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
        public string UserName { get; set; }
        
        public DateTime Date { get; set; }
        public string EventDesc { get; set; }
        public IList<ElementaryActivity> Activities { get; set; }
    }
}