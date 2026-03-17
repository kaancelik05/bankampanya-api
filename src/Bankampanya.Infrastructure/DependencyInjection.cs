using Bankampanya.Application.Features.Assistant;
using Bankampanya.Application.Features.Auth;
using Bankampanya.Application.Features.Campaigns;
using Bankampanya.Application.Features.Credits;
using Bankampanya.Application.Features.Dashboard;
using Bankampanya.Application.Features.MobileAssistant;
using Bankampanya.Application.Features.MobileCampaignJoin;
using Bankampanya.Application.Features.MobileCampaigns;
using Bankampanya.Application.Features.MobileCredits;
using Bankampanya.Application.Features.MobileEarnings;
using Bankampanya.Application.Features.MobileNotifications;
using Bankampanya.Application.Features.MobileProfile;
using Bankampanya.Application.Features.MobileTracking;
using Bankampanya.Application.Features.MobileTrackingEvents;
using Bankampanya.Application.Features.MobileWallet;
using Bankampanya.Application.Features.Notifications;
using Bankampanya.Application.Features.Tracking;
using Bankampanya.Infrastructure.Options;
using Bankampanya.Infrastructure.Persistence;
using Bankampanya.Infrastructure.Persistence.Repositories;
using Bankampanya.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Bankampanya.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");

        services.Configure<SupabaseOptions>(configuration.GetSection(SupabaseOptions.SectionName));

        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
            dataSourceBuilder.EnableDynamicJson();
            var dataSource = dataSourceBuilder.Build();
            services.AddSingleton(dataSource);
        }

        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            var dataSource = serviceProvider.GetService<NpgsqlDataSource>();

            if (dataSource is not null)
            {
                options.UseNpgsql(dataSource, npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                    npgsqlOptions.CommandTimeout(180);
                });
            }
        });

        services.AddScoped<PasswordHasher>();
        services.AddScoped<IAuthUserRepository, AuthUserRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IAuthTokenService, AuthTokenService>();
        services.AddScoped<IRefreshTokenGrantRepository, RefreshTokenGrantRepository>();
        services.AddScoped<IAdminDashboardRepository, AdminDashboardRepository>();
        services.AddScoped<IAssistantPromptAdminRepository, AssistantPromptAdminRepository>();
        services.AddScoped<ICampaignAdminRepository, CampaignAdminRepository>();
        services.AddScoped<ICreditAdminRepository, CreditAdminRepository>();
        services.AddScoped<IMobileAssistantPromptQueryRepository, MobileAssistantPromptQueryRepository>();
        services.AddScoped<IMobileCampaignJoinRepository, MobileCampaignJoinRepository>();
        services.AddScoped<IMobileCampaignQueryRepository, MobileCampaignQueryRepository>();
        services.AddScoped<IMobileCreditQueryRepository, MobileCreditQueryRepository>();
        services.AddScoped<IMobileEarningsQueryRepository, MobileEarningsQueryRepository>();
        services.AddScoped<IMobileNotificationQueryRepository, MobileNotificationQueryRepository>();
        services.AddScoped<IMobileProfileQueryRepository, MobileProfileQueryRepository>();
        services.AddScoped<IMobileTrackingEventMutationRepository, MobileTrackingEventMutationRepository>();
        services.AddScoped<IMobileTrackingQueryRepository, MobileTrackingQueryRepository>();
        services.AddScoped<IMobileWalletMutationRepository, MobileWalletMutationRepository>();
        services.AddScoped<IMobileWalletQueryRepository, MobileWalletQueryRepository>();
        services.AddScoped<INotificationAdminRepository, NotificationAdminRepository>();
        services.AddScoped<ITrackingAdminRepository, TrackingAdminRepository>();

        return services;
    }
}
