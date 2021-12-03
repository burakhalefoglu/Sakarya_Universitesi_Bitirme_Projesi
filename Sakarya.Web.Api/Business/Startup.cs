using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using Autofac;
using Business.Constants;
using Business.DependencyResolvers;
using Business.Fakes.DArch;
using Business.MessageBrokers;
using Business.MessageBrokers.Kafka;
using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Microsoft;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.ElasticSearch;
using Core.Utilities.IoC;
using Core.Utilities.MessageBrokers.RabbitMq;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Concrete.MongoDb;
using DataAccess.Concrete.MongoDb.Collections;
using DataAccess.Concrete.MongoDb.Context;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Business
{
    public class BusinessStartup
    {
        private readonly IHostEnvironment _hostEnvironment;

        public BusinessStartup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        protected IConfiguration Configuration { get; }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <remarks>
        ///     It is common to all configurations and must be called. Aspnet core does not call this method because there are
        ///     other methods.
        /// </remarks>
        /// <param name="services"></param>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            Func<IServiceProvider, ClaimsPrincipal> getPrincipal = sp =>
                sp.GetService<IHttpContextAccessor>().HttpContext?.User ??
                new ClaimsPrincipal(new ClaimsIdentity(Messages.Unknown));

            services.AddScoped<IPrincipal>(getPrincipal);
            services.AddMemoryCache();

            services.AddDependencyResolvers(Configuration, new ICoreModule[]
            {
                new CoreModule()
            });

            services.AddSingleton<ConfigurationManager>();
            services.AddTransient<ITokenHelper, JwtHelper>();

            services.AddTransient<IElasticSearch, ElasticSearchManager>();

            services.AddTransient<IMessageBrokerHelper, MqQueueHelper>();
            services.AddTransient<IMessageConsumer, MqConsumerHelper>();
            services.AddSingleton<ICacheManager, MemoryCacheManager>();

            services.AddAutoMapper(typeof(ConfigurationManager));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(typeof(BusinessStartup).GetTypeInfo().Assembly);

            ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) =>
            {
                return memberInfo.GetCustomAttribute<DisplayAttribute>()?.GetName();
            };
        }

        /// <summary>
        ///     This method gets called by the Development
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            ConfigureServices(services);
            services.AddTransient<IMlInfoModelRepository>(x=> new MlInfoModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.MlInfoModels));
            services.AddTransient<IUserRepository>(x =>
                new UserRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.Users));
            services.AddTransient<IOperationClaimRepository>(x =>
                new OperationClaimRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.OperationClaims));
            services.AddTransient<IClientModelRepository>(x =>
                new ClientModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.ClientModels));

            services.AddTransient<IMessageBroker, KafkaMessageBroker>();

            services.AddDbContext<ProjectDbContext, DArchInMemory>(ServiceLifetime.Transient);
            services.AddSingleton<MongoDbContextBase, MongoDbContext>();
        }

        /// <summary>
        ///     This method gets called by the Staging
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureStagingServices(IServiceCollection services)
        {
            ConfigureServices(services);
            services.AddTransient<IMlInfoModelRepository>(x=> new MlInfoModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.MlInfoModels));
            services.AddTransient<IUserRepository>(x =>
                new UserRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.Users));
            services.AddTransient<IOperationClaimRepository>(x =>
                new OperationClaimRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.OperationClaims));
            services.AddTransient<IClientModelRepository>(x =>
                new ClientModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.ClientModels));
            services.AddTransient<IMessageBroker, KafkaMessageBroker>();

            services.AddDbContext<ProjectDbContext>();

            services.AddSingleton<MongoDbContextBase, MongoDbContext>();
        }

        /// <summary>
        ///     This method gets called by the Production
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureProductionServices(IServiceCollection services)
        {
            ConfigureServices(services);
            services.AddTransient<IMlInfoModelRepository>(x=> new MlInfoModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.MlInfoModels));
            services.AddTransient<IUserRepository>(x =>
                new UserRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.Users));
            services.AddTransient<IOperationClaimRepository>(x =>
                new OperationClaimRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.OperationClaims));
            services.AddTransient<IClientModelRepository>(x =>
                new ClientModelRepository(x.GetRequiredService<MongoDbContextBase>(), Collections.ClientModels));
            services.AddTransient<IMessageBroker, KafkaMessageBroker>();

            services.AddDbContext<ProjectDbContext>();

            services.AddSingleton<MongoDbContextBase, MongoDbContext>();
        }

        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(
                new AutofacBusinessModule(new ConfigurationManager(Configuration, _hostEnvironment)));
        }
    }
}
