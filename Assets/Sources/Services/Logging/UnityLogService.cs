using System.Runtime.CompilerServices;
using UnityEngine;

namespace VH.Services
{
    public class UnityLogService : ILogService
    {
        public bool logsEnabled { get; set; }

        public UnityLogService()
        {
#if UNITY_EDITOR
            logsEnabled = true;
#endif
        }

        public void Log(object message, [CallerLineNumber]int lineNumber = -1, [CallerMemberName] string methodName = null, [CallerFilePath] string fileName = null)
        {
            if (!logsEnabled) return;
            Debug.Log($"{message}");
        }


        public void Log(LogType type, object message, [CallerLineNumber]int lineNumber = -1, [CallerMemberName] string methodName = null, [CallerFilePath] string fileName = null)
        {
            if (!logsEnabled) return;

            switch (type)
            {
                case LogType.Error:
                    Debug.LogError($"{message}");
                    break;
                case LogType.Warning:
                    Debug.LogWarning($"{message}");
                    break;
                case LogType.Log:
                    Debug.Log($"{message}");
                    break;

            }

        }

        public void LogError(object message, [CallerLineNumber]int lineNumber = -1, [CallerMemberName] string methodName = null, [CallerFilePath] string fileName = null)
        {
            if (!logsEnabled) return;
            Debug.LogError($"{message}");
        }

        public void LogWarning(object message, [CallerLineNumber]int lineNumber = -1, [CallerMemberName] string methodName = null, [CallerFilePath] string fileName = null)
        {
            if (!logsEnabled) return;
            Debug.LogWarning($"{message}");
        }


    }
}
