using System.Net;
using FluentAssertions;

namespace Bankampanya.Api.Tests;

public sealed class AdminDashboardEndpointTests : IClassFixture<BankampanyaApiFactory>
{
    private readonly HttpClient _client;

    public AdminDashboardEndpointTests(BankampanyaApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetDashboardSummary_ShouldReturnSuccessStatusCode()
    {
        var response = await _client.GetAsync("/api/admin/dashboard-summary");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
