using System.Net;
using System.Net.Http.Json;
using Bankampanya.Application.Features.Campaigns.Dtos;
using Bankampanya.Domain.Enums;
using FluentAssertions;

namespace Bankampanya.Api.Tests;

public sealed class AdminCampaignsEndpointTests : IClassFixture<BankampanyaApiFactory>
{
    private readonly HttpClient _client;

    public AdminCampaignsEndpointTests(BankampanyaApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostCampaign_WithInvalidPayload_ShouldReturnBadRequest()
    {
        var payload = new UpsertCampaignRequest
        {
            BankName = "",
            Category = "",
            Title = "Kısa",
            ShortDescription = "Kısa açıklama",
            RewardText = "",
            RewardType = RewardType.Cashback,
            DeadlineText = "",
            ValidDateRangeLabel = "",
            Status = PublishStatus.Live,
            IsProgressive = true,
            ProgressTarget = null,
            NextActionText = null,
            Terms = Array.Empty<string>(),
        };

        var response = await _client.PostAsJsonAsync("/api/admin/campaigns", payload);
        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        content.Should().Contain("validation errors occurred");
        content.Should().Contain("Progressive kampanyalarda NextActionText zorunludur");
    }
}
