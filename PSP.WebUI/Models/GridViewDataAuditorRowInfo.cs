namespace PSP.WebUI.Models
{
    public class GridViewDataAuditorRowInfo
    {
        public int Count { get; set; }
        public string Auditor { get; set; }
        public string State { get; set; }
        public int Days { get; set; }
        public int CalendarDays { get; set; }
        public string TotalMinutes { get; set; }
        public string Factories { get; set; }
    }
}