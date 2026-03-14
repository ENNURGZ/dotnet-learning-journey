using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tpl;

public static class TaskStatusHandler
{
    public static Task CreateTaskWithCreatedStatus()
    {
        return new Task(() => { });
    }

    public static Task CreateTaskWithWaitingForActivationStatus()
    {
        var tcs = new TaskCompletionSource<int>();
        return tcs.Task;
    }

    public static Task CreateTaskWithWaitingToRunStatus()
    {
        var scheduler = new ConcurrentExclusiveSchedulerPair().ExclusiveScheduler;
        Task.Factory.StartNew(() => Thread.Sleep(200), CancellationToken.None, TaskCreationOptions.None, scheduler);
        var task = Task.Factory.StartNew(() => { }, CancellationToken.None, TaskCreationOptions.None, scheduler);

        // Ensure the task is definitely queued and WaitingToRun
        while (task.Status != TaskStatus.WaitingToRun)
        {
            Thread.Sleep(10);
        }

        return task;
    }

    public static Task CreateTaskWithRunningStatus()
    {
        var task = new Task(() => { Thread.Sleep(1000); });
        task.Start();
        while (task.Status != TaskStatus.Running)
        {
            Thread.Sleep(10);
        }

        return task;
    }

    public static Task CreateTaskWithRanToCompletionStatus()
    {
        var task = Task.Run(() => { });
        task.Wait();
        return task;
    }

    public static Task CreateTaskWithWaitingForChildrenToCompleteStatus()
    {
        var task = new Task(() =>
        {
            var child = new Task(() => Thread.Sleep(500), TaskCreationOptions.AttachedToParent);
            child.Start();
        });
        task.Start();
        while (task.Status != TaskStatus.WaitingForChildrenToComplete)
        {
            Thread.Sleep(10);
        }

        return task;
    }

    public static Task CreateTaskWithIsCompletedStatus()
    {
        var task = Task.Run(() => { });
        task.Wait();
        return task;
    }

    public static Task CreateTaskWithIsCancelledStatus()
    {
        using var cts = new CancellationTokenSource();
        var task = new Task(
            () =>
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    Thread.Sleep(10);
                }

                cts.Token.ThrowIfCancellationRequested();
            },
            cts.Token);

        task.Start();
        cts.Cancel();
        try
        {
            task.Wait();
        }
        catch (AggregateException)
        {
        }

        return task;
    }

    public static Task CreateTaskWithIsFaultedStatus()
    {
        var task = Task.Run(() => { throw new InvalidOperationException("Error"); });
        try
        {
            task.Wait();
        }
        catch (AggregateException)
        {
        }

        return task;
    }
}
