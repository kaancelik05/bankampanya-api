using System.Net;
using FluentAssertions;

namespace Bankampanya.Api.Tests;

public sealed class MobileCreditsEndpointTests : IClassFixture<BankampanyaApiFactory>
{
    private readonly HttpClient _client;

    public MobileCreditsEndpointTests(BankampanyaApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCredits_ShouldReturnSuccessStatusCode()
    {
        var response = await _client.GetAsync("/api/mobile/credits");
        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Contain("İhtiyaç Kredisi - Hızlı Başvuru");
    }
}
