using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityTicker : VMonoSingleton<UnityTicker> {

    private event Listener onUpdate;
    private event Listener onFixedUpdate;


    public void AddUpdateCallback(Listener _callback) {
        onUpdate += _callback;
    }

    public void RemoveUpdateCallback(Listener _callback) {
        onUpdate -= _callback;
    }

    public void AddFixedUpdateCallback(Listener _callback) {
        onFixedUpdate += _callback;
    }

    public void RemoveFixedUpdateCallback(Listener _callback) {
        onFixedUpdate -= _callback;
    }

    private void Update() {
        if(onUpdate != null) {
            onUpdate();
        }
    }

    private void FixedUpdate() {
        if (onFixedUpdate != null) {
            onFixedUpdate();
        }
    }
}
