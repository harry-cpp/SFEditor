using System;
using Xwt;
using System.IO;

namespace SFEditor
{
    public enum Platform
    {
        Windows,
        Linux,
        Other,
        NotSet
    }

    public static class Settings
    {
        public static string SettingsFile = AppDomain.CurrentDomain.BaseDirectory + "Settings.ini";

        public static Platform CurrentPlatform = Platform.NotSet;
        public static ToolkitType[] SuportedPlatformToolkits;

        private static ToolkitType _toolKitType;

        public static void Init(Platform platform, ToolkitType DefaultToolKit, ToolkitType[] supportedToolkits)
        {
            CurrentPlatform = platform;
            SuportedPlatformToolkits = supportedToolkits;

            bool tkset = false;

            try
            {
                var settings = File.ReadAllLines(SettingsFile);
                foreach (var setting in settings)
                {
                    string[] split = setting.Split('=');
                    if (split[0] == "Toolkit")
                    {
                        try
                        {
                            _toolKitType = (ToolkitType)Enum.Parse(typeof(ToolkitType), split[1]);
                            tkset = true;
                        }
                        catch {
                        }
                    }
                }
            }
            catch
            {
            }

            if(!tkset)
                _toolKitType = DefaultToolKit;
        }

        public static ToolkitType GetToolkit()
        {
            return _toolKitType;
        }

        public static void SetToolkit(ToolkitType toolkitType)
        {
            _toolKitType = toolkitType;
        }

        public static void Save()
        {
            string text = "[Settings]\r\nToolkit=" + _toolKitType + "\r\n\r\n";

            using (Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Resources.SettingsText"))
            using (StreamReader reader = new StreamReader(stream))
                text += reader.ReadToEnd();

            File.WriteAllText(SettingsFile, text);
        }
    }
}

