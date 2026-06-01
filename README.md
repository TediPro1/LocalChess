# ♟️ LocalChess

> "How hard can it be to make a chess game?"
>
> — Me, moments before implementing checkmate, en passant, castling, pins, promotion, repetition detection, online multiplayer, and questioning my life choices.

---

## 🎯 What is LocalChess?

LocalChess is a chess application built with **C#**, **Windows Forms**, **SignalR**, and a concerning amount of caffeine.

The project started as a simple local chess board and somehow evolved into a fully-featured chess game with:

- ♟️ Complete chess rules
- 🌐 Online multiplayer
- 🏰 Castling
- 👻 En passant
- 👑 Promotion
- 🔒 Pin detection
- ⚠️ Check detection
- ☠️ Checkmate detection
- 🤝 Stalemate detection
- 🔄 Draw by repetition
- 📜 Game history (planned)
- 💾 Database support (planned)

---

## 📸 Features

### 🎮 Play Chess

Because every chess application should probably include chess.

### ⚔️ Online Multiplayer

Create lobbies and challenge your friends.

Or strangers.

Or your future self after opening two instances.

### 🏰 Castling

Both kingside and queenside castling are supported.

No teleporting kings allowed.

### 👻 En Passant

Yes.

Unfortunately.

### 👑 Promotion

Promote pawns to:

- Queen
- Rook
- Bishop
- Knight

Promoting to another pawn is prohibited by the Geneva Convention.

### 🔴 Check Detection

The king's square turns red when checked.

Because panic should be visual.

### ☠️ Checkmate Detection

The game knows when it's over.

Unlike some players.

### 🤝 Stalemate Detection

For when nobody wins but everyone loses.

---

## 🛠️ Tech Stack

| Technology | Purpose |
|------------|----------|
| C# | Main language |
| Windows Forms | UI |
| SignalR | Online multiplayer |
| .NET | Runtime |
| MSSQL | Planned game storage |
| EF Core | Planned persistence layer |

---

## 🏗️ Project Structure

```text
LocalChess
├── LocalChess.View
│   ├── ChessBoardForm
│   ├── MainMenu
│   └── PromotionDialog
│
├── LocalChess.Controll
│   ├── ChessGame
│   ├── Move Generation
│   ├── Rule Validation
│   └── Game Sessions
│
├── LocalChess.Data
│   ├── DTOs
│   └── Models
│
└── LocalChess.Server
    └── SignalR Hub
```

---

## 🧠 Things the Engine Understands

- Legal moves
- Check
- Checkmate
- Stalemate
- Pins
- Discovered attacks
- Castling rights
- Promotion
- En passant
- Repetition

Things it does **not** understand:

- Why someone would sacrifice a queen on move 4.

---

## 🔬 Testing

The engine is tested using custom FEN positions covering:

- Checkmate
- Stalemate
- Castling
- Promotion
- Pins
- En passant
- Insufficient material
- Illegal positions

If a bug survives all of these tests, it has earned the right to exist.

---

## 🚧 Roadmap

### Current

- [x] Local games
- [x] Online games
- [x] Full move validation
- [x] Promotion UI
- [x] Checkmate detection

### Planned

- [ ] Save games to database
- [ ] PGN export
- [ ] Move notation
- [ ] Replay viewer
- [ ] Game history
- [ ] Analysis mode
- [ ] AI opponent

### Future

- [ ] Beat Stockfish

(Confidence level: 0%)

---

## 🐛 Known Bugs

If you find one:

1. Congratulations.
2. Please tell me before it reaches production.
3. If the king starts moving like a knight, run.

---

## 📖 Fun Facts

- En passant took longer than expected.
- Every chess rule is more complicated than it looks.
- The number of times "just one small fix" caused a new bug is classified information.
- Chess engines are an excellent way to learn humility.

---

## 👨‍💻 Author

**Tedi Penev**

Built for learning, fun, and discovering just how many edge cases can fit into a board with only 64 squares.

---

> "The code is temporary.
>
> The bugs are forever."
