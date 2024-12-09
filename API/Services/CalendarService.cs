using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Models;

namespace API.Services
{
    public class CalendarService
    {
        private readonly AppDbContext _context;
        public CalendarService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Day> GetCalendars(int userId)
        {
            return _context.Days.Where( d => d.UserId == userId);
        }

        public bool CreateDay(Day day)
        {
            bool result = false;
            if(!DoesDayExist(day.UserId, day.Date))
            {
                Day newDay = new Day();
                newDay.UserId = day.UserId;
                newDay.Date = day.Date;
                newDay.Mood = day.Mood;
                newDay.Comment = day.Comment;

                _context.Add(newDay);
                result = _context.SaveChanges() != 0;
            }

            return result;
        }

        public bool DoesDayExist(int userId, DateTime date)
        {
            return _context.Days.SingleOrDefault(d => d.UserId == userId && d.Date == date) !=null;
        }

        public bool UpdateDay(Day day)
        {
            Day existingDay = _context.Days.SingleOrDefault(d => d.UserId == day.UserId && d.Date == day.Date);

            if(existingDay != null)
            {
            existingDay.Mood = day.Mood;
            existingDay.Comment = day.Comment;
            }
            return _context.SaveChanges() != 0;
        }
    }
}