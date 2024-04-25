using LethalLevelLoader;
using LethalSDK.Converter;
using LethalSDK.ScriptableObjects;
using LethalSDK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;
using static UnityEditor.Progress;

namespace LethalSDK.Conversions
{
    public static class SelectableLevelConverter
    {
        public static List<GameObject> prefabs = new List<GameObject>();
        public static List<SelectableLevel> selectableLevels = new List<SelectableLevel>();
        public static List<Item> items = new List<Item>();
        public static List<EnemyType> enemyTypes = new List<EnemyType>();
        public static List<ModManifest> manifests = new List<ModManifest>();
        public static List<AudioClip> audioClips = new List<AudioClip>();
        public static List<ExtendedItem> extendedItems = new List<ExtendedItem>();
        public static List<AudioMixerGroup> audioMixers = new List<AudioMixerGroup>();
        public static List<Sprite> sprites = new List<Sprite>();

        [InitializeOnLoadMethod]
        public static void ForcePopulate()
        {
            prefabs.Clear();
            selectableLevels.Clear();
            items.Clear();
            enemyTypes.Clear();
            manifests.Clear();
            audioClips.Clear();
            extendedItems.Clear();
            sprites.Clear();
            //
            if (LEConverterWindow._settings != null)
                PopulateReferenceLists(LEConverterWindow.WindowSettings.modsRootDirectory);
            else
                PopulateReferenceLists("Assets/LethalCompany/Mods/");
        }

        public static void PopulateReferenceLists(string blacklistedPath)
        {
            //blacklistedPath = string.Empty;
            if (items.Count == 0)
            {
                items = new List<Item>();
                string[] itemGuids = AssetDatabase.FindAssets("t:Item");
                foreach (string guid in itemGuids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (!path.Contains(blacklistedPath))
                    {
                        Item item = (Item)AssetDatabase.LoadAssetAtPath<Item>(path);
                        items.Add(item);
                    }
                }
                //Debug.Log("Populated Items List With " + items.Count + " Entries.");
            }

            extendedItems = new List<ExtendedItem>();
            string[] extendedItemGuids = AssetDatabase.FindAssets("t:ExtendedItem");
            foreach (string guid in extendedItemGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ExtendedItem extendedItem = (ExtendedItem)AssetDatabase.LoadAssetAtPath<ExtendedItem>(path);
                if (extendedItem.Item != null)
                    extendedItems.Add(extendedItem);
            }

            if (enemyTypes.Count == 0)
            {
                enemyTypes = new List<EnemyType>();
                string[] enemyGuids = AssetDatabase.FindAssets("t:EnemyType");
                foreach (string guid in enemyGuids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (!path.Contains(blacklistedPath))
                    {
                        EnemyType enemyType = (EnemyType)AssetDatabase.LoadAssetAtPath<EnemyType>(path);
                        enemyTypes.Add(enemyType);
                    }
                }
                //Debug.Log("Populated Enemies List With " + enemyTypes.Count + " Entries.");
            }

            if (selectableLevels.Count == 0)
            {
                selectableLevels = new List<SelectableLevel>();
                string[] levelGuids = AssetDatabase.FindAssets("t:SelectableLevel");
                foreach (string guid in levelGuids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (!string.IsNullOrEmpty(path))
                    {
                        if (!path.Contains(blacklistedPath))
                        {
                            SelectableLevel level = AssetDatabase.LoadAssetAtPath<SelectableLevel>(path);
                            if (level != null)
                            {
                                //Debug.Log("Found Prefab: " + level.name);
                                selectableLevels.Add(level);
                            }
                        }
                    }
                }
                //Debug.Log("Populated Levels List With " + selectableLevels.Count + " Entries.");
            }

            if (prefabs.Count == 0)
            {
                prefabs = new List<GameObject>();
                string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");
                foreach (string guid in prefabGuids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (!string.IsNullOrEmpty(path))
                    {
                        if (!path.Contains(blacklistedPath))
                        {
                            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                            if (prefab != null)
                                prefabs.Add(prefab);
                        }
                    }
                }
                //Debug.Log("Populated Prefabs List With " + prefabs.Count + " Entries.");
            }

            if (manifests.Count == 0)
            {
                manifests = new List<ModManifest>();
                string[] manifestGuids = AssetDatabase.FindAssets("t:ModManifest");
                foreach (string guid in manifestGuids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (!string.IsNullOrEmpty(path))
                    {
                        ModManifest modManifest = AssetDatabase.LoadAssetAtPath<ModManifest>(path);
                        if (modManifest != null)
                        {
                            manifests.Add(modManifest);
                        }
                    }
                }
            }

            if (audioClips.Count == 0)
            {
                audioClips = new List<AudioClip>();
                string[] audioClipGuids = AssetDatabase.FindAssets("t:AudioClip");
                foreach (string guid in audioClipGuids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (!string.IsNullOrEmpty(path))
                    {
                        if (!path.Contains(blacklistedPath))
                        {
                            AudioClip audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
                            if (audioClip != null)
                                audioClips.Add(audioClip);
                        }
                    }
                }
            }

            if (audioMixers.Count == 0)
            {
                audioMixers = new List<AudioMixerGroup>();
                string[] audioMixerGuids = AssetDatabase.FindAssets("t:AudioMixerGroup");
                foreach (string guid in audioMixerGuids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (!string.IsNullOrEmpty(path))
                    {
                        if (!path.Contains(blacklistedPath))
                        {
                            AudioMixerGroup audioMixer = AssetDatabase.LoadAssetAtPath<AudioMixerGroup>(path);
                            if (audioMixer != null)
                                audioMixers.Add(audioMixer);
                        }
                    }
                }
            }

            if (sprites.Count == 0)
            {
                sprites = new List<Sprite>();
                string[] spriteGuids = AssetDatabase.FindAssets("t:Sprite");
                foreach (string guid in spriteGuids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (!string.IsNullOrEmpty(path))
                    {
                        if (!path.Contains(blacklistedPath))
                        {
                            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                            if (sprite != null)
                                sprites.Add(sprite);
                        }
                    }
                }
            }

            //AssetDatabase.Refresh();
        }



        public static void ConvertAsset(Moon moon, ModManifest moonManifest, ref SelectableLevel selectableLevel)
        {
            //Debug.Log("Coverting Moon: " + moon.MoonName + " To " + selectableLevel.name);

            //Undo.RecordObject(selectableLevel, "Converted Moon Data");

            selectableLevel.PlanetName = moon.PlanetName;
            selectableLevel.sceneName = LEConverterWindow.GetSanitisedPlanetName(moon) + "Scene";

            Dictionary<string, GameObject> potentialPlanetPrefabsDict = new Dictionary<string, GameObject>();
            foreach (SelectableLevel level in selectableLevels)
                if (level.planetPrefab != null && !potentialPlanetPrefabsDict.ContainsKey(level.planetPrefab.name.SkipToLetters().RemoveWhitespace().ToLower()))
                    potentialPlanetPrefabsDict.Add(level.planetPrefab.name.SkipToLetters().RemoveWhitespace().ToLower(), level.planetPrefab);
            if (moonManifest.assetBank != null)
                foreach (PlanetPrefabInfoPair planetPrefabInfo in moonManifest.assetBank._planetPrefabs)
                    if (planetPrefabInfo.PlanetPrefab != null && !potentialPlanetPrefabsDict.ContainsKey(planetPrefabInfo.PlanetPrefabName.SkipToLetters().RemoveWhitespace().ToLower()))
                        potentialPlanetPrefabsDict.Add(planetPrefabInfo.PlanetPrefabName.SkipToLetters().RemoveWhitespace().ToLower(), planetPrefabInfo.PlanetPrefab);

            //Primary Search
            foreach (KeyValuePair<string, GameObject> keyValuePair in potentialPlanetPrefabsDict)
                if (moon.OrbitPrefabName.SkipToLetters().RemoveWhitespace().ToLower() == keyValuePair.Key)
                    selectableLevel.planetPrefab = keyValuePair.Value;

            //Secondary Search
            if (selectableLevel.planetPrefab == null)
                foreach (KeyValuePair<string, GameObject> keyValuePair in potentialPlanetPrefabsDict)
                    if (moon.OrbitPrefabName.SkipToLetters().RemoveWhitespace().ToLower().Contains(keyValuePair.Key))
                        selectableLevel.planetPrefab = keyValuePair.Value;

            //Safeguard
            if (selectableLevel.planetPrefab == null)
            {
                selectableLevel.planetPrefab = potentialPlanetPrefabsDict.Values.ToList()[0];
                string debugString = "Couldn't Find Planet Prefab For Level: " + selectableLevel.PlanetName + ". Assigning One For Safety!" + "\n" + "\n";
                foreach (KeyValuePair<string, GameObject> planetPrefab in potentialPlanetPrefabsDict)
                    debugString += "Moon OrbitName: " + moon.OrbitPrefabName + " | Found Moon Dictionary Name: " + planetPrefab.Key + "\n";
                Debug.LogError(debugString);
            }

            GameObject clonedPlanetPrefab = PrefabUtility.SaveAsPrefabAsset(selectableLevel.planetPrefab, LEConverterWindow.currentModContentDataDirectory + "/" + LEConverterWindow.GetSanitisedPlanetName(moon) + "PlanetPrefab" + ".prefab");
            selectableLevel.planetPrefab = clonedPlanetPrefab;

            if (selectableLevel.planetPrefab != null && selectableLevel.planetPrefab.GetComponent<Animator>() == null)
                selectableLevel.planetPrefab.AddComponent<Animator>().runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>(AssetDatabase.GetAssetPath(potentialPlanetPrefabsDict.Values.ToList()[0].GetComponent<Animator>().runtimeAnimatorController));

            selectableLevel.spawnEnemiesAndScrap = moon.SpawnEnemiesAndScrap;
            selectableLevel.LevelDescription = moon.PlanetDescription;

            if (moon.PlanetVideo != null)
            {
                string targetPath = LEConverterWindow.currentModContentDataDirectory + "/" + LEConverterWindow.GetSanitisedPlanetName(moon) + "VideoReel" + ".mp4";
                string sourcePath = AssetDatabase.GetAssetPath(moon.PlanetVideo);
                AssetDatabase.CopyAsset(sourcePath, targetPath);
                selectableLevel.videoReel = AssetDatabase.LoadAssetAtPath<VideoClip>(targetPath);
                selectableLevel.videoReel.name = LEConverterWindow.GetSanitisedPlanetName(moon) + "VideoReel";
            }


            selectableLevel.riskLevel = moon.RiskLevel;
            selectableLevel.timeToArrive = moon.TimeToArrive;
            selectableLevel.DaySpeedMultiplier = moon.DaySpeedMultiplier;
            selectableLevel.planetHasTime = moon.PlanetHasTime;

            foreach (SelectableLevel level in selectableLevels)
                if (level.levelAmbienceClips != null && level.levelAmbienceClips.name.Contains("Level1"))
                    selectableLevel.levelAmbienceClips = level.levelAmbienceClips;

            //Weather Values
            List<RandomWeatherWithVariables> randomWeatherList = new List<RandomWeatherWithVariables>();
            foreach (RandomWeatherPair randomWeatherPair in moon.RandomWeatherTypes())
            {
                RandomWeatherWithVariables newRandomWeather = new RandomWeatherWithVariables();
                newRandomWeather.weatherType = (LevelWeatherType)randomWeatherPair.Weather;
                newRandomWeather.weatherVariable = randomWeatherPair.WeatherVariable1;
                newRandomWeather.weatherVariable2 = randomWeatherPair.WeatherVariable2;
                randomWeatherList.Add(newRandomWeather);
            }
            selectableLevel.randomWeathers = randomWeatherList.ToArray();
            selectableLevel.overrideWeather = moon.OverwriteWeather;
            selectableLevel.overrideWeatherType = (LevelWeatherType)moon.OverwriteWeatherType;
            selectableLevel.currentWeather = LevelWeatherType.None;

            //Dungeon Values
            selectableLevel.factorySizeMultiplier = moon.FactorySizeMultiplier;

            List<IntWithRarity> dungeonFlowTypes = new List<IntWithRarity>();
            foreach (DungeonFlowPair dungeonFlowPair in moon.DungeonFlowTypes())
            {
                IntWithRarity intWithRarity = new IntWithRarity();
                intWithRarity.rarity = dungeonFlowPair.Rarity;
                intWithRarity.id = dungeonFlowPair.ID;
                dungeonFlowTypes.Add(intWithRarity);
            }
            selectableLevel.dungeonFlowTypes = dungeonFlowTypes.ToArray();


            //Scrap Values
            selectableLevel.minScrap = moon.MinScrap;
            selectableLevel.maxScrap = moon.MaxScrap;
            List<SpawnableItemWithRarity> spawnableItemsList = new List<SpawnableItemWithRarity>();
            foreach (SpawnableScrapPair spawnableScrapPair in moon.SpawnableScrap())
            {
                foreach (Item item in items)
                    if (item.spawnPrefab != null && (spawnableScrapPair.ObjectName == item.spawnPrefab.name || spawnableScrapPair.ObjectName == item.name))
                    {
                        SpawnableItemWithRarity spawnableItemWithRarity = new SpawnableItemWithRarity();
                        spawnableItemWithRarity.spawnableItem = item;
                        spawnableItemWithRarity.rarity = spawnableScrapPair.SpawnWeight;
                        spawnableItemsList.Add(spawnableItemWithRarity);
                    }

                foreach (ExtendedItem extendedItem in extendedItems)
                {
                    string sanitisedExtendedItemName = extendedItem.Item.itemName.SkipToLetters().RemoveWhitespace().StripSpecialCharacters().ToLower();
                    string sanitisedExtendedItemSpawnPrefabName = extendedItem.Item.spawnPrefab.name.SkipToLetters().RemoveWhitespace().StripSpecialCharacters().ToLower();
                    string sanitisedscrapItemName = spawnableScrapPair.ObjectName.SkipToLetters().RemoveWhitespace().StripSpecialCharacters().ToLower();

                    if (extendedItem.Item.spawnPrefab != null && (sanitisedscrapItemName == sanitisedExtendedItemSpawnPrefabName || sanitisedscrapItemName == sanitisedExtendedItemName))
                    {
                        SpawnableItemWithRarity spawnableItemWithRarity = new SpawnableItemWithRarity();
                        spawnableItemWithRarity.spawnableItem = extendedItem.Item;
                        spawnableItemWithRarity.rarity = spawnableScrapPair.SpawnWeight;
                        spawnableItemsList.Add(spawnableItemWithRarity);
                    }
                }
            }




            selectableLevel.spawnableScrap = new List<SpawnableItemWithRarity>(spawnableItemsList);
            if (selectableLevel.spawnableScrap.Count == 0)
            {
                Debug.LogError("No Scrap In List Found! Adding Temp To Avoid Crash!");
                foreach (Item item in items)
                    if (item.name == "Bell")
                    {
                        SpawnableItemWithRarity spawnableItemWithRarity = new SpawnableItemWithRarity();
                        spawnableItemWithRarity.spawnableItem = item;
                        spawnableItemWithRarity.rarity = 0;
                        selectableLevel.spawnableScrap.Add(spawnableItemWithRarity);
                        break;
                    }
            }

            //Spawnable Map & Outside Values
            List<SpawnableMapObject> spawnableMapObjects = new List<SpawnableMapObject>();
            List<SpawnableOutsideObject> spawnableOutsideObjects = new List<SpawnableOutsideObject>();
            List<GameObject> spawnableMapObjectPrefabs = new List<GameObject>();
            foreach (SelectableLevel level in selectableLevels)
            {
                foreach (SpawnableMapObject spawnableMapObject in level.spawnableMapObjects)
                    if (spawnableMapObject.prefabToSpawn != null && !spawnableMapObjectPrefabs.Contains(spawnableMapObject.prefabToSpawn))
                        spawnableMapObjectPrefabs.Add(spawnableMapObject.prefabToSpawn);

                foreach (SpawnableOutsideObjectWithRarity spawnableOutsideObjectWithRarity in level.spawnableOutsideObjects)
                    if (spawnableOutsideObjectWithRarity.spawnableObject != null && !spawnableOutsideObjects.Contains(spawnableOutsideObjectWithRarity.spawnableObject))
                        spawnableOutsideObjects.Add(spawnableOutsideObjectWithRarity.spawnableObject);
            }

            foreach (SpawnableMapObjectPair spawnableMapObjectPair in moon.SpawnableMapObjects())
                foreach (GameObject spawnableMapObject in spawnableMapObjectPrefabs)
                    if (spawnableMapObjectPair.ObjectName == spawnableMapObject.name)
                    {
                        SpawnableMapObject newSpawnableMapObject = new SpawnableMapObject();
                        newSpawnableMapObject.prefabToSpawn = spawnableMapObject;
                        newSpawnableMapObject.spawnFacingAwayFromWall = spawnableMapObjectPair.SpawnFacingAwayFromWall;
                        newSpawnableMapObject.numberToSpawn = spawnableMapObjectPair.SpawnRate;
                        spawnableMapObjects.Add(newSpawnableMapObject);
                    }

            List<SpawnableOutsideObjectWithRarity> spawnableOutsideObjectWithRarities = new List<SpawnableOutsideObjectWithRarity>();
            foreach (SpawnableOutsideObjectPair spawnableOutsideObjectPair in moon.SpawnableOutsideObjects())
                foreach (SpawnableOutsideObject spawnableOutsideObject in spawnableOutsideObjects)
                    if (spawnableOutsideObjectPair.ObjectName == spawnableOutsideObject.name || spawnableOutsideObjectPair.ObjectName == spawnableOutsideObject.prefabToSpawn.name)
                    {
                        SpawnableOutsideObjectWithRarity spawnableOutsideObjectWithRarity = new SpawnableOutsideObjectWithRarity();
                        spawnableOutsideObjectWithRarity.spawnableObject = spawnableOutsideObject;
                        spawnableOutsideObjectWithRarity.randomAmount = spawnableOutsideObjectPair.SpawnRate;
                        spawnableOutsideObjectWithRarities.Add(spawnableOutsideObjectWithRarity);
                    }

            selectableLevel.spawnableMapObjects = spawnableMapObjects.ToArray();
            selectableLevel.spawnableOutsideObjects = spawnableOutsideObjectWithRarities.ToArray();

            //Enemy Values
            foreach (EnemyType enemyType in enemyTypes)
            {
                foreach (SpawnableEnemiesPair spawnableEnemiesPair in moon.Enemies())
                    if (spawnableEnemiesPair.EnemyName == enemyType.enemyName || spawnableEnemiesPair.EnemyName == enemyType.name)
                        selectableLevel.Enemies.Add(CreateNewSpawnableEnemy(enemyType, spawnableEnemiesPair.SpawnWeight));
                foreach (SpawnableEnemiesPair spawnableEnemiesPair in moon.OutsideEnemies())
                    if (spawnableEnemiesPair.EnemyName == enemyType.enemyName || spawnableEnemiesPair.EnemyName == enemyType.name)
                        selectableLevel.OutsideEnemies.Add(CreateNewSpawnableEnemy(enemyType, spawnableEnemiesPair.SpawnWeight));
                foreach (SpawnableEnemiesPair spawnableEnemiesPair in moon.DaytimeEnemies())
                    if (spawnableEnemiesPair.EnemyName == enemyType.enemyName || spawnableEnemiesPair.EnemyName == enemyType.name)
                        selectableLevel.DaytimeEnemies.Add(CreateNewSpawnableEnemy(enemyType, spawnableEnemiesPair.SpawnWeight));
            }
            selectableLevel.maxEnemyPowerCount = moon.MaxEnemyPowerCount;
            selectableLevel.maxOutsideEnemyPowerCount = moon.MaxOutsideEnemyPowerCount;
            selectableLevel.maxDaytimeEnemyPowerCount = moon.MaxDaytimeEnemyPowerCount;

            selectableLevel.enemySpawnChanceThroughoutDay = moon.EnemySpawnChanceThroughoutDay;
            selectableLevel.outsideEnemySpawnChanceThroughDay = moon.OutsideEnemySpawnChanceThroughDay;
            selectableLevel.daytimeEnemySpawnChanceThroughDay = moon.DaytimeEnemySpawnChanceThroughDay;
            selectableLevel.spawnProbabilityRange = moon.SpawnProbabilityRange;
            selectableLevel.daytimeEnemiesProbabilityRange = moon.DaytimeEnemiesProbabilityRange;
            selectableLevel.levelIncludesSnowFootprints = moon.LevelIncludesSnowFootprints;

            SelectableLevel closestComparableVanillaLevel = FindSelectableLevelWithClosestDifficultyRating(selectableLevel);

            if (closestComparableVanillaLevel != null)
            {
                selectableLevel.maxOutsideEnemyPowerCount = closestComparableVanillaLevel.maxOutsideEnemyPowerCount;
                selectableLevel.maxDaytimeEnemyPowerCount = closestComparableVanillaLevel.maxDaytimeEnemyPowerCount;
                selectableLevel.minTotalScrapValue = closestComparableVanillaLevel.minTotalScrapValue;
                selectableLevel.maxTotalScrapValue = closestComparableVanillaLevel.maxTotalScrapValue;
                selectableLevel.riskLevel = closestComparableVanillaLevel.riskLevel;
            }

            //EditorUtility.SetDirty(selectableLevel);
            //AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            //Debug.Log("Finished Conversion");
        }

        public static SpawnableEnemyWithRarity CreateNewSpawnableEnemy(EnemyType enemyType, int rarity)
        {
            SpawnableEnemyWithRarity returnEnemy = new SpawnableEnemyWithRarity();
            returnEnemy.enemyType = enemyType;
            returnEnemy.rarity = rarity;
            return (returnEnemy);
        }

        public static bool TryGetModManifest(Moon moon, out ModManifest manifest)
        {
            manifest = null;
            foreach (ModManifest modManifest in manifests)
                foreach (Moon manifestMoon in modManifest.moons)
                    if (manifestMoon == moon)
                        manifest = modManifest;

            return (manifest);
        }

        public static SelectableLevel FindSelectableLevelWithClosestDifficultyRating(SelectableLevel selectableLevel)
        {
            SelectableLevel returnLevel = null;

            int targetLevelRating = GetDifficultyRating(selectableLevel);
            int closestFoundLevelRating = 0;

            List<int> levelScores = new List<int>();

            foreach (SelectableLevel level in selectableLevels)
                levelScores.Add(GetDifficultyRating(level));

            closestFoundLevelRating = levelScores.OrderBy(item => Math.Abs(targetLevelRating - item)).First();

            foreach (SelectableLevel level in selectableLevels)
                if (GetDifficultyRating(level) == closestFoundLevelRating)
                    returnLevel = level;

            return (returnLevel);

        }

        public static int GetDifficultyRating(SelectableLevel selectableLevel)
        {
            int targetLevelRating = 0;

            int baselineRouteValue = selectableLevel.maxTotalScrapValue;
            targetLevelRating += baselineRouteValue;

            int scrapValue = 0;
            foreach (SpawnableItemWithRarity spawnableScrap in selectableLevel.spawnableScrap)
            {
                if (((spawnableScrap.spawnableItem.minValue + spawnableScrap.spawnableItem.maxValue) * 5) != 0 && spawnableScrap.rarity != 0)
                    if ((spawnableScrap.rarity / 10) != 0)
                        scrapValue += (spawnableScrap.spawnableItem.maxValue - spawnableScrap.spawnableItem.minValue) / (spawnableScrap.rarity / 10);
            }
            targetLevelRating += scrapValue;

            int enemySpawnValue = (selectableLevel.maxEnemyPowerCount + selectableLevel.maxOutsideEnemyPowerCount + selectableLevel.maxDaytimeEnemyPowerCount) * 15;
            enemySpawnValue = enemySpawnValue * 2;
            targetLevelRating += enemySpawnValue;

            float enemyValue = 0;
            foreach (SpawnableEnemyWithRarity spawnableEnemy in selectableLevel.Enemies.Concat(selectableLevel.OutsideEnemies).Concat(selectableLevel.DaytimeEnemies))
                if (spawnableEnemy.rarity != 0)
                    if ((spawnableEnemy.rarity / 10) != 0)
                        enemyValue += (spawnableEnemy.enemyType.PowerLevel * 100) / (spawnableEnemy.rarity / 10);
            targetLevelRating += (int)enemyValue;
            targetLevelRating += Mathf.RoundToInt(targetLevelRating * (selectableLevel.factorySizeMultiplier * 0.5f));

            return (targetLevelRating);
        }
    }
}
