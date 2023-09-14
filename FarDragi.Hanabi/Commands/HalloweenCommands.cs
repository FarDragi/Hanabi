using Discord;
using Discord.Interactions;
using FarDragi.Hanabi.Services.Interfaces;

namespace FarDragi.Hanabi.Commands;

[Group("halloween", "Comandos de halloween")]
public class HalloweenCommands : InteractionModuleBase
{
    private readonly IHalloweenService _halloweenService;

    public HalloweenCommands(IHalloweenService halloweenService)
    {
        _halloweenService = halloweenService;
    }

    [SlashCommand("treating", "Fazer uma travessura com alguém")]
    public async Task Treating([Summary("target", "Usuario alvo")] IGuildUser targetUser)
    {
        var (ok, treating) = await _halloweenService.Treating(Context.User.Id, targetUser.Id);

        if (!ok)
        {
            await RespondAsync("Você não tem travesuras a fazer");
        }
        else
        {
            await RespondAsync($"Travessura feita, {treating?.Count} travessuras restantes");
        }
    }

    [SlashCommand("info", "Informações sobre o usuario")]
    public async Task Info([Summary("target", "Usuario alvo")] IGuildUser? targetUser = null)
    {
        var user = await Context.Guild.GetUserAsync(targetUser?.Id ?? Context.User.Id);
        
        var (candy, treating) = await _halloweenService.UserInfo(user.Id);

        var embed = new EmbedBuilder()
            .WithTitle($"Halloween {user.DisplayName} status")
            .WithColor(0xd6a018)
            .WithThumbnailUrl(user.GetDisplayAvatarUrl())
            .WithFields(new[]
            {
                new EmbedFieldBuilder()
                    .WithName("🍬 Doces")
                    .WithValue(candy?.Count ?? 0),
                new EmbedFieldBuilder()
                    .WithName("🕸️ Travessuras")
                    .WithValue(treating?.Count ?? 0)
            });

        await RespondAsync(embed: embed.Build());
    }
}