namespace JobRunna.Interfaces;

public interface IQueueService
{
    ValueTask QueueAsync(Func<CancellationToken, ValueTask> job);
    ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken);
}