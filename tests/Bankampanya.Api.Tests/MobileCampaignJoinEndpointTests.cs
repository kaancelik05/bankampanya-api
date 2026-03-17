using System.Net;
using System.Text;
using FluentAssertions;

namespace Bankampanya.Api.Tests;

public sealed class MobileCampaignJoinEndpointTests : IClassFixture<BankampanyaApiFactory>
{
    private readonly HttpClient _client;

    public MobileCampaignJoinEndpointTests(BankampanyaApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task JoinCampaign_ShouldReturnOk()
    {
        const string json = "{}";
        var campaignId = Guid.NewGuid();

        var response = await _client.PostAsync(
            $"/api/mobile/campaigns/{campaignId}/join",
            new StringContent(json, Encoding.UTF8, "application/json"));

        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Contain("InProgress");
        content.Should().Contain("Bugün katıldı");
    }

    [Fact]
    public async Task JoinCampaign_WhenCampaignNotFound_ShouldReturnNotFound()
    {
        const string json = "{}";

        var response = await _client.PostAsync(
            "/api/mobile/campaigns/00000000-0000-0000-0000-000000000000/join",
            new StringContent(json, Encoding.UTF8, "application/json"));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
