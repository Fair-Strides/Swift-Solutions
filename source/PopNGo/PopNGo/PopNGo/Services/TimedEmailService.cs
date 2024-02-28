namespace PopNGo.Services;

public class TimedEmailService : IHostedService, IDisposable
{
    private int executionCount = 0;
    private readonly ILogger<TimedEmailService> _logger;
    private Timer? _timer = null;

    public TimedEmailService(ILogger<TimedEmailService> logger)
    {
        _logger = logger;
        _logger.LogInformation("Timed Hosted Service running.");

    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        DateTime current = DateTime.Now;
        DateTime next = new DateTime(current.Year, current.Month, current.Day, current.Hour, current.Minute, current.Second).AddMinutes(1);
        _timer = new Timer(DoWork, null, TimeSpan.FromSeconds((next - current).TotalSeconds), TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        var count = Interlocked.Increment(ref executionCount);

        _logger.LogInformation(
            "Timed Hosted Service is working. Count: {Count}, Time: {time}", count, DateTime.Now);
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}