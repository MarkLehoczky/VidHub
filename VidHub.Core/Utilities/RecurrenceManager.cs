namespace VidHub.Core.Utilities
{
    public class RecurrenceManager : IDisposable
    {
        private readonly IList<Task> recurringTasks = [];


        public void Add(Action action, TimeSpan timeout)
        {
            Task task = Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Run(action);
                    await Task.Delay(timeout).ConfigureAwait(false);
                }
            });
            recurringTasks.Add(task);
        }

        public void Dispose()
        {
            foreach (Task task in recurringTasks)
            {
                task.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
