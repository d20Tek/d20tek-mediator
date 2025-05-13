namespace D20Tek.Mediator.UnitTests.Mocks;

internal static class ConsoleMock
{
    private static readonly StringWriter _stdout;

    static ConsoleMock()
    {
        _stdout = new StringWriter();
        Console.SetOut(_stdout);
    }

    public static void Initialize() => _ = _stdout;

    public static string GetOutput() => _stdout.ToString();
}
