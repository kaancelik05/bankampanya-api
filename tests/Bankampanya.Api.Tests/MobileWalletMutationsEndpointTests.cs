using System.Net;
using System.Text;
using FluentAssertions;

namespace Bankampanya.Api.Tests;

public sealed class MobileWalletMutationsEndpointTests : IClassFixture<BankampanyaApiFactory>
{
    private readonly HttpClient _client;

    public MobileWalletMutationsEndpointTests(BankampanyaApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateWalletCard_ShouldReturnCreated()
    {
        const string json = """
        {
          "bankName": "Akbank",
          "cardType": "Kredi Kartı",
          "customName": "Axess Platinum"
        }
        """;

        var response = await _client.PostAsync(
            "/api/mobile/wallet/cards",
            new StringContent(json, Encoding.UTF8, "application/json"));

        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        content.Should().Contain("Axess Platinum");
        content.Should().Contain("Akbank");
    }

    [Fact]
    public async Task CreateWalletCard_WithInvalidPayload_ShouldReturnBadRequest()
    {
        const string json = """
        {
          "bankName": "",
          "cardType": "",
          "customName": "ab"
        }
        """;

        var response = await _client.PostAsync(
            "/api/mobile/wallet/cards",
            new StringContent(json, Encoding.UTF8, "application/json"));

        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        content.Should().Contain("validation errors occurred");
    }

    [Fact]
    public async Task UpdateWalletCardStatus_ShouldReturnOk()
    {
        const string json = """
        {
          "isActive": false
        }
        """;

        var response = await _client.PatchAsync(
            $"/api/mobile/wallet/cards/{Guid.NewGuid()}/status",
            new StringContent(json, Encoding.UTF8, "application/json"));

        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Contain("false");
    }

    [Fact]
    public async Task UpdateWalletCardStatus_WhenCardNotFound_ShouldReturnNotFound()
    {
        const string json = """
        {
          "isActive": true
        }
        """;

        var response = await _client.PatchAsync(
            "/api/mobile/wallet/cards/00000000-0000-0000-0000-000000000000/status",
            new StringContent(json, Encoding.UTF8, "application/json"));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteWalletCard_ShouldReturnNoContent()
    {
        var response = await _client.DeleteAsync($"/api/mobile/wallet/cards/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteWalletCard_WhenCardNotFound_ShouldReturnNotFound()
    {
        var response = await _client.DeleteAsync("/api/mobile/wallet/cards/00000000-0000-0000-0000-000000000000");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
