using System;
using System.Collections.Generic;
using UnityEngine;

public class Task : ITask {
    public event Listener<ITask> onEnd;

    protected bool running = false;
    protected bool _isSuccess = false;

    public Task() {

    }

    ~Task() {
        this.Dispose();
    }

    public virtual void Dispose() {
        if(this.onEnd != null) {
            System.Delegate.RemoveAll(this.onEnd, this.onEnd);
        }
        this.onEnd = null;
    }

    public virtual void AddOnEnd(Listener<ITask> _onEnd) {
        this.onEnd += _onEnd;
    }

    public virtual void RemoveOnEnd(Listener<ITask> _onEnd) {
        this.onEnd -= _onEnd;
    }

    protected virtual void FireOnEnd(bool _isSuccess) {
        this.running = false;
        this._isSuccess = _isSuccess;
        if(this.onEnd != null) {
            this.onEnd(this);
        }
    }
    
    public virtual void StartTask() {
        this.running = true;
    }

    public virtual void StopTask() {
        this.running = false;
    }


    public bool isSuccess{
        get { return this._isSuccess; }
    }
}
