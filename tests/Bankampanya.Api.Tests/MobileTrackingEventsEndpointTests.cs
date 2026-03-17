using System.Net;
using System.Text;
using FluentAssertions;

namespace Bankampanya.Api.Tests;

public sealed class MobileTrackingEventsEndpointTests : IClassFixture<BankampanyaApiFactory>
{
    private readonly HttpClient _client;

    public MobileTrackingEventsEndpointTests(BankampanyaApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateTrackingEvent_ShouldReturnOk()
    {
        const string json = """
        {
          "merchantName": "Opet",
          "amount": 750,
          "amountText": "750 TL",
          "note": "Akaryakıt harcaması"
        }
        """;

        var response = await _client.PostAsync(
            $"/api/mobile/tracking/{Guid.NewGuid()}/events",
            new StringContent(json, Encoding.UTF8, "application/json"));

        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Contain("InProgress");
        content.Should().Contain("true");
    }

    [Fact]
    public async Task CreateTrackingEvent_WithInvalidPayload_ShouldReturnBadRequest()
    {
        const string json = """
        {
          "merchantName": "",
          "amountText": ""
        }
        """;

        var response = await _client.PostAsync(
            $"/api/mobile/tracking/{Guid.NewGuid()}/events",
            new StringContent(json, Encoding.UTF8, "application/json"));

        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        content.Should().Contain("validation errors occurred");
    }

    [Fact]
    public async Task CreateTrackingEvent_WhenTrackingCampaignNotFound_ShouldReturnNotFound()
    {
        const string json = """
        {
          "merchantName": "Opet",
          "amountText": "750 TL"
        }
        """;

        var response = await _client.PostAsync(
            "/api/mobile/tracking/00000000-0000-0000-0000-000000000000/events",
            new StringContent(json, Encoding.UTF8, "application/json"));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
