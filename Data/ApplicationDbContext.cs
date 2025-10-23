using Microsoft.EntityFrameworkCore;
using PDSCalculatorDesktop.Models;
using System.Windows.Navigation;

namespace PDSCalculatorDesktop.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ControlPoint> ControlPoints { get; set; }
        public DbSet<Discharge> Discharges { get; set; }
        public DbSet<Enterprise> Enterprises { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Substance> Substances { get; set; }
        public DbSet<TechnicalParameters> TechnicalParameters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Enterprise>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.HasIndex(e => e.Code)
                    .IsUnique();
            });

            modelBuilder.Entity<Discharge>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property<string>(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.HasOne(d => d.Enterprise)
                    .WithMany(e => e.Discharges)
                    .HasForeignKey(d => d.EnterpriseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.ControlPoint)
                    .WithMany(e => e.Discharges)
                    .HasForeignKey(d => d.ControlPointId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ControlPoint>(entity =>
            {
                entity.Property(e => e.Number)
                    .HasMaxLength(30);

                entity.Property(e => e.Name)
                    .HasMaxLength(100);

                entity.HasIndex(e => e.Number)
                    .IsUnique();
            });

            modelBuilder.Entity<Substance>(entity =>
            {
                entity.Property(e => e.Code)
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .HasMaxLength(200);

                entity.HasIndex(e => e.Code)
                    .IsUnique();

                entity.Property(e => e.GroupLFV)
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<TechnicalParameters>(entity =>
            {
                entity.HasOne(d => d.Discharge)
                    .WithMany(e => e.TechnicalParameters)
                    .HasForeignKey(d => d.DischargeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_TechnicalParameters_Diameter", "\"Diameter\" > 0");
                    t.HasCheckConstraint("CK_TechnicalParameters_FlowRate", "\"FlowRate\" > 0");
                    t.HasCheckConstraint("CK_TechnicalParameters_WaterFlowVelocity", "\"WaterFlowVelocity\" > 0");
                    t.HasCheckConstraint("CK_TechnicalParameters_DischargeAngle", "\"DischargeAngle\" > 0");
                    t.HasCheckConstraint("CK_TechnicalParameters_DistanceToWaterSurface", "\"DistanceToWaterSurface\" > 0");
                    t.HasCheckConstraint("CK_TechnicalParameters_DistanceToShore", "\"DistanceToShore\" > 0");
                    t.HasCheckConstraint("CK_TechnicalParameters_DistanceToControlPoint", "\"DistanceToControlPoint\" > 0");
                });
       
            });

            modelBuilder.Entity<Measurement>(entity =>
            {
                entity.HasOne(m => m.Substance)
                    .WithMany()
                    .HasForeignKey(m => m.SubstanceId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Discharge)
                    .WithMany()
                    .HasForeignKey(m => m.DischargeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                entity.HasOne(m => m.ControlPoint)
                    .WithMany()
                    .HasForeignKey(m => m.ControlPointId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                entity.ToTable(t => t.HasCheckConstraint(
                    "CK_Measurement_XOR",
                    "(\"DischargeId\" IS NOT NULL AND \"ControlPointId\" IS NULL) OR (\"DischargeId\" IS NULL AND \"ControlPointId\" IS NOT NULL)"
                ));
            });
        }
    }
}