using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using UnityEditor;
using UnityEditor.Android;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.UIElements;
using static System.Net.Mime.MediaTypeNames;

public class AndroidIconsExtended : EditorWindow
{
    private static PropertyInfo s_IconDescriptionProperty;
    private static MethodInfo s_ExportTextureToImageFile;

    [Serializable]
    public class IconInformation
    {
        public bool OverrideSize;
        public int OverridenWidth;
        public int OverridenHeight;
    }

    class AutoIndent : IDisposable
    {
        int m_Indent;
        public AutoIndent(int indent)
        {
            m_Indent = indent;
            EditorGUI.indentLevel += m_Indent;
        }

        public void Dispose()
        {
            EditorGUI.indentLevel -= m_Indent;
        }
    }

    // extern static void ExportTextureToImageFile(Texture2D texture, int resx, int resy, string path, bool use_alpha, bool clear_alpha, bool use_indexed);
    MethodInfo ExportTextureToImageFile
    {
        get
        {
            if (s_ExportTextureToImageFile != null)
                return s_ExportTextureToImageFile;

            var iconUtilityName = "UnityEditor.Utils.IconUtility";
            var iconUtility = typeof(EditorApplication).Assembly.GetType(iconUtilityName);
            if (iconUtility == null)
                throw new Exception($"Failed to find '{iconUtilityName}' type.");

            s_ExportTextureToImageFile = iconUtility.GetMethod(nameof(ExportTextureToImageFile), BindingFlags.Static | BindingFlags.NonPublic);
            if (s_ExportTextureToImageFile == null)
                throw new Exception($"Failed to find '{nameof(ExportTextureToImageFile)}' in '{iconUtility.FullName}' type");
            return s_ExportTextureToImageFile;
        }
    }

    [MenuItem("Window/Android/Icons")]
    public static void ShowExample()
    {
        var wnd = GetWindow<AndroidIconsExtended>();
        wnd.titleContent = new GUIContent("Android Icons");
    }

    PlatformIconKind[] m_IconKinds;
    AnimBool[] m_Folded;
    Dictionary<string, IconInformation> m_IconInfo;
    Vector2 m_ScrollPosition;

    private void OnEnable()
    {
        m_IconKinds = PlayerSettings.GetSupportedIconKinds(UnityEditor.Build.NamedBuildTarget.Android);
        m_Folded = m_IconKinds.Select(i =>
        {
            var a = new AnimBool();
            a.valueChanged.AddListener(Repaint);
            return a;
        }).ToArray();
        m_IconInfo = new Dictionary<string, IconInformation>();
    }

    public string GetIconDescription(PlatformIcon icon)
    {
        if (s_IconDescriptionProperty == null)
        {
            var name = "description";
            s_IconDescriptionProperty = icon.GetType().GetProperty(name, BindingFlags.NonPublic | BindingFlags.Instance);
            if (s_IconDescriptionProperty == null)
                throw new Exception($"Failed to get '{name}' property from type '{icon.GetType().FullName}'");
        }
        return s_IconDescriptionProperty.GetValue(icon) as string;
    }

    private IconInformation GetIconInformation(string key)
    {
        if (m_IconInfo.TryGetValue(key, out var value))
            return value;
        var newInfo = new IconInformation();
        m_IconInfo.Add(key, newInfo);
        return newInfo;

    }
    public void DrawOutline(Rect rc)
    {
        if (rc.width < 0 || rc.height < 0)
            return;
        var orgColor = GUI.color;
        GUI.color = Color.black;

        var thickness = 1;
        GUI.DrawTexture(new Rect(rc.x, rc.y, rc.width, thickness), EditorGUIUtility.whiteTexture);
        GUI.DrawTexture(new Rect(rc.x, rc.y + rc.height, rc.width, thickness), EditorGUIUtility.whiteTexture);

        GUI.DrawTexture(new Rect(rc.x, rc.y, thickness, rc.height), EditorGUIUtility.whiteTexture);
        GUI.DrawTexture(new Rect(rc.x + rc.width, rc.y, thickness, rc.height), EditorGUIUtility.whiteTexture);
        GUI.color = orgColor;
    }

    public void OnGUI()
    {
        EditorGUILayout.LabelField("Android Icons Extended", EditorStyles.boldLabel);

        m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition);
        for (int k = 0; k < m_IconKinds.Length; k++)
        {
            using var indent1 = new AutoIndent(1);
            var kind = m_IconKinds[k];
            var icons = PlayerSettings.GetPlatformIcons(BuildTargetGroup.Android, kind);
            m_Folded[k].target = EditorGUILayout.Foldout(m_Folded[k].target, $"{kind} {icons.Length} icons");
            if (EditorGUILayout.BeginFadeGroup(m_Folded[k].faded))
            {
                foreach (var icon in icons)
                {
                    var key = $"{kind}:{GetIconDescription(icon)}";
                    var info = GetIconInformation(key);

                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    GUILayout.Label($"{GetIconDescription(icon)}");
                    EditorGUI.indentLevel++;
                    info.OverrideSize = EditorGUILayout.Toggle("Override", info.OverrideSize);
                    if (info.OverrideSize)
                    {
                        info.OverridenWidth = EditorGUILayout.IntSlider("Width", info.OverridenWidth, 1, 2048);
                        info.OverridenHeight = EditorGUILayout.IntSlider("Height", info.OverridenHeight, 1, 2048);
                    }
                    else
                    {
                        EditorGUI.BeginDisabledGroup(true);
                        info.OverridenWidth = EditorGUILayout.IntSlider("Width", icon.width, 1, 2048);
                        info.OverridenHeight = EditorGUILayout.IntSlider("Height", icon.height, 1, 2048);
                        EditorGUI.EndDisabledGroup();
                    }
                    EditorGUI.indentLevel--;
                    GUILayout.Space(15);
                    GUILayout.EndVertical();
                    var textures = icon.GetTextures();
                      
                    GUILayout.BeginVertical(GUILayout.Width(500));
                    if (textures != null && textures.Length > 0)
                    {
                        EditorGUI.BeginDisabledGroup(true);
                        for (int i = 0; i < textures.Length; i++)
                        {
                            var t = textures[i];
                            EditorGUILayout.ObjectField(t, typeof(Texture2D));

                        }
                        EditorGUI.EndDisabledGroup();
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Texture not set in PlayerSettings", MessageType.Info);
                    }
                    GUILayout.Space(15);
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();

                    var rc = GUILayoutUtility.GetLastRect();
                    DrawOutline(rc);
                }
            }

            EditorGUILayout.EndFadeGroup();

            
        }
        EditorGUILayout.EndScrollView();
    }
}