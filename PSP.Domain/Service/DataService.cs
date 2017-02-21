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
        public static baseusers GetBaseUser(string id)
        {
            return _entities.BaseUsers.FirstOrDefault(g => g.ID.Contains(id));

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

        // Значительно дольше чем Func<string, string> GetAuditorName = K => (from User in AllUsers where User.ID.ToLower() == K.ToLower() select User.Name).FirstOrDefault();
        public static string GetAuditorNameByIdService(string ident)
        {
            var user = _entities.Users.FirstOrDefault(item => item.ID.Equals(ident));
            return user == null ? string.Empty : user.Name;
        }
    }
}
