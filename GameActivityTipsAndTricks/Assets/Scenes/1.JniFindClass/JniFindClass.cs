using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class JniFindClass : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern string GetData();

    string m_Data;

    // Start is called before the first frame update
    void Start()
    {
        m_Data = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        var opt = new[] { GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true) };
        GUILayout.BeginVertical(GUILayout.Height(Screen.height), GUILayout.Width(Screen.width));
        if (GUILayout.Button("Test 1", opt))
        {
            m_Data = GetData();
        }
        if (GUILayout.Button("Test 2", opt))
        {

        }

        GUILayout.Label($"Data: '{m_Data}'", opt);


        GUILayout.EndVertical();
    }

}
