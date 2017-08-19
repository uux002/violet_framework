using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Violet.Tasks;

public class XTheFinalWar : VMonoSingleton<XTheFinalWar> {

    public override void Initialize() {
        base.Initialize();
        InitGame();
    }


    private async void InitGame() {
        // Open Loading UI

        // 第一次运行游戏要解压资源包
        if (!PlayerPrefs.HasKey("XTHeFinalWar_FirstTimeRunGame")) {
            PlayerPrefs.SetInt("XTHeFinalWar_FirstTimeRunGame", 0);
            Debug.Log("解压资源");
            await V.Instance.vBundle.UnZipLocalBundleAsync();
            if (V.Instance.vBundle.bundleSystemState != EN_BundleSystemState.AllLocalBundleUnZipSuccess) {
                Debug.LogError("资源解压失败");
                return;
            }
        }

        // Load AssetBundles
        await V.Instance.vBundle.LoadBundlesAsync();
        if (V.Instance.vBundle.bundleSystemState != EN_BundleSystemState.AllRemoveBundleLoadSuccess) {
            Debug.LogError("资源更新失败");
            return;
        }


        // Load Config Tables
        await V.Instance.vTable.LoadTablesAsync(VioletConst.ConfigTableFilePath);
        if (V.Instance.vTable.moduleState == ENModuleState.Error) {
            Debug.LogError(V.Instance.vTable.ERROR_MSG);
            return;
        }


        // Init Game Models
    }

}
