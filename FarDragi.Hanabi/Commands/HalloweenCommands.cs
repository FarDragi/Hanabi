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
}