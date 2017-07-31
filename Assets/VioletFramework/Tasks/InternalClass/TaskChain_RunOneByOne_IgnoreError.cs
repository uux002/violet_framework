using System;
using System.Collections.Generic;

public class TaskChain_RunOneByOne_IgnoreError : TaskChain_Queue {
    private int currIndex = 0;

    private bool hasSuccess = false;

    ~TaskChain_RunOneByOne_IgnoreError(){
        this.Dispose();
    }

    public override void Dispose(){
        base.Dispose();
    }


    public override void StartTask() {
        base.StartTask();

        currIndex = 0;
        hasSuccess = false;

        if (this.taskList.Count > 0)
        {
            ITask task = taskList[currIndex];
            task.AddOnEnd(OnTaskEnd);
            task.StartTask();
        }
        else{
            this.FireOnEnd(false);
        }
    }


    public override void StopTask(){
        base.StopTask();
        foreach(ITask task in this.taskList){
            task.RemoveOnEnd(OnTaskEnd);
            task.StopTask();
        }
    }


    private void OnTaskEnd(ITask _task){
		++this.currIndex;

        _task.RemoveOnEnd(OnTaskEnd);

        if(_task.isSuccess){
            hasSuccess = true;
        }

        if(this.currIndex >= this.taskList.Count){
            if(hasSuccess){
                this.FireOnEnd(true);
            }
            else{
                this.FireOnEnd(false);
            }
        }
    }
}
