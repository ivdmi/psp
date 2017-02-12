using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSP.WebUI.Models
{
    public class UsersTreeViewModel
    {
        public UsersTreeViewModel()
        {
            this.List = new List<UsersTreeViewModel>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public IList<UsersTreeViewModel> List { get; private set; }

        public bool IsChild
        {
            get
            {
                return this.List.Count == 0;
            }

        }

    }
}