using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFConsole
{
    public partial class PWS_NamesContext : DbContext
    {
        public PWS_NamesContext()
        {
        }

        public PWS_NamesContext(DbContextOptions<PWS_NamesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<NameRepoJunction> NameRepoJunction { get; set; }
        public virtual DbSet<Names> Names { get; set; }
        public virtual DbSet<Repo> Repo { get; set; }
        public virtual DbSet<WinHistory> WinHistory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\;Database=PWS_Names;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NameRepoJunction>(entity =>
            {
                entity.HasKey(e => e.NameRepoId)
                    .HasName("nameRepo_PK");

                entity.Property(e => e.NameRepoId).HasColumnName("nameRepoId");

                entity.Property(e => e.NameRef).HasColumnName("nameRef");

                entity.Property(e => e.RepoRef).HasColumnName("repoRef");

                entity.HasOne(d => d.NameRefNavigation)
                    .WithMany(p => p.NameRepoJunction)
                    .HasForeignKey(d => d.NameRef)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("name_fk");

                entity.HasOne(d => d.RepoRefNavigation)
                    .WithMany(p => p.NameRepoJunction)
                    .HasForeignKey(d => d.RepoRef)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("repo_fk");
            });

            modelBuilder.Entity<Names>(entity =>
            {
                entity.HasKey(e => e.NameId)
                    .HasName("name_pk");

                entity.HasIndex(e => e.Name)
                    .HasName("name_uc")
                    .IsUnique();

                entity.Property(e => e.NameId).HasColumnName("nameId");

                entity.Property(e => e.DateWin).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.TopBool)
                    .IsRequired()
                    .HasColumnName("topBool")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.TopCount)
                    .HasColumnName("topCount")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.Wins).HasDefaultValueSql("('0')");
            });

            modelBuilder.Entity<Repo>(entity =>
            {
                entity.HasIndex(e => e.RepoName)
                    .HasName("repo_uc")
                    .IsUnique();

                entity.Property(e => e.RepoId).HasColumnName("repoId");

                entity.Property(e => e.RepoName)
                    .IsRequired()
                    .HasColumnName("repoName")
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<WinHistory>(entity =>
            {
                entity.Property(e => e.WinHistoryId).HasColumnName("winHistoryId");

                entity.Property(e => e.NameId).HasColumnName("nameId");

                entity.Property(e => e.WinDate)
                    .HasColumnName("winDate")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Name)
                    .WithMany(p => p.WinHistory)
                    .HasForeignKey(d => d.NameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_name_win");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
