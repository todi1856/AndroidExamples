using UnityEngine;

class Utilities
{
    public static void Log(string message)
    {
        var go = GameObject.Find("LogReceiver");
        if (go != null)
            go.SendMessage("OnLogMessage", message);
    }

    public static void SetupUI(float multiplier = 1.0f)
    {
        var m = (int)((Screen.height / 40.0f) * multiplier);
        GUI.skin.button.fontSize = m;
        GUI.skin.label.fontSize = m;
        GUI.skin.textField.fontSize = m;
        GUI.skin.toggle.fontSize = m;
    }
}