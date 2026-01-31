using Microsoft.Extensions.Logging;
using VidHub.Platform.VidHubEnvironment;

namespace VidHub.Core.Utilities
{
    public class RecurrenceManager : IDisposable
    {
        private readonly IList<Task> recurringTasks = [];
        private readonly ILogger logger = VidHubContext.Logger;


        public void Add(Action action, TimeSpan timeout, string? tag = null)
        {
            logger.LogTrace("Adding recurring action with timeout {Timeout} tag={Tag}", timeout, tag);
            Task task = Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Run(action);
                    await Task.Delay(timeout).ConfigureAwait(false);
                }
            });
            recurringTasks.Add(task);
            logger.LogDebug("Recurring task added{TagInfo}, total count={Count}", tag is null ? string.Empty : $" ({tag})", recurringTasks.Count);
        }

        public void Dispose()
        {
            logger.LogTrace("Disposing RecurrenceManager with {Count} tasks", recurringTasks.Count);
            foreach (Task task in recurringTasks)
            {
                try
                {
                    task.Dispose();
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Failed to dispose recurring task");
                }
            }
            GC.SuppressFinalize(this);
            logger.LogDebug("RecurrenceManager disposed");
        }
    }
}
