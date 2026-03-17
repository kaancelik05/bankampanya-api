using Bankampanya.Domain.Entities;
using Bankampanya.Domain.Enums;
using Bankampanya.Infrastructure.Persistence;
using Bankampanya.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace Bankampanya.Infrastructure.Persistence;

public static class DemoDataSeeder
{
    private static readonly Guid DemoUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid DemoCampaignId = Guid.Parse("22222222-2222-2222-2222-222222222221");
    private static readonly Guid DemoTrackingTemplateId = Guid.Parse("22222222-2222-2222-2222-222222222331");

    public static async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken = default)
    {
        await SeedUsersAsync(dbContext, cancellationToken);
        await SeedContentAsync(dbContext, cancellationToken);
        await SeedWalletCardsAsync(dbContext, cancellationToken);
        await SeedUserCampaignsAsync(dbContext, cancellationToken);
    }

    private static async Task SeedUsersAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var hasDemoUser = await dbContext.AppUsers.AnyAsync(x => x.Id == DemoUserId, cancellationToken);
        if (hasDemoUser)
        {
            return;
        }

        var now = DateTime.UtcNow;
        var passwordHasher = new PasswordHasher();

        dbContext.AppUsers.Add(new AppUser
        {
            Id = DemoUserId,
            FullName = "Demo Kullanıcı",
            Email = "demo@bankampanya.com",
            Phone = "+90 555 000 00 00",
            PasswordHash = passwordHasher.Hash("123456"),
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
        });

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedContentAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var hasCampaigns = await dbContext.Campaigns.AnyAsync(cancellationToken);
        var hasTrackingTemplates = await dbContext.TrackingTemplates.AnyAsync(cancellationToken);

        if (hasCampaigns && hasTrackingTemplates)
        {
            return;
        }

        var now = DateTime.UtcNow;

        if (!hasCampaigns)
        {
            dbContext.Campaigns.Add(new Campaign
            {
                Id = DemoCampaignId,
                BankName = "Akbank",
                Category = "Akaryakıt",
                Title = "Akaryakıtta 400 TL chip-para",
                ShortDescription = "4 farklı akaryakıt işleminde toplam 400 TL chip-para kazan.",
                RewardText = "400 TL chip-para",
                RewardType = RewardType.Cashback,
                DeadlineText = "31 Mart'a kadar katıl",
                ValidFromUtc = now.AddDays(-10),
                ValidToUtc = now.AddDays(20),
                ValidDateRangeLabel = "10 Mart - 31 Mart",
                IsProgressive = true,
                ProgressTarget = 4,
                NextActionText = "Bu hafta bir akaryakıt işlemi daha yap.",
                Status = PublishStatus.Live,
                PublishedAtUtc = now.AddDays(-10),
                CreatedAtUtc = now.AddDays(-10),
                UpdatedAtUtc = now.AddDays(-2),
                Terms =
                {
                    new CampaignTerm
                    {
                        Id = Guid.NewGuid(),
                        SortOrder = 1,
                        Text = "Kampanyaya katıldıktan sonra yapılan harcamalar geçerlidir.",
                        CreatedAtUtc = now.AddDays(-10),
                        UpdatedAtUtc = now.AddDays(-10),
                    },
                    new CampaignTerm
                    {
                        Id = Guid.NewGuid(),
                        SortOrder = 2,
                        Text = "Her işlem en az 750 TL olmalıdır.",
                        CreatedAtUtc = now.AddDays(-10),
                        UpdatedAtUtc = now.AddDays(-10),
                    },
                },
            });
        }

        if (!hasTrackingTemplates)
        {
            dbContext.TrackingTemplates.Add(new TrackingTemplate
            {
                Id = DemoTrackingTemplateId,
                CampaignId = DemoCampaignId,
                BankName = "Akbank",
                Title = "Akaryakıt Takibi",
                Description = "Akaryakıt kampanyasındaki ilerlemeni adım adım takip et.",
                RequirementText = "4 farklı gün 750 TL ve üzeri akaryakıt harcaması yap.",
                NextActionText = "Kalan 2 işlem için harcamalarını tamamla.",
                RewardText = "400 TL chip-para",
                DefaultProgressTarget = 4,
                ProgressStatus = TrackingProgressStatus.Active,
                Status = PublishStatus.Live,
                PublishedAtUtc = now.AddDays(-10),
                CreatedAtUtc = now.AddDays(-10),
                UpdatedAtUtc = now.AddDays(-2),
            });
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedWalletCardsAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var hasWalletCards = await dbContext.WalletCards.AnyAsync(x => x.UserId == DemoUserId, cancellationToken);
        if (hasWalletCards)
        {
            return;
        }

        var now = DateTime.UtcNow;
        dbContext.WalletCards.AddRange(
            new WalletCard
            {
                Id = Guid.NewGuid(),
                UserId = DemoUserId,
                BankName = "Akbank",
                CardType = "Kredi Kartı",
                CustomName = "Axess Platinum",
                Status = WalletCardStatus.Active,
                CreatedAtUtc = now,
                UpdatedAtUtc = now,
            },
            new WalletCard
            {
                Id = Guid.NewGuid(),
                UserId = DemoUserId,
                BankName = "Yapı Kredi",
                CardType = "Banka Kartı",
                CustomName = "World Everyday",
                Status = WalletCardStatus.Passive,
                CreatedAtUtc = now,
                UpdatedAtUtc = now,
            });

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task SeedUserCampaignsAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var hasUserCampaigns = await dbContext.UserCampaigns.AnyAsync(x => x.UserId == DemoUserId, cancellationToken);
        if (hasUserCampaigns)
        {
            return;
        }

        var campaign = await dbContext.Campaigns
            .AsNoTracking()
            .Where(x => x.Id == DemoCampaignId)
            .Select(x => new
            {
                x.Id,
                x.ProgressTarget,
            })
            .FirstOrDefaultAsync(cancellationToken);

        var trackingTemplate = await dbContext.TrackingTemplates
            .AsNoTracking()
            .Where(x => x.Id == DemoTrackingTemplateId)
            .Select(x => new
            {
                x.Id,
                x.CampaignId,
                x.DefaultProgressTarget,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (campaign is null || trackingTemplate is null)
        {
            return;
        }

        var now = DateTime.UtcNow;
        var progressTarget = trackingTemplate.DefaultProgressTarget ?? campaign.ProgressTarget ?? 4;
        var userCampaign = new UserCampaign
        {
            Id = Guid.NewGuid(),
            UserId = DemoUserId,
            CampaignId = campaign.Id,
            TrackingTemplateId = trackingTemplate.Id,
            Status = UserCampaignStatus.InProgress,
            ProgressCurrent = 2,
            ProgressTarget = progressTarget,
            JoinedAtUtc = now.AddDays(-5),
            CreatedAtUtc = now.AddDays(-5),
            UpdatedAtUtc = now.AddDays(-1),
        };

        dbContext.UserCampaigns.Add(userCampaign);
        dbContext.TrackingEvents.AddRange(
            new TrackingEvent
            {
                Id = Guid.NewGuid(),
                UserCampaignId = userCampaign.Id,
                OccurredAtUtc = now.AddDays(-3),
                MerchantName = "Opet",
                Amount = 800,
                AmountText = "800 TL",
                Qualified = true,
                Note = "Akaryakıt harcaması",
                CreatedAtUtc = now.AddDays(-3),
                UpdatedAtUtc = now.AddDays(-3),
            },
            new TrackingEvent
            {
                Id = Guid.NewGuid(),
                UserCampaignId = userCampaign.Id,
                OccurredAtUtc = now.AddDays(-1),
                MerchantName = "Shell",
                Amount = 900,
                AmountText = "900 TL",
                Qualified = true,
                Note = "İkinci uygun işlem",
                CreatedAtUtc = now.AddDays(-1),
                UpdatedAtUtc = now.AddDays(-1),
            });

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
