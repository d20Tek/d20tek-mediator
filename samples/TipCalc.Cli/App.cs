using Spectre.Console;
using TipCalc.Cli.Commands;

namespace TipCalc.Cli;

internal class App
{
    private readonly ICommandHandler<GatherTipDetails.Command, Result<TipCommand>> _gatherDetailsHandler;
    private readonly ICommandHandler<TipCommand, Result<TipResponse>> _calculateTipHandler;

    public App(
        ICommandHandler<GatherTipDetails.Command, Result<TipCommand>> gatherDetailsHandler,
        ICommandHandler<TipCommand, Result<TipResponse>> calculateTipHandler)
    {
        _gatherDetailsHandler = gatherDetailsHandler;
        _calculateTipHandler = calculateTipHandler;
    }

    public void Run()
    {
        DisplayTitle();
        var result = _gatherDetailsHandler.Handle(new GatherTipDetails.Command())
                                          .Map(tipCommand => _calculateTipHandler.Handle(tipCommand));
    }

    private static void DisplayTitle() =>
        AnsiConsole.Write(new FigletText(Constants.AppTitle).Centered().Color(Color.Green));
}
