using LethalLevelLoader;
using LethalSDK.Conversions;
using LethalSDK.ScriptableObjects;
using LethalToolkit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Unity.AI.Navigation;
using Unity.Netcode;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace LethalSDK.Converter
{
    public class LEConverterWindow : EditorWindow
    {
        public static LEConverterWindow window;
        public static LEConverterWindowSettings _settings;
        public static LEConverterWindowSettings WindowSettings
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
        public static Moon moon;

        public static string currentModDirectory = string.Empty;
        public static string currentModContentDirectory = string.Empty;
        public static string currentModContentDataDirectory = string.Empty;
        public static string currentModContentItemsDirectory = string.Empty;
        public static ExtendedLevel currentExtendedLevel = null;
        public static ExtendedItem currentExtendedItem = null;
        public static bool collapseMoonAssets;
        public static bool collapseScrapAssets;
        public static Vector2 moonScrollPos;
        public static Vector2 scrapScrollPos;

        public static float progressFloat;

        public static List<AssetImporter> assetImporterList = new List<AssetImporter>();
        public static List<UnityEngine.Object> dirtiedAssetsList = new List<UnityEngine.Object>();

        public enum WindowMode { Moons, Scraps, AssetBundles };
        public static WindowMode windowMode;

        public static bool moonModeToggle;
        public static bool scrapModeToggle;
        public static bool assetBundlesToggle;

        public static Dictionary<ExtendedContent, string> debugLogs = new Dictionary<ExtendedContent, string>();

        [MenuItem("LethalSDK/LethalExpansion Conversion Tool")]
        public static void OpenWindow()
        {
            if (WindowSettings != null)
            {
                window = GetWindow<LEConverterWindow>("LethalSDK: LethalExpansion Conversion Tool");
            }
        }

        public void OnGUI()
        {
            if (WindowSettings == null)
            {
                this.Close();
                OpenWindow();
            }
            GUILayout.ExpandWidth(true);
            GUILayout.ExpandHeight(true);

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("General Settings", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("AssetRip Game Root Directory", EditorStyles.boldLabel);
            if (GUILayout.Button("Select", GUILayout.ExpandHeight(false), GUILayout.Width(120)))
            {
                string modPath = EditorUtility.OpenFolderPanel("Select The Game AssetRip Root Folder", WindowSettings.assetRipGameRootDirectory, "");
                if (!string.IsNullOrEmpty(modPath))
                {
                    modPath = modPath.Substring(modPath.IndexOf("Assets/"));
                    if (AssetDatabase.IsValidFolder(modPath))
                        WindowSettings.assetRipGameRootDirectory = modPath;
                }
            }
            EditorGUILayout.TextField(WindowSettings.assetRipGameRootDirectory);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Mods Root Directory", EditorStyles.boldLabel);
            if (GUILayout.Button("Select", GUILayout.ExpandHeight(false), GUILayout.Width(120)))
            {
                string modPath = EditorUtility.OpenFolderPanel("Select New Root Mod Folder", WindowSettings.modsRootDirectory, "");
                if (!string.IsNullOrEmpty(modPath))
                {
                    modPath = modPath.Substring(modPath.IndexOf("Assets/"));
                    if (AssetDatabase.IsValidFolder(modPath))
                        WindowSettings.modsRootDirectory = modPath;
                }
            }
            EditorGUILayout.TextField(WindowSettings.modsRootDirectory);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            EditorGUILayout.LabelField("AssetBundle Assignment Source", EditorStyles.boldLabel);
            WindowSettings.assetBundleAssignmentSetting = (LEConverterWindowSettings.AssetBundleAssignmentSetting)EditorGUILayout.EnumPopup(WindowSettings.assetBundleAssignmentSetting);

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Selected Conversion Tool", EditorStyles.boldLabel);

            GUIStyle newStyle = new GUIStyle();
            newStyle.alignment = TextAnchor.MiddleCenter;

            EditorGUILayout.BeginHorizontal(newStyle);

            GUIStyle buttonStyleUnselected = new GUIStyle(GUI.skin.GetStyle("Button"));
            Texture2D unselectedTexture = new Texture2D(1, 1);
            unselectedTexture.SetPixel(0, 0, WindowSettings.defaultButtonColor);
            unselectedTexture.Apply();
            buttonStyleUnselected.normal.background = unselectedTexture;
            buttonStyleUnselected.alignment = TextAnchor.MiddleCenter;
            buttonStyleUnselected.richText = true;

            GUIStyle buttonStyleSelected = new GUIStyle(GUI.skin.GetStyle("Button"));
            Texture2D selectedTexture = new Texture2D(1, 1);
            selectedTexture.SetPixel(0, 0, WindowSettings.selectedButtonColor);
            selectedTexture.Apply();
            buttonStyleSelected.normal.background = selectedTexture;
            buttonStyleSelected.alignment = TextAnchor.MiddleCenter;
            buttonStyleSelected.richText = true;

            GUIStyle buttonStyleDisabled = new GUIStyle(GUI.skin.GetStyle("Button"));
            Texture2D disabledTexture = new Texture2D(1, 1);
            disabledTexture.SetPixel(0, 0, WindowSettings.disabledButtonColor);
            disabledTexture.Apply();
            buttonStyleDisabled.normal.background = disabledTexture;
            buttonStyleDisabled.alignment = TextAnchor.MiddleCenter;
            buttonStyleDisabled.richText = true;

            if (windowMode == WindowMode.Moons)
            {
                if (GUILayout.Button("Moons".Colorize(Color.white).Bold(), buttonStyleSelected, GUILayout.ExpandHeight(false), GUILayout.Width(120)))
                    windowMode = WindowMode.Moons;
                if (GUILayout.Button("Scrap".Colorize(Color.white), buttonStyleUnselected, GUILayout.ExpandHeight(false), GUILayout.Width(120)))
                    windowMode = WindowMode.Scraps;
                if (GUILayout.Button("Asset Bundles".Colorize(Color.white), buttonStyleUnselected, GUILayout.ExpandHeight(false), GUILayout.Width(120)))
                    windowMode = WindowMode.AssetBundles;
            }
            else if (windowMode == WindowMode.Scraps)
            {
                if (GUILayout.Button("Moons".Colorize(Color.white), buttonStyleUnselected, GUILayout.ExpandHeight(false), GUILayout.Width(120)))
                    windowMode = WindowMode.Moons;
                if (GUILayout.Button("Scrap".Colorize(Color.white).Bold(), buttonStyleSelected, GUILayout.ExpandHeight(false), GUILayout.Width(120)))
                    windowMode = WindowMode.Scraps;
                if (GUILayout.Button("Asset Bundles".Colorize(Color.white), buttonStyleUnselected, GUILayout.ExpandHeight(false), GUILayout.Width(120)))
                    windowMode = WindowMode.AssetBundles;
            }
            else
            {
                if (GUILayout.Button("Moons".Colorize(Color.white), buttonStyleUnselected, GUILayout.ExpandHeight(false), GUILayout.Width(120)))
                    windowMode = WindowMode.Moons;
                if (GUILayout.Button("Scrap".Colorize(Color.white), buttonStyleUnselected, GUILayout.ExpandHeight(false), GUILayout.Width(120)))
                    windowMode = WindowMode.Scraps;
                if (GUILayout.Button("Asset Bundles".Colorize(Color.white).Bold(), buttonStyleSelected, GUILayout.ExpandHeight(false), GUILayout.Width(120)))
                    windowMode = WindowMode.AssetBundles;
            }

            EditorGUILayout.EndHorizontal();


            //////////////////// MOON SETTINGS ////////////////////

            if (windowMode == WindowMode.Moons)
            {
                EditorGUILayout.Space(10);

                EditorGUILayout.TextField("Moon Conversion Settings", EditorStyles.boldLabel);

                EditorGUILayout.Space(5);

                EditorGUILayout.BeginHorizontal();

                collapseMoonAssets = EditorGUILayout.BeginFoldoutHeaderGroup(collapseMoonAssets, "Moon Assets", EditorStyles.boldLabel);


                if (GUILayout.Button("Populate", GUILayout.ExpandHeight(false), GUILayout.Width(120)))
                {
                    List<Moon> foundMoon = GetMoonInCurrentModFolder();
                    if (foundMoon.Count != 0)
                    {
                        WindowSettings.moonListCount = foundMoon.Count;
                        WindowSettings.moonList = new List<Moon>(foundMoon);
                    }
                }

                WindowSettings.moonListCount = EditorGUILayout.IntField(WindowSettings.moonListCount);
                EditorGUILayout.EndHorizontal();

                if (WindowSettings.moonListCount != 0)
                {
                    moonScrollPos = EditorGUILayout.BeginScrollView(moonScrollPos, GUILayout.MaxHeight(200));
                    List<Moon> tempList = new List<Moon>();
                    while (tempList.Count != WindowSettings.moonListCount)
                        tempList.Add(null);
                    int counter = 0;
                    foreach (Moon moon in WindowSettings.moonList)
                    {
                        if (counter < WindowSettings.moonListCount)
                            tempList[counter] = moon;
                        counter++;
                    }
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        UnityEngine.Object moonListObject = EditorGUILayout.ObjectField(tempList[i], typeof(Moon), true);

                        if (moonListObject is Moon)
                            tempList[i] = (Moon)moonListObject;
                    }
                    WindowSettings.moonList = new List<Moon>(tempList);

                    EditorGUILayout.EndScrollView();
                    EditorGUILayout.Space(10);
                }

                EditorGUILayout.EndFoldoutHeaderGroup();

                bool foundAtleastOneMoon = false;
                foreach (Moon moon in WindowSettings.moonList)
                    if (moon != null)
                        foundAtleastOneMoon = true;


                if (foundAtleastOneMoon == true && !string.IsNullOrEmpty(WindowSettings.modsRootDirectory) && !string.IsNullOrEmpty(WindowSettings.assetRipGameRootDirectory))
                {
                    if (GUILayout.Button("Convert Moons", buttonStyleUnselected, GUILayout.ExpandHeight(false), GUILayout.Width(240)))
                        ConvertMoons(WindowSettings.modsRootDirectory, new List<Moon>(WindowSettings.moonList));
                }
                else
                    GUILayout.Button("Convert Moons", buttonStyleDisabled, GUILayout.ExpandHeight(false), GUILayout.Width(240));
            }

            //////////////////// SCRAP SETTINGS ////////////////////
            
            if (windowMode == WindowMode.Scraps)
            {
                EditorGUILayout.Space(10);

                EditorGUILayout.TextField("Scrap Conversion Settings", EditorStyles.boldLabel);

                EditorGUILayout.Space(5);

                EditorGUILayout.BeginHorizontal();

                collapseScrapAssets = EditorGUILayout.BeginFoldoutHeaderGroup(collapseScrapAssets, "Scrap Assets", EditorStyles.boldLabel);


                if (GUILayout.Button("Populate", GUILayout.ExpandHeight(false), GUILayout.Width(120)))
                {
                    List<Scrap> foundScrap = GetScrapInCurrentModFolder();
                    if (foundScrap.Count != 0)
                    {
                        WindowSettings.scrapListCount = foundScrap.Count;
                        WindowSettings.scrapList = new List<Scrap>(foundScrap);
                    }
                }

                WindowSettings.scrapListCount = EditorGUILayout.IntField(WindowSettings.scrapListCount);
                EditorGUILayout.EndHorizontal();

                if (WindowSettings.scrapListCount != 0)
                {
                    scrapScrollPos = EditorGUILayout.BeginScrollView(scrapScrollPos, GUILayout.MaxHeight(200));
                    List<Scrap> scrapTempList = new List<Scrap>();
                    while (scrapTempList.Count != WindowSettings.scrapListCount)
                        scrapTempList.Add(null);
                    int scrapCounter = 0;
                    foreach (Scrap scrap in WindowSettings.scrapList)
                    {
                        if (scrapCounter < WindowSettings.scrapListCount)
                            scrapTempList[scrapCounter] = scrap;
                        scrapCounter++;
                    }
                    for (int i = 0; i < scrapTempList.Count; i++)
                    {
                        UnityEngine.Object scrapListObject = EditorGUILayout.ObjectField(scrapTempList[i], typeof(Scrap), true);

                        if (scrapListObject is Scrap)
                            scrapTempList[i] = (Scrap)scrapListObject;
                    }
                    WindowSettings.scrapList = new List<Scrap>(scrapTempList);

                    EditorGUILayout.EndScrollView();

                    EditorGUILayout.Space(10);
                }

                EditorGUILayout.EndFoldoutHeaderGroup();

                bool foundAtleastOneScrap = false;
                foreach (Scrap scrap in WindowSettings.scrapList)
                    if (scrap != null)
                        foundAtleastOneScrap = true;

                if (foundAtleastOneScrap == true && !string.IsNullOrEmpty(WindowSettings.modsRootDirectory) && !string.IsNullOrEmpty(WindowSettings.assetRipGameRootDirectory))
                {
                    if (GUILayout.Button("Convert Scrap", buttonStyleUnselected, GUILayout.ExpandHeight(false), GUILayout.Width(240)))
                        ConvertItems(WindowSettings.modsRootDirectory, WindowSettings.scrapList);
                }
                else
                    GUILayout.Button("Convert Scrap", buttonStyleDisabled, GUILayout.ExpandHeight(false), GUILayout.Width(240));

            }

            //////////////////// ASSETBUNDLE SETTINGS ////////////////////

            if (windowMode == WindowMode.AssetBundles)
            {
                EditorGUILayout.Space(10);

                EditorGUILayout.TextField("AssetBundle Clearing Settings", EditorStyles.boldLabel);

                WindowSettings.selectedVariantToClear = EditorGUILayout.TextField(WindowSettings.selectedVariantToClear);

                if (GUILayout.Button("Clear AssetBundles", GUILayout.ExpandHeight(false), GUILayout.Width(240)))
                {
                    ClearAssetBundleLabels(WindowSettings.selectedVariantToClear);
                }
            }
           
        }

        public static void ClearAssetBundleLabels(string clearLabel)
        {
            foreach (string assetBundleName in AssetDatabase.GetAllAssetBundleNames())
            {
                Debug.Log("AssetBundleName: " +  assetBundleName);
                if (assetBundleName.ToLower().Contains(clearLabel))
                {
                    Debug.Log("Found Match: " + assetBundleName.ToLower());
                    foreach (string assetPath in AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName))
                    {
                        //AssetImporter direct = AssetImporter.GetAtPath(assetPath);
                        //direct.SetAssetBundleNameAndVariant(string.Empty, string.Empty);
                        //direct.SaveAndReimport();
                    }
                    AssetDatabase.RemoveAssetBundleName(assetBundleName, true);
                }
            }
        }

        public static void ConvertMoons(string modRootDirectory, List<Moon> moonList)
        {
            AssetDatabase.DisallowAutoRefresh();

            debugLogs.Clear();

            WindowSettings.GetExtendedMods();
            SelectableLevelConverter.PopulateReferenceLists(WindowSettings);
            ExtendedLevel lastCreatedExtendedLevel = null;

            foreach (ExtendedMod extendedMod in WindowSettings.extendedModList)
                extendedMod.ExtendedLevels.Clear();

            List<ExtendedLevel> convertedLevels = new List<ExtendedLevel>();

            foreach (Moon moon in moonList)
                if (moon != null)
                {
                    EditorUtility.DisplayProgressBar("Converting Moons", "Converting Moon: " + moon.MoonName + " (" + convertedLevels.Count + " / " + moonList.Count + ")", (float)(convertedLevels.Count / moonList.Count));
                    lastCreatedExtendedLevel = ConvertMoon(modRootDirectory, moon);
                    convertedLevels.Add(lastCreatedExtendedLevel);
                }
            EditorUtility.ClearProgressBar();

            RefreshAssetDatabase();

            if (lastCreatedExtendedLevel != null)
                Selection.activeObject = lastCreatedExtendedLevel;

            string levelDebugString = "Level Conversion Report";
            foreach (KeyValuePair<ExtendedContent, string> debugLog in debugLogs)
                if (debugLog.Key != null && !string.IsNullOrEmpty(debugLog.Value))
                    levelDebugString += "\n\n" + debugLog.Key.name + " Logs. \n \n" + debugLog.Value;

            Debug.Log(levelDebugString);

            AssetDatabase.AllowAutoRefresh();
        }

        public static void ConvertItems(string modRootDirectory, List<Scrap> scrapList)
        {
            AssetDatabase.DisallowAutoRefresh();

            debugLogs.Clear();

            WindowSettings.GetExtendedMods();
            SelectableLevelConverter.PopulateReferenceLists(WindowSettings);
            ScrapConverter.GetDefaultAudioClips();

            foreach (ExtendedMod extendedMod in WindowSettings.extendedModList)
                extendedMod.ExtendedItems.Clear();

            List<ExtendedItem> convertedItems = new List<ExtendedItem>();



            ExtendedItem lastCreatedExtendedItem = null;
            foreach (Scrap scrap in scrapList)
                if (scrap != null)
                {
                    EditorUtility.DisplayProgressBar("Converting Scrap", "Converting Scrap: " + scrap.itemName + " (" + convertedItems.Count + " / " + scrapList.Count + ")", (float)(convertedItems.Count / scrapList.Count));
                    lastCreatedExtendedItem = ConvertItem(modRootDirectory, scrap);
                    convertedItems.Add(lastCreatedExtendedItem);
                }
            EditorUtility.ClearProgressBar();

            RefreshAssetDatabase();

            if (lastCreatedExtendedItem != null)    
                Selection.activeObject = lastCreatedExtendedItem;

            string itemDebugString = "Item Conversion Report";
            foreach (KeyValuePair<ExtendedContent, string> debugLog in debugLogs)
                if (debugLog.Key != null && !string.IsNullOrEmpty(debugLog.Value))
                itemDebugString += "\n\n" + debugLog.Key.name + " Logs. \n \n" + debugLog.Value;

            Debug.Log(itemDebugString);

            AssetDatabase.AllowAutoRefresh();
        }

        public static ExtendedItem ConvertItem(string modRootDirectory, Scrap scrap)
        {
            //Debug.Log("Converting " + GetSanitisedScrapName(scrap) + " At Path: " + modRootDirectory);

            if (scrap.prefab != null)
            {
                if (TryGetModManifest(scrap, out ModManifest scrapManifest))
                {
                    //Debug.Log("Found ScrapManifest!");

                    //Create Mod Directory.

                    Stopwatch fileStopwatch = new Stopwatch();
                    fileStopwatch.Start();

                    CreateModsDirectory(modRootDirectory, scrapManifest, GetSanitisedScrapName(scrap), typeof(Item));

                    Item newItem = ScriptableObject.CreateInstance<Item>();
                    newItem.name = GetSanitisedScrapName(scrap) + "Item";
                    AssetDatabase.CreateAsset(newItem, currentModContentDataDirectory + "/" + GetSanitisedScrapName(scrap) + "Item" + ".asset");
                    Item item = (Item)AssetDatabase.LoadAssetAtPath(currentModContentDataDirectory + "/" + GetSanitisedScrapName(scrap) + "Item" + ".asset", typeof(Item));
                    

                    ExtendedItem newExtendedItem = ScriptableObject.CreateInstance<ExtendedItem>();
                    newExtendedItem.name = GetSanitisedScrapName(scrap) + "ExtendedItem";
                    AssetDatabase.CreateAsset(newExtendedItem, currentModContentDataDirectory + "/" + newExtendedItem.name + ".asset");
                    ExtendedItem extendedItem = (ExtendedItem)AssetDatabase.LoadAssetAtPath(currentModContentDataDirectory + "/" + GetSanitisedScrapName(scrap) + "ExtendedItem" + ".asset", typeof(ExtendedItem));

                    currentExtendedItem = extendedItem;

                    GameObject clonedScrapPrefab = PrefabUtility.SaveAsPrefabAsset(scrap.prefab, currentModContentDataDirectory + "/" + GetSanitisedScrapName(scrap) + "Prefab" + ".prefab");
                    item.spawnPrefab = clonedScrapPrefab;

                    fileStopwatch.Stop();

                    debugLogs.Add(extendedItem, string.Empty);

                    LogTime(ref fileStopwatch, extendedItem, "File Creation Time Was: ");

                    Stopwatch itemConversionStopwatch = new Stopwatch();
                    itemConversionStopwatch.Start();

                    ScrapConverter.ConvertComponent(item, scrap);

                    itemConversionStopwatch.Stop();
                    LogTime(ref itemConversionStopwatch, extendedItem, "Item Conversion Time Was: ");

                    Stopwatch extendedItemConversionStopwatch = new Stopwatch();
                    extendedItemConversionStopwatch.Start();

                    ExtendedItemConverter.ConvertScrap(extendedItem, item, scrap);

                    extendedItemConversionStopwatch.Stop();
                    LogTime(ref extendedItemConversionStopwatch, extendedItem, "Extended Item Conversion Time Was: ");

                    string path = modRootDirectory + "/" + scrapManifest.modName + "/" + typeof(Item).Name + "s" + "/" + GetSanitisedScrapName(scrap);
                    if (WindowSettings.assetBundleAssignmentSetting == LEConverterWindowSettings.AssetBundleAssignmentSetting.ModName)
                        AssignAssetBundleLabelToAsset(path, scrapManifest.modName.ToLower(), "lethalbundle");
                    else
                        AssignAssetBundleLabelToAsset(path, scrapManifest.author.ToLower(), "lethalbundle");

                    if (TryGetExtendedMod(scrapManifest.author.SkipToLetters().RemoveWhitespace().StripSpecialCharacters(), out ExtendedMod extendedMod))
                    {
                        extendedMod.ExtendedItems.Add(extendedItem);
                        string extendedModPath = AssetDatabase.GetAssetPath(extendedMod);
                        AssignAssetBundleLabelToAsset(extendedModPath, GetAssetBundleLabelsFromAsset(AssetDatabase.GetAssetPath(extendedItem)));
                        if (!dirtiedAssetsList.Contains(extendedMod))
                            dirtiedAssetsList.Add(extendedMod);
                    }

                    Selection.activeObject = extendedItem;

                    dirtiedAssetsList.Add(extendedItem);
                    dirtiedAssetsList.Add(item);

                    currentModContentDirectory = string.Empty;
                    currentModDirectory = string.Empty;
                    currentModContentDataDirectory = string.Empty;
                    currentModContentDataDirectory = string.Empty;
                    currentExtendedItem = null;
                    //Debug.Log("Finished Converting Scrap: " + scrap.name + " To Item Function.");

                    return (extendedItem);
                }
                else
                    Debug.LogError("Scrap Conversion Failed! Could Not Find ModManifest For " + scrap.itemName + "!");
            }
            else
                Debug.LogError("Scrap Conversion Failed! " + scrap.itemName + " Has No Prefab!");

            return (null);
        }

        public static ExtendedLevel ConvertMoon(string modRootDirectory, Moon moon)
        {
            //Debug.Log("Converting " + moon.MoonName + " At Path: " + modRootDirectory);

            //Populating Lists Of Content In Unity Project For Reference.

            if (moon.MainPrefab != null)
            {
                if (SelectableLevelConverter.TryGetModManifest(moon, out ModManifest moonManifest))
                {
                    //Debug.Log("Found MoonManifest!");

                    Stopwatch fileStopwatch = new Stopwatch();
                    fileStopwatch.Start();

                    //Create Mod Directory.
                    CreateModsDirectory(modRootDirectory, moonManifest, GetSanitisedPlanetName(moon), typeof(SelectableLevel));

                    //Create New SelectableLevel In /Mod/Content/Data/
                    SelectableLevel newSelectableLevel = ScriptableObject.CreateInstance<SelectableLevel>();
                    newSelectableLevel.name = GetSanitisedPlanetName(moon) + "Level";
                    AssetDatabase.CreateAsset(newSelectableLevel, currentModContentDataDirectory + "/" + newSelectableLevel.name + ".asset");
                    //Debug.Log("Created New SelectableLevel Asset " + newSelectableLevel.name + ".asset At: " + currentModContentDataDirectory);

                    //Create New ExtendedLevel In /Mod/Content/Data/
                    ExtendedLevel newExtendedLevel = ScriptableObject.CreateInstance<ExtendedLevel>();
                    newExtendedLevel.name = GetSanitisedPlanetName(moon) + "ExtendedLevel";
                    AssetDatabase.CreateAsset(newExtendedLevel, currentModContentDataDirectory + "/" + newExtendedLevel.name + ".asset");
                    //Debug.Log("Created New SelectableLevel Asset " + newExtendedLevel.name + ".asset At: " + currentModContentDataDirectory);

                    //Create New Scene In /Mod/
                    Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                    scene.name = GetSanitisedPlanetName(moon) + "Scene";
                    EditorSceneManager.SaveScene(scene, currentModDirectory + "/" + scene.name + ".unity");
                    //Debug.Log("Created New Scene Asset: " + GetSanitisedPlanetName(moon) + "Scene.unity");

                    //Get SelectableLevel & ExtendedLevel References After Scene-Load Destroyed Previous References.
                    SelectableLevel selectableLevel = (SelectableLevel)AssetDatabase.LoadAssetAtPath(currentModContentDataDirectory + "/" + GetSanitisedPlanetName(moon) + "Level" + ".asset", typeof(SelectableLevel));
                    ExtendedLevel extendedLevel = (ExtendedLevel)AssetDatabase.LoadAssetAtPath(currentModContentDataDirectory + "/" + GetSanitisedPlanetName(moon) + "ExtendedLevel" + ".asset", typeof(ExtendedLevel));

                    currentExtendedLevel = extendedLevel;

                    fileStopwatch.Stop();

                    debugLogs.Add(extendedLevel, string.Empty);

                    LogTime(ref fileStopwatch, extendedLevel, "File Creation Time Was: ");

                    Stopwatch levelConversionStopwatch = new Stopwatch();
                    levelConversionStopwatch.Start();

                    //Converting Data Inside Moon And ModManifest Into New SelectableLevel And ExtendedLevel.
                    SelectableLevelConverter.ConvertAsset(moon, moonManifest, ref selectableLevel);

                    levelConversionStopwatch.Stop();
                    LogTime(ref levelConversionStopwatch, extendedLevel, "Level Conversion Time Was: ");

                    Stopwatch extendedLevelConversionStopwatch = new Stopwatch();
                    extendedLevelConversionStopwatch.Start();

                    ExtendedLevelConverter.ConvertAsset(moon, selectableLevel, moonManifest, ref extendedLevel);

                    extendedLevelConversionStopwatch.Stop();
                    LogTime(ref extendedLevelConversionStopwatch, extendedLevel, "ExtendedLevel Conversion Time Was: ");

                    //Unpacking PlanetPrefab From Moon Into New Scene.
                    Debug.Log("Instansiating MainPrefab: " + moon.MainPrefab.name);
                    GameObject moonPrefab = GameObject.Instantiate(moon.MainPrefab);
                    //Debug.Log("Unparenting MainPrefab Children");
                    foreach (Transform rootObject in moonPrefab.transform)
                        rootObject.parent = null;
                    //Debug.Log("Destroying MainPrefab");
                    GameObject.Destroy(moonPrefab);

                    EditorSceneManager.SaveScene(scene, currentModDirectory + "/" + scene.name + ".unity");

                    //Converting LethalSDK Components To Lethal Company Components In New Scene.

                    Stopwatch componentConversionStopwatch = new Stopwatch();
                    componentConversionStopwatch.Start();

                    ConvertComponents.ConvertMatchLocalPlayerPosition();
                    ConvertComponents.ConvertAnimatedSun();
                    ConvertComponents.ConvertScanNodes();
                    ConvertComponents.ConvertInteractTriggers();
                    ConvertComponents.ConvertDungeonGenerator();
                    ConvertComponents.ConvertEntranceTeleports();
                    ConvertComponents.ConvertAudioReverbTriggers();
                    ConvertComponents.ConvertAudioReverbPresets();
                    ConvertComponents.ConvertItemDropship();
                    ConvertComponents.ConvertLadders();
                    ConvertComponents.ConvertWaterSurfaces();

                    componentConversionStopwatch.Stop();
                    LogTime(ref componentConversionStopwatch, extendedLevel, "Component Conversion Time Was: ");

                    //Setting Terrain GPU Instacing To True (To Avoid Crashes)
                    foreach (Terrain terrain in GameObject.FindObjectsOfType<Terrain>())
                        terrain.drawInstanced = true;

                    NavMeshSurface navMeshSurface = GameObject.FindObjectOfType<NavMeshSurface>();
                    GameObject environmentObject = GameObject.FindGameObjectWithTag("OutsideLevelNavMesh");
                    if (navMeshSurface != null && environmentObject != null)
                        if (navMeshSurface.gameObject != environmentObject)
                        {
                            Debug.LogWarning("NavMeshSurface Attatched To Wrong GameObject! Reconstructing!");
                            NavMeshSurface newSurface = environmentObject.AddComponent<NavMeshSurface>();
                            newSurface.agentTypeID = navMeshSurface.agentTypeID;
                            newSurface.defaultArea = navMeshSurface.defaultArea;
                            newSurface.ignoreNavMeshAgent = navMeshSurface.ignoreNavMeshAgent;
                            newSurface.useGeometry = navMeshSurface.useGeometry;
                            newSurface.collectObjects = navMeshSurface.collectObjects;
                            newSurface.layerMask = navMeshSurface.layerMask;
                            newSurface.overrideVoxelSize = navMeshSurface.overrideVoxelSize;
                            newSurface.voxelSize = navMeshSurface.voxelSize;
                            newSurface.overrideTileSize = navMeshSurface.overrideTileSize;
                            newSurface.tileSize = navMeshSurface.tileSize;
                            newSurface.minRegionArea = navMeshSurface.minRegionArea;
                            newSurface.buildHeightMesh = navMeshSurface.buildHeightMesh;
                            newSurface.navMeshData = navMeshSurface.navMeshData;
                            UnityEngine.Object.DestroyImmediate(navMeshSurface);
                        }


                    EditorSceneManager.SaveScene(scene, currentModDirectory + "/" + scene.name + ".unity");

                    Selection.activeObject = extendedLevel;

                    if (WindowSettings.assetBundleAssignmentSetting == LEConverterWindowSettings.AssetBundleAssignmentSetting.AuthorName)
                        AssignAssetBundleLabels(GetSanitisedModName(moonManifest.author), currentModContentDirectory, currentModDirectory + "/" + scene.name + ".unity");
                    else
                        AssignAssetBundleLabels(GetSanitisedModName(moonManifest.modName), currentModContentDirectory, currentModDirectory + "/" + scene.name + ".unity");

                    if (TryGetExtendedMod(moonManifest, out ExtendedMod extendedMod))
                    {
                        extendedMod.ExtendedLevels.Add(extendedLevel);
                        string extendedModPath = AssetDatabase.GetAssetPath(extendedMod);
                        AssignAssetBundleLabelToAsset(extendedModPath, GetAssetBundleLabelsFromAsset(AssetDatabase.GetAssetPath(extendedLevel)));
                        if (!dirtiedAssetsList.Contains(extendedMod))
                            dirtiedAssetsList.Add(extendedMod);
                    }

                    currentModContentDirectory = string.Empty;
                    currentModDirectory = string.Empty;
                    currentModContentDataDirectory = string.Empty;
                    currentModContentDataDirectory = string.Empty;
                    currentExtendedLevel = null;

                    dirtiedAssetsList.Add(extendedLevel);
                    dirtiedAssetsList.Add(extendedLevel.selectableLevel);

                    //Debug.Log("Finished MoonConversion Function.");

                    return (newExtendedLevel);
                }
                else
                    Debug.LogError("Could Not Find ModManifest For Moon: " + moon.PlanetName);
            }
            else
                Debug.LogError("Could Not Find MainPrefab For Moon: " + moon.PlanetName);

            return (null);
        }

        public static void CreateModsDirectory(string modRootDirectory, ModManifest modManifest, string assetName, Type assetType)
        {
            string newDirectoryPath = modRootDirectory + "/" + modManifest.modName + "/" + assetType.Name + "s" + "/" + assetName;
            //Create Mod/Content/ and Mod/Content/Data/ Directories.
            if (AssetDatabase.IsValidFolder(modRootDirectory + "/" + modManifest.modName) == false)
                AssetDatabase.CreateFolder(modRootDirectory, modManifest.modName);
            if (AssetDatabase.IsValidFolder(modRootDirectory + "/" + modManifest.modName + "/" + assetType.Name + "s") == false)
                AssetDatabase.CreateFolder(modRootDirectory + "/" + modManifest.modName, assetType.Name + "s");
            if (AssetDatabase.IsValidFolder(modRootDirectory + "/" + modManifest.modName + "/" + assetType.Name + "s" + "/" + assetName) == true)
            {
                AssetDatabase.DeleteAsset(newDirectoryPath);
                AssetDatabase.Refresh();
            }
            AssetDatabase.CreateFolder(modRootDirectory + "/" + modManifest.modName + "/" + assetType.Name + "s", assetName);
            currentModDirectory = newDirectoryPath;
            //Debug.Log("Created New Mods Folder At: " + currentModDirectory);

            string sanitisedModName = modManifest.modName.SkipToLetters().RemoveWhitespace().StripSpecialCharacters();
            string sanitisedAuthorName = modManifest.author.SkipToLetters().RemoveWhitespace().StripSpecialCharacters();

            if (TryGetExtendedMod(sanitisedModName, out ExtendedMod extendedMod) == false)
            {
                //Debug.Log("Creating New ExtendedMod Using Names: " + sanitisedModName + " --- " + sanitisedAuthorName);
                ExtendedMod newExtendedMod = CreateNewExtendedMod(modRootDirectory + "/" + modManifest.modName + "/", sanitisedModName, sanitisedAuthorName);
                if (newExtendedMod != null)
                {
                    //Debug.Log("Created New ExtendedMod: " + newExtendedMod.AuthorName + newExtendedMod.ModName);
                    WindowSettings.extendedModList.Add(newExtendedMod);
                }
                else
                    Debug.LogError("Failed To Create New ExtendedMod");
            }

            if (assetType == typeof(SelectableLevel))
            {
                AssetDatabase.CreateFolder(currentModDirectory, "Content");
                currentModContentDirectory = currentModDirectory + "/Content";
                //Debug.Log("Created New Mod Content Folder At: " + currentModContentDirectory);

                AssetDatabase.CreateFolder(currentModContentDirectory, "Data");
                currentModContentDataDirectory = currentModContentDirectory + "/Data";
                //Debug.Log("Created New Mod Data Folder At: " + currentModContentDataDirectory);
            }
            else if (assetType == typeof(Item))
            {
                AssetDatabase.CreateFolder(currentModDirectory, "Data");
                currentModContentDataDirectory = currentModDirectory + "/Data";
                //Debug.Log("Created New Mod Data Folder At: " + currentModContentDataDirectory);
            }
        }

        public static (string, string)GetAssetBundleLabelsFromAsset(string assetPath)
        {
            string assetBundleName = AssetDatabase.GetImplicitAssetBundleName(assetPath);
            string assetBundleVariantName = AssetDatabase.GetImplicitAssetBundleVariantName(assetPath);

            return (assetBundleName, assetBundleVariantName);
        }

        public static void AssignAssetBundleLabels(string modName, string modContentDirectory, string modSceneAssetPath)
        {
            AssignAssetBundleLabelToAsset(modContentDirectory, modName.ToLower(), "lethalbundle");
            AssignAssetBundleLabelToAsset(modSceneAssetPath, modName.ToLower() + "scenes", "lethalbundle");
        }

        public static void AssignAssetBundleLabelToAsset(string assetPath, (string,string) assetBundleNames)
        {
            AssignAssetBundleLabelToAsset(assetPath, assetBundleNames.Item1, assetBundleNames.Item2);
        }

        public static void AssignAssetBundleLabelToAsset(string assetPath, string assetBundleName, string assetBundleVariantName)
        {
            AssetImporter modContentDirectoryImporter = AssetImporter.GetAtPath(assetPath);
            modContentDirectoryImporter.SetAssetBundleNameAndVariant(assetBundleName, assetBundleVariantName);
            assetImporterList.Add(modContentDirectoryImporter);
        }

        public static string GetSanitisedPlanetName(Moon moon)
        {
            if (moon != null)
            {
                string returnString = moon.MoonName.SkipToLetters().RemoveWhitespace().StripSpecialCharacters();
                char[] chars = returnString.ToCharArray();
                chars[0] = char.ToUpper(chars[0]);
                return (new string(chars));
            }
            else
                return (string.Empty);
        }

        public static void RefreshAssetDatabase()
        {
            foreach (UnityEngine.Object dirtiedAsset in dirtiedAssetsList)
                EditorUtility.SetDirty(dirtiedAsset);
            dirtiedAssetsList.Clear();
            
            foreach (AssetImporter assetImporter in assetImporterList)
                assetImporter.SaveAndReimport();
            assetImporterList.Clear();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            AssetDatabase.ReleaseCachedFileHandles();
        }

        public static string GetSanitisedScrapName(Scrap scrap)
        {
            if (scrap != null)
            {
                string returnString = scrap.itemName.SkipToLetters().RemoveWhitespace().StripSpecialCharacters();
                if (string.IsNullOrEmpty(returnString))
                    returnString = scrap.itemName.RemoveWhitespace().StripSpecialCharacters();
                char[] chars = returnString.ToCharArray();
                chars[0] = char.ToUpper(chars[0]);
                return (new string(chars));
            }
            else
                return (string.Empty);
        }

        public static string GetSanitisedModName(string name)
        {
            return (name.SkipToLetters().RemoveWhitespace());
        }

        public static bool GetOrCreateFolder(string path, string folder, out string folderPath)
        {
            folderPath = path + "/" + folder;
            if (AssetDatabase.IsValidFolder(path + "/" + folder))
            {
                AssetDatabase.CreateFolder(path, folder);
                return (false);
            }
            else
                return (true);
        }

        public static bool TryGetModManifest(Scrap scrap, out ModManifest manifest)
        {
            manifest = null;
            foreach (ModManifest modManifest in SelectableLevelConverter.manifests)
                foreach (Scrap manifestItem in modManifest.scraps)
                    if (manifestItem == scrap)
                        manifest = modManifest;

            return (manifest);
        }

        public static List<Scrap> GetScrapInCurrentModFolder()
        {
            List<Scrap> returnList = new List<Scrap>();
            if (string.IsNullOrEmpty(WindowSettings.modsRootDirectory))
                return returnList;

            string[] scrapGuids = AssetDatabase.FindAssets("t:Scrap");

            foreach (string scrapGuid in scrapGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(scrapGuid);
                if (!string.IsNullOrEmpty(path))
                {
                    Scrap scrap = AssetDatabase.LoadAssetAtPath<Scrap>(path);
                    if (scrap != null)
                        returnList.Add(scrap);
                }
            }

            return (returnList);
        }

        public static List<Moon> GetMoonInCurrentModFolder()
        {
            List<Moon> returnList = new List<Moon>();
            if (string.IsNullOrEmpty(WindowSettings.modsRootDirectory))
                return returnList;

            string[] moonGuids = AssetDatabase.FindAssets("t:Moon");

            foreach (string moonGuid in moonGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(moonGuid);
                if (!string.IsNullOrEmpty(path))
                {
                    Moon moon = AssetDatabase.LoadAssetAtPath<Moon>(path);
                    if (moon != null)
                        returnList.Add(moon);
                }
            }

            return (returnList);
        }

        public static bool TryGetExtendedMod(ModManifest modManifest, out ExtendedMod extendedMod)
        {
            extendedMod = null;

            string comparisonName = string.Empty;
            if (WindowSettings.assetBundleAssignmentSetting == LEConverterWindowSettings.AssetBundleAssignmentSetting.ModName)
                comparisonName = modManifest.modName.SkipToLetters().RemoveWhitespace().StripSpecialCharacters();
            else
                comparisonName = modManifest.author.SkipToLetters().RemoveWhitespace().StripSpecialCharacters();

            foreach (ExtendedMod preexistingExtendedMod in WindowSettings.extendedModList)
                if (preexistingExtendedMod.AuthorName == comparisonName || preexistingExtendedMod.ModName == comparisonName)
                    extendedMod = preexistingExtendedMod;

            return (extendedMod);
        }

        public static bool TryGetExtendedMod(string authorName, out ExtendedMod extendedMod)
        {
            extendedMod = null;

            string comparisonName = authorName.SkipToLetters().RemoveWhitespace().StripSpecialCharacters();
            foreach (ExtendedMod preexistingExtendedMod in WindowSettings.extendedModList)
                if (preexistingExtendedMod.AuthorName == comparisonName || preexistingExtendedMod.ModName == comparisonName)
                    extendedMod = preexistingExtendedMod;

            return (extendedMod);
        }

        public static ExtendedMod CreateNewExtendedMod(string directory, string modName, string authorName)
        {
            ExtendedMod newExtendedMod = ExtendedMod.Create(modName, authorName);
            newExtendedMod.name = modName + "ExtendedMod";
            AssetDatabase.CreateAsset(newExtendedMod, directory + "/" + newExtendedMod.name + ".asset");
            ExtendedMod extendedMod = (ExtendedMod)AssetDatabase.LoadAssetAtPath(directory + "/" + modName + "ExtendedMod" + ".asset", typeof(ExtendedMod));

            return (extendedMod);
        }

        public static void LogTime(ref Stopwatch stopWatch, ExtendedContent extendedContent, string description)
        {
            try
            {
                if (extendedContent != null)
                {
                    TimeSpan timeSpan = TimeSpan.FromMilliseconds(stopWatch.ElapsedMilliseconds);
                    string elapsedSeconds = string.Format("{0:D2}", timeSpan.Seconds);
                    if (string.IsNullOrEmpty(elapsedSeconds))
                        elapsedSeconds = "--";
                    string elapsedMilliseconds = string.Format("{0:D1}", timeSpan.Milliseconds);
                    if (string.IsNullOrEmpty(elapsedMilliseconds))
                        elapsedMilliseconds = "--";
                    elapsedMilliseconds = new string(new char[] { elapsedMilliseconds[0], elapsedMilliseconds[1] });
                    if (debugLogs.TryGetValue(extendedContent, out string debugLog))
                        debugLogs[extendedContent] += description + elapsedSeconds + "." + elapsedMilliseconds + " Seconds. (" + stopWatch.ElapsedMilliseconds + "ms)" + "\n";
                }
            }
            catch
            {
                Debug.LogError("Time Could Not Be Logged");
            }
        }

        public static void LogInfo(ExtendedContent extendedContent, string description)
        {
            try
            {
                if (extendedContent != null)
                    if (debugLogs.TryGetValue(extendedContent, out string debugLog))
                        debugLogs[extendedContent] += description + "\n";
            }
            catch
            {
                Debug.LogError("Info Could Not Be Logged");
            }
        }
    }
}
