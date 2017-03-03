using System.Collections.Generic;
using System.Drawing;
using PSP.Domain;

namespace PSP.WebUI.Models
{
    public class EventUser
    {
        public EventUser()
        {
 //           DaysList = new List<string>();
            Cells = new List<CellElement>();
        }
        public string UserName { get; set; }
//        public IList<string> DaysList { get; set; }

  //      public IList<events> EventsList { get; set; }
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
    }
}