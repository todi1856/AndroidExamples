using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

class AndroidResUpgraderPrebuild : IPreprocessBuildWithReport
{
    static readonly string AndroidResDirectory = "Assets/Plugins/Android/res";
    static readonly string AndroidResLegacyDirectory = "Assets/Plugins/Android/res-legacy";
    public int callbackOrder { get { return 0; } }
    public void OnPreprocessBuild(BuildReport report)
    {
        if (!Directory.Exists(AndroidResDirectory))
            return;

        AssetDatabase.RenameAsset(AndroidResDirectory, AndroidResLegacyDirectory);
    }
}