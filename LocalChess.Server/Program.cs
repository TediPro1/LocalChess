using LocalChess.Controll.Controllers;
using LocalChess.Controll.Interfaces;
using LocalChess.Controll.Sessions;
using LocalChess.Data.Enums;
using LocalChess.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

builder.Services.AddSingleton<ILobbyManager, OfflineLobbyManager>();

var app = builder.Build();

app.MapHub<ChessHub>("/chesshub");

app.Run();