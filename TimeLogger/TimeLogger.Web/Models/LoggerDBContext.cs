using Microsoft.EntityFrameworkCore;

namespace TimeLogger.Web.Models
{
    public partial class LoggerDBContext : DbContext
    {
        public LoggerDBContext()
        {
        }

        public LoggerDBContext(DbContextOptions<LoggerDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DayLog> DayLog { get; set; }
        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=LoggerDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DayLog>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(50);

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.DayLog)
                    .HasForeignKey(d => d.EmpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DayLog_Profile");
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.HasKey(e => e.EmpId);

                entity.Property(e => e.Designation).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Token).IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Profile)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Profile_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .HasName("IX_User");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Phone).HasMaxLength(15);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}