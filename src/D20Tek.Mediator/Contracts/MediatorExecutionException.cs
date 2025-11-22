namespace D20Tek.Mediator;

public class MediatorExecutionException(string operation, Exception innerException) :
    Exception(_message, innerException)
{
    private const string _message = "An exception was thrown during Mediator operation execution. " +
                                    "Check the inner exception for more details.";

    public string Operation { get; } = operation;
}
