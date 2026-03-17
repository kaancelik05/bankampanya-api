using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bankampanya.Api.Controllers.Admin;
using Bankampanya.Application.Features.Assistant;
using Bankampanya.Application.Features.Assistant.Dtos;
using Bankampanya.Application.Features.Auth;
using Bankampanya.Application.Features.Auth.Dtos;
using Bankampanya.Application.Features.Campaigns;
using Bankampanya.Application.Features.Campaigns.Dtos;
using Bankampanya.Application.Features.Credits;
using Bankampanya.Application.Features.Credits.Dtos;
using Bankampanya.Application.Features.Dashboard;
using Bankampanya.Application.Features.Dashboard.Dtos;
using Bankampanya.Application.Features.MobileAssistant;
using Bankampanya.Application.Features.MobileAssistant.Dtos;
using Bankampanya.Application.Features.MobileCampaignJoin;
using Bankampanya.Application.Features.MobileCampaignJoin.Dtos;
using Bankampanya.Application.Features.MobileCampaigns;
using Bankampanya.Application.Features.MobileCampaigns.Dtos;
using Bankampanya.Application.Features.MobileCredits;
using Bankampanya.Application.Features.MobileCredits.Dtos;
using Bankampanya.Application.Features.MobileEarnings;
using Bankampanya.Application.Features.MobileEarnings.Dtos;
using Bankampanya.Application.Features.MobileNotifications;
using Bankampanya.Application.Features.MobileNotifications.Dtos;
using Bankampanya.Application.Features.MobileProfile;
using Bankampanya.Application.Features.MobileProfile.Dtos;
using Bankampanya.Application.Features.MobileTracking;
using Bankampanya.Application.Features.MobileTracking.Dtos;
using Bankampanya.Application.Features.MobileTrackingEvents;
using Bankampanya.Application.Features.MobileTrackingEvents.Dtos;
using Bankampanya.Application.Features.MobileWallet;
using Bankampanya.Application.Features.MobileWallet.Dtos;
using Bankampanya.Application.Features.Notifications;
using Bankampanya.Application.Features.Notifications.Dtos;
using Bankampanya.Application.Features.Tracking;
using Bankampanya.Application.Features.Tracking.Dtos;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Bankampanya.Api.Tests;

public sealed class BankampanyaApiFactory : WebApplicationFactory<AdminCampaignsController>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("environment", "Testing");
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Default"] = "Host=localhost;Port=5432;Database=bankampanya_test;Username=postgres;Password=postgres",
                ["Jwt:Issuer"] = "bankampanya-tests",
                ["Jwt:Audience"] = "bankampanya-tests-clients",
                ["Jwt:SigningKey"] = "bankampanya-tests-signing-key-1234567890",
                ["Jwt:AccessTokenLifetimeMinutes"] = "480",
            });
        });

        builder.ConfigureServices(services =>
        {
            services.AddScoped<ICampaignAdminRepository, FakeCampaignAdminRepository>();
            services.AddScoped<ICreditAdminRepository, FakeCreditAdminRepository>();
            services.AddScoped<INotificationAdminRepository, FakeNotificationAdminRepository>();
            services.AddScoped<ITrackingAdminRepository, FakeTrackingAdminRepository>();
            services.AddScoped<IAssistantPromptAdminRepository, FakeAssistantPromptAdminRepository>();
            services.AddScoped<IAdminDashboardRepository, FakeAdminDashboardRepository>();
            services.AddScoped<IMobileAssistantPromptQueryRepository, FakeMobileAssistantPromptQueryRepository>();
            services.AddScoped<IMobileCampaignJoinRepository, FakeMobileCampaignJoinRepository>();
            services.AddScoped<IMobileCampaignQueryRepository, FakeMobileCampaignQueryRepository>();
            services.AddScoped<IMobileCreditQueryRepository, FakeMobileCreditQueryRepository>();
            services.AddScoped<IMobileEarningsQueryRepository, FakeMobileEarningsQueryRepository>();
            services.AddScoped<IMobileNotificationQueryRepository, FakeMobileNotificationQueryRepository>();
            services.AddScoped<IMobileProfileQueryRepository, FakeMobileProfileQueryRepository>();
            services.AddScoped<IMobileTrackingEventMutationRepository, FakeMobileTrackingEventMutationRepository>();
            services.AddScoped<IMobileTrackingQueryRepository, FakeMobileTrackingQueryRepository>();
            services.AddScoped<IMobileWalletMutationRepository, FakeMobileWalletMutationRepository>();
            services.AddScoped<IMobileWalletQueryRepository, FakeMobileWalletQueryRepository>();
            services.AddScoped<IAuthUserRepository, FakeAuthUserRepository>();
            services.AddScoped<IPasswordHasher, FakePasswordHasher>();
            services.AddScoped<IAuthTokenService, FakeAuthTokenService>();
            services.AddScoped<IRefreshTokenGrantRepository, FakeRefreshTokenGrantRepository>();
        });
    }

    private sealed class FakeCampaignAdminRepository : ICampaignAdminRepository
    {
        public Task<IReadOnlyList<CampaignAdminListItemDto>> GetListAsync(CampaignListQuery query, CancellationToken cancellationToken)
            => Task.FromResult<IReadOnlyList<CampaignAdminListItemDto>>([]);

        public Task<CampaignAdminListItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => Task.FromResult<CampaignAdminListItemDto?>(null);

        public Task<CampaignAdminListItemDto> CreateAsync(UpsertCampaignRequest request, CancellationToken cancellationToken)
            => Task.FromResult(new CampaignAdminListItemDto { Id = Guid.NewGuid() });

        public Task<CampaignAdminListItemDto?> UpdateAsync(Guid id, UpsertCampaignRequest request, CancellationToken cancellationToken)
            => Task.FromResult<CampaignAdminListItemDto?>(new CampaignAdminListItemDto { Id = id });
    }

    private sealed class FakeCreditAdminRepository : ICreditAdminRepository
    {
        public Task<IReadOnlyList<CreditAdminListItemDto>> GetListAsync(CreditListQuery query, CancellationToken cancellationToken)
            => Task.FromResult<IReadOnlyList<CreditAdminListItemDto>>([]);

        public Task<CreditAdminListItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => Task.FromResult<CreditAdminListItemDto?>(null);

        public Task<CreditAdminListItemDto> CreateAsync(UpsertCreditRequest request, CancellationToken cancellationToken)
            => Task.FromResult(new CreditAdminListItemDto { Id = Guid.NewGuid() });

        public Task<CreditAdminListItemDto?> UpdateAsync(Guid id, UpsertCreditRequest request, CancellationToken cancellationToken)
            => Task.FromResult<CreditAdminListItemDto?>(new CreditAdminListItemDto { Id = id });
    }

    private sealed class FakeNotificationAdminRepository : INotificationAdminRepository
    {
        public Task<IReadOnlyList<NotificationAdminListItemDto>> GetListAsync(NotificationListQuery query, CancellationToken cancellationToken)
            => Task.FromResult<IReadOnlyList<NotificationAdminListItemDto>>([]);

        public Task<NotificationAdminListItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => Task.FromResult<NotificationAdminListItemDto?>(null);

        public Task<NotificationAdminListItemDto> CreateAsync(UpsertNotificationRequest request, CancellationToken cancellationToken)
            => Task.FromResult(new NotificationAdminListItemDto { Id = Guid.NewGuid() });

        public Task<NotificationAdminListItemDto?> UpdateAsync(Guid id, UpsertNotificationRequest request, CancellationToken cancellationToken)
            => Task.FromResult<NotificationAdminListItemDto?>(new NotificationAdminListItemDto { Id = id });
    }

    private sealed class FakeTrackingAdminRepository : ITrackingAdminRepository
    {
        public Task<IReadOnlyList<TrackingAdminListItemDto>> GetListAsync(TrackingListQuery query, CancellationToken cancellationToken)
            => Task.FromResult<IReadOnlyList<TrackingAdminListItemDto>>([]);

        public Task<TrackingAdminListItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => Task.FromResult<TrackingAdminListItemDto?>(null);

        public Task<TrackingAdminListItemDto> CreateAsync(UpsertTrackingRequest request, CancellationToken cancellationToken)
            => Task.FromResult(new TrackingAdminListItemDto { Id = Guid.NewGuid() });

        public Task<TrackingAdminListItemDto?> UpdateAsync(Guid id, UpsertTrackingRequest request, CancellationToken cancellationToken)
            => Task.FromResult<TrackingAdminListItemDto?>(new TrackingAdminListItemDto { Id = id });
    }

    private sealed class FakeAssistantPromptAdminRepository : IAssistantPromptAdminRepository
    {
        public Task<IReadOnlyList<AssistantPromptAdminListItemDto>> GetListAsync(AssistantPromptListQuery query, CancellationToken cancellationToken)
            => Task.FromResult<IReadOnlyList<AssistantPromptAdminListItemDto>>([]);

        public Task<AssistantPromptAdminListItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => Task.FromResult<AssistantPromptAdminListItemDto?>(null);

        public Task<AssistantPromptAdminListItemDto> CreateAsync(UpsertAssistantPromptRequest request, CancellationToken cancellationToken)
            => Task.FromResult(new AssistantPromptAdminListItemDto { Id = Guid.NewGuid() });

        public Task<AssistantPromptAdminListItemDto?> UpdateAsync(Guid id, UpsertAssistantPromptRequest request, CancellationToken cancellationToken)
            => Task.FromResult<AssistantPromptAdminListItemDto?>(new AssistantPromptAdminListItemDto { Id = id });
    }

    private sealed class FakeAdminDashboardRepository : IAdminDashboardRepository
    {
        public Task<AdminDashboardSummaryDto> GetSummaryAsync(CancellationToken cancellationToken)
            => Task.FromResult(new AdminDashboardSummaryDto());
    }

    private sealed class FakeMobileAssistantPromptQueryRepository : IMobileAssistantPromptQueryRepository
    {
        public Task<IReadOnlyList<MobileAssistantPromptSuggestionDto>> GetListAsync(MobileAssistantPromptListQuery query, CancellationToken cancellationToken)
            => Task.FromResult<IReadOnlyList<MobileAssistantPromptSuggestionDto>>
            ([
                new MobileAssistantPromptSuggestionDto
                {
                    Id = Guid.NewGuid(),
                    Text = "Bu ay en avantajlı market kampanyaları hangileri?",
                }
            ]);
    }

    private sealed class FakeMobileCampaignQueryRepository : IMobileCampaignQueryRepository
    {
        public Task<IReadOnlyList<MobileCampaignListItemDto>> GetListAsync(MobileCampaignListQuery query, CancellationToken cancellationToken)
            => Task.FromResult<IReadOnlyList<MobileCampaignListItemDto>>
            ([
                new MobileCampaignListItemDto
                {
                    Id = Guid.NewGuid(),
                    BankName = "Akbank",
                    Title = "Akaryakıt Harcamana 500 TL Nakit İade",
                    ShortDescription = "4 farklı günde 750 TL ve üzeri yakıt alışverişine özel.",
                    RewardText = "500 TL",
                    RewardType = Bankampanya.Domain.Enums.RewardType.Cashback,
                    Category = "Akaryakıt",
                    DeadlineText = "Son 6 gün",
                }
            ]);

        public Task<MobileCampaignDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => Task.FromResult<MobileCampaignDetailDto?>
            (new MobileCampaignDetailDto
            {
                Id = id,
                BankName = "Akbank",
                Title = "Akaryakıt Harcamana 500 TL Nakit İade",
                ShortDescription = "4 farklı günde 750 TL ve üzeri yakıt alışverişine özel.",
                RewardText = "500 TL",
                RewardType = Bankampanya.Domain.Enums.RewardType.Cashback,
                Category = "Akaryakıt",
                DeadlineText = "Son 6 gün",
                ValidDateRange = "1 Nisan - 27 Nisan",
                Terms = ["Her işlem en az 750 TL olmalı"],
            });
    }

    private sealed class FakeMobileCampaignJoinRepository : IMobileCampaignJoinRepository
    {
        public Task<JoinedCampaignDto> JoinAsync(Guid campaignId, CancellationToken cancellationToken)
        {
            if (campaignId == Guid.Empty)
            {
                throw new Bankampanya.Application.Common.Exceptions.EntityNotFoundException($"Campaign '{campaignId}' not found.");
            }

            return Task.FromResult(new JoinedCampaignDto
            {
                Id = Guid.NewGuid(),
                CampaignId = campaignId,
                TrackingTemplateId = Guid.NewGuid(),
                Status = "InProgress",
                ProgressCurrent = 0,
                ProgressTarget = 4,
                JoinedAtLabel = "Bugün katıldı",
            });
        }
    }

    private sealed class FakeMobileCreditQueryRepository : IMobileCreditQueryRepository
    {
        public Task<IReadOnlyList<MobileCreditListItemDto>> GetListAsync(MobileCreditListQuery query, CancellationToken cancellationToken)
            => Task.FromResult<IReadOnlyList<MobileCreditListItemDto>>
            ([
                new MobileCreditListItemDto
                {
                    Id = Guid.NewGuid(),
                    Title = "İhtiyaç Kredisi - Hızlı Başvuru",
                    BankName = "Akbank",
                    Rate = "%3.19",
                    AmountRange = "10.000 - 250.000 TL",
                    Type = Bankampanya.Domain.Enums.CreditOfferType.Kredi,
                    DetailSummary = "Mobil kredi ekranında öne çıkan ihtiyaç kredisi teklifi.",
                }
            ]);

        public Task<MobileCreditDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => Task.FromResult<MobileCreditDetailDto?>
            (new MobileCreditDetailDto
            {
                Id = id,
                Title = "İhtiyaç Kredisi - Hızlı Başvuru",
                BankName = "Akbank",
                Rate = "%3.19",
                AmountRange = "10.000 - 250.000 TL",
                Type = Bankampanya.Domain.Enums.CreditOfferType.Kredi,
                DetailSummary = "Mobil kredi ekranında öne çıkan ihtiyaç kredisi teklifi.",
                Terms = ["Gelir belgesi gerekebilir"],
            });
    }

    private sealed class FakeMobileNotificationQueryRepository : IMobileNotificationQueryRepository
    {
        public Task<IReadOnlyList<MobileNotificationItemDto>> GetListAsync(MobileNotificationListQuery query, CancellationToken cancellationToken)
            => Task.FromResult<IReadOnlyList<MobileNotificationItemDto>>
            ([
                new MobileNotificationItemDto
                {
                    Id = Guid.NewGuid(),
                    Type = Bankampanya.Domain.Enums.NotificationType.Campaign,
                    Title = "Kampanyada son günler",
                    Body = "Akaryakıt kampanyanı tamamlamak için son günler.",
                    TimeLabel = "Az önce",
                    CtaLabel = "Kampanyayı Gör",
                    Route = "/campaigns/cmp-1",
                    Tone = "warning",
                }
            ]);
    }

    private sealed class FakeMobileTrackingQueryRepository : IMobileTrackingQueryRepository
    {
        public Task<IReadOnlyList<MobileTrackedCampaignDto>> GetListAsync(MobileTrackingListQuery query, CancellationToken cancellationToken)
            => Task.FromResult<IReadOnlyList<MobileTrackedCampaignDto>>
            ([
                new MobileTrackedCampaignDto
                {
                    Id = Guid.NewGuid(),
                    Title = "Akaryakıt Harcamana 500 TL Nakit İade",
                    BankName = "Akbank",
                    ProgressCurrent = 2,
                    ProgressTarget = 4,
                    DeadlineText = "Son 6 gün",
                    RewardText = "500 TL",
                    ShortDescription = "Takipteki akaryakıt kampanyası",
                    NextActionText = "2 farklı gün daha alışveriş yap",
                    Status = Bankampanya.Domain.Enums.TrackingProgressStatus.NearComplete,
                    Requirements = ["Her işlem min. 750 TL"],
                    Events =
                    [
                        new MobileTrackingEventDto
                        {
                            Id = "event-1",
                            DateLabel = "Bugün",
                            AmountText = "750 TL",
                            MerchantName = "Opet",
                            Qualified = true,
                        }
                    ],
                }
            ]);

        public Task<MobileTrackedCampaignDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => Task.FromResult<MobileTrackedCampaignDto?>
            (new MobileTrackedCampaignDto
            {
                Id = id,
                Title = "Akaryakıt Harcamana 500 TL Nakit İade",
                BankName = "Akbank",
                ProgressCurrent = 2,
                ProgressTarget = 4,
                DeadlineText = "Son 6 gün",
                RewardText = "500 TL",
                ShortDescription = "Takipteki akaryakıt kampanyası",
                NextActionText = "2 farklı gün daha alışveriş yap",
                Status = Bankampanya.Domain.Enums.TrackingProgressStatus.NearComplete,
                Requirements = ["Her işlem min. 750 TL"],
                Events =
                [
                    new MobileTrackingEventDto
                    {
                        Id = "event-1",
                        DateLabel = "Bugün",
                        AmountText = "750 TL",
                        MerchantName = "Opet",
                        Qualified = true,
                    }
                ],
            });
    }

    private sealed class FakeMobileTrackingEventMutationRepository : IMobileTrackingEventMutationRepository
    {
        public Task<TrackingEventMutationResultDto> CreateAsync(Guid trackingTemplateId, CreateTrackingEventRequest request, CancellationToken cancellationToken)
        {
            if (trackingTemplateId == Guid.Empty)
            {
                throw new Bankampanya.Application.Common.Exceptions.EntityNotFoundException($"Tracking campaign '{trackingTemplateId}' not found for current user.");
            }

            return Task.FromResult(new TrackingEventMutationResultDto
            {
                EventId = Guid.NewGuid(),
                UserCampaignId = Guid.NewGuid(),
                ProgressCurrent = 3,
                ProgressTarget = 4,
                Qualified = true,
                Status = "InProgress",
            });
        }
    }

    private sealed class FakeMobileProfileQueryRepository : IMobileProfileQueryRepository
    {
        public Task<MobileProfileDto> GetAsync(CancellationToken cancellationToken)
            => Task.FromResult(new MobileProfileDto
            {
                Summary = new MobileProfileSummaryDto
                {
                    FullName = "Kaan Çelik",
                    Email = "kaan@example.com",
                    Phone = "+90 555 000 00 00",
                    JoinedLabel = "Nisan 2026’dan beri üye",
                    TotalCards = 5,
                    ActiveTrackingCount = 2,
                    MonthlyPotentialText = "1.450 TL",
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
                            }
                        ]
                    }
                ]
            });
    }

    private sealed class FakeMobileWalletQueryRepository : IMobileWalletQueryRepository
    {
        public Task<MobileWalletDto> GetAsync(CancellationToken cancellationToken)
            => Task.FromResult(new MobileWalletDto
            {
                Cards =
                [
                    new MobileWalletCardDto
                    {
                        Id = "w-1",
                        BankName = "Akbank",
                        CardType = "Kredi Kartı",
                        CustomName = "Axess Platinum",
                        IsActive = true,
                    },
                    new MobileWalletCardDto
                    {
                        Id = "w-2",
                        BankName = "Yapı Kredi",
                        CardType = "Banka Kartı",
                        CustomName = "World Everyday",
                        IsActive = false,
                    }
                ]
            });
    }

    private sealed class FakeMobileWalletMutationRepository : IMobileWalletMutationRepository
    {
        public Task<MobileWalletCardDto> CreateAsync(CreateWalletCardRequest request, CancellationToken cancellationToken)
            => Task.FromResult(new MobileWalletCardDto
            {
                Id = Guid.NewGuid().ToString(),
                BankName = request.BankName,
                CardType = request.CardType,
                CustomName = request.CustomName,
                IsActive = true,
            });

        public Task<MobileWalletCardDto?> UpdateStatusAsync(Guid id, UpdateWalletCardStatusRequest request, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                return Task.FromResult<MobileWalletCardDto?>(null);
            }

            return Task.FromResult<MobileWalletCardDto?>(new MobileWalletCardDto
            {
                Id = id.ToString(),
                BankName = "Akbank",
                CardType = "Kredi Kartı",
                CustomName = "Axess Platinum",
                IsActive = request.IsActive,
            });
        }

        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
            => Task.FromResult(id != Guid.Empty);
    }

    private sealed class FakeAuthUserRepository : IAuthUserRepository
    {
        private static readonly Dictionary<Guid, Bankampanya.Domain.Entities.AppUser> Users = new();

        public Task<Bankampanya.Domain.Entities.AppUser?> GetByEmailOrPhoneAsync(string identifier, CancellationToken cancellationToken)
        {
            var normalized = identifier.Trim();
            var user = Users.Values.SingleOrDefault(x =>
                x.Email.Equals(normalized, StringComparison.OrdinalIgnoreCase)
                || x.Phone.Equals(normalized, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult<Bankampanya.Domain.Entities.AppUser?>(user);
        }

        public Task<Bankampanya.Domain.Entities.AppUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            Users.TryGetValue(id, out var user);
            return Task.FromResult<Bankampanya.Domain.Entities.AppUser?>(user);
        }

        public Task<bool> ExistsByEmailOrPhoneAsync(string email, string phone, CancellationToken cancellationToken)
            => Task.FromResult(Users.Values.Any(x =>
                x.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase)
                || x.Phone.Equals(phone.Trim(), StringComparison.OrdinalIgnoreCase)));

        public Task<Bankampanya.Domain.Entities.AppUser> CreateAsync(Bankampanya.Domain.Entities.AppUser user, CancellationToken cancellationToken)
        {
            Users[user.Id] = user;
            return Task.FromResult(user);
        }
    }

    private sealed class FakePasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
            => $"hashed::{password}";

        public bool Verify(string password, string passwordHash)
            => passwordHash == Hash(password);
    }

    private sealed class FakeAuthTokenService : IAuthTokenService
    {
        private const string Issuer = "bankampanya-tests";
        private const string Audience = "bankampanya-tests-clients";
        private const string SigningKey = "bankampanya-tests-signing-key-1234567890";

        public AuthSessionDto CreateSession(Guid userId, string identifier)
            => BuildSession(userId, identifier);

        public AuthSessionDto RefreshSession(Guid userId, string identifier, string refreshToken)
            => BuildSession(userId, identifier);

        public Guid? GetUserIdFromRefreshToken(string refreshToken)
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(refreshToken))
            {
                return null;
            }

            var token = handler.ReadJwtToken(refreshToken);
            var userIdValue = token.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub || claim.Type == "user_id")?.Value;
            return Guid.TryParse(userIdValue, out var userId) ? userId : null;
        }

        public string HashRefreshToken(string refreshToken)
        {
            var bytes = System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));
            return Convert.ToHexString(bytes);
        }

        private AuthSessionDto BuildSession(Guid userId, string identifier)
        {
            var expiresAt = DateTime.UtcNow.AddHours(8);
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, identifier),
                new Claim("user_id", userId.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAt,
                signingCredentials: credentials);

            return new AuthSessionDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = CreateRefreshToken(userId, identifier),
                ExpiresAt = expiresAt.ToString("O"),
            };
        }

        private static string CreateRefreshToken(Guid userId, string identifier)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var refreshToken = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, identifier),
                    new Claim("user_id", userId.ToString()),
                    new Claim("typ", "refresh"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                },
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(14),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(refreshToken);
        }
    }

    private sealed class FakeRefreshTokenGrantRepository : IRefreshTokenGrantRepository
    {
        private static readonly Dictionary<Guid, List<(string TokenHash, DateTime ExpiresAtUtc, DateTime? RevokedAtUtc)>> Grants = new();

        public Task StoreAsync(Guid userId, string tokenHash, DateTime expiresAtUtc, CancellationToken cancellationToken)
        {
            if (!Grants.TryGetValue(userId, out var items))
            {
                items = [];
                Grants[userId] = items;
            }

            items.Add((tokenHash, expiresAtUtc, null));
            return Task.CompletedTask;
        }

        public Task<bool> IsValidAsync(Guid userId, string tokenHash, DateTime nowUtc, CancellationToken cancellationToken)
        {
            var isValid = Grants.TryGetValue(userId, out var items)
                          && items.Any(x => x.TokenHash == tokenHash && x.RevokedAtUtc is null && x.ExpiresAtUtc > nowUtc);
            return Task.FromResult(isValid);
        }

        public Task RevokeAsync(Guid userId, string tokenHash, DateTime revokedAtUtc, CancellationToken cancellationToken)
        {
            if (!Grants.TryGetValue(userId, out var items))
            {
                return Task.CompletedTask;
            }

            var index = items.FindIndex(x => x.TokenHash == tokenHash && x.RevokedAtUtc is null);
            if (index >= 0)
            {
                var item = items[index];
                items[index] = (item.TokenHash, item.ExpiresAtUtc, revokedAtUtc);
            }

            return Task.CompletedTask;
        }
    }

    private sealed class FakeMobileEarningsQueryRepository : IMobileEarningsQueryRepository
    {
        public Task<MobileEarningsDashboardDto> GetDashboardAsync(CancellationToken cancellationToken)
            => Task.FromResult(new MobileEarningsDashboardDto
            {
                Summary = new MobileEarningsSummaryDto
                {
                    MonthLabel = "Mart 2026",
                    TotalEarnedText = "960 TL",
                    PendingRewardText = "420 TL",
                    PotentialRewardText = "1.500 TL",
                }
            });
    }
}
