

using MediatR;

namespace Application.Abstractions.Messaging
{
    public interface IBaseCommand
    {
        
    }

    internal interface ICommand<out TResponse> : IRequest<TResponse>, IBaseCommand
    {
    }
    internal interface ICommand : IRequest, IBaseCommand
    {
    }
}
