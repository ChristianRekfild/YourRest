using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using YouRest.Orchestrator.Consumers;
using YouRest.Orchestrator.StateMachines.BookingStateMachine;

namespace YouRest.Orchestrator
{
    public static class OrchestratorDependencyInjections
    {
        public static IServiceCollection AddOrchestrator(this IServiceCollection services)
        {
            //Подключение настроек RabbitMq из appsettings.json
            services.AddOptions<RabbitMqTransportOptions>("RabbitMqConfiguration");
            services.AddMassTransit(x =>
            {
                //Регистрация потребителей
                x.AddConsumer<BookingSubmittedConsumer>();
                //Регистрация машины состояний
                x.AddSagaStateMachine<BookingStateMashine, BookingState>()
                .InMemoryRepository();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });

            });
            return services;
        }
    }
}
