using LethalSDK.Converter;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;

namespace LethalSDK
{
    public class SubWindow : EditorWindow
    {
        public static LEConverterWindow MainWindow;
        public static LEConverterWindowSettings _settings;
        public static LEConverterWindowSettings MainWindowSettings
        {
            get
            {
                if (_settings == null)
                {
                    string[] guids = AssetDatabase.FindAssets("t:LEConverterWindowSettings");
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    _settings = AssetDatabase.LoadAssetAtPath<LEConverterWindowSettings>(path);
                }
                return (_settings);
            }
        }

        public virtual void LoadSubWindow(LEConverterWindow leConverterWindow)
        {
            MainWindow = leConverterWindow;
        }

    }
}
