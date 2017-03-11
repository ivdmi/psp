using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PSP.WebUI.Models
{
    public class EventsOfDayViewModel
    {
        public EventsOfDayViewModel()
        {
            EventsOfDay = new EventsOfDay();
        }

        public IList<string> ClientsFrom { get; set; }
        public IList<string> AuditorsFrom { get; set; }
        public IList<string> AuditorsTo { get; set; }
        public EventsOfDay EventsOfDay { get; set; }
    }
}