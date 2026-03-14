using System.Threading.Tasks;
using NUnit.Framework;

namespace Tpl.Tests;

[TestFixture]
public class TaskStatusHandlerTest
{
    [Test]
    public void TaskCreated_WhenNewTaskIsCreated_ShouldBeInCreatedState()
    {
        var actual = TaskStatusHandler.CreateTaskWithCreatedStatus();
        Assert.That(actual.Status, Is.EqualTo(TaskStatus.Created));
        Assert.That(actual.IsCompleted, Is.False, "Created task should not be completed");
        Assert.That(actual.IsFaulted, Is.False, "Created task should not be faulted");
        Assert.That(actual.IsCanceled, Is.False, "Created task should not be canceled");
    }

    [Test]
    public void TaskWaitingForActivation_WhenTaskCompletionSourceIsCreated_ShouldBeInWaitingForActivationState()
    {
        var actual = TaskStatusHandler.CreateTaskWithWaitingForActivationStatus();
        Assert.That(actual.Status, Is.EqualTo(TaskStatus.WaitingForActivation));
        Assert.That(actual.IsCompleted, Is.False, "WaitingForActivation task should not be completed");
        Assert.That(actual.IsFaulted, Is.False, "WaitingForActivation task should not be faulted");
        Assert.That(actual.IsCanceled, Is.False, "WaitingForActivation task should not be canceled");
    }

    [Test]
    public void TaskWaitingToRun_WhenTaskIsScheduled_ShouldBeInWaitingToRunState()
    {
        var task = TaskStatusHandler.CreateTaskWithWaitingToRunStatus();
        Assert.That(
            task.Status,
            Is.EqualTo(TaskStatus.WaitingToRun),
            "Task should be in WaitingToRun state");
        Assert.That(
            task.IsCompleted,
            Is.False,
            "WaitingToRun task should not be completed");
        Assert.That(
            task.IsFaulted,
            Is.False,
            "WaitingToRun task should not be faulted");
        Assert.That(
            task.IsCanceled,
            Is.False,
            "WaitingToRun task should not be canceled");
    }

    [Test]
    public void TaskRunning_WhenTaskIsStarted_ShouldBeInRunningState()
    {
        var task = TaskStatusHandler.CreateTaskWithRunningStatus();
        Assert.That(
            task.Status,
            Is.EqualTo(TaskStatus.Running),
            "Task should be in Running state");
        Assert.That(
            task.IsCompleted,
            Is.False,
            "Running task should not be completed");
        Assert.That(
            task.IsFaulted,
            Is.False,
            "Running task should not be faulted");
        Assert.That(
            task.IsCanceled,
            Is.False,
            "Running task should not be canceled");
    }

    [Test]
    public void TaskRanToCompletion_WhenTaskIsCompleted_ShouldBeInRanToCompletionState()
    {
        var task = TaskStatusHandler.CreateTaskWithRanToCompletionStatus();
        Assert.That(
            task.Status,
            Is.EqualTo(TaskStatus.RanToCompletion),
            "Task should be in RanToCompletion state");
        Assert.That(
            task.IsCompleted,
            Is.True,
            "Task should be completed");
        Assert.That(
            task.IsFaulted,
            Is.False,
            "Task should not be faulted");
        Assert.That(
            task.IsCanceled,
            Is.False,
            "Task should not be canceled");
    }

    [Test]
    public void TaskWaitingForChildren_WhenParentTaskHasChildTasks_ShouldBeInWaitingForChildrenState()
    {
        var task = TaskStatusHandler.CreateTaskWithWaitingForChildrenToCompleteStatus();
        Assert.That(
            task.Status,
            Is.EqualTo(TaskStatus.WaitingForChildrenToComplete),
            "Task should be in WaitingForChildrenToComplete state");
        Assert.That(
            task.IsCompleted,
            Is.False,
            "Task should not be completed while waiting for children");
        Assert.That(
            task.IsFaulted,
            Is.False,
            "Task should not be faulted");
        Assert.That(
            task.IsCanceled,
            Is.False,
            "Task should not be canceled");
    }

    [Test]
    public void TaskIsCompleted_WhenTaskFinishes_ShouldBeInCompletedState()
    {
        var task = TaskStatusHandler.CreateTaskWithIsCompletedStatus();

        Assert.That(
            task.IsCompleted,
            Is.True,
            "Task should be completed");
        Assert.That(
            task.Status,
            Is.EqualTo(TaskStatus.RanToCompletion),
            "Task should be in RanToCompletion state");
        Assert.That(
            task.IsFaulted,
            Is.False,
            "Task should not be faulted");
        Assert.That(
            task.IsCanceled,
            Is.False,
            "Task should not be canceled");
    }

    [Test]
    public void TaskIsCancelled_WhenTaskIsCancelled_ShouldBeInCancelledState()
    {
        var task = TaskStatusHandler.CreateTaskWithIsCancelledStatus();
        Assert.That(
            task.IsCanceled,
            Is.True,
            "Task should be canceled");
        Assert.That(
            task.Status,
            Is.EqualTo(TaskStatus.Canceled),
            "Task should be in Canceled state");
        Assert.That(
            task.IsCompleted,
            Is.True,
            "Canceled task should be in completed state");
        Assert.That(
            task.IsFaulted,
            Is.False,
            "Task should not be faulted");
    }

    [Test]
    public void TaskIsFaulted_WhenTaskThrowsException_ShouldBeInFaultedState()
    {
        var task = TaskStatusHandler.CreateTaskWithIsFaultedStatus();
        Assert.That(
            task.IsFaulted,
            Is.True,
            "Task should be faulted");
        Assert.That(
            task.Status,
            Is.EqualTo(TaskStatus.Faulted),
            "Task should be in Faulted state");
        Assert.That(
            task.IsCompleted,
            Is.True,
            "Faulted task should be in completed state");
        Assert.That(
            task.IsCanceled,
            Is.False,
            "Faulted task should not be canceled");
    }
}
