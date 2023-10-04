using System.Net.Http.Headers;
using System.Net.Http.Json;
using FarDragi.Hanabi.Adapters.Interfaces;
using FarDragi.Hanabi.Exceptions;
using FarDragi.Hanabi.Models;
using FarDragi.Hanabi.Models.Interfaces;
using Lina.DynamicServicesProvider.Attributes;

namespace FarDragi.Hanabi.Adapters;

[HttpClient(typeof(IDiscordAdapter))]
public class DiscordAdapter : IDiscordAdapter
{
    private readonly HttpClient _httpClient;
    private readonly IAppConfig _appConfig;

    public DiscordAdapter(HttpClient httpClient, IAppConfig appConfig)
    {
        httpClient.BaseAddress = new Uri("https://discord.com");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(appConfig.Bot.NiceToken);
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/118.0");
        
        _httpClient = httpClient;
        _appConfig = appConfig;
    }

    public async Task<UserJoinDto?> GetUserInvite(ulong userId)
    {
        var response = await _httpClient.PostAsJsonAsync($"/api/v9/guilds/{_appConfig.Bot.OwnerGuild}/members/supplemental",
            new UserIdsDto(new[]
            {
                userId.ToString()
            }));

        if (!response.IsSuccessStatusCode)
            throw new DiscordAdapterException("Falha em buscar os dados do convite");

        var result = await response.Content.ReadFromJsonAsync<IEnumerable<UserJoinDto>>();

        return result?.FirstOrDefault();
    }
}