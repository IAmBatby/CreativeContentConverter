using LethalLevelLoader;
using LethalToolkit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.WSA;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

namespace LethalSDK.Converter
{
    public class ProjectOrganiserWindow : EditorWindow
    {
        public static ProjectOrganiserWindow Window;

        public static LEConverterWindowSettings _settings;
        public static LEConverterWindowSettings WindowSettings
        {
            get
            {
                if (_settings == null)
                {
                    string[] guids = AssetDatabase.FindAssets("t:LEConverterWindowSettings");
                    if (guids.Length > 0 )
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                        _settings = AssetDatabase.LoadAssetAtPath<LEConverterWindowSettings>(path);
                    }
                }
                return (_settings);
            }
        }

        public static string CurrentDirectory { get { return (WindowSettings.organiserSelectedDirectory); } set { WindowSettings.organiserSelectedDirectory = value; } }
        public static string TargetDirectory { get { return (WindowSettings.organiserTargetDirectory); } set { WindowSettings.organiserTargetDirectory = value; } }
        public static AssetType CurrentAssetType { get { return (WindowSettings.organiserSelectedAssetType); } set { WindowSettings.organiserSelectedAssetType = value; } }
        //[MenuItem("Creative Content Converter/LethalLevelLoader ExtendedMod Organiser", true)]
        public static bool ValidateOpenWindow()
        {
            return (LEConverterWindow.WindowSettings != null);
        }
        //[MenuItem("Creative Content Converter/LethalLevelLoader ExtendedMod Organiser")]
        public static void OpenWindow()
        {
            Window = GetWindow<ProjectOrganiserWindow>("ExtendedMod Organiser");
        }

        public void OnGUI()
        {
            GUILayout.ExpandWidth(true);
            GUILayout.ExpandHeight(true);

            EditorGUILayout.LabelField("Warning, This is an experimental wip tool that will attempt to organise and move your project assets. Make a backup of your original project files before usage.", EditorStyles.boldLabel);

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("General Settings", EditorStyles.boldLabel);


            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Working Project Directory", EditorStyles.boldLabel);
            if (GUILayout.Button("Select", GUILayout.ExpandHeight(false), GUILayout.Width(120)))
            {
                string modPath = EditorUtility.OpenFolderPanel("Select The Working Project Directory", CurrentDirectory, "");
                if (!string.IsNullOrEmpty(modPath))
                {
                    modPath = modPath.Substring(modPath.IndexOf("Assets/"));
                    if (AssetDatabase.IsValidFolder(modPath))
                        CurrentDirectory = modPath;
                }
            }
            EditorGUILayout.TextField(CurrentDirectory);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Target Project Directory", EditorStyles.boldLabel);
            if (GUILayout.Button("Select", GUILayout.ExpandHeight(false), GUILayout.Width(120)))
            {
                string modPath = EditorUtility.OpenFolderPanel("Select The Targeted Project Directory", TargetDirectory, "");
                if (!string.IsNullOrEmpty(modPath))
                {
                    modPath = modPath.Substring(modPath.IndexOf("Assets/"));
                    if (AssetDatabase.IsValidFolder(modPath))
                        TargetDirectory = modPath;
                }
            }
            EditorGUILayout.TextField(TargetDirectory);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Asset Type", EditorStyles.boldLabel);
            CurrentAssetType = (AssetType)EditorGUILayout.EnumPopup(CurrentAssetType);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(15);

            if (!string.IsNullOrEmpty(CurrentDirectory))
            {
                if (GUILayout.Button("Get " + CurrentAssetType.ToString() + " Assets", GUILayout.ExpandHeight(false), GUILayout.Width(200)))
                    GetAssets(CurrentAssetType);
                if (GUILayout.Button("Get All Assets", GUILayout.ExpandHeight(false), GUILayout.Width(200)))
                    GetAllAssets();
                if (!string.IsNullOrEmpty(TargetDirectory))
                {
                    if (GUILayout.Button("Get Folders", GUILayout.ExpandHeight(false), GUILayout.Width(200)))
                        GetAllAssetFolders();
                    if (GUILayout.Button("Create Folders", GUILayout.ExpandHeight(false), GUILayout.Width(200)))
                        CreateAllAssetFolders();
                }
            }
        }

        public OrganisationInfo GetAllAssets()
        {
            OrganisationInfo organisationInfo = new OrganisationInfo();
            List<string> filters = null;
            Dictionary<string, List<UnityEngine.Object>> tempCollectedAssets = null;
            string debugLog = string.Empty;
            foreach (AssetType assetType in Enum.GetValues(typeof(AssetType)))
            {
                tempCollectedAssets = GetAssets(assetType);
                filters = new List<string>(SplitDirectory(CurrentDirectory));
                filters.Add(assetType.ToString());
                debugLog = "Retrieved All Assets For AssetType: ".Bold() + assetType.ToString() + "\n" + "\n";

                int counter = 1;
                foreach (KeyValuePair<string, List<UnityEngine.Object>> directoryWithAssets in tempCollectedAssets)
                {
                    debugLog += "Directory (Current) #".Bold() + counter + ": ".Bold() + directoryWithAssets.Key+ "\n";
                    debugLog += "Directory (Generated) #".Bold() + counter + ": ".Bold() + CurrentDirectory + "/" + assetType.ToString() + "/" + GetFormattedNewFolderName(directoryWithAssets.Key, filters) + "\n";
                    debugLog += "Directory #".Bold() + counter + " Assets: ".Bold();
                    foreach (UnityEngine.Object asset in directoryWithAssets.Value)
                        debugLog += (asset.name + ", ").Italic();
                    debugLog = debugLog.Remove(debugLog.LastIndexOf(","), 2);
                    debugLog += "\n\n";
                    counter++;
                }

                Debug.Log(debugLog);

                organisationInfo.AddInfo(tempCollectedAssets, assetType);
            }

            foreach (AssetDirectoryInfo assetDirectoryInfo in organisationInfo.AssetDirectoryInfos)
            {
                debugLog = "All Directories For AssetType: ".Bold() + assetDirectoryInfo.AssetType.ToString() + "\n\n";

                List<string> singleAssetDirectories = assetDirectoryInfo.GetDirectoriesWithSingleAsset();
                List<UnityEngine.Object> orphanedAssets = new List<UnityEngine.Object>();

                int counter = 1;
                foreach (KeyValuePair<string, List<UnityEngine.Object>> directoryWithAssets in assetDirectoryInfo.DirectoriesWithAssets)
                {
                    if (!singleAssetDirectories.Contains(directoryWithAssets.Key))
                    {
                        debugLog += "Directory (Current) #".Bold() + counter + ": ".Bold() + directoryWithAssets.Key + "\n";
                        debugLog += "Directory (Generated) #".Bold() + counter + ": ".Bold() + CurrentDirectory + "/" + assetDirectoryInfo.AssetType.ToString() + "/" + GetFormattedNewFolderName(directoryWithAssets.Key, filters) + "\n";
                        debugLog += "Directory #".Bold() + counter + " Has: ".Bold() + directoryWithAssets.Value.Count + " Assets." + "\n";
                        counter++;
                    }
                    else
                        foreach (UnityEngine.Object orphanAsset in directoryWithAssets.Value)
                            orphanedAssets.Add(orphanAsset);
                }

                debugLog += "Orphaned Directory: ".Bold() + CurrentDirectory + "/" + assetDirectoryInfo.AssetType.ToString() + "/" + "\n";
                if (orphanedAssets.Count > 0)
                {
                    debugLog += "Orphaned Assets: ".Bold();
                    foreach (UnityEngine.Object orphanAsset in orphanedAssets)
                        debugLog += (orphanAsset.name + ", ").Italic();
                    debugLog = debugLog.Remove(debugLog.LastIndexOf(","), 2);
                    debugLog += "\n";
                }
                Debug.Log(debugLog);
            }

            return (organisationInfo);
            //Debug.Log(debugLog);
        }

        public Dictionary<string, List<UnityEngine.Object>> GetAssets(AssetType assetType)
        {
            //Debug.Log("Filter Is: " + GetFilter(selectedDirectory, fileExtension));
            Dictionary<string, List<UnityEngine.Object>> collectedAssets = new Dictionary<string, List<UnityEngine.Object>>();

            List<string> allAssetGuids = new List<string>();
            List<UnityEngine.Object> allAssets = new List<UnityEngine.Object>();

            List<string> filters = new List<string>(SplitDirectory(CurrentDirectory));

            filters.Add(assetType.ToString());

            foreach (string fileExtension in GetAssetInfoExtensions(assetType))
                allAssetGuids = allAssetGuids.Concat(AssetDatabase.FindAssets(GetFilter(CurrentDirectory, fileExtension))).ToList();

            foreach (Type assetInfoType in GetAssetInfoTypes(assetType))
                allAssets = allAssets.Concat(GetAssetsOfType(allAssetGuids.ToArray(), assetInfoType)).ToList();

            foreach (UnityEngine.Object asset in allAssets)
            {
                if (asset != null)
                {
                    string assetDirectory = AssetDatabase.GetAssetPath(asset).Replace(asset.name, string.Empty);
                    assetDirectory = assetDirectory.Remove(assetDirectory.IndexOf("."));
                    if (!collectedAssets.ContainsKey(assetDirectory))
                        collectedAssets.Add(assetDirectory, new List<UnityEngine.Object>(){asset});
                    else
                        collectedAssets[assetDirectory].Add(asset);
                }
            }

            foreach (KeyValuePair<string, List<UnityEngine.Object>> kvp in collectedAssets)
            {
                string logString = "Assets Found!";

                logString += "\n" + "Current Directory: " + kvp.Key;

                logString += "\n" + "Potential New Directory: " + CurrentDirectory + "/" + CurrentAssetType.ToString() + "/" + GetFormattedNewFolderName(kvp.Key, filters);

                logString += "\n" + "Files: ";

                foreach (UnityEngine.Object asset in kvp.Value)
                    logString += asset.name + ", ";

                //Debug.Log(logString);
            }

            return (collectedAssets);
        }

        public List<UnityEngine.Object> GetAssetsOfType(string[] guids, Type type)
        {
            List<UnityEngine.Object> returnList = new List<UnityEngine.Object>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (!string.IsNullOrEmpty(path))
                {
                    UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath(path, type);
                    if (asset != null && asset.GetType() == type && !returnList.Contains(asset))
                        returnList.Add(asset);
                }
            }

            return (returnList);
        }

        public string GetFilter(string directory, string fileExtension = null)
        {
            if (string.IsNullOrEmpty(directory))
                return (string.Empty);

            string workingDirectory = directory;
            if (workingDirectory.StartsWith("/"))
                workingDirectory = workingDirectory.Substring(1);

            if (!workingDirectory.EndsWith("/"))
                workingDirectory = workingDirectory + "/";

            if (string.IsNullOrEmpty(fileExtension))
                return ("glob:" + "\"" + workingDirectory + "**/*" + "\"");
            else
                return ("glob:" + "\"" + workingDirectory + "**/*" + fileExtension + "\"");
        }

        public string GetFormattedNewFolderName(string directory, List<string> filters)
        {
            string debug = "Getting Formatted Folders For Directory: " + directory + ", Filters Are: ";
            foreach (string filter in filters)
                debug += filter + ", ";
            //Debug.Log(debug);

            List<string> folders = SplitDirectory(directory);

            for (int i = 0; i < folders.Count; i++)
                folders[i] = folders[i].SkipToLetters().UpperFirstLetters().RemoveWhitespace().StripSpecialCharacters().FirstToUpper();

            for (int i = 0; i < filters.Count; i++)
                filters[i] = filters[i].SkipToLetters().UpperFirstLetters().RemoveWhitespace().StripSpecialCharacters().ToLower();

            string firstFolderName = string.Empty;
            string secondFolderName = string.Empty;
            try
            {
                string debugLog = "Found Folders From: " + directory;
                foreach (string folder in folders)
                    debugLog += "\n" + folder;
                //Debug.Log(debugLog);

                string constructedPath = string.Empty;
                foreach (string folder in folders)
                    constructedPath += "/" + folder;

                string validatedPath = string.Empty;
                foreach (string folder in folders)
                    if (!filters.Contains(folder.ToLower()))
                        validatedPath += "/" + folder;

                List<string> validatedFolders = new List<string>();
                foreach (string folder in folders)
                    if (!filters.Contains(folder.ToLower()))
                        validatedFolders.Add(folder);

                if (validatedFolders.Count >= 2)
                {
                    firstFolderName = validatedFolders[validatedFolders.Count - 2];
                    secondFolderName = validatedFolders[validatedFolders.Count - 1];
                }
                else if (validatedFolders.Count == 1)
                    firstFolderName = validatedFolders.Last();



                //Debug.Log("Constructed Path: " + constructedPath + "\n" + "Validated Path: " + validatedPath + "\n" + "FirstFolderName: " + firstFolderName + "\n" + "SecondFolderName: " + secondFolderName); ;
            }
            catch
            {
                Debug.LogError("Failed To Format Folder Name: " + directory);
            }

            string returnString = string.Empty;
            if (!string.IsNullOrEmpty(firstFolderName) && !string.IsNullOrEmpty(secondFolderName))
                return (firstFolderName + "_" + secondFolderName + "/");
            else if (!string.IsNullOrEmpty(firstFolderName) && string.IsNullOrEmpty(secondFolderName))
                return (firstFolderName + "/");
            else if (string.IsNullOrEmpty(firstFolderName) && !string.IsNullOrEmpty(secondFolderName))
                return (secondFolderName + "/");
            else
                return (string.Empty);
        }

        public void GetAllAssetFolders()
        {
            WindowSettings.primaryDirectoriesToBuild = new List<(string, string)>();
            WindowSettings.subDirectoriesToBuild = new List<(string, string)>();
            List<string> newDirectories = new List<string>();
            List<string> filters = new List<string>(SplitDirectory(TargetDirectory)).Concat(SplitDirectory(CurrentDirectory)).ToList();
            foreach (AssetType assetType in Enum.GetValues(typeof(AssetType)))
            {
                //newDirectories.Add(TargetDirectory + "/" + assetType.ToString());
                WindowSettings.primaryDirectoriesToBuild.Add((TargetDirectory, assetType.ToString()));
                //LEConverterWindow.GetOrCreateFolder(TargetDirectory, assetType.ToString(), out string _);
                filters.Add(assetType.ToString());
            }

            foreach (AssetDirectoryInfo assetDirectoryInfo in GetAllAssets().AssetDirectoryInfos)
            {
                foreach (KeyValuePair<string, List<UnityEngine.Object>> directoryWithAssets in assetDirectoryInfo.DirectoriesWithAssets)
                    if (directoryWithAssets.Value.Count > 1)
                    {
                        string directory = GetFormattedNewFolderName(directoryWithAssets.Key, filters);
                        directory = directory.Remove(directory.LastIndexOf("/"));
                        WindowSettings.subDirectoriesToBuild.Add((TargetDirectory + "/" + assetDirectoryInfo.AssetType.ToString(), directory));
                    }
            }
        }

        public void CreateAllAssetFolders()
        {
            foreach ((string, string) primaryDirectory in WindowSettings.primaryDirectoriesToBuild)
                AssetDatabase.CreateFolder(primaryDirectory.Item1, primaryDirectory.Item2);

            foreach ((string,string) subDirectory in WindowSettings.subDirectoriesToBuild)
                AssetDatabase.CreateFolder(subDirectory.Item1, subDirectory.Item2);
        }

        public void CreateAssetFolders(List<string> directories)
        { 
            string debugLog = "Totally Creating Folders rn: " + "\n\n";

            foreach (string directory in directories)
                debugLog += "Directory: ".Bold() + directory + "\n";

            Debug.Log(debugLog);
        }

        //Insanely Cursed I Know
        public List<Type> GetAssetInfoTypes(AssetType assetType)
        {
            switch (assetType)
            {
                case AssetType.Textures:
                    return (new List<Type>(){typeof(Texture2D)});
                case AssetType.Materials:
                    return (new List<Type>(){typeof(Material), typeof(PhysicMaterial)});
                case AssetType.Meshes:
                    return (new List<Type>(){typeof(Mesh)});
                case AssetType.Audio:
                    return (new List<Type>(){typeof(AudioClip)});
                case AssetType.Animation:
                    return (new List<Type>() { typeof(AnimationClip), typeof(AnimatorController), typeof(Avatar) });
                case AssetType.Prefabs:
                    return (new List<Type>() { typeof(GameObject) });
                case AssetType.Scenes:
                    return (new List<Type>() { typeof(SceneAsset) });
                case AssetType.Terrain:
                    return (new List<Type>() { typeof(TerrainData), typeof(TerrainLayer) });
                case AssetType.Other:
                    return (new List<Type>());
                default:
                    return (new List<Type>());
            }
        }

        public List<string> GetAssetInfoExtensions(AssetType assetType)
        {
            switch (assetType)
            {
                case AssetType.Textures:
                    return (new List<string>() { ".png", ".jpg", ".jpeg", ".tif"});
                case AssetType.Materials:
                    return (new List<string>() { ".mat", ".physicMaterial" });
                case AssetType.Meshes:
                    return (new List<string>() { ".fbx", ".obj", ".stl" });
                case AssetType.Audio:
                    return (new List<string>() { ".mp3", ".wav", ".ogg", ".m4a" });
                case AssetType.Animation:
                    return (new List<string>() { ".anim", ".fbx", ".controller" });
                case AssetType.Prefabs:
                    return (new List<string>() { ".prefab" });
                case AssetType.Scenes:
                    return (new List<string>() { ".unity" });
                case AssetType.Terrain:
                    return (new List<string>());
                case AssetType.Other:
                    return (new List<string>());
                default:
                    return (new List<string>());
            }
        }

        public List<string> SplitDirectory(string directory)
        {
            List<string> returnList = new List<string>();
            string tempDirectory = directory;


            /*while (!string.IsNullOrEmpty(directory) && directory.Length > 1 && directory.Contains("/"))
            {
                returnList.Add(directory.Replace(directory.Substring(directory.IndexOf("/")), string.Empty).SkipToLetters().RemoveWhitespace().StripSpecialCharacters());
                if (!(directory.StartsWith("/") && !directory.EndsWith("/") && directory.Length == 1))
                {
                    directory = directory.Substring(directory.IndexOf('/') + 1);
                    returnList.Add(directory.SkipToLetters().RemoveWhitespace().StripSpecialCharacters());
                }
            }*/

            while (tempDirectory.Contains("/"))
            {
                if (tempDirectory.StartsWith("/"))
                {
                    if (tempDirectory.Length > 1)
                        tempDirectory = tempDirectory.Substring(1);
                    else
                        tempDirectory = string.Empty;
                }

                if (tempDirectory.Contains('/'))
                {
                    returnList.Add(tempDirectory.Replace(tempDirectory.Substring(tempDirectory.IndexOf('/')), string.Empty));
                    tempDirectory = tempDirectory.Substring(tempDirectory.IndexOf('/'));
                }
                else if (!string.IsNullOrEmpty(tempDirectory))
                    returnList.Add(tempDirectory);
            }

            string debugLog = "Split Directory: " + directory;
            foreach (string folder in returnList)
                debugLog += "\n" + folder;
            //Debug.Log(debugLog);

            return (returnList);
        }
    }
}
