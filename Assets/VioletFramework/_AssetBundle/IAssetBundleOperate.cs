using System;
using System.Collections.Generic;

namespace RedQueen.Beta.AssetBundles {
    public interface IAssetBundleOperate {
        /// <summary>
        /// 获取当前操作的名字
        /// </summary>
        /// <returns></returns>
        string GetOperationName();

        /// <summary>
        /// 获取当前操作的进度
        /// </summary>
        /// <returns></returns>
        int GetOperationProgress();
    }
}

