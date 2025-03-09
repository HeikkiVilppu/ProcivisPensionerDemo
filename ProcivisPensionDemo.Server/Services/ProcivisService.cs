using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class ProcivisService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;
    private readonly string _clientId;
    private readonly string _clientSecret;


    //QR-koodi sisältää linkin teidän backendin API:in, esimerkiksi:
    //https://yourbackend.com/api/verification/start?sessionId=abc123
    public ProcivisService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiBaseUrl = config["Procivis:ApiBaseUrl"];
        _clientId = config["Procivis:ClientId"];
        _clientSecret = config["Procivis:ClientSecret"];
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var requestBody = new
        {
            client_id = _clientId,
            client_secret = _clientSecret,
            grant_type = "client_credentials"
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_apiBaseUrl}/auth/token", content);

        if (!response.IsSuccessStatusCode)
            throw new Exception("Failed to obtain access token from Procivis");

        var responseContent = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(responseContent);
        return doc.RootElement.GetProperty("access_token").GetString();
    }

    public async Task<bool> VerifyPensionerStatusAsync(string proofToken)
    {
        string accessToken = await GetAccessTokenAsync();

        var request = new HttpRequestMessage(HttpMethod.Post, $"{_apiBaseUrl}/verify-proof")
        {
            Content = new StringContent(JsonSerializer.Serialize(new { proof_token = proofToken }), Encoding.UTF8, "application/json")
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return false;

        var responseContent = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(responseContent);

        // Tarkistetaan eläkeläisstatus todistuksesta
        return doc.RootElement.GetProperty("isPensioner").GetBoolean();
    }
}
