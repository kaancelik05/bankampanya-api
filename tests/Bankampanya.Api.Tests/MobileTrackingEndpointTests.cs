using System.Net;
using FluentAssertions;

namespace Bankampanya.Api.Tests;

public sealed class MobileTrackingEndpointTests : IClassFixture<BankampanyaApiFactory>
{
    private readonly HttpClient _client;

    public MobileTrackingEndpointTests(BankampanyaApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTracking_ShouldReturnSuccessStatusCode()
    {
        var response = await _client.GetAsync("/api/mobile/tracking");
        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Contain("Takipteki akaryakıt kampanyası");
    }
}
