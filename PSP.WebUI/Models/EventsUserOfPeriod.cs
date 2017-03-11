using System;
using System.Collections.Generic;
using System.Drawing;
using PSP.Domain;

namespace PSP.WebUI.Models
{
    public class EventsUserOfPeriod
    {
        public EventsUserOfPeriod()
        {
            Cells = new List<CellElement>();
        }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public IList<CellElement> Cells { get; set; }
    }

    public class CellElement
    {
        public events DayEvent { get; set; }
        public string Text { get; set; }
        public string ToolTipText { get; set; }
        public Color BackColor { get; set; }
        public Color BackColor2 { get; set; }
        public bool DrawFill { get; set; }
        public int ColumnIndex { get; set; }
        public DateTime Date { get; set; }
    }
}