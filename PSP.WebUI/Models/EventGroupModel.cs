using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSP.WebUI.Models
{
    public class EventGroupModel
    {
        public EventGroupModel()
        {
            Users = new List<EventsUserOfPeriod>();
        }

        public string GroupName { get; set; }
        public IList<EventsUserOfPeriod> Users { get; set; }
    }
}