using AdvancedTodoApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdvancedTodoApplication.Infrastructure
{
    public class ToDoContext : IdentityDbContext<ApplicationUser>
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
        {}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserBoard>()
                .HasKey(ub => new { ub.UserId, ub.BoardId });

            modelBuilder.Entity<UserBoard>()
                .HasOne(ub => ub.ApplicationUser)
                .WithMany(au => au.UserBoards)
                .HasForeignKey(ub => ub.UserId);

            modelBuilder.Entity<UserBoard>()
                .HasOne(ub => ub.Board)
                .WithMany(b => b.UserBoards) // If you add `public ICollection<UserBook> UserBooks { get; set; }` navigation property to Book model class then replace `.WithMany()` with `.WithMany(b => b.UserBooks)`
                .HasForeignKey(ub => ub.BoardId);


            modelBuilder.Entity<UserToDo>()
                .HasKey(ub => new { ub.UserId, ub.ToDoId });

            modelBuilder.Entity<UserToDo>()
                .HasOne(ub => ub.ApplicationUser)
                .WithMany(au => au.UserTodos)
                .HasForeignKey(ub => ub.UserId);

            modelBuilder.Entity<UserToDo>()
                .HasOne(ub => ub.ToDo)
                .WithMany(b => b.ToDoUsers) // If you add `public ICollection<UserBook> UserBooks { get; set; }` navigation property to Book model class then replace `.WithMany()` with `.WithMany(b => b.UserBooks)`
                .HasForeignKey(ub => ub.ToDoId);

        }

        public DbSet<Board> Board { get; set; }
        public DbSet<ToDo> ToDo { get; set; }
        public DbSet<AdvancedTodoApplication.Models.Category> Category { get; set; }
        public DbSet<UserBoard> UserBoard { get; set; }
        public DbSet<UserToDo> UserToDo { get; set; }

    }
}
