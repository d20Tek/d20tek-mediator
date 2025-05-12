namespace TipCalc.Cli.Commands;

public record TipCommand(decimal OriginalPrice, decimal TipPercentage, int TipperCount) :
    ICommand<Result<TipResponse>>
{
    public bool HasMultipleTippers() => TipperCount > Constants.MinimumTippers;
}
