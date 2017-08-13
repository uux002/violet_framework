using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unity 时间相关
/// </summary>
public class UnityTicker : BaseModule {

    private event Listener onUpdate;
    private event Listener onFixedUpdate;



    public void AddUpdateCallback(Listener _callback) {
        onUpdate += _callback;
    }

    public void RemoveUpdateCallback(Listener _callback) {
        
    }

    public void AddFixedUpdateCallback(Listener _callback) {
        onFixedUpdate += _callback;
    }

    public void RemoveFixedUpdateCallback(Listener _callback) {
        onFixedUpdate -= _callback;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (onUpdate != null) {
            onUpdate();
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (onFixedUpdate != null) {
            onFixedUpdate();
        }
    }
}
