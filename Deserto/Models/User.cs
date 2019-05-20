using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Deserto.Models;

namespace Deserto.Models
{
    public class User
    {
        [Key]
        public int userID { set; get; }
        [Required]
        [StringLength(50)]
        public string nome { set; get; }
        [Required]
        //[DataType(DataType.EmailAddress)]
        public string email { set; get; }
        [Required]
        [StringLength(50)]
        public string password { set; get; }
        [Required]
        public char premium { set; get; }

        public virtual ICollection<ExcludedIngredients> ExcludedIngredients { get; set; }

        public virtual ICollection<UserRecipe> UserRecipes { get; set; } 

    }

    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InstructionExplanation>()
                .HasKey(pc => new { pc.instructionID, pc.explanationID });
            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(pc => new { pc.recipeID, pc.ingredientID });
            modelBuilder.Entity<RecipeInstruction>()
                .HasKey(pc => new { pc.recipeID, pc.instructionID });
            modelBuilder.Entity<ExcludedIngredients>()
                .HasKey(pc => new { pc.userID, pc.ingredientID });
            modelBuilder.Entity<UserRecipe>()
                .HasKey(pc => new { pc.userID, pc.recipeID });


            modelBuilder.Entity<UserRecipe>()
                .HasOne(pc => pc.user)
                .WithMany(p => p.UserRecipes)
                .HasForeignKey(pc => pc.userID);

            modelBuilder.Entity<UserRecipe>()
                .HasOne(pc => pc.recipe)
                .WithMany(c => c.UserRecipes)
                .HasForeignKey(pc => pc.recipeID);
        }
        /*
        modelBuilder.Entity<Task>()
                .HasOne(t => t.user)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.user_id)
                .HasConstraintName("ForeignKey_User_Task");
                */
        public DbSet<User> user { get; set; }
        public DbSet<Recipe> recipe { get; set; }
        public DbSet<UserRecipe> userRecipe { get; set; }
        public DbSet<Instruction> instruction { get; set; }
        public DbSet<Explanation> explanation { get; set; }
        public DbSet<Ingredient> ingredient { get; set; }
        public DbSet<ExcludedIngredients> excludedIngredients { get; set; }
        public DbSet<RecipeIngredient> recipeIngredient { get; set; }
        public DbSet<RecipeInstruction> recipeInstruction { get; set; }
        public DbSet<InstructionExplanation> instructionExplanation { get; set; }
        public int step ;

    }
}