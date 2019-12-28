using Microsoft.Extensions.Logging;

namespace UpdateLib.Tests
{
    public static class Helpers
    {
        public static ILogger<T> CreateLogger<T>() => LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<T>();
    }
}
