using System.Runtime.CompilerServices;

namespace VH.Services
{
    public interface ILogService
    {
        bool logsEnabled { get; set; }

        void Log(object message, [CallerLineNumber]int lineNumber = -1, [CallerMemberName] string methodName = null, [CallerFilePath] string fileName = null);
        void LogWarning(object message, [CallerLineNumber]int lineNumber = -1, [CallerMemberName] string methodName = null, [CallerFilePath] string fileName = null);
        void LogError(object message, [CallerLineNumber]int lineNumber = -1, [CallerMemberName] string methodName = null, [CallerFilePath] string fileName = null);

        void Log(UnityEngine.LogType type, object message, [CallerLineNumber]int lineNumber = -1, [CallerMemberName] string methodName = null, [CallerFilePath] string fileName = null);
    }
}
