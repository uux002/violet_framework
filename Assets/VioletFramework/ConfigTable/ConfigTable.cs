using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;

/// <summary>
/// 异步加载所有配置表，
/// 最好在AssetBundle加载完后，开始加载配置表数据，可以从文件加载，也可以从AssetBundle里的二进制数据加载
/// public async void AfterAssetBundleLoaded() {
///     await AsyncLoadTables(VioletConst.ConfigTableFilePath);
///     Debug.Log("Config Table Loaded!");
/// }
/// </summary>
public class ConfigTable : BaseModule {

    /// <summary>
    /// 异步从文件加载配表数据
    /// </summary>
    /// <param name="_tableFilePath"></param>
    /// <returns></returns>
    public async Task AsyncLoadTables(string _tableFilePath) {
        UnityWebRequest request = UnityWebRequest.Get(_tableFilePath);
        await request.Send();

        if (request.isNetworkError) {
            moduleState = ENModuleState.Error;
            ERROR_MSG = "配置表加载失败";
        } else {
            byte[] confBytes = request.downloadHandler.data;
            await AsyncLoadTables(confBytes);
        }
    }

    /// <summary>
    /// 异步从二进制数据加载配表数据
    /// </summary>
    /// <param name="_tableData"></param>
    /// <returns></returns>
    public async Task AsyncLoadTables(byte[] _tableData) {
        Task t1 = new Task(() => { InitConfigTables(_tableData); });
        t1.Start();
        await t1;
    }

    /// <summary>
    /// 同步从文件加载配表数据
    /// </summary>
    /// <param name="_tableFilePath"></param>
    public async void LoadTables(string _tableFilePath) {
        UnityWebRequest request = UnityWebRequest.Get(_tableFilePath);
        
        await request.Send();
        if (request.isNetworkError) {
            moduleState = ENModuleState.Error;
            ERROR_MSG = "配置表加载失败";
        } else {
            byte[] confBytes = request.downloadHandler.data;
            LoadTables(confBytes);
        }
    }


    /// <summary>
    /// 同步从二进制数据加载配表数据
    /// </summary>
    /// <param name="_tableData"></param>
    public void LoadTables(byte[] _tableData) {
        InitConfigTables(_tableData);
    }


    private void InitConfigTables(byte[] _tableData) {

    }
}
