using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace Unity.AndroidIcons
{
    public class AndroidIconsExtended : EditorWindow
    {
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

        [MenuItem("Window/Android/Icons")]
        public static void ShowExample()
        {
            var wnd = GetWindow<AndroidIconsExtended>();
            wnd.titleContent = new GUIContent("Android Icons");
        }

        PlatformIconKind[] m_IconKinds;
        AnimBool[] m_Folded;

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
            AndroidIconInformation.Load();
        }

        private void OnDisable()
        {
            AndroidIconInformation.Save();
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
                var icons = PlayerSettings.GetPlatformIcons(UnityEditor.Build.NamedBuildTarget.Android, kind);
                m_Folded[k].target = EditorGUILayout.Foldout(m_Folded[k].target, $"{kind} {icons.Length} icons");
                if (EditorGUILayout.BeginFadeGroup(m_Folded[k].faded))
                {
                    bool wasFirstTexture = true;
                    foreach (var icon in icons)
                    {
                        var key = kind.GetKey(icon);
                        var info = AndroidIconInformation.GetInfo(key);

                        GUILayout.BeginHorizontal();
                        GUILayout.BeginVertical();
                        GUILayout.Label($"{AndroidIconsInternal.GetIconDescription(icon)}");
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
                                EditorGUILayout.ObjectField(t, typeof(Texture2D), false);

                            }
                            EditorGUI.EndDisabledGroup();
                        }
                        else
                        {
                            if (wasFirstTexture)
                                EditorGUILayout.HelpBox("Texture not set in PlayerSettings.", MessageType.Info);
                            else
                                EditorGUILayout.HelpBox("Texture not set in PlayerSettings.\nIf above texture is set, it will be used instead.", MessageType.Info);
                        }
                        GUILayout.Space(15);
                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();

                        var rc = GUILayoutUtility.GetLastRect();
                        DrawOutline(rc);

                        wasFirstTexture = false;
                    }
                }

                EditorGUILayout.EndFadeGroup();
            }
            EditorGUILayout.EndScrollView();
        }
    }
}