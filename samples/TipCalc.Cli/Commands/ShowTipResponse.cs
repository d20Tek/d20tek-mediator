using Spectre.Console;

namespace TipCalc.Cli.Commands;

internal class ShowTipResponse
{
    public record Command(TipCommand Request, decimal TipAmount, decimal TotalAmount, decimal AmountPerTipper) : ICommand;

    public class Handler : ICommandHandler<Command>
    {
        public void Handle(Command command)
        {
            AnsiConsole.WriteLine();
            var table = CreateTableForReponse(command);
            AddRowsFor(table, command);

            AnsiConsole.Write(table);
        }

        private static Table CreateTableForReponse(Command tipResponse) =>
            new Table()
                .Border(TableBorder.Rounded)
                .AddColumns(
                    new TableColumn(string.Empty).Width(Constants.ColumnNameLen),
                    new TableColumn(string.Empty).RightAligned().Width(Constants.ColumnAmountsLen))
                .HideHeaders();

        private static void AddRowsFor(Table table, Command tipResponse)
        {
            table.AddRow(Constants.PriceRowLabel, tipResponse.Request.OriginalPrice.CurrencyDisplay())
                 .AddRow(Constants.TipPercentRowLabel, tipResponse.Request.TipPercentage.PercentageDisplay())
                 .AddRow(Constants.TipAmountRowLabel, tipResponse.TipAmount.CurrencyDisplay())
                 .AddRow(Constants.TotalAmountRowLabel, tipResponse.TotalAmount.CurrencyDisplay());

            if (tipResponse.Request.HasMultipleTippers() is true)
                table.AddRow(Constants.AmountPerTipperLabel, tipResponse.AmountPerTipper.CurrencyDisplay());
        }
    }
}
