using System.Net;
using FluentAssertions;

namespace Bankampanya.Api.Tests;

public sealed class MobileProfileEndpointTests : IClassFixture<BankampanyaApiFactory>
{
    private readonly HttpClient _client;

    public MobileProfileEndpointTests(BankampanyaApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProfile_ShouldReturnSuccessStatusCode()
    {
        var response = await _client.GetAsync("/api/mobile/profile");
        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Contain("Kaan Çelik");
        content.Should().Contain("Kazanç Paneli");
    }
}
