using System.Net;
using FluentAssertions;

namespace Bankampanya.Api.Tests;

public sealed class MobileWalletEndpointTests : IClassFixture<BankampanyaApiFactory>
{
    private readonly HttpClient _client;

    public MobileWalletEndpointTests(BankampanyaApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetWallet_ShouldReturnSuccessStatusCode()
    {
        var response = await _client.GetAsync("/api/mobile/wallet");
        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Contain("Axess Platinum");
        content.Should().Contain("World Everyday");
    }
}
