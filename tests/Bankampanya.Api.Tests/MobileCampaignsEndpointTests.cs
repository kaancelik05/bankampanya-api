using System.Net;
using FluentAssertions;

namespace Bankampanya.Api.Tests;

public sealed class MobileCampaignsEndpointTests : IClassFixture<BankampanyaApiFactory>
{
    private readonly HttpClient _client;

    public MobileCampaignsEndpointTests(BankampanyaApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCampaigns_ShouldReturnSuccessStatusCode()
    {
        var response = await _client.GetAsync("/api/mobile/campaigns");
        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Contain("Akaryakıt Harcamana 500 TL Nakit İade");
    }
}
