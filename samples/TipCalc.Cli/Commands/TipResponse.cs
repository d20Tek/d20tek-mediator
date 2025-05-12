namespace TipCalc.Cli.Commands;

internal sealed record TipResponse(decimal TipAmount, decimal TotalAmount, decimal AmountPerTipper);
