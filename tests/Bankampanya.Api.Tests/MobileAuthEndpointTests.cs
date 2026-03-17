using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;

namespace Bankampanya.Api.Tests;

public sealed class MobileAuthEndpointTests : IClassFixture<BankampanyaApiFactory>
{
    private readonly HttpClient _client;

    public MobileAuthEndpointTests(BankampanyaApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_Should_Create_User_And_Return_Session()
    {
        var response = await _client.PostAsJsonAsync("/api/mobile/auth/register", new
        {
            fullName = "Auth Test Kullanıcısı",
            email = "auth.test@bankampanya.com",
            phone = "05550001122",
            password = "123456",
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        payload.GetProperty("success").GetBoolean().Should().BeTrue();
        payload.GetProperty("user").GetProperty("email").GetString().Should().Be("auth.test@bankampanya.com");
        payload.GetProperty("session").GetProperty("accessToken").GetString().Should().NotBeNullOrWhiteSpace();
        payload.GetProperty("session").GetProperty("refreshToken").GetString().Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Login_With_Valid_Password_Should_Return_Jwt_Session()
    {
        await _client.PostAsJsonAsync("/api/mobile/auth/register", new
        {
            fullName = "JWT Login Kullanıcısı",
            email = "jwt.login@bankampanya.com",
            phone = "05550002233",
            password = "123456",
        });

        var response = await _client.PostAsJsonAsync("/api/mobile/auth/login", new
        {
            identifier = "jwt.login@bankampanya.com",
            password = "123456",
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        var accessToken = payload.GetProperty("session").GetProperty("accessToken").GetString();
        accessToken.Should().NotBeNullOrWhiteSpace();
        accessToken!.Split('.').Length.Should().Be(3);
    }

    [Fact]
    public async Task Login_With_Invalid_Password_Should_Return_Unauthorized()
    {
        await _client.PostAsJsonAsync("/api/mobile/auth/register", new
        {
            fullName = "Wrong Password Kullanıcısı",
            email = "wrong.password@bankampanya.com",
            phone = "05550003344",
            password = "123456",
        });

        var response = await _client.PostAsJsonAsync("/api/mobile/auth/login", new
        {
            identifier = "wrong.password@bankampanya.com",
            password = "654321",
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Me_With_Jwt_Should_Return_Current_User()
    {
        await _client.PostAsJsonAsync("/api/mobile/auth/register", new
        {
            fullName = "Me Endpoint Kullanıcısı",
            email = "me.endpoint@bankampanya.com",
            phone = "05550004455",
            password = "123456",
        });

        var loginResponse = await _client.PostAsJsonAsync("/api/mobile/auth/login", new
        {
            identifier = "me.endpoint@bankampanya.com",
            password = "123456",
        });

        var loginPayload = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
        var token = loginPayload.GetProperty("session").GetProperty("accessToken").GetString();

        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/mobile/auth/me");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        payload.GetProperty("email").GetString().Should().Be("me.endpoint@bankampanya.com");
    }

    [Fact]
    public async Task Refresh_With_Valid_RefreshToken_Should_Return_New_Session()
    {
        await _client.PostAsJsonAsync("/api/mobile/auth/register", new
        {
            fullName = "Refresh Kullanıcısı",
            email = "refresh.user@bankampanya.com",
            phone = "05550005566",
            password = "123456",
        });

        var loginResponse = await _client.PostAsJsonAsync("/api/mobile/auth/login", new
        {
            identifier = "refresh.user@bankampanya.com",
            password = "123456",
        });

        var loginPayload = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
        var previousRefreshToken = loginPayload.GetProperty("session").GetProperty("refreshToken").GetString();

        var refreshResponse = await _client.PostAsJsonAsync("/api/mobile/auth/refresh", new
        {
            refreshToken = previousRefreshToken,
        });

        refreshResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var refreshPayload = await refreshResponse.Content.ReadFromJsonAsync<JsonElement>();
        refreshPayload.GetProperty("session").GetProperty("accessToken").GetString().Should().NotBeNullOrWhiteSpace();
        refreshPayload.GetProperty("session").GetProperty("refreshToken").GetString().Should().NotBe(previousRefreshToken);
    }

    [Fact]
    public async Task Logout_Should_Revoke_RefreshToken()
    {
        await _client.PostAsJsonAsync("/api/mobile/auth/register", new
        {
            fullName = "Logout Kullanıcısı",
            email = "logout.user@bankampanya.com",
            phone = "05550006677",
            password = "123456",
        });

        var loginResponse = await _client.PostAsJsonAsync("/api/mobile/auth/login", new
        {
            identifier = "logout.user@bankampanya.com",
            password = "123456",
        });

        var loginPayload = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
        var refreshToken = loginPayload.GetProperty("session").GetProperty("refreshToken").GetString();

        var logoutResponse = await _client.PostAsJsonAsync("/api/mobile/auth/logout", new
        {
            refreshToken,
        });

        logoutResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var refreshResponse = await _client.PostAsJsonAsync("/api/mobile/auth/refresh", new
        {
            refreshToken,
        });

        refreshResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Me_Without_Token_Should_Return_Unauthorized()
    {
        var response = await _client.GetAsync("/api/mobile/auth/me");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
