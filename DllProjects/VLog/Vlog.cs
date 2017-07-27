using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

/// <summary>
/// 日志写入方式
/// </summary>
public enum VLogWriteType {
    None,                   // 日志不写入文件
    SplitWriteAndSummery,   // 日志Info, Error, Warning, Fatal 分别写入不同文件, 并且写入一份汇总文件
    SplitWrite,             // 日志Info, Error, Warning, Fatal 分别写入不同文件, 无汇总文件
    Summery,                // 将所有日志只写一份汇总文件
}


public class Vlog {

    enum enLogType {
        Info,
        Warning,
        Error,
        Fatal,
    }

    /// <summary>
    /// 禁用所有日志
    /// </summary>
    public static bool disable = false;
    /// <summary>
    /// 禁用Info
    /// </summary>
    public static bool disableInfo = false;
    /// <summary>
    /// 禁用Error
    /// </summary>
    public static bool disableError = false;
    /// <summary>
    /// 禁用Warning
    /// </summary>
    public static bool disableWarning = false;

    /// <summary>
    /// 禁用Fatal
    /// </summary>
    public static bool disableFatal = false;

    /// <summary>
    /// 日志写入方式
    /// </summary>
    public static VLogWriteType logWriteType = VLogWriteType.None;

    /// <summary>
    /// 当日志的字节数 > maxLogFileBytes 的时候就删除日志重写，如果设为-1, 则不设置大小上限
    /// </summary>
    public static int maxInfoLogFileBytes = 10000000;
    public static int maxErrorLogFileBytes = 10000000;
    public static int maxWarningLogFileBytes = 10000000;
    public static int maxFatalLogFileBytes = 10000000;
    public static int maxSummaryFileBytes = 10000000;

    private const string logInfoFileName = "Info.txt";
    private const string logErrorFileName = "Error.txt";
    private const string logWarningFileName = "Warning.txt";
    private const string logFatalFileName = "Fatal.txt";
    private const string logSummaryFileName = "Summary.txt";

    /// <summary>
    /// 日志写入路径, 如果不指定路径，即使设置了将日志写入文件，也不会写入
    /// </summary>
    public static string logFilePath = "";

    public static void OInfo(object _owner, params object[] _args) {
        if(disable || disableInfo) {
            return;
        }
        _Log(enLogType.Info, _owner, _args);
    }

    public static void OError(object _owner, params object[] _args) {
        if(disable || disableError) {
            return;
        }
        _Log(enLogType.Error, _owner, _args);
    }

    public static void OWarning(object _owner, params object[] _args) {
        if(disable || disableWarning) {
            return;
        }
        _Log(enLogType.Warning, _owner, _args);
    }

    public static void OFatal(object _owner, params object[] _args) {
        if (disable || disableFatal) {
            return;
        }
        _Log(enLogType.Fatal, _owner, _args);
    }

    public static void Info(params object[] _args) {
        if (disable || disableInfo) {
            return;
        }
        _Log(enLogType.Info, null, _args);
    }

    public static void Error(params object[] _args) {
        if (disable || disableError) {
            return;
        }
        _Log(enLogType.Error, null, _args);
    }

    public static void Warning(params object[] _args) {
        if (disable || disableWarning) {
            return;
        }
        _Log(enLogType.Warning, null, _args);
    }

    public static void Fatal(params object[] _args) {
        if (disable || disableFatal) {
            return;
        }
        _Log(enLogType.Fatal, null, _args);
    }



    private static void _Log(enLogType _logType, object _owner = null, params object[] _args) {
        string logStr = _GetLogStr(_logType, _owner, _args);

        if (Application.isEditor) {
            if(_logType == enLogType.Info) {
                Debug.Log(logStr);
            }
            else if(_logType == enLogType.Error) {
                Debug.LogError(logStr);
            }
            else if(_logType == enLogType.Warning) {
                Debug.LogWarning(logStr);
            }
            else if(_logType == enLogType.Fatal) {
                Debug.LogError(logStr);
            }
        }

        if (logWriteType != VLogWriteType.None && !String.IsNullOrEmpty(logFilePath)) {
            
            switch (logWriteType) {
                case VLogWriteType.SplitWrite: {
                        string logFilePath = _GetLogFilePathByLogType(_logType);
                        long maxLogFileLength = _GetMaxLogFileLengthByLogType(_logType);
                        _WriteLogToFile(logStr, logFilePath, maxLogFileLength);
                    }
                    break;
                case VLogWriteType.SplitWriteAndSummery: {
                        string logFilePath = _GetLogFilePathByLogType(_logType);
                        long maxLogFileLength = _GetMaxLogFileLengthByLogType(_logType);
                        _WriteLogToFile(logStr, logFilePath, maxLogFileLength);

                        string summaryLogFilePath = _GetSummaryLogFilePath();
                        long maxSummaryFileLength = _GetMaxSummaryLogFileLength();
                        _WriteLogToFile(logStr, summaryLogFilePath, maxSummaryFileLength);
                    }
                    break;
                case VLogWriteType.Summery: {
                        string summaryLogFilePath = _GetSummaryLogFilePath();
                        long maxSummaryFileLength = _GetMaxSummaryLogFileLength();
                        _WriteLogToFile(logStr, summaryLogFilePath, maxSummaryFileLength);
                    }
                    break;
            }
        }
    }

    private static string _GetLogFilePathByLogType (enLogType _logType) {
        if(_logType == enLogType.Info) {
            return Path.Combine(logFilePath, logInfoFileName);
        }
        else if(_logType == enLogType.Error) {
            return Path.Combine(logFilePath, logErrorFileName);
        }
        else if(_logType == enLogType.Warning) {
            return Path.Combine(logFilePath, logWarningFileName);
        }
        else if(_logType == enLogType.Fatal) {
            return Path.Combine(logFilePath, logFatalFileName);
        }

        return "";
    }

    private static string _GetSummaryLogFilePath() {
        return Path.Combine(logFilePath, logSummaryFileName);
    }

    private static long _GetMaxLogFileLengthByLogType(enLogType _logType) {
        if (_logType == enLogType.Info) {
            return maxInfoLogFileBytes;
        } else if (_logType == enLogType.Error) {
            return maxErrorLogFileBytes;
        } else if (_logType == enLogType.Warning) {
            return maxWarningLogFileBytes;
        } else if (_logType == enLogType.Fatal) {
            return maxFatalLogFileBytes;
        }
        return -1;
    }

    private static long _GetMaxSummaryLogFileLength() {
        return maxSummaryFileBytes;
    }


    private static string _GetLogStr(enLogType _logType, object _owner = null, params object[] _args) {
        string logStr = String.Format("[{0}] " + DateTime.Now.ToString("T") + " ", _logType.ToString());
        if(_owner != null) {
            logStr += String.Format("[owner:'{0}'] - ", _owner);
        } else {
            logStr += " - ";
        }

        if (_args != null) {
            for (int i = 0; i < _args.Length; ++i) {
                if(_args[i] == null) {
                    logStr += "null ";
                } else {
                    logStr += _args[i] + " ";
                }
            }
        }

        return logStr;
    }


    private static void _WriteLogToFile(string _logStr, string _logFilePath, long _maxLogFileLength) {
        if (_maxLogFileLength > 0 && File.Exists(_logFilePath) && new FileInfo(_logFilePath).Length >= _maxLogFileLength) {
            File.Delete(_logFilePath);
        }

        _AppendTxtFile(_logStr + "\n", _logFilePath);
    }

    private static void _CreateParentDirectory(string _filePath) {
        string parentFolderName = new FileInfo(_filePath).DirectoryName;
        if (!Directory.Exists(parentFolderName))
            Directory.CreateDirectory(parentFolderName);
    }

    private static void _AppendTxtFile(string _txt, string _filePath) {
        try {
            //如果要写入的目录不存在
            _CreateParentDirectory(_filePath);
            using (FileStream fs = new FileStream(_filePath, FileMode.Append, FileAccess.Write)) {
                byte[] data = new UTF8Encoding().GetBytes(_txt);
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();
            }
        } catch (Exception e) {
            Debug.LogError("AppendTxtFile失败 " + _txt + "  " + e);
        }
    }

}
