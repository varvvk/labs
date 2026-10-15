using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL004.Models
{
    public class Celebrity
    {
        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string? Surname { get; set; }
        public string? PhotoPath { get; set; }
    }
}