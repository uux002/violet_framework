using System;
using System.Collections.Generic;

public class TaskChain : Task {
    private TaskChainType type = TaskChainType.RunOneByOne;

    private TaskChain_Queue taskQueue = null;

    public TaskChain(TaskChainType _chainType){
        type = _chainType;

        switch(type){
            case TaskChainType.RunOneByOne:
                {
                    taskQueue = new TaskChain_RunOneByOne();
                }
                break;
            case TaskChainType.RunOneByOneIgnoreError:
                {
                    taskQueue = new TaskChain_RunOneByOne_IgnoreError();
                }
                break;
            case TaskChainType.RunParallel:
                {
                    taskQueue = new TaskChain_RunParallel();
                }
                break;
            case TaskChainType.RunParallelIgnoreError:
                {
                    taskQueue = new TaskChain_RunParallel_IgnoreError();
                }
                break;
        }

        taskQueue.AddOnEnd(OnTaskChainEnd);
    }

    ~TaskChain(){
        this.Dispose();
    }

    public override void Dispose(){
        base.Dispose();
        taskQueue.RemoveOnEnd(OnTaskChainEnd);
        taskQueue.Dispose();
        taskQueue = null;
    }

    public override void StartTask()
    {
        base.StartTask();
        taskQueue.StartTask();
    }

    public override void StopTask(){
        base.StartTask();
        taskQueue.RemoveOnEnd(OnTaskChainEnd);
        taskQueue.StopTask();
    }

    public void AddTask(ITask _task){
        taskQueue.AddTask(_task);
    }

    public void RemoveTask(ITask _task){
        taskQueue.RemoveTask(_task);
    }

    private void OnTaskChainEnd(ITask _task){
        _task.RemoveOnEnd(OnTaskChainEnd);
        this.FireOnEnd(_task.isSuccess);
    }
}
