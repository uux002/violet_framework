//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using System.Threading.Tasks;

//namespace RedQueen.Beta.AssetBundles {
//    public class ManifestLoader {
//        static public async Task<ManifestLoadResult> LoadManifest(string manifestBundleUrl) {
//            ManifestLoadResult result = new ManifestLoadResult();
//            AssetBundleLoadResult bundleLoadResult = await AssetBundleLoader.LoadAssetBundleAsync(manifestBundleUrl);
//            if (bundleLoadResult.state != enHTTPState.SUCCEED) {
//                result.state = bundleLoadResult.state;
//            } else {
//                result.state = enHTTPState.SUCCEED;
//                AssetBundleManifest manifest = (AssetBundleManifest)bundleLoadResult.bundle.LoadAsset(AssetBundleConst.MAIN_MANIFEST_NAME);
//                result.manifest = manifest;
//            }
//            return result;
//        }
//    }

//    public class ManifestLoadResult : AssetBundleResult {
//        public AssetBundleManifest manifest = null;        

//        public Dictionary<string,Hash128> GetAllBundleInfo() {
//            if(manifest == null) {
//                return null;
//            }

//            Dictionary<string, Hash128> bundleInfoDict = new Dictionary<string, Hash128>();

//            string[] allBundleNames = manifest.GetAllAssetBundles();
//            for(int i = 0; i < allBundleNames.Length; ++i) {
//                string bundleName = allBundleNames[i];
//                Hash128 bundleHash = manifest.GetAssetBundleHash(bundleName);
//                bundleInfoDict.Add(bundleName, bundleHash);
//            }

//            return bundleInfoDict;
//        }
//    }
//}
