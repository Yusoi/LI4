using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Deserto.Models
{
    public class RecipeBook
    {
        [Key]
        public int recipeID { get; set; }
        [NotMapped]
        [JsonIgnore]
        public virtual Recipe Recipe { set; get; }
        [Key]
        public int userID { set; get; }
        [NotMapped]
        [JsonIgnore]
        public virtual User User { set; get; }

        public RecipeBook(int recipeID, int userID)
        {
            this.recipeID = recipeID;
            this.userID = userID;
        }
    }
}
