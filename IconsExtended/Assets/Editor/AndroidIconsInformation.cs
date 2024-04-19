using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Unity.AndroidIcons
{
    [Serializable]
    public class AndroidIcon
    {
        public bool OverrideSize;
        public int OverridenWidth;
        public int OverridenHeight;
    }


    public class AndroidIconInformation
    {
        [Serializable]
        public class SerializedIcons
        {
            public string[] Keys;
            public AndroidIcon[] Values;
        }

        const string SettingsPath = "ProjectSettings/AndroidIcons.json";

        static Dictionary<string, AndroidIcon> s_IconInfo;

        public static void Load()
        {
            if (s_IconInfo != null)
                return;
            s_IconInfo = new Dictionary<string, AndroidIcon>();
            if (!File.Exists(SettingsPath))
                return;
            var contents = File.ReadAllText(SettingsPath);
            var icons = JsonUtility.FromJson<SerializedIcons>(contents);
            if (icons.Keys.Length != icons.Values.Length)
                throw new Exception($"Keys/Values count mismatch in '{SettingsPath}'");
            for (int i = 0; i < icons.Keys.Length; i++)
                s_IconInfo.Add(icons.Keys[i], icons.Values[i]);
        }

        public static void Save()
        {
            if (s_IconInfo == null)
                return;
            var icons = new SerializedIcons()
            {
                Keys = s_IconInfo.Keys.ToArray(),
                Values = s_IconInfo.Values.ToArray()
            };

            File.WriteAllText(SettingsPath, JsonUtility.ToJson(icons));
        }


        public static AndroidIcon GetInfo(string key)
        {
            if (s_IconInfo == null)
                throw new Exception($"Android icon information was not yet loaded");

            if (s_IconInfo.TryGetValue(key, out var value))
                return value;
            var newInfo = new AndroidIcon();
            s_IconInfo.Add(key, newInfo);
            return newInfo;
        }
    }
}