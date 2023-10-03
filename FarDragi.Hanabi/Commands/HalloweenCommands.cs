using Discord;
using Discord.Interactions;
using FarDragi.Hanabi.Exceptions;
using FarDragi.Hanabi.Services.Interfaces;

namespace FarDragi.Hanabi.Commands;

[Group("halloween", "Comandos de halloween")]
public class HalloweenCommands : InteractionModuleBase
{
    private readonly IHalloweenService _halloweenService;

    private const int OrangeColor = 0xd6a018;

    public HalloweenCommands(IHalloweenService halloweenService)
    {
        _halloweenService = halloweenService;
    }

    [SlashCommand("trick", "Fazer uma travessura com alguém")]
    public async Task Treating([Summary("target", "Usuario alvo")] IGuildUser targetUser)
    {
        if (targetUser.IsBot)
        {
            var isBotEmbed = new EmbedBuilder()
                .WithTitle("O usuario selecionado é um bot")
                .WithColor(OrangeColor);

            await RespondAsync(embed: isBotEmbed.Build(), ephemeral: true);
            return;
        }

        var (ok, treating) = await _halloweenService.Treating(Context.User.Id, targetUser.Id);

        if (!ok)
        {
            await RespondAsync("Você não tem travesuras a fazer", ephemeral: true);
        }
        else
        {
            await RespondAsync($"Travessura feita, {treating?.Count} travessuras restantes", ephemeral: true);
        }
    }

    [SlashCommand("info", "Informações sobre o usuario")]
    public async Task Info([Summary("target", "Usuario alvo")] IGuildUser? targetUser = null)
    {
        var user = await Context.Guild.GetUserAsync(targetUser?.Id ?? Context.User.Id);

        if (user.IsBot)
        {
            var isBotEmbed = new EmbedBuilder()
                .WithTitle("O usuario selecionado é um bot")
                .WithColor(OrangeColor);

            await RespondAsync(embed: isBotEmbed.Build(), ephemeral: true);
            return;
        }

        var (candy, treating) = await _halloweenService.UserInfo(user.Id);

        var embed = new EmbedBuilder()
            .WithTitle($"Halloween {user.DisplayName} status")
            .WithColor(OrangeColor)
            .WithThumbnailUrl(user.GetDisplayAvatarUrl())
            .WithFields(new[]
            {
                new EmbedFieldBuilder()
                    .WithName("🍬 Doces")
                    .WithValue((candy?.Count ?? 0).ToString()),
                new EmbedFieldBuilder()
                    .WithName("🕸️ Travessuras")
                    .WithValue((treating?.Count ?? 0).ToString())
            });

        await RespondAsync(embed: embed.Build(), ephemeral: true);
    }

    [SlashCommand("candy-add", "Adicionar doces manualmente")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public async Task CandyAdd([Summary("target", "Usuario alvo")] IGuildUser targetUser,
        [Summary("amount", "Quantidade de doces")]
        int amount)
    {
        var candy = await _halloweenService.AddManualCandies(targetUser.Id, amount);

        var responseEmbed = new EmbedBuilder()
            .WithTitle($"{targetUser.DisplayName} agora esta com {candy.Count} doces")
            .WithColor(OrangeColor);

        await RespondAsync(embed: responseEmbed.Build(), ephemeral: true);
    }

    [SlashCommand("trick-add", "Adicionar travesuras manualmente")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public async Task TreatingAdd([Summary("target", "Usuario alvo")] IGuildUser targetUser,
        [Summary("amount", "Quantidade de travesuras")]
        int amount)
    {
        var treating = await _halloweenService.AddManualTreating(targetUser.Id, amount);

        var responseEmbed = new EmbedBuilder()
            .WithTitle($"{targetUser.DisplayName} agora esta com {treating.Count} travesuras")
            .WithColor(OrangeColor);

        await RespondAsync(embed: responseEmbed.Build(), ephemeral: true);
    }

    [SlashCommand("leaderboard", "Listagem de quem tem mais doces")]
    public async Task Leaderboard([Summary("page", "Pagina")] int page = 1)
    {
        var candies = await _halloweenService.GetLeaderboard(page);

        var count = ((page - 1) * 10) + 1;

        var embed = new EmbedBuilder()
            .WithTitle("Leaderboard")
            .WithColor(OrangeColor)
            .WithDescription(string.Join("\n", candies.Select(x => $"{count++}º - <@{x.Id}>: {x.Count}")));

        await RespondAsync(embed: embed.Build(), ephemeral: true);
    }

    [SlashCommand("remove", "Remover usuario do bando de dados")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public async Task Remove([Summary("target", "Usuario alvo")] IGuildUser targetUser)
    {
        await _halloweenService.RemoveUser(targetUser.Id);

        var embed = new EmbedBuilder()
            .WithTitle("Usuario removido com sucesso")
            .WithColor(OrangeColor);

        await RespondAsync(embed: embed.Build(), ephemeral: true);
    }

    [SlashCommand("transfer", "Transfere seus doces para outra pessoa")]
    public async Task Transfer([Summary("target", "Usuario alvo")] IGuildUser targetUser,
        [Summary("quantity", "Quantidade de doces")] int quantity)
    {
        if (targetUser.IsBot)
        {
            var isBotEmbed = new EmbedBuilder()
                .WithTitle("O usuario selecionado é um bot")
                .WithColor(OrangeColor);

            await RespondAsync(embed: isBotEmbed.Build(), ephemeral: true);
            return;
        }
        
        if (targetUser.Id == Context.User.Id)
        {
            var isBotEmbed = new EmbedBuilder()
                .WithTitle("O usuario selecionado é você mesmo")
                .WithColor(OrangeColor);

            await RespondAsync(embed: isBotEmbed.Build(), ephemeral: true);
            return;
        }

        try
        {
            var candy = await _halloweenService.Transfer(Context.User.Id, targetUser.Id, quantity);

            var embed = new EmbedBuilder()
                .WithTitle($"{targetUser.DisplayName} agora esta com {candy.Count} doces")
                .WithColor(OrangeColor);

            await RespondAsync(embed: embed.Build(), ephemeral: true);
        }
        catch (HalloweenException hEx)
        {
            var embed = new EmbedBuilder()
                .WithTitle(hEx.Message)
                .WithColor(OrangeColor);

            await RespondAsync(embed: embed.Build(), ephemeral: true);
        }
    }
}