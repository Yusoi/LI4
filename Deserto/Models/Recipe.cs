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
        public int original { set; get; }
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
        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<RecipeBook> RecipeBooks { get; set; }
        [NotMapped]
        [JsonIgnore]
        public char Rating { set; get; }

        public virtual ICollection<RecipeIngredient> RecipeIngredient { get; set; }

        public virtual ICollection<RecipeInstruction> RecipeInstruction { get; set; }

        [NotMapped]
        [JsonIgnore]
        public List<Ingredient> ingredients { set; get; } = new List<Ingredient>();
        [NotMapped]
        [JsonIgnore]
        public List<Instruction> instrucoes { set; get; } = new List<Instruction>();

        public Recipe()
        {

        }


        public Recipe(Recipe r)
        {
            title = r.title;
            duration = r.duration;
            difficulty = r.difficulty;
            nutValues = r.nutValues;
            imagePath = r.imagePath;
        }

        public Recipe( string title, char original, int duration, char difficulty, string nutValues, string imagePath)
        {
            this.title = title;
            this.original = original;
            this.duration = duration;
            this.difficulty = difficulty;
            this.nutValues = nutValues;
            this.imagePath = imagePath;
        }
    }
}
