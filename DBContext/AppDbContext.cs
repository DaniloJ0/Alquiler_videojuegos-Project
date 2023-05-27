using System;
using System.Collections.Generic;
using Alquiler_videojuegos.Models;
using Microsoft.EntityFrameworkCore;

namespace Alquiler_videojuegos.DBContext;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Rent> Rents { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rent>(entity =>
        {
            entity.HasOne(d => d.IdGameNavigation).WithMany(p => p.Rents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_rent_game");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Rents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_rent_user");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_client");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
