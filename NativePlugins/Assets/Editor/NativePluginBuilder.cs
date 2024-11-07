using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
using UnityEditor.Android;

public class NativePluginBuilder : EditorWindow
{
    private static readonly AndroidArchitecture[] Architectures = new[] { AndroidArchitecture.ARMv7, AndroidArchitecture.ARM64, AndroidArchitecture.X86, AndroidArchitecture.X86_64 };
    
    [SerializeField]
    private List<AndroidArchitecture> m_SelectedArchitectures = new List<AndroidArchitecture>(new[] { AndroidArchitecture.ARMv7, AndroidArchitecture.ARM64 });

    private string LastResult = string.Empty;

    private string SourceDirectory => Path.GetFullPath(Path.Combine(Application.dataPath, "Source~"));

    [MenuItem("Plugins/Show Builder")]
    
    static void Init()
    {
        NativePluginBuilder window = (NativePluginBuilder)EditorWindow.GetWindow(typeof(NativePluginBuilder));
        window.Show();
    }

    // Force users to see this window
    [InitializeOnLoadMethod]
    static void ShowWindow()
    {
        EditorApplication.delayCall += () =>
        {
            Init();
        };
    }

    string GetABI(AndroidArchitecture architecture)
    {
        switch(architecture)
        {
            case AndroidArchitecture.ARMv7: return "armeabi-v7a";
            case AndroidArchitecture.ARM64: return "arm64-v8a";
            case AndroidArchitecture.X86: return "x86";
            case AndroidArchitecture.X86_64: return "x86_64";
            default:
                throw new NotImplementedException(architecture.ToString());
        }
    }

    void DeleteDirectoryOrFile(string path)
    {
        if (Directory.Exists(path))
            Directory.Delete(path, true);
        if (File.Exists(path))
            File.Delete(path);
    }

    void Build()
    {
        var sourceDirectory = SourceDirectory;
        var pluginsDirectory = Path.Combine(Application.dataPath, "Plugins");

        AssetDatabase.DeleteAsset("Assets/Plugins");
        DeleteDirectoryOrFile(Path.Combine(sourceDirectory, "libs"));
        DeleteDirectoryOrFile(Path.Combine(sourceDirectory, "obj"));

        var abis = string.Join(" ", m_SelectedArchitectures.Select(a => GetABI(a)));
        var applicationMkContents = string.Join("\n", new[]
        {
            "APP_OPTIM        := release",
            $"APP_ABI          := {abis}",
            "APP_PLATFORM     := android-22",
            "APP_BUILD_SCRIPT := Android.mk"
        });
        File.WriteAllText(Path.Combine(sourceDirectory, "Application.mk"), applicationMkContents);
        var ndkBuild = Path.Combine(AndroidExternalToolsSettings.ndkRootPath, "ndk-build");
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            ndkBuild += ".cmd";
        }

        var result = Shell.RunProcess(ndkBuild, $"NDK_PROJECT_PATH=. NDK_APPLICATION_MK=Application.mk", sourceDirectory);
        var output = $"StdOut:\n{result.GetStandardOut()}\nStdErr:\n{result.GetStandardErr()}";
        if (result.GetExitCode() == 0)
        {
            Debug.Log(output);
            LastResult = "Success";
        }
        else
        {
            Debug.LogError(output);
            LastResult = "Failure. See Console for more details";
            return;
        }


        Directory.CreateDirectory(pluginsDirectory);
        foreach (var a in m_SelectedArchitectures)
        {
            Directory.CreateDirectory(Path.Combine(pluginsDirectory, GetABI(a)));
            var src = Path.Combine(sourceDirectory, "libs", GetABI(a), "libnative.so");
            var dst = Path.Combine(pluginsDirectory, GetABI(a), "libnative.so");
            File.Copy(src, dst, true);
        }

        AssetDatabase.Refresh();

        foreach (var a in m_SelectedArchitectures)
        {
            var importer = (PluginImporter) PluginImporter.GetAtPath($"Assets/Plugins/{GetABI(a)}/libnative.so");
            importer.SetCompatibleWithAnyPlatform(false);
            importer.SetCompatibleWithPlatform(BuildTarget.Android, true);
            importer.SetPlatformData(BuildTarget.Android, "CPU", a.ToString());
            importer.SaveAndReimport();
        }
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Target Architectures", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        foreach (var a in Architectures)
        {
            EditorGUI.BeginChangeCheck();
            var selected = GUILayout.Toggle(m_SelectedArchitectures.Contains(a), a.ToString());
            if (EditorGUI.EndChangeCheck())
            {
                if (!selected)
                    m_SelectedArchitectures.Remove(a);
                else
                    m_SelectedArchitectures.Add(a);
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.Label($"Source Directory=");
        GUILayout.TextField(SourceDirectory);
        GUILayout.EndHorizontal();
        
        if (GUILayout.Button("Open Source Directory"))
        {
            EditorUtility.RevealInFinder(SourceDirectory);
        }
        EditorGUI.BeginDisabledGroup(m_SelectedArchitectures.Count == 0);
        if (GUILayout.Button("Build Native Plugins"))
        {
            Build();
        }
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Result: " + LastResult, EditorStyles.boldLabel);
    }
}