using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Libs.Editor;
using System.Diagnostics;

namespace Yule.Editor
{
    public class CodeEditor
    {
        //[MenuItem("Tools/Export CSV Data, Generate VO Code")]
        public static void ExportData()
        {
            //// 打包数据
            //ConfigExportor.ExportConfig(AppConst.PATH_CSV_SOURCE,AppConst.PATH_CONFIG_FILE);  

            //Debug.Log("导出数据配置成功");

            //FileUtil.DeleteFileOrDirectory(AppConst.PATH_CONFIG_SCRIPT_VO);

            //// 根据CSV文件导出C#类
            //ConfigExportor.ExportVoCodeFromCSV(AppConst.NAMESPACE_CONFIG,
            //                                        AppConst.PATH_CSV_SOURCE,
            //                                        AppConst.PATH_CONFIG_SCRIPT_VO);

            //Debug.Log("值对象导出完成");
            //AssetDatabase.Refresh();
        }

    }
}