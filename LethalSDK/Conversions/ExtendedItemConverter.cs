using LethalLevelLoader;
using LethalSDK.Converter;
using LethalSDK.ScriptableObjects;
using LethalSDK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace LethalSDK.Conversions
{
    public static class ExtendedItemConverter
    {
        public static void ConvertScrap(ExtendedItem extendedItem, Item item, Scrap scrap)
        {
            extendedItem.Item = item;
            LevelMatchingProperties newMatchingProperties = null;

            if (scrap.useGlobalSpawnWeight == true && scrap.globalSpawnWeight != 0)
            {
                if (newMatchingProperties == null)
                    newMatchingProperties = CreateNewMatchingProperties(LEConverterWindow.GetSanitisedScrapName(scrap));
                newMatchingProperties.levelTags.Add(new StringWithRarity("Vanilla", scrap.globalSpawnWeight));
                newMatchingProperties.levelTags.Add(new StringWithRarity("Custom", scrap.globalSpawnWeight));
            }

            if (scrap.perPlanetSpawnWeight().Length != 0)
            {
                if (newMatchingProperties == null)
                    newMatchingProperties = CreateNewMatchingProperties(LEConverterWindow.GetSanitisedScrapName(scrap));
                foreach (ScrapSpawnChancePerScene scrapSpawnChancePerScene in scrap.perPlanetSpawnWeight())
                        if (!string.IsNullOrEmpty(scrapSpawnChancePerScene.SceneName) && scrapSpawnChancePerScene.SceneName != "Others" && scrapSpawnChancePerScene.SpawnWeight != 0)
                            newMatchingProperties.planetNames.Add(new StringWithRarity(scrapSpawnChancePerScene.SceneName, scrapSpawnChancePerScene.SpawnWeight));
            }

            if (newMatchingProperties != null)
                extendedItem.SetLevelMatchingProperties(newMatchingProperties);

            //EditorUtility.SetDirty(extendedItem);
        }

        public static LevelMatchingProperties CreateNewMatchingProperties(string assetName)
        {
            LevelMatchingProperties newMatchingProperties = ScriptableObject.CreateInstance<LevelMatchingProperties>();
            newMatchingProperties.name = assetName + "LevelMatchingProperties";
            AssetDatabase.CreateAsset(newMatchingProperties, LEConverterWindow.currentModContentDataDirectory + "/" + newMatchingProperties.name + ".asset");
            LevelMatchingProperties matchingProperties = (LevelMatchingProperties)AssetDatabase.LoadAssetAtPath(LEConverterWindow.currentModContentDataDirectory + "/" + assetName + "LevelMatchingProperties" + ".asset", typeof(LevelMatchingProperties));
            if (!LEConverterWindow.dirtiedAssetsList.Contains(newMatchingProperties))
                LEConverterWindow.dirtiedAssetsList.Add(newMatchingProperties);
            return (matchingProperties);
        }
    }
}
