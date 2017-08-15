using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VioletConst {
   
    /// <summary>
    /// 服务器地址
    /// </summary>
    public const string IP = "127.0.0.1";
    /// <summary>
    /// 服务器端口
    /// </summary>
    public const int PORT = 9999;
    /// <summary>
    /// 唯一标识
    /// </summary>
    public static ulong GUID = ulong.MaxValue;


    /// <summary>
    /// 本地资源配置(用于本地调试加载和打Bundle)
    /// </summary>
    public const string LocalResPath = "Assets/Res";

    /// <summary>
    /// 所有CSV配置表打成的总文件路径，或者直接改成网络文件路径
    /// </summary>
    public readonly static string ConfigTableFilePath = Application.streamingAssetsPath + "/ConfigTable.byte";


    // ------------------------------> AssetBundle 配置
    /// <summary>
    /// 调试设置 - 编辑器下使用Bundle加载
    /// </summary>
    public bool DEBUG_useBundle = false;

    /// <summary>
    /// AssetBundle更新地址
    /// </summary>
    public const string URL_BUNDLE = "http://127.0.0.1/ab/";
   
}
