using D20Tek.Functional;
using D20Tek.Mediator;
using Spectre.Console;
using TipCalc.Cli.Common;

namespace TipCalc.Cli.Commands;

internal class GatherTipDetails
{
    public record Command : ICommand<Result<TipCommand>>;

    public class Handler : ICommandHandler<Command, Result<TipCommand>>
    {
        public Result<TipCommand> Handle(Command command)
        {
            try
            {
                return new TipCommand(GetOriginalPrice(), GetTipPercentage(), GetTipperCount());
            }
            catch (Exception ex)
            {
                return Result<TipCommand>.Failure(ex);
            }
        }

        private static decimal GetOriginalPrice() =>
            AnsiConsole.Ask<decimal>(Constants.OriginalPriceLabel);

        private static decimal GetTipPercentage() =>
            AnsiConsole.Prompt(new TextPrompt<decimal>(Constants.TipPercentageLabel)
                       .DefaultValue(15)
                       .Validate(v => Constants.PercentRange.InRange(v), Constants.PercentRangeError));

        private static int GetTipperCount() =>
            AnsiConsole.Prompt(new TextPrompt<int>(Constants.TipperCountLabel)
                       .DefaultValue(1)
                       .Validate(v => Constants.TipperCountRange.InRange(v), Constants.TipperCountRangeError));
    }
}
