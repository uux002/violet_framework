using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformUtil {

    
    public static string GetRelativeBundlePath(string bundleName) {
        switch (Application.platform) {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer: {
                    return "Windows/" + bundleName;
                }
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer: {
                    return "OSX/" + bundleName;
                }
            case RuntimePlatform.IPhonePlayer: {
                    return "iOS/" + bundleName;
                }
            case RuntimePlatform.Android: {
                    return "Android/" + bundleName;
                }
        }

        return string.Empty;
    }


    public static string GetRelativeMainManifestBundlePath() {
        switch (Application.platform) {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer: {
                    return "Windows/Windows";
                }
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor: {
                    return "OSX/OSX";
                }
            case RuntimePlatform.IPhonePlayer: {
                    return "iOS/iOS";
                }
            case RuntimePlatform.Android: {
                    return "Android/Android";
                }
        }

        return string.Empty;
    }
}
