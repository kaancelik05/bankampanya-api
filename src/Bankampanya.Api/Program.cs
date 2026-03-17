using Bankampanya.Api.Configuration;
using Bankampanya.Api.Middleware;
using Bankampanya.Api.Services;
using Bankampanya.Application.Common.Interfaces;
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
using Bankampanya.Infrastructure;
using Bankampanya.Infrastructure.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.AddScoped<ICurrentUserService, HttpCurrentUserService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<IAuthTokenService, JwtAuthTokenService>();
builder.Services.AddScoped<AdminDashboardService>();
builder.Services.AddScoped<AssistantPromptAdminService>();
builder.Services.AddScoped<AuthCurrentUserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CampaignAdminService>();
builder.Services.AddScoped<CreditAdminService>();
builder.Services.AddScoped<MobileAssistantPromptQueryService>();
builder.Services.AddScoped<MobileCampaignJoinService>();
builder.Services.AddScoped<MobileCampaignQueryService>();
builder.Services.AddScoped<MobileCreditQueryService>();
builder.Services.AddScoped<MobileEarningsQueryService>();
builder.Services.AddScoped<MobileNotificationQueryService>();
builder.Services.AddScoped<MobileProfileQueryService>();
builder.Services.AddScoped<MobileTrackingEventMutationService>();
builder.Services.AddScoped<MobileTrackingQueryService>();
builder.Services.AddScoped<MobileWalletMutationService>();
builder.Services.AddScoped<MobileWalletQueryService>();
builder.Services.AddScoped<NotificationAdminService>();
builder.Services.AddScoped<TrackingAdminService>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CampaignAdminService>();

builder.Services
  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
    var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      ValidIssuer = jwtOptions.Issuer,
      ValidAudience = jwtOptions.Audience,
      IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
      ClockSkew = TimeSpan.FromMinutes(1)
    };
  });

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevClient", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();
var reseedDemoDataOnStartup = app.Configuration.GetValue<bool>("DemoData:ReseedOnStartup");

if (!app.Environment.IsEnvironment("Testing") && reseedDemoDataOnStartup)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await ResetAndSeedDemoDataAsync(dbContext);
}

app.UseMiddleware<ApiExceptionMiddleware>();
app.UseCors("DevClient");
app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");
app.MapGet("/", () => Results.Ok(new
{
  service = "bankampanya-api",
  framework = ".NET 8",
  status = "ok"
}));

app.Run();

static async Task ResetAndSeedDemoDataAsync(AppDbContext dbContext)
{
    var demoUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    await dbContext.Database.ExecuteSqlRawAsync(
        "DELETE FROM tracking_events WHERE \"UserCampaignId\" IN (SELECT \"Id\" FROM user_campaigns WHERE \"UserId\" = {0})",
        demoUserId);
    await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM user_campaigns WHERE \"UserId\" = {0}", demoUserId);
    await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM wallet_cards WHERE \"UserId\" = {0}", demoUserId);
    await DemoDataSeeder.SeedAsync(dbContext);
}
