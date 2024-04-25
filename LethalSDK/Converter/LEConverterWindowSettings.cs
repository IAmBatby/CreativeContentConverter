using LethalLevelLoader;
using LethalSDK.Conversions;
using LethalSDK.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;

namespace LethalSDK.Converter
{
    [CreateAssetMenu(fileName = "ConversionSettings", menuName = "LethalSDK/Conversion Settings")]
    public class LEConverterWindowSettings : ScriptableObject
    {
        public string modsRootDirectory = string.Empty;
        public string assetRipGameRootDirectory = string.Empty;

        [Space(15)]

        public Sprite scrapItemIcon;
        public Sprite handIcon;
        public GameObject itemDropshipPrefab;
        public GameObject waterSurfacePrefab;
        public AudioMixerGroup diageticMasterMixer;

        [HideInInspector] public enum AssetBundleAssignmentSetting { AuthorName, ModName };
        [HideInInspector] public AssetBundleAssignmentSetting assetBundleAssignmentSetting;
        [HideInInspector] public enum BundleVariantToggle { Lethalbundle, None }
        [HideInInspector] public BundleVariantToggle bundleVariantToggle;

        [HideInInspector] public Moon moon;
        [HideInInspector] public Scrap scrap;
        [HideInInspector] public List<Moon> moonList = new List<Moon>();
        [HideInInspector] public List<Scrap> scrapList = new List<Scrap>();
        [HideInInspector] public string organiserSelectedDirectory = string.Empty;
        [HideInInspector] public string organiserTargetDirectory = string.Empty;
        [HideInInspector] public AssetType organiserSelectedAssetType;

        [HideInInspector] public List<(string, string)> primaryDirectoriesToBuild = new List<(string, string)>();
        [HideInInspector] public List<(string, string)> subDirectoriesToBuild = new List<(string, string)>();
        [HideInInspector] public int moonListCount;
        [HideInInspector] public int scrapListCount;
        [HideInInspector] public string selectedVariantToClear;

        public Color defaultButtonColor = new Color(88, 88, 88, 255);
        public Color selectedButtonColor = new Color(56, 158, 207, 255);
        public Color disabledButtonColor = new Color(42, 42, 42, 255);

        [HideInInspector] public List<ExtendedMod> extendedModList = new List<ExtendedMod>();

        public void GetExtendedMods()
        {
            extendedModList.Clear();

            string[] extendedModGuids = AssetDatabase.FindAssets("t:ExtendedMod");
            foreach (string guid in extendedModGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                extendedModList.Add(AssetDatabase.LoadAssetAtPath<ExtendedMod>(path));

            }
        }

        public static void CreateNewSettings()
        {
            if (LEConverterWindow._settings == null)
            {
                Debug.Log("Creating New Converter Settings!");
                if (AssetDatabase.IsValidFolder("Assets/LethalCompany/Game" + "/Scenes"))
                    EditorSceneManager.OpenScene("Assets/LethalCompany/Game" + "/Scenes/" + "Level1Experimentation.unity");

                string location = Assembly.GetExecutingAssembly().Location;
                location = location.Remove(location.IndexOf("LethalSDK.dll"));
                location = location.Substring(location.IndexOf("Assets"));
                Debug.Log(location);
                LEConverterWindowSettings newConverterSettings = ScriptableObject.CreateInstance<LEConverterWindowSettings>();
                newConverterSettings.name = "ConverterSettings";

                AssetDatabase.CreateAsset(newConverterSettings, location + "/" + newConverterSettings.name + ".asset");

                LEConverterWindowSettings converterSettings = (LEConverterWindowSettings)AssetDatabase.LoadAssetAtPath(location + "/" + "ConverterSettings" + ".asset", typeof(LEConverterWindowSettings));

                Debug.Log("Converter Settings Is: " + converterSettings);

                converterSettings.defaultButtonColor = new Color(88f / 255, 88f / 255, 88f / 255, 255f / 255);
                converterSettings.selectedButtonColor = new Color(56f / 255, 158f / 255, 207f / 255, 255f / 255);
                converterSettings.disabledButtonColor = new Color(42f / 255, 42f / 255, 42f / 255, 255f / 255);
                converterSettings.selectedVariantToClear = "lem";


                if (AssetDatabase.IsValidFolder("Assets/LethalCompany/Mods"))
                    converterSettings.modsRootDirectory = "Assets/LethalCompany/Mods";
                if (AssetDatabase.IsValidFolder("Assets/LethalCompany/Game"))
                    converterSettings.assetRipGameRootDirectory = "Assets/LethalCompany/Game";

                foreach (AudioMixerGroup audioMixer in SelectableLevelConverter.audioMixers)
                {
                    if (audioMixer.name == "Master" && audioMixer.audioMixer.name == "Diagetic")
                    {
                        Debug.Log("Found Master Diagetic MixerGroup!");
                        converterSettings.diageticMasterMixer = audioMixer;
                    }
                }

                foreach (Sprite sprite in SelectableLevelConverter.sprites)
                {
                    if (sprite.name == "ScrapItemIcon2" && AssetDatabase.GetAssetPath(sprite).Contains(converterSettings.assetRipGameRootDirectory))
                    {
                        Debug.Log("Found Scrap Item Icon Sprite!");
                        converterSettings.scrapItemIcon = sprite;
                    }
                    if (sprite.name == "HandIcon" && AssetDatabase.GetAssetPath(sprite).Contains(converterSettings.assetRipGameRootDirectory))
                    {
                        Debug.Log("Found Hand Icon Sprite!");
                        converterSettings.handIcon = sprite;
                    }
                }

                foreach (GameObject prefab in SelectableLevelConverter.prefabs)
                    if (prefab.name == "Water" && AssetDatabase.GetAssetPath(prefab).Contains("LethalExpansion"))
                    {
                        Debug.Log("Found WaterSurface Prefab!");
                        converterSettings.waterSurfacePrefab = prefab;
                    }

                EditorUtility.SetDirty(converterSettings);
                AssetDatabase.SaveAssetIfDirty(converterSettings);
                AssetDatabase.Refresh();
                EditorApplication.QueuePlayerLoopUpdate();



                GameObject itemShipLandingPosition = GameObject.FindGameObjectWithTag("ItemShipLandingNode");
                if (itemShipLandingPosition != null)
                {
                    Debug.Log("Found Item Ship: " + itemShipLandingPosition.transform.parent.name);
                    GameObject prefab = PrefabUtility.SaveAsPrefabAsset(itemShipLandingPosition.transform.parent.gameObject, location + "/" + "ItemDropship" + ".prefab");
                    converterSettings = (LEConverterWindowSettings)AssetDatabase.LoadAssetAtPath(location + "/" + "ConverterSettings" + ".asset", typeof(LEConverterWindowSettings));
                    converterSettings.itemDropshipPrefab = prefab;
                }

                Debug.Log("Finishing!");

                EditorUtility.SetDirty(converterSettings);
                AssetDatabase.SaveAssetIfDirty(converterSettings);
                AssetDatabase.Refresh();
                EditorApplication.QueuePlayerLoopUpdate();
                Selection.activeObject = converterSettings;
            }
            else
            {
                Debug.LogError("Cannot create new Converter Settings as one already exists!");
            }
        }
    }
}
