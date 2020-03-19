using Microsoft.EntityFrameworkCore;

namespace SalaryApp.Models
{
    public partial class MSSqlLocalDBContext : DbContext
    {
        public MSSqlLocalDBContext()
        {
        }

        public MSSqlLocalDBContext(DbContextOptions<MSSqlLocalDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BaseSalary> BaseSalary { get; set; }
        public virtual DbSet<Bonus> Bonus { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Position> Position { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<BaseSalary>(entity =>
            {
                entity.HasKey(e => new { e.PositionId, e.Years })
                    .HasName("PK__BaseSala__A68DAC6C0D6B15C7");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.BaseSalary)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BaseSalar__Posit__2F10007B");
            });

            modelBuilder.Entity<Bonus>(entity =>
            {
                entity.HasKey(e => e.Score)
                    .HasName("PK__Bonus__E028AC0417EE072A");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.InverseManager)
                    .HasForeignKey(d => d.ManagerId)
                    .HasConstraintName("FK__Employee__Manage__286302EC");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.PositionId)
                    .HasConstraintName("FK__Employee__Positi__276EDEB3");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Position__737584F6B42F2A47")
                    .IsUnique();
            });
        }
    }
}
