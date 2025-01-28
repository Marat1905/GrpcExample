namespace GrpcServerApp
{
    public class TimerHostService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimerHostService> _logger;
        private readonly HostModel _hostModel;
        private Timer? _timer = null;

        public TimerHostService(ILogger<TimerHostService> logger, HostModel hostModel)
        {
            _logger = logger;
            _hostModel = hostModel;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", count);
            _hostModel.Text = $"{DateTime.Now} - Счетчик: {count}";
        }

        public Task StopAsync(CancellationToken cancellationToken)
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
}
