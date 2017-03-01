using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PSP.Domain.Concrete;

namespace PSP.WebUI.Models
{
    public class CloseMonthModel
    {
//        [Display(Name = "Предприятия")]
//        public List<string> Factories { get; set; }

        [Display(Name = "Предприятие")]
        public string SelectedFactory { get; set; }
        
        [Required]
        [Display(Name = "Год")]
        public int Year { get; set; }

        [Required]
        [Range(1, 12)]
        [Display(Name = "Месяц")]
        public int Month { get; set; }
        public int Money { get; set; }
        public List<GridViewDataAuditorCloseMonthRowInfo> AuditorsMonthList { get; set; }
    }
}