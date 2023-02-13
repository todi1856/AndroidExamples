using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Android;
using System.Text;

public class AndroidResUpgraderPostprocessor : IPostGenerateGradleAndroidProject
{
    static readonly string AskAboutUpgradeResFolders = nameof(AskAboutUpgradeResFolders);
    static readonly string AndroidResPath = "Assets/Plugins/Android/res";
    static readonly string AndroidResLegacyDirectory = "res-legacy";
    static readonly string AndroidResLegacyPath = $"Assets/Plugins/Android/{AndroidResLegacyDirectory}";

    [InitializeOnLoadMethod]
    public static void ValidateResFolder()
    {
        if (!SessionState.GetBool(AskAboutUpgradeResFolders, true))
            return;

        if (!Directory.Exists(AndroidResPath))
            return;

        var result = EditorUtility.DisplayDialog($"Upgrade {AndroidResPath} folder ? ",
            $@"Starting Unity 2021.3 {AndroidResPath} folder can no longer be used for copying res files to gradle project, this has to be done either via android plugins or manually.
Proceed with upgrade? 
('{AndroidResPath}' will be moved into '{AndroidResLegacyPath}')",
            "Yes",
            "No and don't ask again in this Editor session");

        if (!result)
        {
            SessionState.SetBool(AskAboutUpgradeResFolders, false);
            return;
        }

        if (Directory.Exists(AndroidResLegacyPath))
        {
            EditorUtility.DisplayDialog(
                "Upgrade failed",
                @$"Cannot upgrade since '{AndroidResLegacyPath}' already exists, delete it.
or manually merge '{AndroidResPath}' into '{AndroidResLegacyPath}' 
and delete '{AndroidResPath}'.
Restart Editor afterwards.",
                "Ok");
            return;
        }

        AssetDatabase.RenameAsset(AndroidResPath, AndroidResLegacyDirectory);
    }

    public static void Log(string message)
    {
        UnityEngine.Debug.LogFormat(UnityEngine.LogType.Log, UnityEngine.LogOption.NoStacktrace, null, message);
    }

    public int callbackOrder { get { return 0; } }

    public void OnPostGenerateGradleAndroidProject(string path)
    {
        if (!Directory.Exists(AndroidResLegacyPath))
            return;

        var destination = Path.Combine(path, "src/main/res").Replace("\\", "/");
        var log = new StringBuilder();
        log.AppendLine("Legacy Android res files copying");
        log.AppendLine($"Copying '{AndroidResLegacyPath}' -> '{destination}':");
        RecursiveCopy(new DirectoryInfo(AndroidResLegacyPath),
            new DirectoryInfo(destination),
            new[] {".meta"},
            log);

        Log(log.ToString());
    }

    private static void RecursiveCopy(DirectoryInfo source, DirectoryInfo target, string[] ignoredExtensions, StringBuilder log)
    {
        if (Directory.Exists(target.FullName) == false)
            Directory.CreateDirectory(target.FullName);

        foreach (FileInfo fi in source.GetFiles())
        {
            if (ignoredExtensions.Contains(fi.Extension))
                continue;
            var destination = Path.Combine(target.ToString(), fi.Name);
            log.AppendLine($" {fi.FullName} -> {destination}");
            fi.CopyTo(destination, true);
        }

        foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
        {
            DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
            RecursiveCopy(diSourceSubDir, nextTargetSubDir, ignoredExtensions, log);
        }
    }
}