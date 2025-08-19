using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Abstractions;
using TaskManager.Domain.Entities;
using DomainTask = TaskManager.Domain.Entities.Task;

namespace TaskManager.Infrastructure.Persistence
{
    public class KanboardDbContext : DbContext, IUnitOfWork
    {
        public KanboardDbContext(DbContextOptions<KanboardDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<DomainTask> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TaskTag> TaskTags { get; set; }
        public DbSet<Swimlane> Swimlanes { get; set; }
        public DbSet<Attachment> Attachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurazione delle relazioni tra entità

            // User - Project (one-to-many)
            modelBuilder
                .Entity<User>()
                .HasMany(u => u.OwnedProjects)
                .WithOne(p => p.Owner)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // User - Task (assignee) (one-to-many)
            modelBuilder
                .Entity<User>()
                .HasMany(u => u.AssignedTasks)
                .WithOne(t => t.Assignee)
                .HasForeignKey(t => t.AssigneeId)
                .OnDelete(DeleteBehavior.SetNull);

            // User - Task (creator) (one-to-many)
            modelBuilder
                .Entity<User>()
                .HasMany(u => u.CreatedTasks)
                .WithOne(t => t.Creator)
                .HasForeignKey(t => t.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            // User - Comment (one-to-many)
            modelBuilder
                .Entity<User>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User - Attachment (one-to-many)
            modelBuilder
                .Entity<User>()
                .HasMany(u => u.UploadedAttachments)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UploadedById)
                .OnDelete(DeleteBehavior.Restrict);

            // Project - Board (one-to-one)
            modelBuilder
                .Entity<Project>()
                .HasOne(p => p.Board)
                .WithOne(b => b.Project)
                .HasForeignKey<Board>(b => b.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Project - Column (one-to-many)
            modelBuilder
                .Entity<Project>()
                .HasMany(p => p.Columns)
                .WithOne(c => c.Project)
                .HasForeignKey(c => c.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Project - Task (one-to-many)
            modelBuilder
                .Entity<Project>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Project - Category (one-to-many)
            modelBuilder
                .Entity<Project>()
                .HasMany(p => p.Categories)
                .WithOne(c => c.Project)
                .HasForeignKey(c => c.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Project - Tag (one-to-many)
            modelBuilder
                .Entity<Project>()
                .HasMany(p => p.Tags)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Project - Swimlane (one-to-many)
            modelBuilder
                .Entity<Project>()
                .HasMany(p => p.Swimlanes)
                .WithOne(s => s.Project)
                .HasForeignKey(s => s.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Column - Task (one-to-many)
            modelBuilder
                .Entity<Column>()
                .HasMany(c => c.Tasks)
                .WithOne(t => t.Column)
                .HasForeignKey(t => t.ColumnId)
                .OnDelete(DeleteBehavior.Cascade);

            // Task - Comment (one-to-many)
            modelBuilder
                .Entity<DomainTask>()
                .HasMany(t => t.Comments)
                .WithOne(c => c.Task)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // Task - Attachment (one-to-many)
            modelBuilder
                .Entity<DomainTask>()
                .HasMany(t => t.Attachments)
                .WithOne(a => a.Task)
                .HasForeignKey(a => a.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // Task - Tag (many-to-many through TaskTag)
            modelBuilder.Entity<TaskTag>().HasKey(tt => new { tt.TaskId, tt.TagId });

            modelBuilder
                .Entity<TaskTag>()
                .HasOne(tt => tt.Task)
                .WithMany()
                .HasForeignKey(tt => tt.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<TaskTag>()
                .HasOne(tt => tt.Tag)
                .WithMany()
                .HasForeignKey(tt => tt.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

        public override async System.Threading.Tasks.Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default
        )
        {
            // Aggiorna automaticamente le date di modifica
            var entries = ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is Domain.Common.BaseEntity
                    && (e.State == EntityState.Added || e.State == EntityState.Modified)
                );

            foreach (var entry in entries)
            {
                var entity = (Domain.Common.BaseEntity)entry.Entity;
                entity.UpdatedAt = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
