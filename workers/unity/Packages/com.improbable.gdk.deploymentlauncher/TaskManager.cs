using System;
using System.Collections.Generic;
using Improbable.Gdk.DeploymentLauncher.Commands;
using UnityEditor;

namespace Improbable.Gdk.DeploymentLauncher
{
    internal class TaskManager
    {
        public enum QueueMode
        {
            Enqueue,
            RunNext
        }

        public bool IsActive => CurrentTask != null;
        public IWrappedTask CurrentTask { get; private set; }
        public readonly List<IWrappedTask> CompletedTasks = new List<IWrappedTask>();

        private List<Func<IWrappedTask>> queuedTasks = new List<Func<IWrappedTask>>();
        private bool isLocked;
        private Func<IWrappedTask> queuedAuthTask;

        public void ClearResults()
        {
            CompletedTasks.Clear();
        }

        public void Cancel()
        {
            CurrentTask?.Cancel();
            queuedTasks.Clear();
        }

        public bool CancelCurrentTask(int originalTaskId)
        {
            if (CurrentTask == null)
            {
                return false;
            }

            if (originalTaskId != CurrentTask.GetId() || CurrentTask.IsDone())
            {
                return false;
            }

            CurrentTask?.Cancel();
            CurrentTask = null;
            return true;
        }

        public void Upload(AssemblyConfig config, QueueMode mode = QueueMode.Enqueue)
        {
            AddTask(mode, () => Assembly.UploadAsync(config));
        }

        public void Launch(string projectName, string assemblyName, BaseDeploymentConfig config,
            QueueMode mode = QueueMode.Enqueue)
        {
            AddTask(mode, () => Deployment.LaunchAsync(projectName, assemblyName, config));
        }

        public void List(string projectName, QueueMode mode = QueueMode.Enqueue)
        {
            AddTask(mode, () => Deployment.ListAsync(projectName));
        }

        public void Stop(DeploymentInfo info, QueueMode mode = QueueMode.Enqueue)
        {
            AddTask(mode, () => Deployment.StopAsync(info));
        }

        public void Auth()
        {
            queuedAuthTask = Authentication.Authenticate;
        }

        public void Update()
        {
            if (!IsActive)
            {
                var hasStartedTask = false;

                // Always prefer the auth task over any queued tasks.
                if (queuedAuthTask != null)
                {
                    CurrentTask = queuedAuthTask();
                    queuedAuthTask = null;
                    hasStartedTask = true;
                }
                else if (queuedTasks.Count > 0)
                {
                    var queuedTask = queuedTasks[0];
                    queuedTasks.RemoveAt(0);

                    CurrentTask = queuedTask();
                    hasStartedTask = true;
                }

                if (hasStartedTask)
                {
                    if (!isLocked)
                    {
                        EditorApplication.LockReloadAssemblies();
                        isLocked = true;
                    }
                }
                else if (isLocked)
                {
                    EditorApplication.UnlockReloadAssemblies();
                    isLocked = false;
                }
            }
            else
            {
                if (CurrentTask.IsDone())
                {
                    CompletedTasks.Add(CurrentTask);
                    CurrentTask = null;
                }
            }
        }

        private void AddTask(QueueMode mode, Func<IWrappedTask> closure)
        {
            switch (mode)
            {
                case QueueMode.Enqueue:
                    queuedTasks.Add(closure);
                    break;
                case QueueMode.RunNext:
                    queuedTasks.Insert(0, closure);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
    }
}
