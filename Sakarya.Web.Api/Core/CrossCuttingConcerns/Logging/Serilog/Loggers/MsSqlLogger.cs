using System;
using Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using Core.Utilities.IoC;
using Core.Utilities.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace Core.CrossCuttingConcerns.Logging.Serilog.Loggers
{
    public class MsSqlLogger : LoggerServiceBase
    {
        public MsSqlLogger()
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();

            var logConfig = configuration.GetSection("SeriLogConfigurations:MsSqlConfiguration")
                .Get<MsSqlConfiguration>() ?? throw new Exception(SerilogMessages.NullOptionsMessage);
            var sinkOpts = new MSSqlServerSinkOptions {TableName = "Logs", AutoCreateSqlTable = true};

            var seriLogConfig = new LoggerConfiguration()
                .WriteTo.MSSqlServer(logConfig.ConnectionString, sinkOpts)
                .CreateLogger();
            Logger = seriLogConfig;
        }
    }
}