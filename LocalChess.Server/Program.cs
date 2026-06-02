using LocalChess.Controll.Controllers;
using LocalChess.Controll.Interfaces;
using LocalChess.Data.DTOs;
using LocalChess.Data.Enums;
using LocalChess.Server.Data;
using LocalChess.Server.Hubs;
using LocalChess.Server.Services;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
 
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

builder.Services.AddDbContext<ChessContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("ChessDatabase")));

Console.WriteLine(builder.Configuration.GetConnectionString("ChessDatabase"));
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

builder.Services.AddSingleton<ILobbyManager, OfflineLobbyManager>();
builder.Services.AddScoped<GameSaveService>();

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    ChessContext context = scope.ServiceProvider.GetRequiredService<ChessContext>();
    await context.Database.MigrateAsync();
    await EnsureSaveKeyColumnAsync(context);
}

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

static async Task EnsureSaveKeyColumnAsync(ChessContext context)
{
    await context.Database.ExecuteSqlRawAsync("""
        IF OBJECT_ID(N'dbo.SavedGames', N'U') IS NOT NULL
           AND COL_LENGTH(N'dbo.SavedGames', N'SaveKey') IS NULL
        BEGIN
            ALTER TABLE dbo.SavedGames ADD SaveKey nvarchar(100) NULL;
        END

        IF OBJECT_ID(N'dbo.SavedGames', N'U') IS NOT NULL
           AND COL_LENGTH(N'dbo.SavedGames', N'SaveKey') IS NOT NULL
           AND NOT EXISTS (
               SELECT 1
               FROM sys.indexes
               WHERE name = N'IX_SavedGames_SaveKey'
                 AND object_id = OBJECT_ID(N'dbo.SavedGames')
           )
        BEGIN
            CREATE UNIQUE INDEX IX_SavedGames_SaveKey
            ON dbo.SavedGames(SaveKey)
            WHERE SaveKey IS NOT NULL;
        END
        """);
}
