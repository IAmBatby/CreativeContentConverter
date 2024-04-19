using LethalSDK.ScriptableObjects;
using System;
using UnityEngine;

namespace LethalSDK.Utils
{
    [Serializable]
    public struct StringIntPair
    {
        public string _string;
        public int _int;
        public StringIntPair(string _string, int _int)
        {
            this._string = _string.RemoveNonAlphanumeric(1);
            this._int = Mathf.Clamp(_int, 0, 100);
        }
    }
    [Serializable]
    public struct StringStringPair
    {
        public string _string1;
        public string _string2;
        public StringStringPair(string _string1, string _string2)
        {
            this._string1 = _string1.RemoveNonAlphanumeric(1);
            this._string2 = _string2.RemoveNonAlphanumeric(1);
        }
    }
    [Serializable]
    public struct IntIntPair
    {
        public int _int1;
        public int _int2;
        public IntIntPair(int _int1, int _int2)
        {
            this._int1 = _int1;
            this._int2 = _int2;
        }
    }
    [Serializable]
    public struct DungeonFlowPair
    {
        public int ID;
        [Range(0, 300)]
        public int Rarity;
        public DungeonFlowPair(int id, int rarity)
        {
            this.ID = id;
            this.Rarity = Mathf.Clamp(rarity, 0, 300); ;
        }
    }
    [Serializable]
    public struct SpawnableScrapPair
    {
        public string ObjectName;
        [Range(0, 100)]
        public int SpawnWeight;
        public SpawnableScrapPair(string objectName, int spawnWeight)
        {
            this.ObjectName = objectName.RemoveNonAlphanumeric(1);
            this.SpawnWeight = Mathf.Clamp(spawnWeight, 0, 100);
        }
    }
    [Serializable]
    public struct SpawnableMapObjectPair
    {
        public string ObjectName;
        public bool SpawnFacingAwayFromWall;
        public AnimationCurve SpawnRate;
        public SpawnableMapObjectPair(string objectName, bool spawnFacingAwayFromWall, AnimationCurve spawnRate)
        {
            this.ObjectName = objectName.RemoveNonAlphanumeric(1);
            this.SpawnFacingAwayFromWall = spawnFacingAwayFromWall;
            this.SpawnRate = spawnRate;
        }
    }
    [Serializable]
    public struct SpawnableOutsideObjectPair
    {
        public string ObjectName;
        public AnimationCurve SpawnRate;
        public SpawnableOutsideObjectPair(string objectName, AnimationCurve spawnRate)
        {
            this.ObjectName = objectName.RemoveNonAlphanumeric(1);
            this.SpawnRate = spawnRate;
        }
    }
    [Serializable]
    public struct SpawnableEnemiesPair
    {
        public string EnemyName;
        [Range(0, 100)]
        public int SpawnWeight;
        public SpawnableEnemiesPair(string enemyName, int spawnWeight)
        {
            this.EnemyName = enemyName.RemoveNonAlphanumeric(1);
            this.SpawnWeight = Mathf.Clamp(spawnWeight, 0, 100);
        }
    }
    [Serializable]
    public struct ScrapSpawnChancePerScene
    {
        public string SceneName;
        [Range(0, 100)]
        public int SpawnWeight;
        public ScrapSpawnChancePerScene(string sceneName, int spawnWeight)
        {
            this.SceneName = sceneName.RemoveNonAlphanumeric(1);
            this.SpawnWeight = Mathf.Clamp(spawnWeight, 0, 100);
        }
    }
    [Serializable]
    public struct ScrapInfoPair
    {
        public string ScrapPath;
        public Scrap Scrap;
        public ScrapInfoPair(string scrapPath, Scrap scrap)
        {
            this.ScrapPath = scrapPath.RemoveNonAlphanumeric(4);
            this.Scrap = scrap;
        }
    }
    [Serializable]
    public struct AudioClipInfoPair
    {
        public string AudioClipName;
        [HideInInspector]
        public string AudioClipPath;
        [SerializeField]
        public AudioClip AudioClip;
        public AudioClipInfoPair(string audioClipName, string audioClipPath)
        {
            this.AudioClipName = audioClipName.RemoveNonAlphanumeric(1);
            this.AudioClipPath = audioClipPath.RemoveNonAlphanumeric(4);
            AudioClip = null;
        }
    }
    [Serializable]
    public struct PlanetPrefabInfoPair
    {
        public string PlanetPrefabName;
        [HideInInspector]
        public string PlanetPrefabPath;
        [SerializeField]
        public GameObject PlanetPrefab;
        public PlanetPrefabInfoPair(string planetPrefabName, string planetPrefabPath)
        {
            this.PlanetPrefabName = planetPrefabName.RemoveNonAlphanumeric(1);
            this.PlanetPrefabPath = planetPrefabPath.RemoveNonAlphanumeric(4);
            PlanetPrefab = null;
        }
    }
    [Serializable]
    public struct PrefabInfoPair
    {
        public string PrefabName;
        [HideInInspector]
        public string PrefabPath;
        [SerializeField]
        public GameObject Prefab;
        public PrefabInfoPair(string prefabName, string prefabPath)
        {
            this.PrefabName = prefabName.RemoveNonAlphanumeric(1);
            this.PrefabPath = prefabPath.RemoveNonAlphanumeric(4);
            Prefab = null;
        }
    }
    [Serializable]
    public struct RandomWeatherPair
    {
        public LevelWeatherType Weather;
        [TooltipAttribute("Thunder Frequency, Flooding speed or minimum initial enemies in eclipses")]
        public int WeatherVariable1;
        [TooltipAttribute("Flooding offset when Weather is Flooded")]
        public int WeatherVariable2;
        public RandomWeatherPair(LevelWeatherType weather, int weatherVariable1, int weatherVariable2)
        {
            this.Weather = weather;
            this.WeatherVariable1 = weatherVariable1;
            this.WeatherVariable2 = weatherVariable2;
        }
    }
    public enum LevelWeatherType
    {
        None = -1,
        DustClouds = 0,
        Rainy = 1,
        Stormy = 2,
        Foggy = 3,
        Flooded = 4,
        Eclipsed = 5
    }
}
