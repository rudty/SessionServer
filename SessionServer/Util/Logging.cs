using System;
using Microsoft.Extensions.Logging;

namespace SessionServer.Util {
    public static class Logging {
        public static void C(this ILogger logger, EventId eventId, Exception exception, string message, params object[] args) => logger.LogCritical(eventId, exception, message, args);
        public static void C(this ILogger logger, EventId eventId, string message, params object[] args) => logger.LogCritical(eventId, message, args);
        public static void C(this ILogger logger, Exception exception, string message, params object[] args) => logger.LogCritical(exception, message, args);
        public static void C(this ILogger logger, string message, params object[] args) => logger.LogCritical(message, args);
        public static void D(this ILogger logger, EventId eventId, Exception exception, string message, params object[] args) => logger.LogDebug(eventId, exception, message, args);
        public static void D(this ILogger logger, EventId eventId, string message, params object[] args) => logger.LogDebug(eventId, message, args);
        public static void D(this ILogger logger, Exception exception, string message, params object[] args) => logger.LogDebug(exception, message, args);
        public static void D(this ILogger logger, string message, params object[] args) => logger.LogDebug(message, args);
        public static void E(this ILogger logger, EventId eventId, Exception exception, string message, params object[] args) => logger.LogError(eventId, exception, message, args);
        public static void E(this ILogger logger, EventId eventId, string message, params object[] args) => logger.LogError(eventId, message, args);
        public static void E(this ILogger logger, Exception exception, string message, params object[] args) => logger.LogError(exception, message, args);
        public static void E(this ILogger logger, string message, params object[] args) => logger.LogError(message, args);
        public static void I(this ILogger logger, EventId eventId, Exception exception, string message, params object[] args) => logger.LogInformation(eventId, exception, message, args);
        public static void I(this ILogger logger, EventId eventId, string message, params object[] args) => logger.LogInformation(eventId, message, args);
        public static void I(this ILogger logger, Exception exception, string message, params object[] args) => logger.LogInformation(exception, message, args);
        public static void I(this ILogger logger, string message, params object[] args) => logger.LogInformation(message, args);
        public static void W(this ILogger logger, EventId eventId, Exception exception, string message, params object[] args) => logger.LogWarning(eventId, exception, message, args);
        public static void W(this ILogger logger, EventId eventId, string message, params object[] args) => logger.LogWarning(eventId, message, args);
        public static void W(this ILogger logger, Exception exception, string message, params object[] args) => logger.LogWarning(exception, message, args);
        public static void W(this ILogger logger, string message, params object[] args) => logger.LogWarning(message, args);
    }
}
