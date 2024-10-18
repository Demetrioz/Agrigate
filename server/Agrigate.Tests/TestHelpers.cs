using Agrigate.Domain.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Agrigate.Tests;

public class TestHelpers
{
    public static AgrigateContext GetUniqueTestDb(string testName) 
    {
        var contextOptions = new DbContextOptionsBuilder<AgrigateContext>()
            .UseInMemoryDatabase(testName)
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        return new AgrigateContext(contextOptions);
    }
}