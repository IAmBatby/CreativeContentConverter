using UnityEngine;
using UnityEditor;
using System;

namespace LethalSDK.Editor
{
    public class Lethal_AssetBundleBuilderWindow : EditorWindow
    {
        private static string assetBundleDirectoryKey = "LethalSDK_AssetBundleBuilderWindow_assetBundleDirectory";
        private static string compressionModeKey = "LethalSDK_AssetBundleBuilderWindow_compressionMode";
        private static string _64BitsModeKey = "LethalSDK_AssetBundleBuilderWindow_64BitsMode";
        
        string assetBundleDirectory = string.Empty;
        compressionOption compressionMode = compressionOption.NormalCompression;
        bool _64BitsMode;

        [MenuItem("LethalSDK/AssetBundle Builder", false, 100)]
        public static void ShowWindow()
        {
            Lethal_AssetBundleBuilderWindow window = GetWindow<Lethal_AssetBundleBuilderWindow>("AssetBundle Builder");
            window.minSize = new Vector2(295, 133);
            window.maxSize = new Vector2(295, 133);
            window.LoadPreferences();
        }

        void OnGUI()
        {
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Output Path", "The directory where the asset bundles will be saved."), GUILayout.Width(84));
            assetBundleDirectory = EditorGUILayout.TextField(assetBundleDirectory, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            GUILayout.Label("Options", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Compression Mode", "Select the compression option for the asset bundle. Faster the compression is, faster the assets will load and less CPU it will use, but the Bundle will be bigger."), GUILayout.Width(145));
            compressionMode = (compressionOption)EditorGUILayout.EnumPopup(compressionMode, GUILayout.Width(140));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("64 Bits Asset Bundle (Not recommended)", "Better performances but incompatible with 32 bits computers."), GUILayout.Width(270));
            _64BitsMode = EditorGUILayout.Toggle(_64BitsMode);
            GUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Build AssetBundles", GUILayout.Width(240)))
            {
                BuildAssetBundles();
            }
            if (GUILayout.Button("Reset", GUILayout.Width(45)))
            {
                ClearPreferences();
            }
            GUILayout.EndHorizontal();
        }
        void ClearPreferences()
        {
            EditorPrefs.DeleteKey(assetBundleDirectoryKey);
            EditorPrefs.DeleteKey(compressionModeKey);
            EditorPrefs.DeleteKey(_64BitsModeKey);
            LoadPreferences();
        }

        void BuildAssetBundles()
        {
            if (!System.IO.Directory.Exists(assetBundleDirectory))
            {
                System.IO.Directory.CreateDirectory(assetBundleDirectory);
            }

            BuildAssetBundleOptions options = BuildAssetBundleOptions.None;
            switch (compressionMode)
            {
                case compressionOption.NormalCompression:
                    options = BuildAssetBundleOptions.None;
                    break;
                case compressionOption.FastCompression:
                    options = BuildAssetBundleOptions.ChunkBasedCompression;
                    break;
                case compressionOption.Uncompressed:
                    options = BuildAssetBundleOptions.UncompressedAssetBundle;
                    break;
                default:
                    options = BuildAssetBundleOptions.None;
                    break;
            }
            BuildTarget target = _64BitsMode ? BuildTarget.StandaloneWindows64 : BuildTarget.StandaloneWindows;

            if(assetBundleDirectory != null || assetBundleDirectory.Length != 0 || assetBundleDirectory != string.Empty)
            {
                AssetBundleManifest manifest = null;
                try
                {
                    manifest = BuildPipeline.BuildAssetBundles(assetBundleDirectory, options, target);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                }
                if (manifest != null)
                {
                    Debug.Log("AssetBundles built successfully.");
                }
                else
                {
                    Debug.LogError("Cannot build AssetBundles.");
                }
            }
            else
            {
                Debug.LogError("AssetBundles path cannot be blank.");
            }

        }
        void OnLostFocus()
        {
            SavePreferences();
        }

        void OnDisable()
        {
            SavePreferences();
        }

        void LoadPreferences()
        {
            assetBundleDirectory = EditorPrefs.GetString(assetBundleDirectoryKey, "Assets/AssetBundles");
            compressionMode = (compressionOption)EditorPrefs.GetInt(compressionModeKey, (int)compressionOption.NormalCompression);
            _64BitsMode = EditorPrefs.GetBool(_64BitsModeKey, false);
        }

        void SavePreferences()
        {
            EditorPrefs.SetString(assetBundleDirectoryKey, assetBundleDirectory);
            EditorPrefs.SetInt(compressionModeKey, (int)compressionMode);
            EditorPrefs.SetBool(_64BitsModeKey, _64BitsMode);
        }

        enum compressionOption
        {
            NormalCompression = 0,
            FastCompression = 1,
            Uncompressed = 2
        }
    }
}