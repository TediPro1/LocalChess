using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocalChess.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace LocalChess.Test.Helpers
{
    public class TestDbFactory
    {
        public static ChessContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ChessContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            ChessContext context = new ChessContext(options);

            context.Database.EnsureCreated();

            return context;
        }
    }
}
