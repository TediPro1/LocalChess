using LocalChess.Data.Entities;
using LocalChess.Data.Enums;
using LocalChess.Server.Data;
using LocalChess.Test.Helpers;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalChess.Test.Services
{
    public class RulesTests
    {
        public ChessContext Context { get; set; }
        public ChessGame Game { get; set; }
        [SetUp]
        public void SetUp()
        {
            Context = TestDbFactory.CreateContext();
            Game = new ChessGame();
        }
        [TearDown]
        public void TearDown()
        {
            Context.Dispose();
        }
        [Test]
        public async Task TestStartingPosition()
        {
            Game.LoadFromFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            List<Point> leagalMoves = Game.GetLegalMoves(Game.FindKing(PieceColor.White));
            CollectionAssert.AreEquivalent(new List<Point>(), leagalMoves);
        }
        [Test]
        public async Task TestInCheckByRook()
        {
            Game.LoadFromFen("4k3/8/8/8/8/8/4r3/4K3 w - - 0 1");
            List<Point> leagalMoves = Game.GetLegalMoves(Game.FindKing(PieceColor.White));
            List<Point> expectedResult = new List<Point>()
            {
                new Point(6,4), // e2 capture rook
                new Point(7,3), // d1
                new Point(7,5)  // f1
            };
            Assert.IsTrue(Game.IsKingInCheck(PieceColor.White));
            Assert.IsFalse(Game.IsKingInCheck(PieceColor.Black));
            CollectionAssert.AreEquivalent(expectedResult, leagalMoves);
        }
        [Test]
        public async Task TestInDoubleCheck()
        {
            Game.LoadFromFen("4k3/8/8/8/8/3b4/4r3/4K3 w - - 0 1");
            List<Point> leagalMoves = Game.GetLegalMoves(Game.FindKing(PieceColor.White));
            List<Point> expectedResult = new List<Point>()
            {
                new Point(7,3), // d1
                new Point(7,5)  // f1
            };
            Assert.IsTrue(Game.IsKingInCheck(PieceColor.White));
            Assert.IsFalse(Game.IsKingInCheck(PieceColor.Black));
            CollectionAssert.AreEquivalent(expectedResult, leagalMoves);
        }
        [Test]
        public async Task TestStalemate()
        {
            Game.LoadFromFen("7k/5Q2/6K1/8/8/8/8/8 b - - 0 1");
            Assert.IsTrue(Game.IsStalemate(PieceColor.Black));
            Assert.IsFalse(Game.IsStalemate(PieceColor.White));
        }
        [Test]
        public async Task TestCheckmate()
        {
            Game.LoadFromFen("6k1/6Q1/6K1/8/8/8/8/8 b - - 0 1");
            Assert.IsTrue(Game.IsCheckmate(PieceColor.Black));
            Assert.IsFalse(Game.IsCheckmate(PieceColor.White));
        }
        [Test]
        public async Task TestCastling()
        {
            Game.LoadFromFen("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1");
            List<Point> legalMoves = Game.GetLegalMoves(Game.FindKing(PieceColor.White));
            List<Point> expectedResult = new List<Point>()
            {
                new Point(6, 3), // d2
                new Point(6, 4), // e2
                new Point(6, 5), // f2
                new Point(7, 2), // queenside castle (c1)
                new Point(7, 3), // d1
                new Point(7, 5), // f1
                new Point(7, 6)  // kingside castle (g1)
            };
            CollectionAssert.AreEquivalent(expectedResult, legalMoves);
        }
        [Test]
        public async Task TestCastlingBlocked()
        {
            Game.LoadFromFen("r3k2r/8/8/8/8/8/8/R2BK2R w KQkq - 0 1");
            List<Point> legalMoves = Game.GetLegalMoves(Game.FindKing(PieceColor.White));
            List<Point> expectedResult = new List<Point>()
            {
                new Point(6,3), // d2
                new Point(6,4), // e2
                new Point(6,5), // f2
                new Point(7,5), // f1
                new Point(7,6)  // g1 castle
            };
            CollectionAssert.AreEquivalent(expectedResult, legalMoves);
        }
        [Test]
        public async Task TestLaoneKingCentre()
        {
            Game.LoadFromFen("8/8/8/3K4/8/8/8/8 w - - 0 1");
            List<Point> legalMoves = Game.GetLegalMoves(Game.FindKing(PieceColor.White));
            List<Point> expectedresult = new List<Point>()
            {
                new Point(2,2),
                new Point(3,2),
                new Point(4,2),

                new Point(2,3),
                new Point(4,3),

                new Point(2,4),
                new Point(3,4),
                new Point(4,4)
            };
            CollectionAssert.AreEquivalent(expectedresult, legalMoves);
        }
        [Test]
        public async Task TestCornerKing()
        {
            Game.LoadFromFen("8/8/8/8/8/8/8/K7 w - - 0 1");
            List<Point> legalMoves = Game.GetLegalMoves(Game.FindKing(PieceColor.White));
            List<Point> expectedresult = new List<Point>()
            {
                new Point(6, 0),
                new Point(6, 1),
                new Point(7, 1)
            };
            CollectionAssert.AreEquivalent(expectedresult, legalMoves);
        }
        [Test]
        public async Task TestEnPassant()
        {
            Assert.IsTrue(Game.TryMove(new Point(6, 4), new Point(4, 4))); // e2 to e4
            Assert.IsTrue(Game.TryMove(new Point(1, 0), new Point(2, 0))); // a7 to a6
            Assert.IsTrue(Game.TryMove(new Point(4, 4), new Point(3, 4))); // e4 to e5
            Assert.IsTrue(Game.TryMove(new Point(1, 3), new Point(3, 3))); // d7 to d5

            List<Point> legalMoves = Game.GetLegalMoves(new Point(3, 4)); // e5 pawn
            List<Point> expectedResult = new List<Point>()
            {
                new Point(2, 3), // d6 en passant
                new Point(2, 4), // e6
            };

            CollectionAssert.AreEquivalent(expectedResult, legalMoves);
            Assert.IsTrue(Game.TryMove(new Point(3, 4), new Point(2, 3)));
            Assert.IsNull(Game.Board.GetPiece(3, 3)); // captured d5 pawn
        }

        [Test]
        public void EnPassantCannotJumpFromD7ToE3AfterE4()
        {
            Assert.IsTrue(Game.TryMove(new Point(6, 4), new Point(4, 4))); // e2 to e4

            Assert.IsFalse(Game.TryMove(new Point(1, 3), new Point(5, 4))); // d7 to e3
            Assert.IsNotNull(Game.Board.GetPiece(1, 3));
            Assert.IsNull(Game.Board.GetPiece(5, 4));
        }

    }
}
