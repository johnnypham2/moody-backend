using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly CalendarService _data;
        public CalendarController(CalendarService dataFromService)
        {
            _data = dataFromService;
        }

        [HttpGet("CalendarByUserId/{userId}")]
        public IEnumerable<Day> GetCalendars(int userId)
        {
            return _data.GetCalendars(userId);
        }

        [HttpPost("AddDay")]
        public bool CreateDay(Day day){
            return _data.CreateDay(day);
        }

        [HttpPost("UpdateDay")]
        public bool UpdateDay(Day day){
            return _data.UpdateDay(day);
        }
    }
}