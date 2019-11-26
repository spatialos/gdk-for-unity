using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    internal static class CodegenLogUtils
    {
        internal enum CodegenLogLevel
        {
            TRACE,
            DEBUG,
            INFO,
            WARN,
            ERROR,
            FATAL,
            OFF
        }

        internal struct CodegenLog
        {
            public readonly string time;
            public readonly CodegenLogLevel level;
            public readonly string logger;
            public readonly string message;
            public readonly string exception;
            public bool isException => !string.IsNullOrEmpty(exception);

            public CodegenLog(RawCodegenLog rawLog)
            {
                time = rawLog.time;
                level = (CodegenLogLevel) Enum.Parse(typeof(CodegenLogLevel), rawLog.level);
                logger = rawLog.logger;
                message = rawLog.message;
                exception = rawLog.exception;
            }
        }

        [Serializable]
        internal struct RawCodegenLog
        {
#pragma warning disable 649
            public string time;
            public string level;
            public string logger;
            public string message;
            public string exception;
#pragma warning restore
        }

        public static List<CodegenLog> ProcessCodegenLogs(string logPath)
        {
            string line;
            var file = new StreamReader(logPath);

            var codegenOutput = new List<CodegenLog>();

            while ((line = file.ReadLine()) != null)
            {
                var rawCodegenLog = JsonUtility.FromJson<RawCodegenLog>(line);
                codegenOutput.Add(new CodegenLog(rawCodegenLog));
            }

            file.Close();
            return codegenOutput;
        }
    }
}
