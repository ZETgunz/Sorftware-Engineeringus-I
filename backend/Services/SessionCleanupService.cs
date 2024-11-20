using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

public class SessionCleanupService : BackgroundService
{
    private readonly SessionManager _sessionManager;
    private readonly TimeSpan _cleanupInterval = TimeSpan.FromMinutes(10);

    public SessionCleanupService(SessionManager sessionManager)
    {
        _sessionManager = sessionManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _sessionManager.CleanupExpiredSessions();
            await Task.Delay(_cleanupInterval, stoppingToken);
        }
    }
}