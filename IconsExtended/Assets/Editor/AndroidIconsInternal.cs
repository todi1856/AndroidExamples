using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace Unity.AndroidIcons
{
    public static class AndroidIconsInternal
    {
        private static PropertyInfo s_IconDescriptionProperty;
        private static MethodInfo s_ExportTextureToImageFile;

        public static string GetIconDescription(PlatformIcon icon)
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

        // extern static void ExportTextureToImageFile(Texture2D texture, int resx, int resy, string path, bool use_alpha, bool clear_alpha, bool use_indexed);
        static MethodInfo ExportTextureToImageFileFunction
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

        public static void ExportTextureToImageFile(Texture2D texture, int resx, int resy, string path, bool use_alpha, bool clear_alpha, bool use_indexed)
        {
            ExportTextureToImageFileFunction.Invoke(null, new object[] { texture, resx, resy, path, use_alpha, clear_alpha, use_indexed });
        }

        public static string GetKey(this PlatformIconKind kind, PlatformIcon icon)
        {
            return $"{kind}:{GetIconDescription(icon)}";
        }
    }
}