//#define DEBUG_BUNDLE


//namespace RedQueen.Beta.AssetBundles {
//    public class AssetBundleConst {

//        static public void Init(string _remoteBundlePath, string _localBundlePath) {
//            REMOTE_BUNDLE_PATH = _remoteBundlePath;
//            LOCAL_BUNDLE_PATH = _localBundlePath;
//        }

//        static private string REMOTE_BUNDLE_PATH = "Assignment from out side";  // http://xxxx.com/xxx/xxx
//        static private string LOCAL_BUNDLE_PATH = "Assignment from out side";   // file://Application.stream

//        public const string MAIN_MANIFEST_NAME = "AssetBundleManifest";
//        static public bool DIRECT_LOAD_ASSETS_ON_EDITOR = false;

//        static public string GetRemoteBundleUrl(string bundleName) {
//            return REMOTE_BUNDLE_PATH + PlatformUtils.GetPlatformBundleRoot() + "/" + bundleName;
//        }

//        static public string GetRemoteManifestUrl() {
//            return GetRemoteBundleUrl(PlatformUtils.GetPlatformMainBundle());
//        }

//        static public string GetLocalBundleUrl(string bundleName) {
//            return LOCAL_BUNDLE_PATH + PlatformUtils.GetPlatformBundleRoot() + "/" + bundleName;
//        }
        
//        static public string GetLocalManifestUrl() {
//            return GetLocalBundleUrl(PlatformUtils.GetPlatformMainBundle());
//        }
//    }
//}
