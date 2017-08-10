﻿using System;
using System.Collections.Generic;

namespace Violet.Tasks {
    public class TaskChain_RunParallel_IgnoreError : TaskChain_Queue {

        private int taskCount = 0;
        private bool hasSuccess = false;

        ~TaskChain_RunParallel_IgnoreError() {
            this.Dispose();
        }

        public override void Dispose() {
            base.Dispose();
        }

        public override void StartTask() {
            taskCount = 0;
            hasSuccess = false;

            foreach (ITask task in this.taskList) {
                task.AddOnEnd(OnTaskEnd);
                task.StartTask();
            }

        }

        public override void StopTask() {
            foreach (ITask task in this.taskList) {
                task.RemoveOnEnd(OnTaskEnd);
                task.StopTask();
            }
        }

        private void OnTaskEnd(ITask _task) {
            ++taskCount;
            if (_task.isSuccess) {
                hasSuccess = true;
            }

            if (taskCount >= this.taskList.Count) {
                if (hasSuccess) {
                    this.FireOnEnd(true);
                } else {
                    this.FireOnEnd(false);
                }
            }
        }
    }
}
