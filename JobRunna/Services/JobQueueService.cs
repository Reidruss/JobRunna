using System.Threading.Channels;
using JobRunna.Interfaces;

namespace JobRunna.Services;

public sealed class JobQueueService : IQueueService
{
    private readonly Channel<Func<CancellationToken, ValueTask>> _queue;

    public JobQueueService(int cap)
    {
        BoundedChannelOptions boundedChannelOptions = new(cap)
        {
            FullMode = BoundedChannelFullMode.Wait,
        };
        _queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(boundedChannelOptions);
    }

    public async ValueTask QueueAsync(Func<CancellationToken, ValueTask> job)
    {
        ArgumentNullException.ThrowIfNull(job);
        
        await _queue.Writer.WriteAsync(job);
    }

    public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}