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
    public class Ingredient
    {
        [Key]
        public int ingredientID { set; get; }
        [Required]
        [StringLength(64)]
        public string name { set; get; }

        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<RecipeIngredient> RecipeIngredient { get; set; }
        [NotMapped]
        [JsonIgnore]
        public int quant { set; get; }

        public Ingredient() { }

        public Ingredient(Ingredient c)
        {
            this.name = c.name;
        }
    }
}
