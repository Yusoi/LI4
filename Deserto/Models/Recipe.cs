using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Deserto.Models
{
    public class Recipe
    {
        [Key]
        public int recipeID { set; get; }
        [Required]
        [StringLength(64)]
        public string title { set; get; }
        [Required]
        public char original { set; get; }
        [Required]
        public int duration { set; get; }
        [Required]
        public char difficulty { set; get; }
        [Required]
        [StringLength(100)]
        public string nutValues { set; get; }
        [Required]
        [StringLength(200)]
        public string imagePath { set; get; }
        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<UserRecipe> UserRecipes { get; set; }


        public virtual ICollection<RecipeIngredient> RecipeIngredient { get; set; }

        public virtual ICollection<RecipeInstruction> RecipeInstruction { get; set; }
    }
}
