using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour
{
    Dictionary<string, Action> m_Buttons = new Dictionary<string, Action>();
    Vector2 m_ScrollView;

    void Start()
    {
        void AddButton(string buttonName, string sceneName)
        {
            m_Buttons[buttonName] = () =>
            {
                SceneManager.LoadScene(sceneName);
            };
        }

        AddButton("AndroidJavaClass Examples", "AndroidJavaClassExamples");
        AddButton("Set/Get Params to C#/Java", "PassingParameters");
        AddButton("Kotlin Examples", "KotlinExamples");        
    }

    void OnGUI()
    {
        Utilities.SetupUI(0.8f);
        var opts = new[] { GUILayout.Height(Screen.height), GUILayout.Width(Screen.width) };
        m_ScrollView = GUILayout.BeginScrollView(m_ScrollView, true, true, opts);
        GUILayout.BeginVertical();
        for (int i = 0; i < m_Buttons.Count; i++)
        {
            var key = m_Buttons.Keys.ToArray()[i];
            var value = m_Buttons.Values.ToArray()[i];

            if (GUILayout.Button(key, new[] { GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true) }))
            {
                value();
            }
        }

        GUILayout.Space(40);
        GUILayout.EndVertical();
        GUILayout.EndScrollView();

    }
}
