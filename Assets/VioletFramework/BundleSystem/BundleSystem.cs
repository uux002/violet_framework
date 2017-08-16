using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BundleSystem : BaseModule {

    private const string BUNDLE_MANIFEST = "AssetBundleManifest";

    /// <summary>
    /// 一共要加载多少个Bundle
    /// </summary>
    public int totalBundles = 0;

    /// <summary>
    /// 当前加载的 Bundle 索引
    /// </summary>
    public int currLoadingBundleIndex = 0;

    /// <summary>
    /// 正在加载的 Bundle 名字
    /// </summary>
    public string currLoadingBundleName = string.Empty;


    private BundleLoader loader = new BundleLoader();

    public async Task LoadBundlesAsync() {
        // Load Bundle Manifest
        BundleLoadResult result = await loader.LoadBundleAsync(GetManifestUrl());

        if(result.state == BundleLoadState.Faild) {
            moduleState = ENModuleState.Error;
            ERROR_MSG = "AssetBundleManifest 加载失败";
            return;
        }

        AssetBundleManifest manifest = (AssetBundleManifest)result.bundle.LoadAsset(BUNDLE_MANIFEST);
    }





    /// <summary>
    /// 从 Manifest 中获取所有的 Bundle 名字和 Bundle Hash128
    /// </summary>
    /// <param name="_manifest"></param>
    /// <returns></returns>
    private Dictionary<string, Hash128> GetAllBundleInfoFromManifest(AssetBundleManifest _manifest) {
        Dictionary<string, Hash128> bundleInfoDict = new Dictionary<string, Hash128>();
        string[] allBundleNames = _manifest.GetAllAssetBundles();
        for(int i = 0; i < allBundleNames.Length; ++i) {
            string bundleName = allBundleNames[i];
            Hash128 bundleHash = _manifest.GetAssetBundleHash(bundleName);
            bundleInfoDict.Add(bundleName, bundleHash);
        }

        return bundleInfoDict;
    }



    private string GetBundleUrl(string _bundleName) {
        return VioletConst.URL_BUNDLE + GetBundleRootByPlatform() + "/" + _bundleName;
    }

    private string GetManifestUrl() {
        return VioletConst.URL_BUNDLE + GetBundleRootByPlatform() + "/" + GetBundleRootByPlatform();
    }

    private string GetBundleRootByPlatform() {
        switch (Application.platform) {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer: {
                    return "Windows";
                }
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer: {
                    return "OSX";
                }
            case RuntimePlatform.Android: {
                    return "Android";
                }

            case RuntimePlatform.IPhonePlayer: {
                    return "iOS";
                }
        }

        return "";
    }
}

