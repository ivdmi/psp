using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using PSP.Domain.Abstract;
using PSP.Domain.Concrete;

namespace PSP.Domain.Service
{
    public class DataService
    {
        private readonly IRepository _entities;

        public DataService(IRepository repository)
        {
            _entities = repository;
        }
       
        //public List<users> GetAllUsers()
        //{
        //    return _entities.Users.ToList();
        //}
        
        // Получение всех событий за промежуток времени
        public List<events> GetEventsByDate(DateTime start, DateTime end)
        {
            var events = _entities.Events.Where(ev => ev.Date >= start && ev.Date <= end).ToList();
            return events;
        }

        // Получение всех событий за промежуток времени
        public List<events> GetEventsByDateAndUserId(DateTime start, DateTime end, string Id)
        {
            var events = _entities.Events.Where(ev => ev.UserID.Contains(Id) && ev.Date >= start && ev.Date <= end).OrderBy(m=>m.Date).ToList();
            return events;
        }

        // Значительно дольше чем Func<string, string> GetAuditorName = K => (from User in AllUsers where User.ID.ToLower() == K.ToLower() select User.Name).FirstOrDefault();
        public string GetAuditorNameByIdService(string ident)
        {
            var user = _entities.Users.FirstOrDefault(item => item.ID.Equals(ident));
            return user == null ? string.Empty : user.Name;
        }
    }
}
