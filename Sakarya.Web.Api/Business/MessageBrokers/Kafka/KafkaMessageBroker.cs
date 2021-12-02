using System;
using System.Threading.Tasks;
using Business.MessageBrokers.Models;
using Confluent.Kafka;
using Core.Utilities.IoC;
using Core.Utilities.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Business.MessageBrokers.Kafka
{
    public class KafkaMessageBroker : IMessageBroker
    {
        private readonly MessageBrokerOption _kafkaOptions;

        public KafkaMessageBroker()
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
            if (configuration != null)
                _kafkaOptions = configuration.GetSection("MessageBrokerOptions").Get<MessageBrokerOption>();
        }

        public async Task GetMessageAsync<T>(string topic, string consumerGroup, Func<T, Task<IResult>> callback)
        {
            await Task.Run(async () =>
            {
                var config = new ConsumerConfig
                {
                    BootstrapServers = $"{_kafkaOptions.HostName}:{_kafkaOptions.Port}",
                    GroupId = consumerGroup,
                    EnableAutoCommit = false,
                    StatisticsIntervalMs = 5000,
                    SessionTimeoutMs = 6000,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnablePartitionEof = true,
                    PartitionAssignmentStrategy = PartitionAssignmentStrategy.CooperativeSticky
                };


                using var consumer = new ConsumerBuilder<Ignore, string>(config)
                    .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                    .SetStatisticsHandler((_, json) => Console.WriteLine($"Statistics: {json}"))
                    .Build();
                consumer.Subscribe(topic);

                try
                {
                    while (true)
                        try
                        {
                            var consumeResult = consumer.Consume();

                            if (consumeResult.IsPartitionEOF)
                            {
                                Console.WriteLine(
                                    $"Reached end of topic {consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");

                                continue;
                            }

                            Console.WriteLine(
                                $"Received message at {consumeResult.TopicPartitionOffset}: {consumeResult.Message.Value}");


                            var message = JsonConvert.DeserializeObject<T>(
                                consumeResult.Message.Value,
                                new JsonSerializerSettings
                                {
                                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                                });

                            var result = await callback(message);

                            if (!result.Success) continue;
                            try
                            {
                                consumer.Commit(consumeResult);
                            }
                            catch (KafkaException e)
                            {
                                Console.WriteLine($"Commit error: {e.Error.Reason}");
                            }
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Consume error: {e.Error.Reason}");
                        }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Closing consumer.");
                    consumer.Close();
                }
            });
        }

        public async Task<IResult> SendMessageAsync<T>(T messageModel) where T :
            class, new()
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = $"{_kafkaOptions.HostName}:{_kafkaOptions.Port}",
                Acks = Acks.All
            };

            var message = JsonConvert.SerializeObject(messageModel);
            var topicName = typeof(T).Name;
            using var p = new ProducerBuilder<Null, string>(producerConfig).Build();
            try
            {
                await p.ProduceAsync(topicName
                    , new Message<Null, string>
                    {
                        Value = message
                    });
                return new SuccessResult();
            }

            catch (ProduceException<Null, string> e)
            {
                return new ErrorResult(e.Message);
            }
        }
    }
}