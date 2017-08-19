using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.Networking;

public class AssetBundleUI : MonoBehaviour {
    public Text stateText = null;
    public Text numberText = null;

    UnityWebRequest r;

    // Use this for initialization
    void Start () {
         //LoadTest();
	}

    public async Task LoadTest() {
        string path = "http://47.94.154.110/abrelease/Windows/ui";
        float startTime = Time.time;
        r = UnityWebRequest.Get(path);
        await r.Send();

        Debug.LogError("Down:" + (Time.time - startTime));
    }

    private async void UnZIPBundle() {
        // Open Loading UI
        await V.Instance.vBundle.UnZipLocalBundleAsync();
        if (V.Instance.vBundle.bundleSystemState != EN_BundleSystemState.AllLocalBundleUnZipSuccess) {
            Debug.LogError("资源解压失败");
            return;
        }
    }

    private async void UpdateBundle() {
        float startTime = Time.realtimeSinceStartup;
        Debug.Log("Start Time:" + startTime);
        // Load AssetBundles
        await V.Instance.vBundle.LoadBundlesAsync();
        if (V.Instance.vBundle.bundleSystemState != EN_BundleSystemState.AllRemoveBundleLoadSuccess) {
            Debug.LogError("资源更新失败");
            return;
        }

        Debug.LogError("UsedTime:" + (Time.realtimeSinceStartup - startTime).ToString());

    }

    private void ClearCache() {
        Caching.ClearCache();
    }


    private void OnGUI() {
        if (GUILayout.Button("Clear Cacheing")) {
            ClearCache();
        }

        if (GUILayout.Button("UnZipBundle")) {
            UnZIPBundle();
        }

        if(GUILayout.Button("UPdate Bundle")) {
            UpdateBundle();
        }
    }


    // Update is called once per frame
    void Update () {


        //if(r != null) {
        //    Debug.Log(r.downloadProgress);
        //    numberText.text = r.downloadProgress.ToString();
        //}
        //return;


        if(V.Instance.vBundle.bundleSystemState == EN_BundleSystemState.OnUnZipLocalBundle) {
            
        }
        stateText.text = "";
        numberText.text = "";

        switch (V.Instance.vBundle.bundleSystemState) {
            case EN_BundleSystemState.OnUnZipLocalBundle: {
                    stateText.text = "正在解压本地AssetBundle: " + V.Instance.vBundle.currBundleName;
                    numberText.text = V.Instance.vBundle.progress + "%" + "  " +  V.Instance.vBundle.currBundleIndex + " / " + V.Instance.vBundle.totalBundles;
                }
                break;
            case EN_BundleSystemState.AllLocalBundleUnZipSuccess: {
                    stateText.text = "本地Bundle加压成功";
                    numberText.text = "";
                }
                break;
            case EN_BundleSystemState.OnLoadingManifest: {
                    stateText.text = "正在加载资源列表";
                    numberText.text = "";
                }
                break;
            case EN_BundleSystemState.OnLoadingRemoveBundle: {
                    stateText.text = "正在加载资源: " + V.Instance.vBundle.currBundleName;
                    numberText.text = V.Instance.vBundle.progress + "%" + "  " + V.Instance.vBundle.currBundleIndex + " / " + V.Instance.vBundle.totalBundles;
                }
                break;
            case EN_BundleSystemState.AllRemoveBundleLoadSuccess: {
                    stateText.text = "资源加载成功";
                    numberText.text = "";
                }
                break;
            case EN_BundleSystemState.Error_LoadLocalManifest: {
                    stateText.text = "本地资源列表加载失败";
                }
                break;
            case EN_BundleSystemState.Error_LoadManifest: {
                    stateText.text = "远程资源列表加载失败";
                }
                break;
            case EN_BundleSystemState.Error_LoadRemoveBundle: {
                    stateText.text = "资源更新失败: " + V.Instance.vBundle.currBundleName;
                    numberText.text = V.Instance.vBundle.progress + "%" + "  " + V.Instance.vBundle.currBundleIndex + " / " + V.Instance.vBundle.totalBundles;
                }
                break;
            case EN_BundleSystemState.Error_UnZipLocalBundle: {
                    stateText.text = "资源解压失败: " + V.Instance.vBundle.currBundleName;
                    numberText.text = V.Instance.vBundle.progress + "%" + "  " + V.Instance.vBundle.currBundleIndex + " / " + V.Instance.vBundle.totalBundles;
                }
                break;
        }
	}
}
