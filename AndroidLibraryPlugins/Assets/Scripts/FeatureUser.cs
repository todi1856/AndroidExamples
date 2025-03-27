using UnityEngine;

public class FeatureUser : MonoBehaviour
{
    readonly string ControllerName = "com.company.feature.Controller";
    AndroidJavaClass m_Class;
    void Start()
    {
        m_Class = new AndroidJavaClass(ControllerName);
    }

    private void OnGUI()
    {
        GUILayout.Space(100);
        GUI.skin.label.fontSize = 30;

        if (m_Class != null)
        {
            GUILayout.Label($"{ControllerName}.getFoo() returns " + m_Class.CallStatic<string>("getFoo"));
        }
        else
        {
            GUILayout.Label($"{ControllerName} was not found?");
        }
    }
}
