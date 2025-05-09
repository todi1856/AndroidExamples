using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogReceiver : MonoBehaviour
{
    List<string> m_Logs = new List<string>();
    Vector2 m_ScrollView;
    public void OnLogMessage(string message)
    {
        m_Logs.Add(message);
        Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, "{0}", message);
    }

    public void OnGUI()
    {

        var opts = new[] { GUILayout.Height(Screen.height), GUILayout.Width(Screen.width) };
        m_ScrollView = GUILayout.BeginScrollView(m_ScrollView, true, true, opts);

        Utilities.SetupUI(1.0f);
        if (GUILayout.Button("Back"))
            SceneManager.LoadScene("Startup");

        Utilities.SetupUI(0.4f);
        foreach (var log in m_Logs)
            GUILayout.TextField(log);
        GUILayout.EndScrollView();



    }
}
