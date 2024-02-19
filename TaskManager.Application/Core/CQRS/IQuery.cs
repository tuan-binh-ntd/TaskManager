namespace TaskManager.Application.Core.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
