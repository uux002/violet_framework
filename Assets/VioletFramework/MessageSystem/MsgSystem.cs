using com.ootii.Messages;

/// <summary>
/// 注册消息监听，发送消息
/// NOTE: 引用了第三方代码 - Event System - Dispatcher
/// </summary>
public static class MsgSystem {

    /// <summary>
    /// 添加监听器
    /// </summary>
    /// <param name="_rMessageType"></param>
    /// <param name="_rHandler"></param>
    public static void AddListener(string _rMessageType, MessageHandler _rHandler) {
        MessageDispatcher.AddListener(_rMessageType, _rHandler, true);
    }

    /// <summary>
    /// 移除监听器
    /// </summary>
    /// <param name="_rMessageType"></param>
    /// <param name="_rHandler"></param>
    public static void RemoveListener(string _rMessageType, MessageHandler _rHandler) {
        MessageDispatcher.RemoveListener(_rMessageType, _rHandler, true);
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="_rType"></param>
    /// <param name="_rDelay"></param>
    public static void SendMessage(string _rType, float _rDelay = 0f) {
        MessageDispatcher.SendMessage(_rType, _rDelay);
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="_rType"></param>
    /// <param name="_rData"></param>
    /// <param name="_rDelay"></param>
    public static void SendMessage(string _rType, object _rData, float _rDelay = 0f) {
        MessageDispatcher.SendMessageData(_rType, _rData, _rDelay);
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="_rSender"></param>
    /// <param name="_rType"></param>
    /// <param name="_rDelay"></param>
    public static void SendMessage(string _rSender, string _rType, float _rDelay = 0f) {
        MessageDispatcher.SendMessage(_rSender, _rType, _rDelay);
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="_rSender"></param>
    /// <param name="_rType"></param>
    /// <param name="_rData"></param>
    /// <param name="_rDelay"></param>
    public static void SendMessage(string _rSender, string _rType, object _rData, float _rDelay = 0f) {
        MessageDispatcher.SendMessage(_rSender, _rType, _rData, _rDelay);
    }
    
}
