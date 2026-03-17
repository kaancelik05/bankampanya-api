using Bankampanya.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bankampanya.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<RefreshTokenGrant> RefreshTokenGrants => Set<RefreshTokenGrant>();
    public DbSet<Campaign> Campaigns => Set<Campaign>();
    public DbSet<CampaignTerm> CampaignTerms => Set<CampaignTerm>();
    public DbSet<TrackingTemplate> TrackingTemplates => Set<TrackingTemplate>();
    public DbSet<CreditOffer> CreditOffers => Set<CreditOffer>();
    public DbSet<NotificationTemplate> NotificationTemplates => Set<NotificationTemplate>();
    public DbSet<AssistantPromptTemplate> AssistantPromptTemplates => Set<AssistantPromptTemplate>();
    public DbSet<UserCampaign> UserCampaigns => Set<UserCampaign>();
    public DbSet<TrackingEvent> TrackingEvents => Set<TrackingEvent>();
    public DbSet<WalletCard> WalletCards => Set<WalletCard>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
