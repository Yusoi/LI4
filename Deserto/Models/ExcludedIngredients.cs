using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Deserto.Models
{
    public class ExcludedIngredients
    {
        [Key]
        public int ingredientID { set; get; }
        [NotMapped]
        [JsonIgnore]
        public virtual Ingredient Ingredient { set; get; }
        [Key]
        public int userID { set; get; }
        [NotMapped]
        [JsonIgnore]
        public virtual User User { set; get; }
    }
}
