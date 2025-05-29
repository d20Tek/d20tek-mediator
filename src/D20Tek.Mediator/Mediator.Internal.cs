using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace D20Tek.Mediator;

public partial class Mediator
{
    private object GetCommandHandler(Type typeInterface, object command, Type? typeResponse = null)
    {
        ArgumentNullException.ThrowIfNull(command, nameof(command));
        var commandType = command.GetType();

        Type handlerType = HandlerTypeDictionary.GetOrAdd(
            commandType,
            et => typeResponse is null ?
                    typeInterface.MakeGenericType(commandType) :
                    typeInterface.MakeGenericType(commandType, typeResponse));

        return _provider.GetRequiredService(handlerType);
    }

    private object[] GetNotificationHandlers(Type typeInterface, object notification)
    {
        ArgumentNullException.ThrowIfNull(notification, nameof(notification));

        Type handlerType = HandlerTypeDictionary.GetOrAdd(
            notification.GetType(),
            et => typeInterface.MakeGenericType(et));

        var handlers = _provider.GetServices(handlerType);
        return [.. handlers.OfType<object>()];
    }

    private TResponse TryOperation<TResponse>(string operationName, Func<TResponse> operation)
    {
        try
        {
            return operation();
        }
        catch (TargetInvocationException invEx)
        {
            throw new MediatorExecutionException(operationName, invEx.InnerException ?? invEx);
        }
        catch (Exception ex)
        {
            throw new MediatorExecutionException(operationName, ex);
        }
    }

    private void TryOperation(string operationName, Action operation)
    {
        try
        {
            operation();
        }
        catch (TargetInvocationException invEx)
        {
            throw new MediatorExecutionException(operationName, invEx.InnerException ?? invEx);
        }
        catch (Exception ex)
        {
            throw new MediatorExecutionException(operationName, ex);
        }
    }
}
