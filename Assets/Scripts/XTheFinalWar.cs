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

        // Load AssetBundles
        await V.Instance.vBundle.LoadBundlesAsync();
        if(V.Instance.vBundle.moduleState == ENModuleState.Error) {
            Debug.LogError(V.Instance.vTable.ERROR_MSG);
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
