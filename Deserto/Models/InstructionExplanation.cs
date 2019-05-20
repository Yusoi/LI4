using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Deserto.Models
{
    public class InstructionExplanation
    {
        [Key]
        public int instructionID { set; get; }
        [NotMapped]
        [JsonIgnore]
        public Instruction Instruction { set; get; }
        [Key]
        public int explanationID { set; get; }
        [NotMapped]
        [JsonIgnore]
        public Explanation Explanation { set; get; }
    }
}
