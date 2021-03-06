﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using PSP.Domain.Abstract;

namespace PSP.Domain.Service
{
    public class GroupService
    {
        private readonly IRepository _entities;

        public GroupService(IRepository repository)
        {
            _entities = repository;
        }
        
        public groups GetGroupbyId(string id)
        {
            return _entities.Groups.FirstOrDefault(g => g.ID.Equals(id));
        }

        public groups GetGroupbyName(string name)
        {
            return _entities.Groups.FirstOrDefault(g => g.Name.Equals(name));
        }

        public void AddGroup(groups group)
        {
            _entities.Context.groups.Add(group);
            SaveChanges();
        }

        public void RemoveGroup(string id)
        {
            var grp = _entities.Context.groups.Find(id);
            if (grp != null)
            {
                _entities.Context.groups.Remove(grp);
            }
            SaveChanges();
        }

        public void UpdateGroup(groups group)
        {
            _entities.Context.Entry(group).State = EntityState.Modified;
            SaveChanges();
        }
        
        public IList<groups> GetAllGroups()
        {
            return _entities.Groups.ToList();
        }

        // НЕ РАБОТАЕТ - УТОЧНИТЬ У АНЮТКИ !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // Проблема во внутреннем цикле по users
        public IList<groups> GetGroupsWithActiveUsers()
        {
            IList<groups> grpSelected = new List<groups>();

            foreach (var group in _entities.Groups)
            {
                var usersActive = group.users.Where(us => us.Hidden == 0);
                group.users = usersActive.ToList();
                grpSelected.Add(group);
            }

            return grpSelected;
        }

        public IList<string> GetAllGroupsNames()
        {
//            var names = from g in GetAllGroups() select g.Name;
            var names = GetAllGroups().Select(p => p.Name);
            return names.ToList();
        }

        public void SaveChanges()
        {
            _entities.Context.SaveChanges();
        }

        public string GetUserNameById(string id)
        {
            var firstOrDefault = _entities.Users.FirstOrDefault(g => g.ID.Equals(id));
            return firstOrDefault != null ? firstOrDefault.Name : null;
        }
    }
}
