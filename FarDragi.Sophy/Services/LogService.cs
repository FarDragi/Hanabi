using Discord;
using FarDragi.Sophy.Services.Interfaces;
using Lina.DynamicServicesProvider.Attributes;
using Microsoft.Extensions.Logging;

namespace FarDragi.Sophy.Services;

[Service(typeof(ILogService))]
public class LogService : ILogService 
{
    private readonly ILogger<LogService> _logger;

    public LogService(ILogger<LogService> logger)
    {
        _logger = logger;
    }

    public void Log(LogMessage message)
    {
        if (message.Exception is not null)
        {
            if(message.Exception.Message.Contains("Expected SocketInteractionContext`1, got SocketInteractionContext"))
                return;
            
            _logger.LogError(message.Exception, "{}", message.Exception);
            return;
        }

        switch (message.Severity)
        {
            case LogSeverity.Info:
                _logger.LogInformation("{} - {}", message.Source, message.Message);
                break;
            case LogSeverity.Critical:
            case LogSeverity.Error:
            case LogSeverity.Warning:
            case LogSeverity.Verbose:
            case LogSeverity.Debug:
            default:
                _logger.LogWarning("{} - {}", message.Source, message.Message);
                break;
        }
    }
}