using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Deserto.Models
{
    public class Instruction
    {
        [Key]
        public int instructionID { set; get; }
        [Required]
        [StringLength(256)]
        public string text { set; get; }
        public char original;
        public virtual ICollection<InstructionExplanation> InstructionExplanations { get; set; }
        [NotMapped]
        [JsonIgnore]
        public int order;
        public Instruction() { }
        public Instruction(Instruction c)
        {
            this.text = c.text;
        }
    }
}
