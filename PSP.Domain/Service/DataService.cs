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
            var events = _entities.Events.Where(ev => ev.UserID.Contains(Id) && ev.Date >= start && ev.Date <= end).OrderBy(m => m.Date).ToList();
            return events;
        }

        public events GetEventsByDayAndUserId(DateTime date, string Id)
        {
            var events = _entities.Events.FirstOrDefault(ev => ev.UserID.Contains(Id) && ev.Date == date);
            return events;
        }

        // Значительно дольше чем Func<string, string> GetAuditorName = K => (from User in AllUsers where User.ID.ToLower() == K.ToLower() select User.Name).FirstOrDefault();
        public string GetAuditorNameByIdService(string ident)
        {
            var user = _entities.Users.FirstOrDefault(item => item.ID.Equals(ident));
            return user == null ? string.Empty : user.Name;
        }

        public void UpdateEvent(events _event)
        {
            _entities.Context.Entry(_event).State = EntityState.Modified;
            SaveChanges();
        }
        public void AddEvent(events _event)
        {
            _entities.Context.events.Add(_event);
            if (_event.ID != null && _event.ID != "00000000-0000-0000-0000-000000000000")
            {
                SaveChanges();
            }
        }

        public List<string> GetFactoryList()
        {
            var factories = _entities.Clients.Select(c => c.Company.Trim()).OrderBy(c => c).ToList();
            return factories;
        }

        public void SaveChanges()
        {
            _entities.Context.SaveChanges();
        }
    }
}
