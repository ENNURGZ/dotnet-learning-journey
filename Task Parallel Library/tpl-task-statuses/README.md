# Task Parallel Library (TPL) Practice

This task helps in understanding different states and behaviors of tasks in .NET's Task Parallel Library (TPL).

Estimated time: 1h.   

Requirements: .NET 8 SDK.  

## Task Description

Implement 9 static methods in the `TaskStatusHandler` class, each demonstrating a specific task state or behavior.

## Task States

### 1. Created State
_Method_: `CreateTaskWithCreatedStatus`.  
_Description_: Create a task that hasn't been scheduled for execution yet.  
_Implementation_:
- Use `new Task()` constructor.  
- Return the task without starting it.  
- Verify task is in `Created` state.  

### 2. WaitingForActivation State
_Method_: `CreateTaskWithWaitingForActivationStatus`.  
_Description_: Create a task that's waiting for an external trigger to start.   
_Implementation_:
- Use `TaskCompletionSource`.  
- Return the task without completing it.   
- Verify task is in `WaitingForActivation` state.   

### 3. WaitingToRun State
_Method_: `CreateTaskWithWaitingToRunStatus`.    
_Description_: Create a task that's scheduled but waiting for a thread to execute.  
_Implementation_:  
- Use `Task.Factory.StartNew`.  
- Configure appropriate scheduling options.  
- Return the task before it starts running.  
- Verify task is in `WaitingToRun` state.  

### 4. Running State
_Method_: `CreateTaskWithRunningStatus`.    
_Description_: Create a task that's actively executing.     
_Implementation_:   
- Create a task with significant work.   
- Ensure task is running when returned.   
- Verify task is in `Running` state.   
  
### 5. RanToCompletion State. 
_Method_: `CreateTaskWithRanToCompletionStatus`.    
_Description_: Create a task that has completed successfully.     
_Implementation_:     
- Create and start a task. 
- Wait for completion. 
- Return the completed task. 
- Verify task is in `RanToCompletion` state. 

### 6. WaitingForChildrenToComplete State
_Method_: `CreateTaskWithWaitingForChildrenToCompleteStatus`.    
_Description_: Create a parent task waiting for its child tasks to complete.     
_Implementation_:  
- Create a parent task with attached child tasks. 
- Complete parent before children. 
- Return the parent task. 
- Verify task is in `WaitingForChildrenToComplete` state. 

## Task Completion States

### 7. IsCompleted
_Method_: `CreateTaskWithIsCompletedStatus`.   
_Description_: Create a task that has finished execution.  
_Implementation_:
- Create and complete a task. 
- Return the task.  
- Verify `IsCompleted` is true.  
  
### 8. IsCancelled
_Method_: `CreateTaskWithIsCancelledStatus`.  
_Description_: Create a task that was cancelled during execution.    
_Implementation_:  
- Create a task with `CancellationToken`. 
- Cancel the task during execution.  
- Handle cancellation properly.  
- Verify `IsCanceled` is true.  

### 9. IsFaulted
_Method_: `CreateTaskWithIsFaultedStatus`.  
_Description_: Create a task that failed due to an exception.   
_Implementation_:   
- Create a task that throws an exception.  
- Handle the exception properly.  
- Return the faulted task.  
- Verify `IsFaulted` is true.  
