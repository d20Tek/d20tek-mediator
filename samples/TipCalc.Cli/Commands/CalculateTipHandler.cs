namespace TipCalc.Cli.Commands;

internal class CalculateTipHandler : ICommandHandler<TipCommand, Result<TipResponse>>
{
    public Result<TipResponse> Handle(TipCommand command)
    {
        try
        {
            var tipAmount = command.OriginalPrice * (command.TipPercentage / Constants.Percent);
            var totalAmount = command.OriginalPrice + tipAmount;
            return new TipResponse(tipAmount, totalAmount, totalAmount / command.TipperCount);
        }
        catch (Exception ex)
        {
            return Result<TipResponse>.Failure(ex);
        }
    }
}
