//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Threading.Tasks;
//using RedQueen.EventSystem;
//using RedQueen.Logic.Events;
//using RedQueen.Managers;
//using RedQueen.Logic.Consts;

//namespace RedQueen.Beta.AssetBundles {
//    public class AssetBundleManager {

//        /// <summary>
//        /// 已经加载完的AssetBundle
//        /// </summary>
//        static private Dictionary<string, AssetBundle> loadedAssetBundle = new Dictionary<string, AssetBundle>();

//        /// <summary>
//        /// 哪一个资源存在哪一个Bundle中，key 类型： aaa.prefab  bbb.png  ccc.mat
//        /// </summary>
//        static private Dictionary<string, string> assetToAssetBundle = new Dictionary<string, string>();

//        /// <summary>
//        /// 已经加载的资源缓存
//        /// </summary>
//        private static Dictionary<string, UnityEngine.Object> loadedAsset = new Dictionary<string, UnityEngine.Object>();

//        /// <summary>
//        /// 异步加载资源的回调字典
//        /// </summary>
//        private static Dictionary<string, List<Action<UnityEngine.Object>>> callbackDict = new Dictionary<string, List<Action<UnityEngine.Object>>>();

//        static private string currOperationName = "";

//        /// <summary>
//        /// 通过一个不带扩展名的资源名称获得一个AssetBundle
//        /// </summary>
//        /// <param name="assetName"></param>
//        /// <returns></returns>
//        static public AssetBundle GetAssetBundle<T>(string assetName) where T : UnityEngine.Object {
//            string bundleName = null;
//            AssetBundle bundle = null;
//            string assetNameWithExtension;
//            if (typeof(T) == typeof(GameObject)) {
//                assetNameWithExtension = assetName + ".prefab";
//                assetToAssetBundle.TryGetValue(assetNameWithExtension, out bundleName);
//                if (!String.IsNullOrEmpty(bundleName)) {
//                    loadedAssetBundle.TryGetValue(bundleName, out bundle);
//                    return bundle;
//                }
//            }
//            else if(typeof(T) == typeof(Texture) ||
//                    typeof(T) == typeof(Texture2D)) {
//                // check png
//                assetNameWithExtension = assetName + ".png";
//                assetToAssetBundle.TryGetValue(assetNameWithExtension, out bundleName);
//                if (!String.IsNullOrEmpty(bundleName)) {
//                    loadedAssetBundle.TryGetValue(bundleName, out bundle);
//                    return bundle;
//                }

//                // check psd
//                assetNameWithExtension = assetName + ".psd";
//                assetToAssetBundle.TryGetValue(assetNameWithExtension, out bundleName);
//                if (!String.IsNullOrEmpty(bundleName)) {
//                    loadedAssetBundle.TryGetValue(bundleName, out bundle);
//                    return bundle;
//                }

//                // check tga
//                assetNameWithExtension = assetName + ".tga";
//                assetToAssetBundle.TryGetValue(assetNameWithExtension, out bundleName);
//                if (!String.IsNullOrEmpty(bundleName)) {
//                    loadedAssetBundle.TryGetValue(bundleName, out bundle);
//                    return bundle;
//                }
//            }

//            return null;
//        }

//        /// <summary>
//        /// 首次运行游戏时解压本地AssetBundle
//        /// </summary>
//        /// <returns></returns>
//        static public async Task<UnZipAssetBundleResult> UnZipLocalBundleOnGameFirstTime() {
//            currOperationName = "正在解压资源";
//            UpdateOperationName(currOperationName);

//            UnZipAssetBundleResult result = new UnZipAssetBundleResult();
//            ManifestLoadResult manifestResult = await ManifestLoader.LoadManifest(AssetBundleConst.GetLocalManifestUrl());
//            if(manifestResult.state == enHTTPState.ERROR) {
//                Log.Error(typeof(AssetBundleManager), "加载本地 Manifest 失败");
//                result.info = "资源解压失败，主资源文件加载失败";
//                result.state = enHTTPState.ERROR;
//                return result;
//            }
            
//            UManagerHelper.GetTickerManager().AddTickCall(UpdateProgress);
//            AssetBundleLoadResult bundleLoadResult = await AssetBundleLoader.LoadAllAssetBundleAsync(manifestResult,true);
//            if(bundleLoadResult.state == enHTTPState.ERROR) {
//                Log.Error(typeof(AssetBundleManager), "解压本地Bundle失败");
//                UManagerHelper.GetTickerManager().RemoveTickCall(UpdateProgress);
//                result.info = "解压AssetBundle失败";
//                result.state = enHTTPState.ERROR;
//                return result;
//            }
//            UManagerHelper.GetTickerManager().RemoveTickCall(UpdateProgress);
//            UpdateProgress();

//            result.info = "资源解压成功";
//            result.state = enHTTPState.SUCCEED;
//            return result;
//        }

//        /// <summary>
//        /// 更新远程的AssetBundle
//        /// </summary>
//        /// <returns></returns>
//        static public async Task<UpdateAssetBundleResult> UpdateRemoteAssetBundle() {
//            currOperationName = "正在更新资源";
//            UpdateOperationName(currOperationName);
//            UpdateAssetBundleResult result = new UpdateAssetBundleResult();
//            ManifestLoadResult manifestResult = await ManifestLoader.LoadManifest(AssetBundleConst.GetRemoteManifestUrl());
//            if(manifestResult.state == enHTTPState.ERROR) {
//                Log.Error(typeof(AssetBundleManager), "加载 Manifest 失败");
//                result.info = "资源更新失败, 主资源文件加载失败";
//                result.state = enHTTPState.ERROR;
//                return result;
//            }

//            UManagerHelper.GetTickerManager().AddTickCall(UpdateProgress);
            
//            AssetBundleLoadResult bundleLoadResult = await AssetBundleLoader.LoadAllAssetBundleAsync(manifestResult);

//            if(bundleLoadResult.state == enHTTPState.ERROR) {
//                Log.Error(typeof(AssetBundleManager), "更新远程Bundle失败");
//                UManagerHelper.GetTickerManager().RemoveTickCall(UpdateProgress);
//                result.info = "资源更新失败";
//                result.state = enHTTPState.ERROR;
//                return result;
//            }

//            UpdateProgress();
//            UManagerHelper.GetTickerManager().RemoveTickCall(UpdateProgress);

//            for (int i = 0; i < bundleLoadResult.bundleList.Count; ++i) {
//                AssetBundle bundle = bundleLoadResult.bundleList[i];
//                SaveLoadedBundleInfo(bundle);
//            }
//            result.info = "资源更新成功";
//            result.state = enHTTPState.SUCCEED;
//            return result;
//        }

//        /// <summary>
//        /// 保存加载后的AssetBundle并且将资源名映射到AssetBundle名
//        /// </summary>
//        /// <param name="bundle"></param>
//        static private void SaveLoadedBundleInfo(AssetBundle bundle) {
//            string bundleName = bundle.name;
//            if (!loadedAssetBundle.ContainsKey(bundleName)) {
//                loadedAssetBundle.Add(bundleName, bundle);
//            }

//            string[] allAssets = bundle.GetAllAssetNames();
//            for(int i = 0; i < allAssets.Length; ++i) {
//                string fullAssetName = allAssets[i];
//                string assetName = System.IO.Path.GetFileName(fullAssetName);
//                if (!assetToAssetBundle.ContainsKey(assetName)) {
//                    assetToAssetBundle.Add(assetName, bundleName);
//                }
//            }
//        }

//        /// <summary>
//        /// 更新操作名称，向外发送事件

//        /// </summary>
//        /// <param name="_operationName"></param>
//        static private void UpdateOperationName(string _operationName) {
//            NotifyCenter.Send(LogicGameEvents.GAME_EVENT_BUNDLE_OPERATION_CHANGE, _operationName);
//        }

//        /// <summary>
//        /// 更新当前操作进度，向外发送事件
//        /// </summary>
//        /// <returns></returns>
//        static private void UpdateProgress() {
//            //int progress = AssetBundleLoader.GetProgress();
//            int progress = AssetBundleLoader.GetSingleProgress();
//            NotifyCenter.Send(LogicGameEvents.GAME_EVENT_BUNDLE_PROGRESS_CHANGE, progress);
//        }



//        // ---------- 对业务层开放接口 ----------
//        /// <summary>
//        /// 同步获取一个资源
//        /// </summary>
//        /// <typeparam name="T">GameObject , Texture2D, ...</typeparam>
//        /// <param name="assetName">tank.prefab, tank.png</param>
//        /// <returns></returns>
//        static public T GetAsset<T>(string assetName) where T : UnityEngine.Object {

//            if (!loadedAsset.ContainsKey(assetName)) {
//                AssetBundle bundle = GetAssetBundle<T>(assetName);
//                UnityEngine.Object obj = AssetLoader.LoadAsset<T>(assetName, bundle);
//                if (obj != null) { 
//                    loadedAsset.Add(assetName, obj);
//                }
//            }

//            if (loadedAsset.ContainsKey(assetName)){
//                if(typeof(T) == typeof(GameObject)) {
//                    return UnityEngine.Object.Instantiate(loadedAsset[assetName]) as T;
//                } else {
//                    return loadedAsset[assetName] as T;
//                }
//            }

//            return default(T);
//        }

//        /// <summary>
//        /// 异步获取一个资源
//        /// </summary>
//        /// <typeparam name="T">GameObject, Texture2D</typeparam>
//        /// <param name="assetName">tank.prefab, tank.png ...</param>
//        /// <param name="callback"></param>
//        static public async Task GetAssetAsync<T>(string assetName, Action<T> callback) where T :UnityEngine.Object {
//            if (!loadedAsset.ContainsKey(assetName)) {

//                bool loadAssetOnThisCall = false;
//                if (!callbackDict.ContainsKey(assetName)) {
//                    callbackDict.Add(assetName, new List<Action<UnityEngine.Object>>());
//                    loadAssetOnThisCall = true;
//                }

//                if (loadAssetOnThisCall) {
//                    callbackDict[assetName].Add((callback as Action<UnityEngine.Object>));
//                    AssetBundle bundle = GetAssetBundle<T>(assetName);
//                    UnityEngine.Object obj = await AssetLoader.LoadAssetAsync<T>(assetName, bundle);
//                    loadedAsset.Add(assetName, obj);

//                    List<Action<UnityEngine.Object>> callbackList = null;
//                    callbackDict.TryGetValue(assetName, out callbackList);
//                    if(callbackList != null) {
//                        callbackDict.Remove(assetName);
//                        for(int i = 0; i < callbackList.Count; ++i) {
//                            if(typeof(T) == typeof(GameObject)) {
//                                T newObj = UnityEngine.Object.Instantiate(loadedAsset[assetName]) as T;
//                                callbackList[i](newObj);
//                            } else {
//                                callbackList[i](loadedAsset[assetName] as T);
//                            }
//                        }
//                        callbackList.Clear();
//                        callbackList = null;
//                    }
//                } else {
//                    callbackDict[assetName].Add((callback as Action<UnityEngine.Object>));
//                }

//            } else {
//                if(typeof(T) == typeof(GameObject)) {
//                    T obj = UnityEngine.Object.Instantiate(loadedAsset[assetName]) as T;
//                    callback(obj);
//                } else {
//                    callback(loadedAsset[assetName] as T);
//                }
//            }
//        }
        
//        /// <summary>
//        /// 加载一组新的AssetBundle，例如一个新副本
//        /// </summary>
//        /// <param name="bundleUrl"></param>
//        /// <returns></returns>
//        static public async Task LoadNewAssetBundle(string bundleManifestUrl) {
//            ManifestLoadResult manifestResult = await ManifestLoader.LoadManifest(bundleManifestUrl);
//            if(manifestResult.state == enHTTPState.ERROR) {
//                Log.Error(typeof(AssetBundleManager), "加载新的资源组失败:" + bundleManifestUrl);
//                return;
//            }

//            currOperationName = "正在加载新的资源";
//            UpdateOperationName(currOperationName);

//            AssetBundleLoadResult bundleLoadResult = await AssetBundleLoader.LoadAllAssetBundleAsync(manifestResult);
//            if(bundleLoadResult.state == enHTTPState.ERROR) {
//                Log.Error(typeof(AssetBundleManager), "加载新的资源组出错中断");
//                return;
//            }

//            for(int i = 0; i < bundleLoadResult.bundleList.Count; ++i) {
//                AssetBundle bundle = bundleLoadResult.bundleList[i];
//                SaveLoadedBundleInfo(bundle);
//            }
//        }       

//    }

//    /// <summary>
//    /// 解压本地 AssetBundle 结果
//    /// </summary>
//    public class UnZipAssetBundleResult : AssetBundleResult {
//        public string info;
//    }

//    /// <summary>
//    /// 更新 AssetBundle 结果
//    /// </summary>
//    public class UpdateAssetBundleResult : AssetBundleResult {
//        public string info;
//    }


//}
