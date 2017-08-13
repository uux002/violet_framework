using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XTheFinalWar : VMonoSingleton<XTheFinalWar> {

    public override void Initialize() {
        base.Initialize();
        InitGame();
    }


    private async void InitGame() {
        // Open Loading UI
        // Load AssetBundles

        // Load Config Tables
        await V.Instance.vTable.AsyncLoadTables(VioletConst.ConfigTableFilePath);
        //await V.Instance.vTable.AsyncLoadTables("http://47.94.154.110/ab/OSX/cube1");
        if (V.Instance.vTable.moduleState == ENModuleState.Error) {
            Debug.LogError(V.Instance.vTable.ERROR_MSG);
            return;
        }


        // Init Game Models
    }
}
