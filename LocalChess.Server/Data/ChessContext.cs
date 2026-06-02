using LocalChess.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LocalChess.Server.Data
{
    public class ChessContext : DbContext
    {
        public ChessContext()
        {
        }

        public ChessContext(DbContextOptions<ChessContext> options)
            : base(options)
        {
        }

        public DbSet<SavedGame> SavedGames { get; set; }
        public DbSet<SavedMove> SavedMoves { get; set; }
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
                .Property(g => g.SaveKey)
                .HasMaxLength(100);

            modelBuilder.Entity<SavedGame>()
                .HasIndex(g => g.SaveKey)
                .IsUnique()
                .HasFilter("[SaveKey] IS NOT NULL");

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
