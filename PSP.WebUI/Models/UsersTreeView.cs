using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PSP.Domain;
using PSP.Domain.Abstract;
using PSP.WebUI.Models;

namespace PSP.WebUI.Helpers
{
    public class UsersTreeView
    {
        private IRepository repository;

        public UsersTreeView(IRepository paramRepository)
        {
            repository = paramRepository;
            TreeViewData = new List<UsersTreeViewModel>();
            
        }
        
        public IList<UsersTreeViewModel> TreeViewData { get; set; }

        public IList<UsersTreeViewModel> GetTreeViewData()
        {
            var groups = from g in repository.Groups select g;
            var users = from u in repository.Users select u;

            foreach (var group in groups)
            {

                var newGroup = new UsersTreeViewModel()
                {
                    Id = group.ID,
                    Name = group.Name,
                    Type = "GROUP"
                };

                TreeViewData.Add(newGroup);
            }

            foreach (var user in users)
            {
                var newUser = new UsersTreeViewModel()
                {
                    Id = user.ID,
                    Name = user.Name,
                    Type = "USER"
                };

                foreach (var group in TreeViewData)
                {
                    if (group.Id.Equals(user.GroupID))
                    {
                        group.List.Add(newUser);
                    }
                }
            }

            return TreeViewData;
        }
    }
}