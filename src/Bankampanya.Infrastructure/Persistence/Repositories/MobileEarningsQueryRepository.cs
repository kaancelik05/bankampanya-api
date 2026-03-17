using Bankampanya.Application.Common.Interfaces;
using Bankampanya.Application.Features.MobileEarnings;
using Bankampanya.Application.Features.MobileEarnings.Dtos;
using Bankampanya.Domain.Entities;
using Bankampanya.Domain.Enums;
using Bankampanya.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bankampanya.Infrastructure.Persistence.Repositories;

public sealed class MobileEarningsQueryRepository(
    AppDbContext dbContext,
    ICurrentUserService currentUserService) : IMobileEarningsQueryRepository
{
    public async Task<MobileEarningsDashboardDto> GetDashboardAsync(CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserId();

        var userCampaigns = await dbContext.UserCampaigns
            .AsNoTracking()
            .Include(x => x.Campaign)
            .Include(x => x.TrackingEvents.OrderByDescending(e => e.OccurredAtUtc))
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.UpdatedAtUtc)
            .ToListAsync(cancellationToken);

        var earnedItems = userCampaigns.Where(x => x.Status is UserCampaignStatus.Completed or UserCampaignStatus.Rewarded).ToList();
        var pendingItems = userCampaigns.Where(x => x.Status is UserCampaignStatus.InProgress or UserCampaignStatus.RewardPending).ToList();
        var potentialItems = userCampaigns.Where(x => x.ProgressCurrent < x.ProgressTarget).ToList();

        var earnedReward = earnedItems.Sum(GetRewardAmount);
        var pendingReward = pendingItems.Sum(GetRewardAmount);
        var totalPotential = potentialItems.Sum(GetRewardAmount);

        return new MobileEarningsDashboardDto
        {
            Summary = new MobileEarningsSummaryDto
            {
                MonthLabel = BuildMonthLabel(),
                TotalEarnedText = FormatCurrency(earnedReward),
                PendingRewardText = FormatCurrency(pendingReward),
                PotentialRewardText = FormatCurrency(totalPotential),
            },
            Stats =
            [
                new MobileEarningsStatDto
                {
                    Id = "earned",
                    Label = "Toplam Kazanç",
                    ValueText = FormatCurrency(earnedReward),
                    Tone = "success",
                },
                new MobileEarningsStatDto
                {
                    Id = "pending",
                    Label = "Bekleyen Ödül",
                    ValueText = FormatCurrency(pendingReward),
                    Tone = "warning",
                },
                new MobileEarningsStatDto
                {
                    Id = "active-campaigns",
                    Label = "Aktif Fırsat",
                    ValueText = pendingItems.Count.ToString(),
                    Tone = "info",
                },
            ],
            History = earnedItems.Take(3).Select(MapHistoryItem).ToArray(),
            Potential = potentialItems.Take(3).Select(MapPotentialItem).ToArray(),
            Alerts = BuildAlerts(pendingItems.Count),
        };
    }

    private static IReadOnlyCollection<MobileEarningsAlertDto> BuildAlerts(int pendingCount)
    {
        if (pendingCount == 0)
        {
            return
            [
                new MobileEarningsAlertDto
                {
                    Id = "alert-clean",
                    Title = "Tüm aktif takiplerin kontrol altında",
                    Description = "Yeni fırsatlara katılarak bu ayki potansiyelini artırabilirsin.",
                    Tone = "info",
                }
            ];
        }

        return
        [
            new MobileEarningsAlertDto
            {
                Id = "alert-pending",
                Title = "Bekleyen ödüllerini takip et",
                Description = $"{pendingCount} aktif kampanyada ilerlemen sürüyor. Tamamlanmaya yakın fırsatları bitirerek kazancını artırabilirsin.",
                Tone = "warning",
            }
        ];
    }

    private static MobileEarningsHistoryItemDto MapHistoryItem(UserCampaign userCampaign)
        => new()
        {
            Id = $"history-{userCampaign.Id}",
            Title = userCampaign.Campaign?.Title ?? "Kampanya",
            BankName = userCampaign.Campaign?.BankName ?? string.Empty,
            RewardText = userCampaign.Campaign?.RewardText ?? FormatCurrency(GetRewardAmount(userCampaign)),
            Status = userCampaign.Status.ToString().ToLowerInvariant(),
            DateLabel = userCampaign.CompletedAtUtc.HasValue
                ? BuildRelativeDayLabel(userCampaign.CompletedAtUtc.Value)
                : BuildRelativeDayLabel(userCampaign.UpdatedAtUtc),
        };

    private static MobilePotentialEarningItemDto MapPotentialItem(UserCampaign userCampaign)
        => new()
        {
            Id = $"potential-{userCampaign.Id}",
            Title = userCampaign.Campaign?.Title ?? "Kampanya",
            BankName = userCampaign.Campaign?.BankName ?? string.Empty,
            PotentialText = userCampaign.Campaign?.RewardText ?? FormatCurrency(GetRewardAmount(userCampaign)),
            RemainingActionText = $"{Math.Max(userCampaign.ProgressTarget - userCampaign.ProgressCurrent, 0)} adım kaldı",
        };

    private static decimal GetRewardAmount(UserCampaign userCampaign)
    {
        var rewardText = userCampaign.Campaign?.RewardText;
        if (string.IsNullOrWhiteSpace(rewardText))
        {
            return 0m;
        }

        var digits = new string(rewardText.Where(char.IsDigit).ToArray());
        return decimal.TryParse(digits, out var parsed) ? parsed : 0m;
    }

    private static string BuildMonthLabel()
    {
        var now = DateTime.UtcNow;
        var months = new[]
        {
            "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran",
            "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"
        };

        return $"{months[now.Month - 1]} {now.Year}";
    }

    private static string BuildRelativeDayLabel(DateTime utcDate)
    {
        var dayDiff = Math.Max((DateTime.UtcNow.Date - utcDate.Date).Days, 0);
        return dayDiff switch
        {
            0 => "Bugün",
            1 => "1 gün önce",
            _ => $"{dayDiff} gün önce",
        };
    }

    private static string FormatCurrency(decimal amount)
        => $"{amount:N0} TL";
}
