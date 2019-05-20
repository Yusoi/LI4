using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Deserto.Models
{
    public class RecipeInstruction
    {
        [Key]
        public int recipeID { get; set; }
        [NotMapped]
        [JsonIgnore]
        public Recipe Recipe { set; get; }

        [Key] 
        public int instructionID { set; get; }
        [NotMapped]
        [JsonIgnore]
        public Instruction Instruction { set; get; }

        [Required]
        public int ordernr { set; get; }
    }
}
