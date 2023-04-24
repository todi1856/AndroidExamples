using System;
using UnityEditor.Android;
using UnityEditor;
using UnityEngine;

public class PostProcessor : AndroidProjectFilesModifier
{
    const string CustomSourceFileSrc = "ExtraSourceFiles/MyFile.cpp";
    const string CustomSourceFileDst = "unityLibrary/src/main/cpp/GameActivity/CustomFolder/MyFile.cpp";

    public override AndroidProjectFilesModifierContext Setup()
    {
        var ctx = new AndroidProjectFilesModifierContext();
        ctx.Dependencies.DependencyFiles = new[]
        {
            CustomSourceFileSrc
        };
        ctx.AddFileToCopy(CustomSourceFileSrc, CustomSourceFileDst);

        return ctx;
    }

    public override void OnModifyAndroidProjectFiles(AndroidProjectFiles projectFiles)
    {
    }
}
