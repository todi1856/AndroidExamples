using UnityEngine;

class Utilities
{
    public static void Log(string message)
    {
        Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, message);
    }
}