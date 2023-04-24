using UnityEngine;

public class SendMessageReceiver : MonoBehaviour
{
    public void SendMessageFromCpp(string message)
    {
        Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, message);
    }
}
