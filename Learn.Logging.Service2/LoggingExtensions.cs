using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Learn.Logging.Service2
{
    public static class LoggingExtensions
    {
        public static async Task WithLogging<T>(this Task task,
            ILogger logger,
            string msg = "",
            object data = null,
            [CallerMemberName] string method = "",
            [CallerFilePath] string srcFilePath = "",
            [CallerLineNumber] int srcLineNumber = 0)
        {
            var source = new { method, srcFilePath, srcLineNumber };

            try
            {
                logger.LogDebug($"Start: {msg} {{@Data}} from {{@Source}}", data, source);

                await task;

                logger.LogInformation($"Done: {msg} {{@Data}} from {{@Source}}", data, source);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error: {msg} {{@Data}} from {{@Source}}", data, source);
                throw;
            }
        }

        public static async Task<T> WithLogging<T>(this Task<T> task,
            ILogger logger,
            string msg = "",
            object data = null,
            [CallerMemberName] string method = "",
            [CallerFilePath] string srcFilePath = "",
            [CallerLineNumber] int srcLineNumber = 0)
        {
            var source = new { method, srcFilePath, srcLineNumber };

            try
            {
                logger.LogDebug($"Start: {msg} {{@Data}} from {{@Source}}", data, source);

                var result = await task;

                logger.LogInformation($"Done: {msg} {{@Data}} from {{@Source}}", data, source);
                return result;
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error: {msg} {{@Data}} from {{@Source}}", data, source);
                throw;
            }
        }
    }
}