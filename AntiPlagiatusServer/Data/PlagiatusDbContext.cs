using AntiPlagiatusServer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AntiPlagiatusServer.Data
{
    public class PlagiatusDbContext : DbContext, IDbContext
    {
        public PlagiatusDbContext(DbContextOptions<PlagiatusDbContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<Content>().ToTable("Contents");
            modelBuilder.Entity<IgnoreRule>().ToTable("IgnoreRules");
            modelBuilder.Entity<LayerByDomain>().ToTable("LayerByDomains");
            modelBuilder.Entity<Domain>().ToTable("Domains");
            modelBuilder.Entity<OperationReport>().ToTable("OperationReports");
            modelBuilder.Entity<User>().ToTable("Users");
            //modelBuilder.Entity<Admin>().ToTable("Admins").HasKey(q => q.Login);

            //modelBuilder.Entity<Content>(entity =>
            //{
            //    entity.ToTable("Contents").HasKey(q => q.Id);
            //    entity.HasMany(item => item.OperationReports).WithOne(item => item.Content).HasForeignKey(item => item.ContentId);
            //});

            //modelBuilder.Entity<IgnoreRule>(entity =>
            //{
            //    entity.ToTable("IgnoreRules").HasKey(q => q.Id);
            //    entity.HasOne(item => item.OperationReport).WithMany(item => item.IgnoreRules).HasForeignKey(item => item.OperationReportId);
            //});

            //modelBuilder.Entity<LayerByDomain>(entity =>
            //{
            //    entity.ToTable("LayerByDomains").HasKey(q => q.Id);
            //    entity.HasOne(item => item.Domain).WithMany(item => item.Layers).HasForeignKey(item => item.DomainId);
            //});

            //modelBuilder.Entity<Domain>(entity =>
            //{
            //    entity.ToTable("Domains").HasKey(q => q.Id);
            //    entity.HasMany(item => item.Layers).WithOne(item => item.Domain).HasForeignKey(item => item.DomainId);
            //    entity.HasOne(item => item.OperationReport).WithMany(item => item.Domains).HasForeignKey(item => item.OperationReportId);
            //});

            //modelBuilder.Entity<OperationReport>(entity =>
            //{
            //    entity.ToTable("OperationReports").HasKey(q => q.Id);
            //    entity.HasOne(item => item.Content).WithMany(item => item.OperationReports).HasForeignKey(item => item.ContentId);
            //    entity.HasMany(item => item.IgnoreRules).WithOne(item => item.OperationReport).HasForeignKey(item => item.OperationReportId);
            //    entity.HasMany(item => item.Domains).WithOne(item => item.OperationReport).HasForeignKey(item => item.OperationReportId);
            //});

            //modelBuilder.Entity<User>(entity => { 
            //    entity.ToTable("Users").HasKey(q => q.Id);
            //    entity.HasMany(item => item.OperationReports).WithOne(item => item.User).HasForeignKey(item => item.UserId);
            //});
        }

        DbContext IDbContext.Database => this;
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<IgnoreRule> IgnoreRules { get; set; }
        public DbSet<Domain> Domains { get; set; }
        public DbSet<LayerByDomain> LayerByDomains { get; set; }
        public DbSet<OperationReport> OperationReports { get; set; }
    }
}
