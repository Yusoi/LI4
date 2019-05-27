using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Deserto.Models
{
    public class RecipeIngredient
    {
        [Key]
        public int recipeID { get; set; }
        [NotMapped]
        [JsonIgnore]
        public virtual Recipe Recipe { set; get; }
        [Key]
        public int ingredientID { set; get; }
        [NotMapped]
        [JsonIgnore]
        public virtual Ingredient Ingredient { set; get; }
        [Required]
        public int quant { get; set; }

        public RecipeIngredient(int recipeID, int ingredientID, int quant)
        {
            this.recipeID = recipeID;
            this.ingredientID = ingredientID;
            this.quant = quant;
        }
    }
}
