using System;
using System.Collections.Generic;
using UnityEngine;

public class VioletInit {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnBeforeSceneLoadRuntimeMethod() {
        OnBefore();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnAfterSceneLoadRuntimeMethod() {
        OnAfter();
    }

    private static void OnBefore() {
        BeforeAwake();
    }

    private static void OnAfter() {
        AfterStart();
    }

    private static void BeforeAwake() {
        Vlog.disable = true;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        V.Instance.Initialize();
        
    }

    private static void AfterStart() {
        XTheFinalWar.Instance.Initialize();
    }
}
