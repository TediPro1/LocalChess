using LocalChess.Controll.Controllers;
using LocalChess.Controll.Interfaces;
using LocalChess.Data.DTOs;
using LocalChess.Data.Enums;
using LocalChess.Server.Data;
using LocalChess.Server.Hubs;
using LocalChess.Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ChessContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ChessDatabase")));

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

builder.Services.AddSingleton<ILobbyManager, OfflineLobbyManager>();
builder.Services.AddScoped<GameSaveService>();

var app = builder.Build();

app.MapHub<ChessHub>("/chesshub");

app.MapGet("/saved-games", async (ChessContext context) =>
{
    var games = await context.SavedGames
        .AsNoTracking()
        .Include(game => game.Moves)
        .OrderByDescending(game => game.FinishedAt)
        .ThenByDescending(game => game.Id)
        .ToListAsync();

    return games.Select(GameSaveService.ToDto).ToList();
});

app.MapPost("/saved-games", async (CompletedGameDTO completedGame, GameSaveService saver) =>
{
    SavedGameDTO savedGame = await saver.SaveGameAsync(completedGame);
    return Results.Created($"/saved-games/{savedGame.Id}", savedGame);
});

app.Run();
