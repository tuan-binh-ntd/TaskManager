namespace TaskManager.Application.Core.CQRS;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
