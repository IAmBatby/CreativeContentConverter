using LethalLevelLoader;
using LethalSDK.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace LethalSDK.Converter
{
    [CreateAssetMenu(fileName = "ConversionSettings", menuName = "LethalSDK/Conversion Settings")]
    public class LEConverterWindowSettings : ScriptableObject
    {
        public string modsRootDirectory = string.Empty;
        public string assetRipGameRootDirectory = string.Empty;
        public Moon moon;
        public Scrap scrap;
        public List<Moon> moonList = new List<Moon>();
        public List<Scrap> scrapList = new List<Scrap>();
        public enum AssetBundleAssignmentSetting { AuthorName, ModName };
        public AssetBundleAssignmentSetting assetBundleAssignmentSetting;
        public int moonListCount;
        public int scrapListCount;
        public string selectedVariantToClear;

        public Color defaultButtonColor;
        public Color selectedButtonColor;
        public Color disabledButtonColor;

        public Sprite scrapItemIcon;
        public Sprite handIcon;
        public GameObject itemDropshipPrefab;
        public GameObject waterSurfacePrefab;
        public AudioMixerGroup diageticMasterMixer;

        public List<ExtendedMod> extendedModList = new List<ExtendedMod>();

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
    }
}
