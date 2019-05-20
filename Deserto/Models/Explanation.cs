using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Deserto.Models
{
    public class Explanation
    {
        [Key]
        public int explanationID { set; get; }
        [Required]
        [StringLength(50)]
        public string term { set; get; }
        [Required]
        [StringLength(256)]
        public string explanation { set; get; }
        public virtual ICollection<InstructionExplanation> InstructionExplanations { get; set; }

    }
}