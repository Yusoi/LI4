using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Deserto.Models
{
    public class UserRecipe
    {
        [Key]
        public int userID { get; set; }
        [NotMapped]
        [JsonIgnore]
        public virtual User user { get; set; }
        [Key]
        public int recipeID { get; set; }
        [NotMapped]
        [JsonIgnore]
        public virtual Recipe recipe { get; set; }
        public char rating { get; set; }
    }
}
