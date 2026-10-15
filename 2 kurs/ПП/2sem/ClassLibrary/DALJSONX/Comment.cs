using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALJSONX
{
    public class Comment
    {
        [Key]
        public int? Id { get; set; }

        [ForeignKey("WSRef")]
        public int? WSRefId { get; set; }
        public DateTime? Stamp { get; set; }
        public string? Commtext { get; set; }
    }
}
