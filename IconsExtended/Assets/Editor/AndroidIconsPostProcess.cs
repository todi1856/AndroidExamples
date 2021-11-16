using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Android;
using UnityEngine;

namespace Unity.AndroidIcons
{
    class AndroidIconsPostProcess : IPostGenerateGradleAndroidProject
    {
        const int MaxTexturesPerIcon = 2;
        public int callbackOrder { get { return 0; } }

        private static string GetIconFileNameBase(PlatformIconKind iconKind, int iconIndex)
        {
            var n = iconKind.ToString();
            if (n.StartsWith("Adaptive"))
                return "ic_launcher_" + (iconIndex == 0 ? "background" : "foreground");
            else if (n.StartsWith("Round"))
                return "app_icon_round";
            else if (n.StartsWith("Legacy"))
                return "app_icon";

            throw new Exception($"Unknown platform icon kind '{iconKind}'.");
        }

        Texture2D Resize(Texture2D texture2D, int targetX, int targetY)
        {
            var rt = new RenderTexture(targetX, targetY, 0);
            RenderTexture.active = rt;
            Graphics.Blit(texture2D, rt);
            var result = new Texture2D(targetX, targetY, TextureFormat.ARGB32, false);
            result.ReadPixels(new Rect(0, 0, targetX, targetY), 0, 0);
            result.Apply();
            return result;
        }

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            AndroidIconInformation.Load();

            var kinds = PlayerSettings.GetSupportedIconKinds(UnityEditor.Build.NamedBuildTarget.Android);

            var rootFolder = Path.GetFullPath(Path.Combine(path, ".."));
            var resFolder = Path.Combine(rootFolder, "launcher/src/main/res");

            var details = new StringBuilder();
            foreach (var kind in kinds)
            {
                var icons = PlayerSettings.GetPlatformIcons(UnityEditor.Build.NamedBuildTarget.Android, kind);

                var lastValidTextures = new Texture2D[MaxTexturesPerIcon];
                foreach (var icon in icons)
                {
                    var key = kind.GetKey(icon);
                    var info = AndroidIconInformation.GetInfo(key);
                    if (!info.OverrideSize)
                        continue;
                    
                    var textures = icon.GetTextures();
                    if (MaxTexturesPerIcon < textures.Length)
                        throw new Exception("MaxTexturesPerIcon is too small increase it");
                    for (int i = 0; i < MaxTexturesPerIcon; i++)
                    {
                        if (i < textures.Length && textures[i] != null)
                            lastValidTextures[i] = textures[i];

                        if (lastValidTextures[i] == null)
                            continue;

                        var textureToUse = lastValidTextures[i];
                        var iconDescription = AndroidIconsInternal.GetIconDescription(icon);
                        var iconPath = Path.Combine(resFolder, $"mipmap-{iconDescription}", GetIconFileNameBase(kind, i ) + ".png");
                        // Only override icons Unity created
                        if (!File.Exists(iconPath))
                            continue;

                        var texturePath = AssetDatabase.GetAssetPath(textureToUse);
                        var strippedPath = iconPath.Substring(rootFolder.Length + 1).Replace("\\", "/");
                        details.AppendLine($"Using texture '{texturePath}' to generate '{strippedPath}' with size {info.OverridenWidth} x {info.OverridenHeight}");

#if UNITY_2023_3_OR_NEWER
                        AndroidIconsInternal.ExportTextureToImageFile(
                            textureToUse,
                            info.OverridenWidth,
                            info.OverridenHeight,
                            iconPath,
                            true,
                            false,
                            false);
#else
                        var newTexture = Resize(textureToUse, info.OverridenWidth, info.OverridenHeight);
                        File.WriteAllBytes(iconPath, newTexture.EncodeToPNG());
                        UnityEngine.Object.DestroyImmediate(newTexture);
#endif
                    }
                }
            }


            if (details.Length > 0)
                Debug.Log($"Overriding icons in '{rootFolder}':\n{details}");
        }
    }
}