using System;
using System.Runtime.CompilerServices;
using Serilog;
using Serilog.Events;

namespace SerilogIntro
{
    class Program
    {
        static void Main(string[] args)
        {
            // ref: http://nblumhardt.com/2013/03/serilog/
            
            const string logTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{MachineName}, {ProcessId}, {ThreadId}] [{Level}] {Message}{NewLine}{Exception}";
            var logger = new LoggerConfiguration()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .MinimumLevel.Is(LogEventLevel.Verbose)
                .WriteTo.ColoredConsole(outputTemplate: logTemplate)
                .WriteTo.Elasticsearch()
                .CreateLogger();

            Log(logger);
            LogForHere(logger);

            Console.ReadLine();
        }

        static void Log(ILogger logger)
        {
            const int elapsedMs = 34;
            var position = new { Latitude = 25, Longitude = 134 };

            logger.ForContext<Program>().Debug("Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);
            logger.ForContext<Program>().Verbose("Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);
            logger.ForContext<Program>().Fatal("Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);
            logger.ForContext<Program>().Information("Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);
            logger.ForContext<Program>().Warning("Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);

            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                logger.ForContext<Program>().Error(ex, "Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);
            }
        }

        static void LogForHere(ILogger logger)
        {
            const int elapsedMs = 34;
            var position = new { Latitude = 25, Longitude = 134 };

            logger.Here().ForContext<Program>().Debug("Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);
            logger.Here().ForContext<Program>().Verbose("Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);
            logger.Here().ForContext<Program>().Fatal("Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);
            logger.Here().ForContext<Program>().Information("Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);
            logger.Here().ForContext<Program>().Warning("Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);

            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                logger.Here().ForContext<Program>().Error(ex, "Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);
            }
        }
    }

    /// <summary>
    /// See: https://gist.github.com/cbdabner/b2c921a3a6da05cdb946
    /// </summary>
    public static class LoggerExtensions
    {
        public static ILogger Here(
            this ILogger logger,
            [CallerFilePath] string callerFilePath = null,
            [CallerMemberName] string callerMemberName = null,
            [CallerLineNumber] int callerLineNumber = 0)
        {
            return logger
                .ForContext("SourceFile", callerFilePath)
                .ForContext("SourceMember", callerMemberName)
                .ForContext("SourceLine", callerLineNumber);
        }
    }
}
