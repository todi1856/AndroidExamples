#if UNITY_2021_2_OR_NEWER
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Android;
using System.Text;

public class AndroidPluginsUpgraderfPostprocessor : IPostGenerateGradleAndroidProject
{
    internal static readonly string PluginsAndroid = "Assets/Plugins/Android";
    class UpgradableItem
    {
        public string Name { get; }
        public string Path { get; }
        public string LegacyDirectory { get; }
        public string LegacyPath { get; }
        
        public UpgradableItem(string directory, string legacyDirectory)
        {
            Name = directory;
            Path = $"{PluginsAndroid}/{directory}";
            LegacyDirectory = legacyDirectory;
            LegacyPath = $"{PluginsAndroid}/{LegacyDirectory}";
        }
    }

    static readonly string AskAboutUpgradeFolders = nameof(AskAboutUpgradeFolders);
    static readonly UpgradableItem[] UpgradableItems = new [] 
    {
        new UpgradableItem("res", "res-legacy"),
        new UpgradableItem("assets", "assets-legacy")
    };

    [InitializeOnLoadMethod]
    public static void ValidateResFolder()
    {
        if (!SessionState.GetBool(AskAboutUpgradeFolders, true))
            return;

        bool upgradableItemsExist = UpgradableItems.Any(s => Directory.Exists(s.Path));
        if (!upgradableItemsExist)
            return;

        var ugpradePaths = new StringBuilder();
        foreach (var u in UpgradableItems)
        {
            ugpradePaths.AppendLine($"'{u.Path}' will be moved into '{u.LegacyPath}')");
        }

        var result = EditorUtility.DisplayDialog($"Upgrade {PluginsAndroid} folder ? ",
            $@"Starting Unity 2021.2 {PluginsAndroid} folder can no longer be used for copying '{string.Join(", ", UpgradableItems.Select(u => u.Name))}' files to gradle project, this has to be done either via android plugins or manually.
Proceed with upgrade? 
{ugpradePaths}",
            "Yes",
            "No and don't ask again in this Editor session");

        if (!result)
        {
            SessionState.SetBool(AskAboutUpgradeFolders, false);
            return;
        }

        foreach (var u in UpgradableItems)
        {
            if (!Directory.Exists(u.Path))
                continue;

            if (Directory.Exists(u.LegacyPath))
            {
                EditorUtility.DisplayDialog(
                    "Upgrade failed",
                    @$"Cannot upgrade since '{u.LegacyPath}' already exists, delete it.
or manually merge '{u.Path}' into '{u.LegacyPath}' 
and delete '{u.Path}'.
Restart Editor afterwards.",
                    "Ok");
                return;
            }


            AssetDatabase.RenameAsset(u.Path, u.LegacyDirectory);
        }
    }

    public static void Log(string message)
    {
        UnityEngine.Debug.LogFormat(UnityEngine.LogType.Log, UnityEngine.LogOption.NoStacktrace, null, message);
    }

    public int callbackOrder { get { return 0; } }

    public void OnPostGenerateGradleAndroidProject(string path)
    {
        foreach (var u in UpgradableItems)
        {
            if (!Directory.Exists(u.LegacyPath))
                continue;

            var destination = Path.Combine(path, $"src/main/{u.Name}").Replace("\\", "/");
            var log = new StringBuilder();
            log.AppendLine("Legacy Android res files copying");
            log.AppendLine($"Copying '{u.LegacyPath}' -> '{destination}':");
            RecursiveCopy(new DirectoryInfo(u.LegacyPath),
                new DirectoryInfo(destination),
                new[] { ".meta" },
                log);

            Log(log.ToString());
        }
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
#endif