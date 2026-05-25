using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocalChess.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LocalChess.Data.Data
{
    public class ChessContext : DbContext
    {

        public DbSet<SavedGame> SavedGames { get; set; }
        public DbSet<SavedMove> SavedMoves { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=LocalChessDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SavedGame>()
                .HasMany(g => g.Moves)
                .WithOne(m => m.SavedGame)
                .HasForeignKey(m => m.SavedGameId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SavedGame>()
                .Property(g => g.LobbyName)
                .HasMaxLength(100);

            modelBuilder.Entity<SavedGame>()
                .Property(g => g.FinalFen)
                .HasMaxLength(100);

            modelBuilder.Entity<SavedMove>()
                .Property(m => m.FromSquare)
                .HasMaxLength(2);

            modelBuilder.Entity<SavedMove>()
                .Property(m => m.ToSquare)
                .HasMaxLength(2);

            modelBuilder.Entity<SavedMove>()
                .Property(m => m.Notation)
                .HasMaxLength(20);
        }
    }
}
