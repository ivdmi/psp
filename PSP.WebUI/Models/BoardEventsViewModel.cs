using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PSP.Domain.Service;

namespace PSP.WebUI.Models
{
    // Модель для окна ввода событий
    public class BoardEventsViewModel
    {
        public BoardEventsViewModel()
        {
            States = EventHelper.States;
        }

        public EventsOfDay EventsOfDay { get; set; }
        public IList<string> Factories { get; set; }

        public EventHelper.StateElement[] States { get; set; }
    }
}