using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class BundleLoader {
    public float progress {
        get {
            if(currWebRequest != null) {
                return currWebRequest.downloadProgress;
            }
            return 0.0f;
        }
    }

    private UnityWebRequest currWebRequest = null;

    public async Task<BundleLoadResult> LoadBundleAsync(string _bundleUrl) {
        BundleLoadResult result = new BundleLoadResult();
        UnityWebRequest request = UnityWebRequest.GetAssetBundle(_bundleUrl);
        currWebRequest = request;
        await request.Send();

        AssetBundle bundle = null;

        if (!request.isNetworkError) {
            bundle = DownloadHandlerAssetBundle.GetContent(request);
        }

        if(bundle == null) {
            result.state = BundleLoadState.Faild;
        } else {
            result.state = BundleLoadState.Success;
            result.bundle = bundle;
        }

        return result;
    }
}
