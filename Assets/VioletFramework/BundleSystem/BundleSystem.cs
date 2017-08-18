using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum EN_BundleSystemState {
    None,                       // 空闲
    OnUnZipLocalBundle,         // 正在解压本地 AssetBundle
    OnLoadingManifest,          // 正在加载 Manifest
    OnLoadingRemoveBundle,      // 正在加载远程 Bundle
    BundleSystemError,          // 错误
}

public class BundleSystem : BaseModule {

    private const string BUNDLE_MANIFEST = "AssetBundleManifest";

    private int _totalBundles = 0;
    private int _currBundleIndex = 0;
    private string _currBundleName = string.Empty;
    private EN_BundleSystemState _bundleSystemState = EN_BundleSystemState.None;
    private string _errorMsg = string.Empty;
    
    /// <summary>
    /// 当前 BundleSystem 状态
    /// </summary>
    public EN_BundleSystemState bundleSystemState {
        get { return _bundleSystemState; }
        private set { _bundleSystemState = value; }
    }

    /// <summary>
    /// 一共要加载多少个Bundle
    /// </summary>
    public int totalBundles {
        get { return _totalBundles; }
        private set { _totalBundles = value; }
    }

    /// <summary>
    /// 当前加载的 Bundle 索引
    /// </summary>
    public int currBundleIndex {
        get { return _currBundleIndex; }
        private set { _currBundleIndex = value; }
    }

    /// <summary>
    /// 正在加载的 Bundle 名字
    /// </summary>
    public string currBundleName {
        get { return _currBundleName; }
        private set { _currBundleName = value; }
    }

    /// <summary>
    /// 错误消息
    /// </summary>
    public string errorMsg {
        get { return _errorMsg; }
        private set { _errorMsg = value; }
    }

    private BundleLoader loader = new BundleLoader();


    private Dictionary<string, Hash128> bundleInfoDict = null;





    public async Task LoadBundlesAsync() {

        SetBundleSystemState(EN_BundleSystemState.OnLoadingManifest, 0, 0, BUNDLE_MANIFEST,"");

        // Load Bundle Manifest
        BundleLoadResult result = await loader.LoadBundleAsync(GetManifestUrl());

        if(result.state == BundleLoadState.Faild) {
            moduleState = ENModuleState.Error;
            ERROR_MSG = "AssetBundleManifest 加载失败";
            return;
        }

        AssetBundleManifest manifest = (AssetBundleManifest)result.bundle.LoadAsset(BUNDLE_MANIFEST);
        bundleInfoDict = GetAllBundleInfoFromManifest(manifest);

        totalBundles = bundleInfoDict.Count;
        int bundleIndex = 0;

        var enumer = bundleInfoDict.GetEnumerator();
        while (enumer.MoveNext()) {
            ++bundleIndex;
            string bundleName = enumer.Current.Key;
            Hash128 bundleHash = enumer.Current.Value;

            SetBundleSystemState(EN_BundleSystemState.OnLoadingRemoveBundle, totalBundles,  bundleName, "");
        }

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


    private void SetBundleSystemState(EN_BundleSystemState _state, int _totalBundle, int _currBundleIndex, string _currBundleName, string _errorMsg) {
        this.bundleSystemState = _state;
        this.totalBundles = _totalBundle;
        this.currBundleIndex = _currBundleIndex;
        this.currBundleName = _currBundleName;
        this.errorMsg = _errorMsg;
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

