using System;
using System.Collections.Generic;

namespace Violet.Tasks {
    public class TaskChain_RunOneByOne : TaskChain_Queue {
        private int currIndex = 0;

        ~TaskChain_RunOneByOne() {
            this.Dispose();
        }

        public override void Dispose() {
            base.Dispose();
        }


        public override void StartTask() {
            base.StartTask();

            currIndex = 0;

            if (this.taskList.Count > 0) {
                ITask task = taskList[currIndex];
                task.AddOnEnd(OnTaskEnd);
                task.StartTask();
            } else {
                this.FireOnEnd(true);
            }
        }


        public override void StopTask() {
            base.StopTask();
            foreach (ITask task in this.taskList) {
                task.RemoveOnEnd(OnTaskEnd);
                task.StopTask();
            }
        }


        private void OnTaskEnd(ITask _task) {
            _task.RemoveOnEnd(OnTaskEnd);
            if (_task.isSuccess) {
                ++this.currIndex;
                if (this.currIndex < this.taskList.Count) {
                    ITask task = taskList[currIndex];
                    task.AddOnEnd(OnTaskEnd);
                    task.StartTask();
                } else {
                    this.FireOnEnd(true);
                }
            } else {
                this.FireOnEnd(false);
            }
        }
    }
}
