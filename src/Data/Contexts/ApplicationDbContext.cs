using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Data.Contexts {

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApplicationDbContext (
            DbContextOptions options,
            IHttpContextAccessor httpContextAccessor) : base (options) {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<QuestionSet> QuestionSets { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<FeedbackBatch> FeedbackBatchs { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MeetingCategory> MeetingCategories { get; set; }

        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.EnableSensitiveDataLogging ();
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            base.OnModelCreating (modelBuilder);

            modelBuilder.Entity<MeetingCategory> ()
                .HasKey (bc => new { bc.MeetingId, bc.CategoryId });
            modelBuilder.Entity<MeetingCategory> ()
                .HasOne (mc => mc.meeting)
                .WithMany (b => b.meetingCategories)
                .HasForeignKey (bc => bc.MeetingId);

            modelBuilder.Entity<MeetingCategory> ()
                .HasOne (mc => mc.Category)
                .WithMany (c => c.meetingCategories)
                .HasForeignKey (mc => mc.CategoryId);

            modelBuilder.Entity<MeetingCategory> ()
                .Ignore (b => b.meeting);

            modelBuilder.Entity<Category> ()
                .Ignore (b => b.meetingCategories);

            // modelBuilder.Entity<Meeting> ()
            //     .HasOne (m => m.ApplicationUser)
            //     .WithMany (m => m.Meetings);

            modelBuilder.Entity<ApplicationUser> (entity => {
                entity.HasKey (n => n.Id);

                entity.HasMany (u => u.Meetings)
                    .WithOne (m => m.ApplicationUser)
                    .HasForeignKey (n => n.ApplicationUserId)
                    .IsRequired ()
                    .OnDelete (DeleteBehavior.Restrict);

                entity.HasMany (u => u.Ratings)
                    .WithOne (m => m.ApplicationUser)
                    .HasForeignKey (n => n.ApplicationUserId)
                    .IsRequired ()
                    .OnDelete (DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Category> (builder => {
                builder.Property (e => e.Name).Metadata.SetAfterSaveBehavior (PropertySaveBehavior.Ignore);
            });

            modelBuilder.Entity<FeedbackBatch> ()
                .HasOne (e => e.QuestionSet)
                .WithMany (e => e.FeedbackBatches)
                .OnDelete (DeleteBehavior.Restrict);

            modelBuilder.Entity<QuestionSet> ()
                .HasOne (e => e.Company)
                .WithMany (e => e.questionSets)
                .HasForeignKey (e => e.CompanyId)
                .OnDelete (DeleteBehavior.Restrict);

            modelBuilder.Entity<QuestionSet> ()
                .HasMany (e => e.Questions)
                .WithOne ()
                .OnDelete (DeleteBehavior.Cascade);

            // // make sure that the primary key is the set of {QuestionSetId, Vertion}
            // modelBuilder.Entity<QuestionSet>().HasKey(q => new
            // {
            //     q.QuestionSetId,
            //     q.Version
            // });

            // modelBuilder.Entity<ApplicationUser>()
            // .Property(u => u.Created)
            // .HasDefaultValueSql("getdate()");
        }

        public override int SaveChanges () {
            AddTimestamps ();
            return base.SaveChanges ();
        }

        public async override Task<int> SaveChangesAsync (CancellationToken cancellationToken = default) {
            AddTimestamps ();
            return await base.SaveChangesAsync ();
        }

        private void AddTimestamps () {
            var entities = ChangeTracker.Entries ().Where (x =>
                x.Entity is BaseEntity &&
                (x.State == EntityState.Added || x.State == EntityState.Modified));

            string userId = null;
            try {
                userId = _httpContextAccessor.HttpContext.User.FindFirst (ClaimTypes.NameIdentifier).Value;
            } catch (Exception e) {
                Console.WriteLine (e);
            }

            var currentUsername = !string.IsNullOrEmpty (userId) ?
                userId :
                "Anonymous";

            foreach (var entity in entities) {
                if (entity.State == EntityState.Added) {
                    ((BaseEntity) entity.Entity).CreatedDate = DateTime.UtcNow;
                    ((BaseEntity) entity.Entity).CreatedBy = currentUsername;
                }

                ((BaseEntity) entity.Entity).ModifiedDate = DateTime.UtcNow;
                ((BaseEntity) entity.Entity).ModifiedBy = currentUsername;
            }

        }
    }
}