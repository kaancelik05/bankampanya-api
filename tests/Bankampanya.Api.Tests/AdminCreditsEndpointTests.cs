using System.Net;
using System.Net.Http.Json;
using Bankampanya.Application.Features.Credits.Dtos;
using Bankampanya.Domain.Enums;
using FluentAssertions;

namespace Bankampanya.Api.Tests;

public sealed class AdminCreditsEndpointTests : IClassFixture<BankampanyaApiFactory>
{
    private readonly HttpClient _client;

    public AdminCreditsEndpointTests(BankampanyaApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostCredit_WithInvalidPayload_ShouldReturnBadRequest()
    {
        var payload = new UpsertCreditRequest
        {
            BankName = "",
            Title = "Kısa",
            Type = CreditOfferType.Kredi,
            Rate = "",
            AmountRange = "",
            DetailSummary = "kısa",
            Terms = Array.Empty<string>(),
            Status = PublishStatus.Live,
        };

        var response = await _client.PostAsJsonAsync("/api/admin/credits", payload);
        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        content.Should().Contain("validation errors occurred");
        content.Should().Contain("En az bir kredi koşulu girilmelidir");
    }
}
