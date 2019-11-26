using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            [Serializable]
            private struct RawCodegenLog
            {
#pragma warning disable 649
                public string time;
                public string level;
                public string logger;
                public string message;
                public string exception;
#pragma warning restore
            }

            public CodegenLog(string rawLog)
            {
                var rawCodegenLog = JsonUtility.FromJson<RawCodegenLog>(rawLog);

                time = rawCodegenLog.time;
                level = (CodegenLogLevel) Enum.Parse(typeof(CodegenLogLevel), rawCodegenLog.level);
                logger = rawCodegenLog.logger;
                message = rawCodegenLog.message;
                exception = rawCodegenLog.exception;
            }
        }

        public static List<CodegenLog> ProcessCodegenLogs(string logPath)
        {
            return File.ReadLines(logPath)
                .Select(line => new CodegenLog(line))
                .ToList();
        }
    }
}
