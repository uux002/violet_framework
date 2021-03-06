﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum EN_BundleSystemState {
    None,                       // 空闲
    OnUnZipLocalBundle,         // 正在解压本地 AssetBundle
    OnLoadingManifest,          // 正在加载 Manifest
    OnLoadingRemoveBundle,      // 正在加载远程 Bundle

    AllLocalBundleUnZipSuccess, // 所有本地Bundle解压成功
    AllRemoveBundleLoadSuccess, // 所有远程Bundle下载成功

    Error_UnZipLocalBundle,     // 解压本地Bundle出错
    Error_LoadLocalManifest,    // 加载本地Manifest出错
    Error_LoadManifest,         // 加载远程Manifest出错
    Error_LoadRemoveBundle,     // 加载远程Bundle出错
}

public class BundleSystem : BaseModule {

    private const string BUNDLE_MANIFEST = "AssetBundleManifest";

    private int _totalBundles = 0;
    private int _currBundleIndex = 0;
    private string _currBundleName = string.Empty;
    private EN_BundleSystemState _bundleSystemState = EN_BundleSystemState.None;
    
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

    public int progress {
        get {
            if (loader.isDown) {
                return 100;
            } else {
                int progress = (int)(loader.progress * 100);
                progress = Mathf.Clamp(progress, 0, 100);
                return progress;
            }
        }
    }

    private BundleLoader loader = new BundleLoader();

    private Dictionary<string, Hash128> bundleInfoDict = null;

    /// <summary>
    /// 已经加载完的AssetBundle
    /// </summary>
    private Dictionary<string, AssetBundle> loadedBundleDict = new Dictionary<string, AssetBundle>();

    /// <summary>
    /// 哪一个资源在哪个Bundle里
    /// </summary>
    private Dictionary<string, string> assetToBundleDict = new Dictionary<string, string>();

    /// <summary>
    /// 已经到内存中的资源
    /// </summary>
    private Dictionary<string, UnityEngine.Object> loadedAssetDict = new Dictionary<string, UnityEngine.Object>();

    /// <summary>
    /// 资源请求回调，如果有多个异步请求同一个资源，则只会有一个加载
    /// </summary>
    private Dictionary<string, Listener<UnityEngine.Object>> assetRequestCallbackDict = new Dictionary<string, Listener<UnityEngine.Object>>();

    /// <summary>
    /// 异步解压本地 AssetBundle
    /// </summary>
    /// <returns></returns>
    public async Task UnZipLocalBundleAsync() {
        bundleSystemState = EN_BundleSystemState.OnUnZipLocalBundle;
        totalBundles = 0;
        currBundleIndex = 0;
        currBundleName = BUNDLE_MANIFEST;

        BundleLoadResult manifestLoadResult = await loader.LoadBundleAsync(GetLocalManifestUrl());
        if(manifestLoadResult.state == BundleLoadState.Faild) {
            bundleSystemState = EN_BundleSystemState.Error_LoadLocalManifest;
            return;
        }

        AssetBundleManifest manifest = (AssetBundleManifest)manifestLoadResult.bundle.LoadAsset(BUNDLE_MANIFEST);
        Dictionary<string, Hash128> localBundleInfo = GetAllBundleInfoFromManifest(manifest);

        manifestLoadResult.bundle.Unload(true);

        totalBundles = localBundleInfo.Count;
        int bundleIndex = 0;

        var enumer = localBundleInfo.GetEnumerator();
        while (enumer.MoveNext()) {
            ++bundleIndex;
            string bundleName = enumer.Current.Key;
            Hash128 bundleHash = enumer.Current.Value;

            bundleSystemState = EN_BundleSystemState.OnUnZipLocalBundle;
            currBundleIndex = bundleIndex;
            currBundleName = bundleName;

            BundleLoadResult localBundleResult = await loader.LoadBundleAsync(GetLocalBundleUrl(bundleName),bundleHash);
            if(localBundleResult.state == BundleLoadState.Faild) {
                bundleSystemState = EN_BundleSystemState.Error_UnZipLocalBundle;
                return;
            } else {
                localBundleResult.bundle.Unload(true);
            }
        }

        bundleSystemState = EN_BundleSystemState.AllLocalBundleUnZipSuccess;
    }

    /// <summary>
    /// 异步加载 AssetBundle
    /// </summary>
    /// <returns></returns>
    public async Task LoadBundlesAsync() {
        bundleSystemState = EN_BundleSystemState.OnLoadingManifest;
        totalBundles = 0;
        currBundleIndex = 0;
        currBundleName = BUNDLE_MANIFEST;

        // Load Bundle Manifest
        BundleLoadResult manifestLoadResult = await loader.LoadBundleAsync(GetManifestUrl());

        if(manifestLoadResult.state == BundleLoadState.Faild) {
            bundleSystemState = EN_BundleSystemState.Error_LoadManifest;
            return;
        }

        AssetBundleManifest manifest = (AssetBundleManifest)manifestLoadResult.bundle.LoadAsset(BUNDLE_MANIFEST);
        bundleInfoDict = GetAllBundleInfoFromManifest(manifest);

        totalBundles = bundleInfoDict.Count;
        int bundleIndex = 0;

        var enumer = bundleInfoDict.GetEnumerator();
        while (enumer.MoveNext()) {
            ++bundleIndex;
            string bundleName = enumer.Current.Key;
            Hash128 bundleHash = enumer.Current.Value;

            bundleSystemState = EN_BundleSystemState.OnLoadingRemoveBundle;
            currBundleIndex = bundleIndex;
            currBundleName = bundleName;

            BundleLoadResult bundleLoadResult = await loader.LoadBundleAsync(GetBundleUrl(bundleName), bundleHash);

            if(bundleLoadResult.state == BundleLoadState.Faild) {
                bundleSystemState = EN_BundleSystemState.Error_LoadRemoveBundle;
                return;
            }

            SaveBundleInfo(bundleLoadResult.bundle);
        }

        bundleSystemState = EN_BundleSystemState.AllRemoveBundleLoadSuccess;
    }

    /// <summary>
    /// 同步获取一个资源
    /// </summary>
    /// <param name="assetName">资源全路径，来自资源Json清单</param>
    /// <returns>返回原始资源(不克隆)</returns>
    public UnityEngine.Object GetAsset(string _fullAssetName) {
        if (!loadedAssetDict.ContainsKey(_fullAssetName)) {
            assetToBundleDict.TryGetValue(_fullAssetName, out string bundleName);
            if (String.IsNullOrEmpty(bundleName)) {
                UnityEngine.Debug.LogError(String.Format("无法找到资源 {0} 所在的 Bundle", _fullAssetName));
                return null;
            }

            loadedBundleDict.TryGetValue(bundleName, out AssetBundle bundle);
            if(bundle == null) {
                UnityEngine.Debug.LogError(String.Format("无法找到Bundle: {0}", bundleName));
                return null;
            }

            UnityEngine.Object asset = bundle.LoadAsset(_fullAssetName);

            if(asset != null) {
                loadedAssetDict.Add(_fullAssetName, asset);
            } else {
                UnityEngine.Debug.LogError(String.Format("无法从Bundle中加载资源 {0}", _fullAssetName));
                return null;
            }
        }

        return loadedAssetDict[_fullAssetName];
    }

    /// <summary>
    /// 异步获取一个资源
    /// </summary>
    /// <param name="_fullAssetName">资源全路径</param>
    /// <param name="_callback">l加载完后的回调</param>
    public async Task GetAssetAsync(string _fullAssetName, Listener<UnityEngine.Object> _callback) {

        // 将回调添加到字典
        if (_callback != null) {
            assetRequestCallbackDict[_fullAssetName] += _callback;
        }

        // 缓存中不存在资源，需要加载
        if (!loadedAssetDict.ContainsKey(_fullAssetName)) {

            // 获取资源所在的 Bundle 名字
            assetToBundleDict.TryGetValue(_fullAssetName, out string bundleName);
            if (String.IsNullOrEmpty(bundleName)) {
                UnityEngine.Debug.LogError(String.Format("无法找到资源 {0} 所在的 Bundle", _fullAssetName));
                CallbackAsNull(_fullAssetName);
                return;
            }

            // 获取资源所在的 Bundle
            loadedBundleDict.TryGetValue(bundleName, out AssetBundle bundle);
            if (bundle == null) {
                UnityEngine.Debug.LogError(String.Format("无法找到Bundle: {0}", bundleName));
                CallbackAsNull(_fullAssetName);
                return;
            }

            // 异步加载资源
            AssetBundleRequest request = bundle.LoadAssetAsync(_fullAssetName);
            await request;

            if(request == null || request.asset == null) {
                UnityEngine.Debug.LogError(String.Format("从Bundle中异步加载资源 {0} 失败", _fullAssetName));
                CallbackAsNull(_fullAssetName);
                return;
            }

            // 资源缓存
            UnityEngine.Object obj = request.asset;
            loadedAssetDict.Add(_fullAssetName, obj);
        }

        // 资源加载完回调
        if (assetRequestCallbackDict[_fullAssetName] != null) {
            assetRequestCallbackDict[_fullAssetName](loadedAssetDict[_fullAssetName]);
            assetRequestCallbackDict[_fullAssetName] = null;
        }
    }



    // ----------------------------- internal function -------------------------------
    /// <summary>
    /// 异步加载资源空回调(没有加载到指定的资源)
    /// </summary>
    /// <param name="_assetLoadCallbackName"></param>
    private void CallbackAsNull(string _assetLoadCallbackName) {
        if (assetRequestCallbackDict[_assetLoadCallbackName] != null) {
            assetRequestCallbackDict[_assetLoadCallbackName](null);
            assetRequestCallbackDict[_assetLoadCallbackName] = null; 
        }
    }

    /// <summary>
    /// 保存一个 Bundle 的信息，将Bundle 保存到字典，将Bundle里面的资源名字保存到字典
    /// </summary>
    /// <param name="_bundle"></param>
    private void SaveBundleInfo(AssetBundle _bundle) {
        string bundleName = _bundle.name;
        if (!loadedBundleDict.ContainsKey(bundleName)) {
            loadedBundleDict.Add(bundleName, _bundle);
        }

        string[] allAssets = _bundle.GetAllAssetNames();
        for(int i = 0; i < allAssets.Length; ++i) {
            string fullAssetName = allAssets[i];        // Full Asset path , 如 Asset/Res/UI/main.png
            //string assetName = System.IO.Path.GetFileName(fullAssetName);
            if (!assetToBundleDict.ContainsKey(fullAssetName)) {
                assetToBundleDict.Add(fullAssetName, bundleName);
            }
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

    /// <summary>
    /// 获取Bundle的远程地址
    /// </summary>
    /// <param name="_bundleName"></param>
    /// <returns></returns>
    private string GetBundleUrl(string _bundleName) {
        return VioletConst.URL_BUNDLE + GetBundleRootByPlatform() + "/" + _bundleName;
    }

    /// <summary>
    /// 获取Manifest远程地址
    /// </summary>
    /// <returns></returns>
    private string GetManifestUrl() {
        return VioletConst.URL_BUNDLE + GetBundleRootByPlatform() + "/" + GetBundleRootByPlatform();
    }

    /// <summary>
    /// 获取本地 Manifest 地址
    /// </summary>
    /// <returns></returns>
    private string GetLocalManifestUrl() {
        return VioletConst.URL_LOCAL_BUNDLE + GetBundleRootByPlatform() + "/" + GetBundleRootByPlatform();
    }

    /// <summary>
    /// 获取本地 Bundle 地址
    /// </summary>
    /// <param name="_bundleName"></param>
    /// <returns></returns>
    private string GetLocalBundleUrl(string _bundleName) {
        return VioletConst.URL_LOCAL_BUNDLE + GetBundleRootByPlatform() + "/" + _bundleName;
    }
    /// <summary>
    /// 获取平台相关的Bundle主目录地址
    /// </summary>
    /// <returns></returns>
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