using System;
using U3D.Threading.Tasks;

/// <summary>
/// 线程桥
/// NOTE: 引用了第三方代码 Arklay
/// </summary>
public static class ThreadBridge {

    /// <summary>
    /// 在主线程中执行
    /// </summary>
    /// <param name="_action"></param>
    public static void RunInMain(Action _action) {
        Task.RunInMainThread(_action);
    }
}
