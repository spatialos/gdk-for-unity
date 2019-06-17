using System;
using System.Threading;
using System.Threading.Tasks;

namespace Improbable.Gdk.DeploymentLauncher.Commands
{
    /// <summary>
    ///     Wraps a task, the context in which it was started, and a cancellation source for that task together.
    /// </summary>
    /// <typeparam name="TResult">The type of the task result.</typeparam>
    /// <typeparam name="TContext">The context of the task.</typeparam>
    internal class WrappedTask<TResult, TContext> : IDisposable, IWrappedTask
    {
        public Task<TResult> Task;
        public CancellationTokenSource CancelSource;
        public TContext Context;

        public void Dispose()
        {
            Task?.Dispose();
            CancelSource?.Dispose();
        }

        public int GetId()
        {
            return Task?.Id ?? 0;
        }

        public bool IsDone()
        {
            return Task.IsCompleted;
        }

        public void Cancel()
        {
            CancelSource.Cancel();
        }

        public void Wait()
        {
            Task.Wait();
        }
    }

    /// <summary>
    ///     An interface that allows us to interact with a WrappedTask in a type erased way.
    /// </summary>
    internal interface IWrappedTask
    {
        int GetId();
        bool IsDone();
        void Cancel();
        void Wait();
    }
}
