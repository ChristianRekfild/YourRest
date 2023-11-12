using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YouRest.Orchestrator.Consumers;
using YouRest.Orchestrator.StateMachines.BookingStateMachine;
using YourRest.Application;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Producer.Infrastructure;

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
            services.AddHttpClient();
            return services;
        }
    }
}
