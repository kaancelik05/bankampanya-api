using Bankampanya.Application.Common.Interfaces;
using Bankampanya.Application.Features.MobileProfile;
using Bankampanya.Application.Features.MobileProfile.Dtos;
using Bankampanya.Domain.Enums;
using Bankampanya.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bankampanya.Infrastructure.Persistence.Repositories;

public sealed class MobileProfileQueryRepository(
    AppDbContext dbContext,
    ICurrentUserService currentUserService) : IMobileProfileQueryRepository
{
    public async Task<MobileProfileDto> GetAsync(CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserId();

        var totalCards = await dbContext.WalletCards
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .CountAsync(cancellationToken);

        var activeTrackingCount = await dbContext.UserCampaigns
            .AsNoTracking()
            .Where(x => x.UserId == userId &&
                (x.Status == UserCampaignStatus.Joined ||
                 x.Status == UserCampaignStatus.InProgress ||
                 x.Status == UserCampaignStatus.RewardPending))
            .CountAsync(cancellationToken);

        var rewardTexts = await dbContext.UserCampaigns
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.ProgressCurrent < x.ProgressTarget)
            .Join(
                dbContext.Campaigns.AsNoTracking(),
                userCampaign => userCampaign.CampaignId,
                campaign => campaign.Id,
                (_, campaign) => campaign.RewardText)
            .ToListAsync(cancellationToken);

        var monthlyPotentialAmount = rewardTexts.Sum(GetRewardAmount);
        var joinedCount = Math.Max(totalCards, activeTrackingCount);

        return new MobileProfileDto
        {
            Summary = new MobileProfileSummaryDto
            {
                FullName = "Kaan Çelik",
                Email = "kaan@example.com",
                Phone = "+90 555 000 00 00",
                JoinedLabel = BuildJoinedLabel(joinedCount),
                TotalCards = totalCards,
                ActiveTrackingCount = activeTrackingCount,
                MonthlyPotentialText = FormatCurrency(monthlyPotentialAmount),
            },
            MenuGroups =
            [
                new MobileProfileMenuGroupDto
                {
                    Id = "financial",
                    Title = "Finans ve Fırsatlar",
                    Items =
                    [
                        new MobileProfileMenuItemDto
                        {
                            Id = "profile-earnings",
                            Title = "Kazanç Paneli",
                            Description = "Toplam kazanımını, bekleyen ödüllerini ve fırsat potansiyelini görüntüle.",
                            Route = "/earnings",
                        },
                        new MobileProfileMenuItemDto
                        {
                            Id = "profile-wallet",
                            Title = "Kartlarını Yönet",
                            Description = "Kayıtlı bankalarını ve kart türlerini güncelle.",
                            Route = "/wallet",
                        },
                        new MobileProfileMenuItemDto
                        {
                            Id = "profile-notifications",
                            Title = "Bildirimler",
                            Description = "Fırsat ve takip bildirimlerini gözden geçir.",
                            Route = "/notifications",
                        },
                    ],
                },
                new MobileProfileMenuGroupDto
                {
                    Id = "account",
                    Title = "Hesap ve Güvenlik",
                    Items =
                    [
                        new MobileProfileMenuItemDto
                        {
                            Id = "profile-security",
                            Title = "Güvenlik",
                            Description = "Şifre ve oturum ayarlarını yönet.",
                        },
                    ],
                },
                new MobileProfileMenuGroupDto
                {
                    Id = "support",
                    Title = "Destek",
                    Items =
                    [
                        new MobileProfileMenuItemDto
                        {
                            Id = "profile-support",
                            Title = "Yardım ve Destek",
                            Description = "Sık sorulan sorular ve destek kanalları.",
                        },
                    ],
                },
            ],
        };
    }

    private static decimal GetRewardAmount(string rewardText)
    {
        if (string.IsNullOrWhiteSpace(rewardText))
        {
            return 0m;
        }

        var digits = new string(rewardText.Where(char.IsDigit).ToArray());
        return decimal.TryParse(digits, out var parsed) ? parsed : 0m;
    }

    private static string BuildJoinedLabel(int activeCount)
        => activeCount > 0
            ? $"{activeCount} aktif kayıtla Bankampanya kullanıyor"
            : "Yeni fırsatlar için hesabını tamamla";

    private static string FormatCurrency(decimal amount)
        => $"{amount:N0} TL";
}
