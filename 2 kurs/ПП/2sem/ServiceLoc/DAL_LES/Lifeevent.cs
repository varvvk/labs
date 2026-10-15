using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_LES
{
    public interface ILifeevent : ILifeevent<Lifeevent> { }
    public class Lifeevent
    {
        public Lifeevent() { this.Description = ""; }
        public int Id { get; set; }
        public int CelebrityId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? ReqPhotoPath { get; set; }

        public Celebrity? Celebrity { get; set; } // Навигационное свойство
    }
}
