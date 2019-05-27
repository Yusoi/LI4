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

        public UserRecipe() { }

        public UserRecipe(int userID, int recipeID, char rating)
        {
            this.userID = userID;
            this.recipeID = recipeID;
            this.rating = rating;
        }

        public UserRecipe(UserRecipe c)
        {
            this.userID = c.userID;
            this.recipeID = c.recipeID;
            this.rating = c.rating;
        }
    }


}
