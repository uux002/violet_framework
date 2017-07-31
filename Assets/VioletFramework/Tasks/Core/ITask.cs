using System;
using System.Collections.Generic;
using UnityEngine;

public interface ITask : IDisposable {

    bool isSuccess{
        get;
    }

    void StartTask();
    void StopTask();

    void AddOnEnd(Listener<ITask> _onEnd);

    void RemoveOnEnd(Listener<ITask> _onEnd);

}
