using System.Net;
using FluentAssertions;

namespace Bankampanya.Api.Tests;

public sealed class MobileEarningsEndpointTests : IClassFixture<BankampanyaApiFactory>
{
    private readonly HttpClient _client;

    public MobileEarningsEndpointTests(BankampanyaApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetEarnings_ShouldReturnSuccessStatusCode()
    {
        var response = await _client.GetAsync("/api/mobile/earnings");
        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Contain("Mart 2026");
        content.Should().Contain("960 TL");
    }
}
