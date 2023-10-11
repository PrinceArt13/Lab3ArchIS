using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Server.Entities;

public partial class PrintsevContext : DbContext
{
    public PrintsevContext()
    {
    }

    public PrintsevContext(DbContextOptions<PrintsevContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddUserSecrets<PrintsevContext>()
        .Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString(nameof(PrintsevContext)));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(e => e.StudentId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("student ID");
            entity.Property(e => e.Course).HasColumnName("course");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("first name");
            entity.Property(e => e.Grant).HasColumnName("grant");
            entity.Property(e => e.Group)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("group");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("last name");
            entity.Property(e => e.Sex).HasColumnName("sex");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
