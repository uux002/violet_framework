using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskChain_Queue : Task {
    protected List<ITask> taskList;

    public TaskChain_Queue() {
        taskList = new List<ITask>();
    }

    ~TaskChain_Queue() {

    }

    public override void Dispose() {
        base.Dispose();
        if(this.taskList != null) {
            for(int i = 0; i < taskList.Count; ++i) {
                taskList[i].Dispose();
            }
            taskList.Clear();
        }

        taskList = null;
    }

    public void AddTask(ITask _task) {
        taskList.Add(_task);
    }

    public void RemoveTask(ITask _task) {
        if (taskList.Contains(_task)) {
            taskList.Remove(_task);
        }
    }
}


