using Spectre.Console;
using TipCalc.Cli.Commands;

namespace TipCalc.Cli;

internal class App
{
    private readonly ICommandHandler<GatherTipDetails.Command, Result<TipCommand>> _gatherDetailsHandler;
    private readonly ICommandHandler<TipCommand, Result<TipResponse>> _calculateTipHandler;
    private readonly ICommandHandler<ShowTipResponse.Command> _showTipHandler;

    public App(
        ICommandHandler<GatherTipDetails.Command, Result<TipCommand>> gatherDetailsHandler,
        ICommandHandler<TipCommand, Result<TipResponse>> calculateTipHandler,
        ICommandHandler<ShowTipResponse.Command> showTipHandler)
    {
        _gatherDetailsHandler = gatherDetailsHandler;
        _calculateTipHandler = calculateTipHandler;
        _showTipHandler = showTipHandler;
    }

    public int Run()
    {
        DisplayTitle();

        var resTipCommand = RunGatherDetailsHandler();
        var resTipResponse = RunCalculateTipHandler(resTipCommand);
        resTipResponse = RunShowTipHandler(resTipResponse, resTipCommand);

        return resTipResponse.ToResultCode();
    }

    private static void DisplayTitle() =>
        AnsiConsole.Write(new FigletText(Constants.AppTitle).Centered().Color(Color.Green));

    private Result<TipCommand> RunGatherDetailsHandler() =>
        _gatherDetailsHandler.Handle(new GatherTipDetails.Command());

    private Result<TipResponse> RunCalculateTipHandler(Result<TipCommand> resTipCommand) =>
        resTipCommand.Match(s => _calculateTipHandler.Handle(s), e => Result<TipResponse>.Failure(e));

    private Result<TipResponse> RunShowTipHandler(Result<TipResponse> resTipResponse, Result<TipCommand> resRequest)
    {
        if (resTipResponse.IsSuccess && resTipResponse.GetValue() is TipResponse r)
        {
            _showTipHandler.Handle(new(resRequest.GetValue(), r.TipAmount, r.TotalAmount, r.AmountPerTipper));
        }

        return resTipResponse;
    }
}
