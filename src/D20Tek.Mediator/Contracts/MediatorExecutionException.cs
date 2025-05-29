namespace D20Tek.Mediator;

public class MediatorExecutionException : Exception
{
    private const string _message = "An exception was thrown during Mediator operation execution. " +
                                    "Check the inner exception for more details.";

    public string Operation { get; }

    public MediatorExecutionException(string operation, Exception innerException)
        : base(_message, innerException)
    {
        Operation = operation;
    }
}
