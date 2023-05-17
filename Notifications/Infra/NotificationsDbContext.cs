using Microsoft.EntityFrameworkCore;
using Notifications.Domain;
using Notifications.Infra.Configuration;

namespace Notifications.Infra;

public class NotificationsDbContext : DbContext
{
    public DbSet<Notification> Notifications { get; set; } = default!;

    public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Notification>()
            .Property(n => n.Payload)
            .HasColumnType("jsonb");
        modelBuilder.Entity<Notification>()
            .Property(n => n.Topic)
            .HasConversion<NotificationTopicConverter>();
    }
}