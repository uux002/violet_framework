//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Threading.Tasks;

//namespace RedQueen.Beta.AssetBundles {
//    public class AssetLoader {
//        /// <summary>
//        /// 同步获取一个资源
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="assetName"></param>
//        /// <returns></returns>
//        static public T LoadAsset<T>(string assetName, AssetBundle bundle) where T : UnityEngine.Object {
//            if(bundle == null) {
//                Log.Error(typeof(AssetLoader), "获取Bundle失败");
//                return default(T);
//            }

//            return bundle.LoadAsset<T>(assetName);
//        }

//        /// <summary>
//        /// 异步获取一个资源
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="assetName"></param>
//        /// <param name="callback"></param>
//        /// <returns></returns>
//        static public async Task<UnityEngine.Object> LoadAssetAsync<T>(string assetName, AssetBundle bundle) where T : UnityEngine.Object {
//            if(bundle == null) {
//                Log.Error(typeof(AssetLoader), "异步获取Bundle失败");
//                return default(T);
//            }

//            AssetBundleRequest request = bundle.LoadAssetAsync<T>(assetName);
//            await request;

//            if(request.asset == null) {
//                Log.Error(typeof(AssetLoader), "从Bundle中异步加载资源失败:" + bundle.name);
//                return default(T);
//            }

//            T obj = (T)request.asset;
//            return obj;
//        }


//        /// <summary>
//        /// 加载一个Bundle里的所有资源
//        /// </summary>
//        /// <param name="bundle"></param>
//        /// <returns></returns>
//        static public UnityEngine.Object[] LoadAllAsset(AssetBundle bundle) {
//            if(bundle == null) {
//                return null;
//            }

//            return bundle.LoadAllAssets();
//        }

//        /// <summary>
//        /// 异步加载一个bundle里的所有资源
//        /// </summary>
//        /// <param name="bundle"></param>
//        /// <returns></returns>
//        static public async Task<UnityEngine.Object[]> LoadAllAssetAsync(AssetBundle bundle) {
//            if(bundle == null) {
//                return null;
//            }

//            AssetBundleRequest request = bundle.LoadAllAssetsAsync();
//            await request;

//            return request.allAssets;
//        } 

//    }

//    public class AssetLoadResult : AssetBundleResult {

//    }
//}
