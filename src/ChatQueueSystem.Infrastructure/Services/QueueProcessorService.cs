namespace ChatQueueSystem.Infrastructure.Services;

public class QueueProcessorService : BackgroundService
{
    private readonly IQueueService _queueService;
    private readonly ILogger<QueueProcessorService> _logger;

    public QueueProcessorService(IQueueService queueService, ILogger<QueueProcessorService> logger)
    {
        _queueService = queueService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Queue Processor Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _queueService.ProcessQueueAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing queue");
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}