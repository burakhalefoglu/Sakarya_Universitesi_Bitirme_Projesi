using System;
using System.Threading.Tasks;
using Core.Utilities.Results;

namespace Business.MessageBrokers
{
    public interface IMessageBroker
    {
        Task<IResult> SendMessageAsync<T>(T messageModel) where T :
            class, new();

        Task GetMessageAsync<T>(string topic, string consumerGroup, Func<T, Task<IResult>> callback);
    }
}