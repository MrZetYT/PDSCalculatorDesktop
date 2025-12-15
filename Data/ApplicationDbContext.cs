using Microsoft.EntityFrameworkCore;
using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Models.DTOs;

namespace PDSCalculatorDesktop.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<WaterUseType> WaterUseTypes { get; set; }
        public DbSet<ControlPoint> ControlPoints { get; set; }
        public DbSet<Enterprise> Enterprises { get; set; }
        public DbSet<Substance> Substances { get; set; }
        public DbSet<SubstanceWaterUseCharacteristic> SubstanceWaterUseCharacteristics { get; set; }
        public DbSet<Discharge> Discharges { get; set; }
        public DbSet<TechnicalParameters> TechnicalParameters { get; set; }
        public DbSet<BackgroundConcentration> BackgroundConcentrations { get; set; }
        public DbSet<DischargeConcentration> DischargeConcentrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ============================================
            // WaterUseType (Тип водопользования)
            // ============================================
            modelBuilder.Entity<WaterUseType>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(e => e.Code)
                    .IsUnique();
            });

            // ============================================
            // Substance (Загрязняющее вещество)
            // ============================================
            modelBuilder.Entity<Substance>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.KNK)
                    .IsRequired();

                entity.HasIndex(e => e.Code)
                    .IsUnique();

                entity.ToTable(t => t.HasCheckConstraint("CK_Substances_KNK", "\"KNK\" >= 0"));
            });

            // ============================================
            // SubstanceWaterUseCharacteristic
            // ============================================
            modelBuilder.Entity<SubstanceWaterUseCharacteristic>(entity =>
            {
                entity.Property(e => e.GroupLFV)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.HazardClass)
                    .IsRequired();

                entity.Property(e => e.PDK)
                    .IsRequired();

                entity.HasOne(e => e.Substance)
                    .WithMany(s => s.WaterUseCharacteristics)
                    .HasForeignKey(e => e.SubstanceId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.WaterUseType)
                    .WithMany(w => w.SubstanceCharacteristics)
                    .HasForeignKey(e => e.WaterUseTypeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.SubstanceId, e.WaterUseTypeId })
                    .IsUnique();

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_SubstanceWaterUseCharacteristics_HazardClass",
                        "\"HazardClass\" BETWEEN 1 AND 4");
                    t.HasCheckConstraint("CK_SubstanceWaterUseCharacteristics_PDK", "\"PDK\" > 0");
                });
            });

            // ============================================
            // ControlPoint (Контрольный створ)
            // ============================================
            modelBuilder.Entity<ControlPoint>(entity =>
            {
                entity.Property(e => e.Number)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(e => e.Number)
                    .IsUnique();

                entity.HasOne(e => e.WaterUseType)
                    .WithMany(w => w.ControlPoints)
                    .HasForeignKey(e => e.WaterUseTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ============================================
            // Enterprise (Предприятие)
            // ============================================
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

            // ============================================
            // Discharge (Выпуск сточных вод)
            // ============================================
            modelBuilder.Entity<Discharge>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.RegistrationDate)
                    .IsRequired();

                entity.HasIndex(e => e.Code)
                    .IsUnique();

                entity.HasOne(d => d.Enterprise)
                    .WithMany(e => e.Discharges)
                    .HasForeignKey(d => d.EnterpriseId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.ControlPoint)
                    .WithMany(c => c.Discharges)
                    .HasForeignKey(d => d.ControlPointId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ============================================
            // TechnicalParameters (Технические параметры)
            // ============================================
            modelBuilder.Entity<TechnicalParameters>(entity =>
            {
                entity.Property(e => e.ValidFrom)
                    .IsRequired();

                entity.HasOne(t => t.Discharge)
                    .WithMany(d => d.TechnicalParameters)
                    .HasForeignKey(t => t.DischargeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.ValidFrom);

                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_TechnicalParameters_Diameter", "\"Diameter\" > 0");
                    t.HasCheckConstraint("CK_TechnicalParameters_FlowRate", "\"FlowRate\" > 0");
                    t.HasCheckConstraint("CK_TechnicalParameters_WaterFlowVelocity", "\"WaterFlowVelocity\" > 0");
                    t.HasCheckConstraint("CK_TechnicalParameters_DischargeAngle",
                        "\"DischargeAngle\" >= 0 AND \"DischargeAngle\" <= 360");
                    t.HasCheckConstraint("CK_TechnicalParameters_DistanceToWaterSurface", "\"DistanceToWaterSurface\" > 0");
                    t.HasCheckConstraint("CK_TechnicalParameters_DistanceToShore", "\"DistanceToShore\" > 0");
                    t.HasCheckConstraint("CK_TechnicalParameters_DistanceToControlPoint", "\"DistanceToControlPoint\" > 0");
                });
            });

            // ============================================
            // BackgroundConcentration (Фоновая концентрация)
            // ============================================
            modelBuilder.Entity<BackgroundConcentration>(entity =>
            {
                entity.Property(e => e.Concentration)
                    .IsRequired();

                entity.Property(e => e.MeasurementDate)
                    .IsRequired();

                entity.HasOne(b => b.ControlPoint)
                    .WithMany(c => c.BackgroundConcentrations)
                    .HasForeignKey(b => b.ControlPointId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(b => b.Substance)
                    .WithMany(s => s.BackgroundConcentrations)
                    .HasForeignKey(b => b.SubstanceId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.MeasurementDate);

                entity.HasIndex(e => new { e.ControlPointId, e.SubstanceId, e.MeasurementDate })
                    .IsUnique();

                entity.ToTable(t => t.HasCheckConstraint("CK_BackgroundConcentrations_Concentration",
                    "\"Concentration\" >= 0"));
            });

            // ============================================
            // DischargeConcentration (Концентрация в выпуске)
            // ============================================
            modelBuilder.Entity<DischargeConcentration>(entity =>
            {
                entity.Property(e => e.Concentration)
                    .IsRequired();

                entity.Property(e => e.MeasurementDate)
                    .IsRequired();

                entity.HasOne(d => d.Discharge)
                    .WithMany(discharge => discharge.DischargeConcentrations)
                    .HasForeignKey(d => d.DischargeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Substance)
                    .WithMany(s => s.DischargeConcentrations)
                    .HasForeignKey(d => d.SubstanceId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.MeasurementDate);

                entity.HasIndex(e => new { e.DischargeId, e.SubstanceId, e.MeasurementDate })
                    .IsUnique();

                entity.ToTable(t => t.HasCheckConstraint("CK_DischargeConcentrations_Concentration",
                    "\"Concentration\" >= 0"));
            });
        }
    }
}