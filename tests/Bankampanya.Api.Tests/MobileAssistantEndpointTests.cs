using System.Net;
using FluentAssertions;

namespace Bankampanya.Api.Tests;

public sealed class MobileAssistantEndpointTests : IClassFixture<BankampanyaApiFactory>
{
    private readonly HttpClient _client;

    public MobileAssistantEndpointTests(BankampanyaApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAssistantPrompts_ShouldReturnSuccessStatusCode()
    {
        var response = await _client.GetAsync("/api/mobile/assistant/prompts");
        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Contain("market kampanyaları");
    }
}
