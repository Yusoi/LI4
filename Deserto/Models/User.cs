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
        public int UserID { set; get; }
        [Required]
        [StringLength(50)]
        public string Nome { set; get; }
        [Required]
        //[DataType(DataType.EmailAddress)]
        public string Email { set; get; }
        [Required]
        [StringLength(50)]
        public string Password { set; get; }
        [Required]
        public char Premium { set; get; }

        public virtual ICollection<ExcludedIngredients> ExcludedIngredients { get; set; }

        public virtual ICollection<UserRecipe> UserRecipes { get; set; } 

        public virtual ICollection<RecipeBook> RecipeBooks { get; set; }

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
            modelBuilder.Entity<RecipeBook>()
                .HasKey(pc => new { pc.recipeID, pc.userID });


            modelBuilder.Entity<UserRecipe>()
                .HasOne(pc => pc.user)
                .WithMany(p => p.UserRecipes)
                .HasForeignKey(pc => pc.userID);

            modelBuilder.Entity<UserRecipe>()
                .HasOne(pc => pc.recipe)
                .WithMany(c => c.UserRecipes)
                .HasForeignKey(pc => pc.recipeID);

            modelBuilder.Entity<RecipeBook>()
                .HasOne(pc => pc.user)
                .WithMany(p => p.RecipeBooks)
                .HasForeignKey(pc => pc.recipeID);

            modelBuilder.Entity<RecipeBook>()
                .HasOne(pc => pc.recipe)
                .WithMany(p => p.RecipeBooks)
                .HasForeignKey(pc => pc.recipeID);
        }
        /*
        modelBuilder.Entity<Task>()
                .HasOne(t => t.user)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.user_id)
                .HasConstraintName("ForeignKey_User_Task");
                */
        public DbSet<User> User { get; set; }
        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<UserRecipe> UserRecipe { get; set; }
        public DbSet<RecipeBook> RecipeBook { get; set; }
        public DbSet<Instruction> Instruction { get; set; }
        public DbSet<Explanation> Explanation { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<ExcludedIngredients> ExcludedIngredients { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredient { get; set; }
        public DbSet<RecipeInstruction> RecipeInstruction { get; set; }
        public DbSet<InstructionExplanation> InstructionExplanation { get; set; }
        public int step ;

    }
}