using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.IO;
using RedQueen.EventSystem;
using RedQueen.Logic.Events;

namespace RedQueen.Beta.AssetBundles {
    public class AssetBundleLoader {

        private static UnityWebRequest currRequest = null;
        private static int totalBundles = 0;
        private static int currBundleIndex = 0;
        private static int partOfSize = 0;
        private static float currRequestDownloadProgress = 0.0f;

        /// <summary>
        /// 同步加载 AssetBundle，无法从远程加载，路径不带 "file://"前缀
        /// </summary>
        /// <param name="bundleUrl"></param>
        /// <returns></returns>
        static public AssetBundle LoadAssetBundle(string bundleUrl) {
            AssetBundle bundle = AssetBundle.LoadFromFile(bundleUrl);
            return bundle;
        }


        /// <summary>
        /// 异步加载AssetBundle
        /// </summary>
        /// <param name="bundleUrl"></param>
        /// <returns></returns>
        static public async Task<AssetBundleLoadResult> LoadAssetBundleAsync(string bundleUrl) {
            AssetBundleLoadResult result = new AssetBundleLoadResult();
            UnityWebRequest request = null;
            request = UnityWebRequest.GetAssetBundle(bundleUrl);
            await request.Send();

            AssetBundle bundle = null;

            if (!request.isError) {
                bundle = DownloadHandlerAssetBundle.GetContent(request);
            }

            if(bundle == null) {
                result.state = enHTTPState.ERROR;
            } else {
                result.state = enHTTPState.SUCCEED;
                result.bundle = bundle;
            }

            request.Dispose();
            request = null;
            return result;
        }



        /// <summary>
        /// 从一个总的 Manifest 信息中加载所有的 AssetBundle
        /// </summary>
        /// <param name="manifestResult"></param>
        /// <returns></returns>
        static public async Task<AssetBundleLoadResult> LoadAllAssetBundleAsync(ManifestLoadResult manifestResult,bool localBundle = false) {

            AssetBundleLoadResult result = new AssetBundleLoadResult();
            Dictionary<string, Hash128> allBundles = manifestResult.GetAllBundleInfo();

            currRequest = null;
            totalBundles = allBundles.Count;
            currBundleIndex = 0;
            partOfSize = Mathf.RoundToInt(100.0f / totalBundles);

            NotifyCenter.Send(LogicGameEvents.GAME_EVENT_BUNDLE_COUNT_CHANGE, " (" + currBundleIndex + "/" + totalBundles + ")");

            var enumer = allBundles.GetEnumerator();
            while (enumer.MoveNext()) {
                string bundleName = enumer.Current.Key;
                Hash128 bundleHash = enumer.Current.Value;
                string bundleUrl = localBundle ? AssetBundleConst.GetLocalBundleUrl(bundleName) : AssetBundleConst.GetRemoteBundleUrl(bundleName);
                UnityWebRequest request = UnityWebRequest.GetAssetBundle(bundleUrl, bundleHash, 0);
                currRequest = request;
                await request.Send();

                AssetBundle bundle = null;

                if (!request.isError) {
                    bundle = DownloadHandlerAssetBundle.GetContent(request);
                }

                if(bundle == null) {
                    Log.Error(typeof(AssetBundleLoader), "加载Bundle被中断：" + bundleName, bundleUrl, bundleHash);
                    request.Dispose();
                    request = null;
                    result.state = enHTTPState.ERROR;
                    return result;
                } else {
                    ++currBundleIndex;
                    result.bundleList.Add(bundle);
                }

                request.Dispose();
                request = null;
                currRequestDownloadProgress = 1.0f;
                NotifyCenter.Send(LogicGameEvents.GAME_EVENT_BUNDLE_PROGRESS_CHANGE, 100);
                NotifyCenter.Send(LogicGameEvents.GAME_EVENT_BUNDLE_COUNT_CHANGE, " (" + currBundleIndex + "/" + totalBundles + ")");
            }
            currRequest = null;
            result.state = enHTTPState.SUCCEED;
            return result;
        }


        static public int GetProgress() {
            int progress = 0;
            float tmpProgress = 0.0f;
            if (totalBundles > 0) {
                tmpProgress = (float)currBundleIndex / (float)totalBundles;
                tmpProgress *= 100;
            }

            if (currRequest != null) {
                tmpProgress += currRequest.downloadProgress * partOfSize;
            }

            progress = (int)tmpProgress;
            progress = Mathf.Clamp(progress, 0, 100);
            return progress;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public int GetSingleProgress() {
            if(currRequest != null) {
                currRequestDownloadProgress = currRequest.downloadProgress;
            }
            return (int)(currRequestDownloadProgress * 100);
        }

        static public int GetTotalDownload() {
            return totalBundles;
        }

        static public int GetCurrentDownloadIndex() {
            return currBundleIndex;
        }
    }

    public class AssetBundleLoadResult : AssetBundleResult {
        public AssetBundle bundle = null;
        public List<AssetBundle> bundleList = new List<AssetBundle>();
    }

}
