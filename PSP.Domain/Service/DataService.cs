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
    public static class DataService
    {
        private readonly static IRepository _entities = new Repository();

        /// <summary>
        /// Gets the group 
        /// </summary>
        /// <param name="id">The group GUID.</param>
        /// <returns></returns>
        public static baseusers GetUser(string id)
        {
            return _entities.BaseUsers.SingleOrDefault(g => g.ID.Contains(id));

        }


        public static List<users> GetAllUsers()
        {
            return _entities.Users.ToList();
        }


        // Получение всех событий за промежуток времени
        public static List<events> GetEventsByDate(DateTime start, DateTime end)
        {
            // List<events> Events = _entities.Events.Find(P => P.Date >= Start && P.Date <= End).ToList();
            var events = _entities.Events.Where(ev => ev.Date >= start && ev.Date <= end).ToList();
            return events;
        }

    }
}
