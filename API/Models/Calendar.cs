using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Day
    {
        public int Id { get; set; }
        public string? Mood { get; set; }
        public string? Comment { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
    }
}