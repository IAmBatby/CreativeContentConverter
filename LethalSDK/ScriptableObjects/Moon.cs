using LethalSDK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

namespace LethalSDK.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Moon", menuName = "LethalSDK/Moon")]
    public class Moon : ScriptableObject
    {
        public string MoonName = "NewMoon";
        public string[] RequiredBundles;
        public string[] IncompatibleBundles;
        public bool IsEnabled = true;
        public bool IsHidden = false;
        public bool IsLocked = false;

        [Header("Info")]
        public string OrbitPrefabName = "Moon1";
        public bool SpawnEnemiesAndScrap = true;
        public string PlanetName = "New Moon";
        public GameObject MainPrefab;
        [TextArea(5, 15)]
        public string PlanetDescription;
        [TextArea(5, 15)]
        public string PlanetLore;
        public VideoClip PlanetVideo;
        public string RiskLevel = "X";
        [Range(0,16)]
        public float TimeToArrive = 1;

        [Header("Time")]
        [Range(0.1f, 5f)]
        public float DaySpeedMultiplier = 1f;
        public bool PlanetHasTime = true;
        [SerializeField]
        private RandomWeatherPair[] _RandomWeatherTypes = new RandomWeatherPair[]
        {
            new RandomWeatherPair(LethalSDK.Utils.LevelWeatherType.None, 0,0),
            new RandomWeatherPair(LethalSDK.Utils.LevelWeatherType.Rainy, 0,0),
            new RandomWeatherPair(LethalSDK.Utils.LevelWeatherType.Stormy, 1,0),
            new RandomWeatherPair(LethalSDK.Utils.LevelWeatherType.Foggy, 1,0),
            new RandomWeatherPair(LethalSDK.Utils.LevelWeatherType.Flooded, -4,5),
            new RandomWeatherPair(LethalSDK.Utils.LevelWeatherType.Eclipsed, 1,0)
        };
        public bool OverwriteWeather = false;
        public LethalSDK.Utils.LevelWeatherType OverwriteWeatherType = LethalSDK.Utils.LevelWeatherType.None;

        [Header("Route")]
        public string RouteWord = "newmoon";
        public int RoutePrice;
        public string BoughtComment = "Please enjoy your flight.";

        [Header("Dungeon")]
        [Range(1f, 5f)]
        public float FactorySizeMultiplier = 1f;
        public int FireExitsAmountOverwrite = 1;
        [SerializeField]
        private DungeonFlowPair[] _DungeonFlowTypes = new DungeonFlowPair[]
        {
            new DungeonFlowPair(0, 300),
            new DungeonFlowPair(1, 1)
        };
        [SerializeField]
        private SpawnableScrapPair[] _SpawnableScrap = new SpawnableScrapPair[]
        {
            new SpawnableScrapPair("Cog1", 80),
            new SpawnableScrapPair("EnginePart1", 90),
            new SpawnableScrapPair("FishTestProp", 12),
            new SpawnableScrapPair("MetalSheet", 88),
            new SpawnableScrapPair("FlashLaserPointer", 4),
            new SpawnableScrapPair("BigBolt", 80),
            new SpawnableScrapPair("BottleBin", 19),
            new SpawnableScrapPair("Ring", 3),
            new SpawnableScrapPair("SteeringWheel", 32),
            new SpawnableScrapPair("MoldPan", 5),
            new SpawnableScrapPair("EggBeater", 10),
            new SpawnableScrapPair("PickleJar", 10),
            new SpawnableScrapPair("DustPan", 32),
            new SpawnableScrapPair("Airhorn", 3),
            new SpawnableScrapPair("ClownHorn", 3),
            new SpawnableScrapPair("CashRegister", 3),
            new SpawnableScrapPair("Candy", 2),
            new SpawnableScrapPair("GoldBar", 1),
            new SpawnableScrapPair("YieldSign", 6)
        };
        public string[] spawnableScrapBlacklist = new string[0];
        [Range(0, 100)]
        public int MinScrap = 8;
        [Range(0, 100)]
        public int MaxScrap = 12;
        public string LevelAmbienceClips = "Level1TypeAmbience";
        [Range(0, 30)]
        public int MaxEnemyPowerCount = 4;
        [SerializeField]
        private SpawnableEnemiesPair[] _Enemies = new SpawnableEnemiesPair[]
        {
            new SpawnableEnemiesPair("Centipede", 51),
            new SpawnableEnemiesPair("SandSpider", 58),
            new SpawnableEnemiesPair("HoarderBug", 28),
            new SpawnableEnemiesPair("Flowerman", 13),
            new SpawnableEnemiesPair("Crawler", 16),
            new SpawnableEnemiesPair("Blob", 31),
            new SpawnableEnemiesPair("DressGirl", 1),
            new SpawnableEnemiesPair("Puffer", 28)
        };
        public AnimationCurve EnemySpawnChanceThroughoutDay = CurveContainer.DeserializeCurve(@"{""curve"":{""serializedVersion"":""2"",""m_Curve"":[{""serializedVersion"":""3"",""time"":0.0015411376953125,""value"":-3.0,""inSlope"":19.556997299194337,""outSlope"":19.556997299194337,""tangentMode"":0,""weightedMode"":3,""inWeight"":0.0,""outWeight"":0.12297855317592621},{""serializedVersion"":""3"",""time"":0.4575331211090088,""value"":4.796203136444092,""inSlope"":24.479534149169923,""outSlope"":24.479534149169923,""tangentMode"":0,""weightedMode"":3,""inWeight"":0.396077424287796,""outWeight"":0.35472238063812258},{""serializedVersion"":""3"",""time"":0.7593884468078613,""value"":4.973001480102539,""inSlope"":2.6163148880004885,""outSlope"":2.6163148880004885,""tangentMode"":0,""weightedMode"":3,""inWeight"":0.2901076376438141,""outWeight"":0.5360636115074158},{""serializedVersion"":""3"",""time"":1.0,""value"":15.0,""inSlope"":35.604026794433597,""outSlope"":35.604026794433597,""tangentMode"":0,""weightedMode"":1,""inWeight"":0.04912583902478218,""outWeight"":0.0}],""m_PreInfinity"":2,""m_PostInfinity"":2,""m_RotationOrder"":4}}");
        [Range(0f, 30f)]
        public float SpawnProbabilityRange = 4f;

        [Header("Outside")]
        [SerializeField]
        private SpawnableMapObjectPair[] _SpawnableMapObjects = new SpawnableMapObjectPair[]
        {
            new SpawnableMapObjectPair("Landmine", false, CurveContainer.DeserializeCurve(@"{""curve"":{""serializedVersion"":""2"",""m_Curve"":[{""serializedVersion"":""3"",""time"":-0.003082275390625,""value"":0.0,""inSlope"":0.23179344832897187,""outSlope"":0.23179344832897187,""tangentMode"":0,""weightedMode"":2,""inWeight"":0.0,""outWeight"":0.27936428785324099},{""serializedVersion"":""3"",""time"":0.8171924352645874,""value"":1.7483322620391846,""inSlope"":7.064207077026367,""outSlope"":7.064207077026367,""tangentMode"":0,""weightedMode"":3,""inWeight"":0.2631833553314209,""outWeight"":0.6898177862167358},{""serializedVersion"":""3"",""time"":1.0002186298370362,""value"":11.760997772216797,""inSlope"":968.80810546875,""outSlope"":968.80810546875,""tangentMode"":0,""weightedMode"":3,""inWeight"":0.029036391526460649,""outWeight"":0.0}],""m_PreInfinity"":2,""m_PostInfinity"":2,""m_RotationOrder"":4}}")),
            new SpawnableMapObjectPair("TurretContainer", true, CurveContainer.DeserializeCurve(@"{""curve"":{""serializedVersion"":""2"",""m_Curve"":[{""serializedVersion"":""3"",""time"":0.0,""value"":0.0,""inSlope"":0.354617178440094,""outSlope"":0.354617178440094,""tangentMode"":0,""weightedMode"":2,""inWeight"":0.0,""outWeight"":0.0},{""serializedVersion"":""3"",""time"":0.9190289974212647,""value"":1.0005745887756348,""inSlope"":Infinity,""outSlope"":1.7338485717773438,""tangentMode"":0,""weightedMode"":3,""inWeight"":0.0,""outWeight"":0.6534967422485352},{""serializedVersion"":""3"",""time"":1.0038425922393799,""value"":7.198680877685547,""inSlope"":529.4945068359375,""outSlope"":529.4945068359375,""tangentMode"":0,""weightedMode"":3,""inWeight"":0.14589552581310273,""outWeight"":0.0}],""m_PreInfinity"":2,""m_PostInfinity"":2,""m_RotationOrder"":4}}"))
        };
        [SerializeField]
        private SpawnableOutsideObjectPair[] _SpawnableOutsideObjects = new SpawnableOutsideObjectPair[]
        {
            new SpawnableOutsideObjectPair("LargeRock1", CurveContainer.DeserializeCurve(@"{""curve"":{""serializedVersion"":""2"",""m_Curve"":[{""serializedVersion"":""3"",""time"":0.0,""value"":0.0,""inSlope"":0.0,""outSlope"":0.0,""tangentMode"":0,""weightedMode"":0,""inWeight"":0.0,""outWeight"":0.0},{""serializedVersion"":""3"",""time"":0.7571572661399841,""value"":0.6448163986206055,""inSlope"":2.974250078201294,""outSlope"":2.974250078201294,""tangentMode"":0,""weightedMode"":0,""inWeight"":0.3333333432674408,""outWeight"":0.3333333432674408},{""serializedVersion"":""3"",""time"":0.9995536804199219,""value"":5.883961200714111,""inSlope"":65.30631256103516,""outSlope"":65.30631256103516,""tangentMode"":0,""weightedMode"":0,""inWeight"":0.12097536772489548,""outWeight"":0.0}],""m_PreInfinity"":2,""m_PostInfinity"":2,""m_RotationOrder"":4}}")),
            new SpawnableOutsideObjectPair("LargeRock2", CurveContainer.DeserializeCurve(@"{""curve"":{""serializedVersion"":""2"",""m_Curve"":[{""serializedVersion"":""3"",""time"":0.0,""value"":0.0,""inSlope"":0.0,""outSlope"":0.0,""tangentMode"":0,""weightedMode"":0,""inWeight"":0.0,""outWeight"":0.0},{""serializedVersion"":""3"",""time"":0.7562879920005798,""value"":1.2308543920516968,""inSlope"":5.111926555633545,""outSlope"":5.111926555633545,""tangentMode"":0,""weightedMode"":0,""inWeight"":0.3333333432674408,""outWeight"":0.21955738961696626},{""serializedVersion"":""3"",""time"":1.0010795593261719,""value"":7.59307336807251,""inSlope"":92.0470199584961,""outSlope"":92.0470199584961,""tangentMode"":0,""weightedMode"":0,""inWeight"":0.05033162236213684,""outWeight"":0.0}],""m_PreInfinity"":2,""m_PostInfinity"":2,""m_RotationOrder"":4}}")),
            new SpawnableOutsideObjectPair("LargeRock3", CurveContainer.DeserializeCurve(@"{""curve"":{""serializedVersion"":""2"",""m_Curve"":[{""serializedVersion"":""3"",""time"":0.0,""value"":0.0,""inSlope"":0.0,""outSlope"":0.0,""tangentMode"":0,""weightedMode"":0,""inWeight"":0.0,""outWeight"":0.0},{""serializedVersion"":""3"",""time"":0.9964686632156372,""value"":2.0009398460388185,""inSlope"":6.82940673828125,""outSlope"":6.82940673828125,""tangentMode"":0,""weightedMode"":0,""inWeight"":0.06891261041164398,""outWeight"":0.0}],""m_PreInfinity"":2,""m_PostInfinity"":2,""m_RotationOrder"":4}}")),
            new SpawnableOutsideObjectPair("LargeRock4", CurveContainer.DeserializeCurve(@"{""curve"":{""serializedVersion"":""2"",""m_Curve"":[{""serializedVersion"":""3"",""time"":0.0,""value"":0.0,""inSlope"":0.0,""outSlope"":0.0,""tangentMode"":0,""weightedMode"":0,""inWeight"":0.0,""outWeight"":0.0},{""serializedVersion"":""3"",""time"":0.9635604619979858,""value"":2.153383493423462,""inSlope"":6.251225471496582,""outSlope"":6.251225471496582,""tangentMode"":0,""weightedMode"":0,""inWeight"":0.07428120821714401,""outWeight"":0.3333333432674408},{""serializedVersion"":""3"",""time"":0.9995394349098206,""value"":5.0,""inSlope"":15.746581077575684,""outSlope"":15.746581077575684,""tangentMode"":0,""weightedMode"":0,""inWeight"":0.06317413598299027,""outWeight"":0.0}],""m_PreInfinity"":2,""m_PostInfinity"":2,""m_RotationOrder"":4}}")),
            new SpawnableOutsideObjectPair("TreeLeafless1", CurveContainer.DeserializeCurve(@"{""curve"":{""serializedVersion"":""2"",""m_Curve"":[{""serializedVersion"":""3"",""time"":0.0,""value"":0.0,""inSlope"":1.6912956237792969,""outSlope"":1.6912956237792969,""tangentMode"":0,""weightedMode"":2,""inWeight"":0.0,""outWeight"":0.27726083993911745},{""serializedVersion"":""3"",""time"":0.776531994342804,""value"":6.162014007568359,""inSlope"":30.075166702270509,""outSlope"":30.075166702270509,""tangentMode"":0,""weightedMode"":3,""inWeight"":0.15920747816562653,""outWeight"":0.5323987007141113},{""serializedVersion"":""3"",""time"":1.0002281665802003,""value"":38.093849182128909,""inSlope"":1448.839111328125,""outSlope"":1448.839111328125,""tangentMode"":0,""weightedMode"":3,""inWeight"":0.0620061457157135,""outWeight"":0.0}],""m_PreInfinity"":2,""m_PostInfinity"":2,""m_RotationOrder"":4}}")),
            new SpawnableOutsideObjectPair("SmallGreyRocks1", CurveContainer.DeserializeCurve(@"{""curve"":{""serializedVersion"":""2"",""m_Curve"":[{""serializedVersion"":""3"",""time"":0.0,""value"":0.0,""inSlope"":1.6912956237792969,""outSlope"":1.6912956237792969,""tangentMode"":0,""weightedMode"":2,""inWeight"":0.0,""outWeight"":0.27726083993911745},{""serializedVersion"":""3"",""time"":0.802714467048645,""value"":1.5478605031967164,""inSlope"":9.096116065979004,""outSlope"":9.096116065979004,""tangentMode"":0,""weightedMode"":3,""inWeight"":0.15920747816562653,""outWeight"":0.58766108751297},{""serializedVersion"":""3"",""time"":1.0002281665802003,""value"":14.584033966064454,""inSlope"":1244.9173583984375,""outSlope"":1244.9173583984375,""tangentMode"":0,""weightedMode"":3,""inWeight"":0.054620321840047839,""outWeight"":0.0}],""m_PreInfinity"":2,""m_PostInfinity"":2,""m_RotationOrder"":4}}")),
            new SpawnableOutsideObjectPair("GiantPumpkin", CurveContainer.DeserializeCurve(@"{""curve"":{""serializedVersion"":""2"",""m_Curve"":[{""serializedVersion"":""3"",""time"":0.0,""value"":0.0,""inSlope"":1.6912956237792969,""outSlope"":1.6912956237792969,""tangentMode"":0,""weightedMode"":2,""inWeight"":0.0,""outWeight"":0.27726083993911745},{""serializedVersion"":""3"",""time"":0.8832725882530212,""value"":0.5284063816070557,""inSlope"":3.2962090969085695,""outSlope"":29.38977813720703,""tangentMode"":0,""weightedMode"":3,""inWeight"":0.19772815704345704,""outWeight"":0.8989489078521729},{""serializedVersion"":""3"",""time"":0.972209095954895,""value"":6.7684478759765629,""inSlope"":140.27394104003907,""outSlope"":140.27394104003907,""tangentMode"":0,""weightedMode"":3,""inWeight"":0.39466607570648196,""outWeight"":0.47049039602279665},{""serializedVersion"":""3"",""time"":1.0002281665802003,""value"":23.0,""inSlope"":579.3037109375,""outSlope"":14.8782377243042,""tangentMode"":0,""weightedMode"":3,""inWeight"":0.648808479309082,""outWeight"":0.0}],""m_PreInfinity"":2,""m_PostInfinity"":2,""m_RotationOrder"":4}}"))
        };
        [Range(0, 30)]
        public int MaxOutsideEnemyPowerCount = 8;
        [Range(0, 30)]
        public int MaxDaytimeEnemyPowerCount = 5;
        [SerializeField]
        private SpawnableEnemiesPair[] _OutsideEnemies = new SpawnableEnemiesPair[]
        {
            new SpawnableEnemiesPair("MouthDog", 75),
            new SpawnableEnemiesPair("ForestGiant", 0),
            new SpawnableEnemiesPair("SandWorm", 56)
        };
        [SerializeField]
        private SpawnableEnemiesPair[] _DaytimeEnemies = new SpawnableEnemiesPair[]
        {
            new SpawnableEnemiesPair("RedLocustBees", 22),
            new SpawnableEnemiesPair("Doublewing", 74),
            new SpawnableEnemiesPair("DocileLocustBees", 52)
        };
        public AnimationCurve OutsideEnemySpawnChanceThroughDay = CurveContainer.DeserializeCurve(@"{""curve"":{""serializedVersion"":""2"",""m_Curve"":[{""serializedVersion"":""3"",""time"":-7.736962288618088e-7,""value"":-2.996999979019165,""inSlope"":Infinity,""outSlope"":0.5040292143821716,""tangentMode"":0,""weightedMode"":3,""inWeight"":0.0,""outWeight"":0.08937685936689377},{""serializedVersion"":""3"",""time"":0.7105481624603272,""value"":-0.6555822491645813,""inSlope"":9.172262191772461,""outSlope"":9.172262191772461,""tangentMode"":0,""weightedMode"":3,""inWeight"":0.3333333432674408,""outWeight"":0.7196550369262695},{""serializedVersion"":""3"",""time"":1.0052626132965088,""value"":5.359400749206543,""inSlope"":216.42247009277345,""outSlope"":11.374387741088868,""tangentMode"":0,""weightedMode"":3,""inWeight"":0.044637180864810947,""outWeight"":0.48315444588661196}],""m_PreInfinity"":2,""m_PostInfinity"":2,""m_RotationOrder"":4}}");
        public AnimationCurve DaytimeEnemySpawnChanceThroughDay = CurveContainer.DeserializeCurve(@"{""curve"":{""serializedVersion"":""2"",""m_Curve"":[{""serializedVersion"":""3"",""time"":0.0,""value"":2.2706568241119386,""inSlope"":-7.500085353851318,""outSlope"":-7.500085353851318,""tangentMode"":0,""weightedMode"":0,""inWeight"":0.3333333432674408,""outWeight"":0.20650266110897065},{""serializedVersion"":""3"",""time"":0.38507816195487978,""value"":-0.0064108967781066898,""inSlope"":-2.7670974731445314,""outSlope"":-2.7670974731445314,""tangentMode"":0,""weightedMode"":0,""inWeight"":0.28388944268226626,""outWeight"":0.30659767985343935},{""serializedVersion"":""3"",""time"":0.6767024993896484,""value"":-7.021658420562744,""inSlope"":-27.286888122558595,""outSlope"":-27.286888122558595,""tangentMode"":0,""weightedMode"":0,""inWeight"":0.10391546785831452,""outWeight"":0.12503522634506226},{""serializedVersion"":""3"",""time"":0.9998173117637634,""value"":-14.818100929260254,""inSlope"":0.0,""outSlope"":0.0,""tangentMode"":0,""weightedMode"":0,""inWeight"":0.0,""outWeight"":0.0}],""m_PreInfinity"":2,""m_PostInfinity"":2,""m_RotationOrder"":4}}");
        [Range(0f, 30f)]
        public float DaytimeEnemiesProbabilityRange = 5f;
        public bool LevelIncludesSnowFootprints = false;

        [HideInInspector]
        public string serializedRandomWeatherTypes;
        [HideInInspector]
        public string serializedDungeonFlowTypes;
        [HideInInspector]
        public string serializedSpawnableScrap;
        [HideInInspector]
        public string serializedEnemies;
        [HideInInspector]
        public string serializedOutsideEnemies;
        [HideInInspector]
        public string serializedDaytimeEnemies;
        [HideInInspector]
        public string serializedSpawnableMapObjects;
        [HideInInspector]
        public string serializedSpawnableOutsideObjects;
        private void OnValidate()
        {
            RequiredBundles = RequiredBundles.RemoveNonAlphanumeric(1);
            IncompatibleBundles = IncompatibleBundles.RemoveNonAlphanumeric(1);
            MoonName = MoonName.RemoveNonAlphanumeric(1);
            OrbitPrefabName = OrbitPrefabName.RemoveNonAlphanumeric(1);
            RiskLevel = RiskLevel.RemoveNonAlphanumeric();
            RouteWord = RouteWord.RemoveNonAlphanumeric(2);
            BoughtComment = BoughtComment.RemoveNonAlphanumeric();
            LevelAmbienceClips = LevelAmbienceClips.RemoveNonAlphanumeric(1);
            TimeToArrive = Mathf.Clamp(TimeToArrive, 0, 16);
            DaySpeedMultiplier = Mathf.Clamp(DaySpeedMultiplier, 0.1f, 5f);
            RoutePrice = Mathf.Clamp(RoutePrice, 0, int.MaxValue);
            FactorySizeMultiplier = Mathf.Clamp(FactorySizeMultiplier, 1f, 5f);
            FireExitsAmountOverwrite = Mathf.Clamp(FireExitsAmountOverwrite, 0, 20);
            MinScrap = Mathf.Clamp(MinScrap, 0, MaxScrap);
            MaxScrap = Mathf.Clamp(MaxScrap, MinScrap, 100);
            MaxEnemyPowerCount = Mathf.Clamp(MaxEnemyPowerCount, 0, 30);
            MaxOutsideEnemyPowerCount = Mathf.Clamp(MaxOutsideEnemyPowerCount, 0, 30);
            MaxDaytimeEnemyPowerCount = Mathf.Clamp(MaxDaytimeEnemyPowerCount, 0, 30);
            SpawnProbabilityRange = Mathf.Clamp(SpawnProbabilityRange, 0f, 30f);
            DaytimeEnemiesProbabilityRange = Mathf.Clamp(DaytimeEnemiesProbabilityRange, 0f, 30f);
            for (int i = 0; i < _SpawnableScrap.Length; i++)
            {
                _SpawnableScrap[i].ObjectName = _SpawnableScrap[i].ObjectName.RemoveNonAlphanumeric(1);
            }
            for (int i = 0; i < _Enemies.Length; i++)
            {
                _Enemies[i].EnemyName = _Enemies[i].EnemyName.RemoveNonAlphanumeric(1);
            }
            for (int i = 0; i < _SpawnableMapObjects.Length; i++)
            {
                _SpawnableMapObjects[i].ObjectName = _SpawnableMapObjects[i].ObjectName.RemoveNonAlphanumeric(1);
            }
            for (int i = 0; i < _SpawnableOutsideObjects.Length; i++)
            {
                _SpawnableOutsideObjects[i].ObjectName = _SpawnableOutsideObjects[i].ObjectName.RemoveNonAlphanumeric(1);
            }
            for (int i = 0; i < _OutsideEnemies.Length; i++)
            {
                _OutsideEnemies[i].EnemyName = _OutsideEnemies[i].EnemyName.RemoveNonAlphanumeric(1);
            }
            for (int i = 0; i < _DaytimeEnemies.Length; i++)
            {
                _DaytimeEnemies[i].EnemyName = _DaytimeEnemies[i].EnemyName.RemoveNonAlphanumeric(1);
            }
            serializedRandomWeatherTypes = string.Join(";", _RandomWeatherTypes.Select(p => $"{(int)p.Weather},{p.WeatherVariable1},{p.WeatherVariable2}"));
            serializedDungeonFlowTypes = string.Join(";", _DungeonFlowTypes.Select(p => $"{p.ID},{p.Rarity}"));
            serializedSpawnableScrap = string.Join(";", _SpawnableScrap.Select(p => $"{p.ObjectName},{p.SpawnWeight}"));
            serializedEnemies = string.Join(";", _Enemies.Select(p => $"{p.EnemyName},{p.SpawnWeight}"));
            serializedOutsideEnemies = string.Join(";", _OutsideEnemies.Select(p => $"{p.EnemyName},{p.SpawnWeight}"));
            serializedDaytimeEnemies = string.Join(";", _DaytimeEnemies.Select(p => $"{p.EnemyName},{p.SpawnWeight}"));
            serializedSpawnableMapObjects = string.Join(";", _SpawnableMapObjects.Select(p => $"{p.ObjectName}|{p.SpawnFacingAwayFromWall}|{CurveContainer.SerializeCurve(p.SpawnRate)}"));
            serializedSpawnableOutsideObjects = string.Join(";", _SpawnableOutsideObjects.Select(p => $"{p.ObjectName}|{CurveContainer.SerializeCurve(p.SpawnRate)}"));
        }
        public RandomWeatherPair[] RandomWeatherTypes()
        {
            return serializedRandomWeatherTypes.Split(';').Select(s => s.Split(',')).Where(split => split.Length == 3).Select(split => new RandomWeatherPair((LethalSDK.Utils.LevelWeatherType)int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]))).ToArray();
        }
        public DungeonFlowPair[] DungeonFlowTypes()
        {
            return serializedDungeonFlowTypes.Split(';').Select(s => s.Split(',')).Where(split => split.Length == 2).Select(split => new DungeonFlowPair(int.Parse(split[0]), int.Parse(split[1]))).ToArray();
        }
        public SpawnableScrapPair[] SpawnableScrap()
        {
            return serializedSpawnableScrap.Split(';').Select(s => s.Split(',')).Where(split => split.Length == 2).Select(split => new SpawnableScrapPair(split[0], int.Parse(split[1]))).ToArray();
        }
        public SpawnableEnemiesPair[] Enemies()
        {
            return serializedEnemies.Split(';').Select(s => s.Split(',')).Where(split => split.Length == 2).Select(split => new SpawnableEnemiesPair(split[0], int.Parse(split[1]))).ToArray();
        }
        public SpawnableEnemiesPair[] OutsideEnemies()
        {
            return serializedOutsideEnemies.Split(';').Select(s => s.Split(',')).Where(split => split.Length == 2).Select(split => new SpawnableEnemiesPair(split[0], int.Parse(split[1]))).ToArray();
        }
        public SpawnableEnemiesPair[] DaytimeEnemies()
        {
            return serializedDaytimeEnemies.Split(';').Select(s => s.Split(',')).Where(split => split.Length == 2).Select(split => new SpawnableEnemiesPair(split[0], int.Parse(split[1]))).ToArray();
        }
        public SpawnableMapObjectPair[] SpawnableMapObjects()
        {
            return serializedSpawnableMapObjects.Split(';').Select(s => s.Split('|')).Where(split => split.Length == 3).Select(split => new SpawnableMapObjectPair(split[0], bool.Parse(split[1]), CurveContainer.DeserializeCurve(split[2]))).ToArray();
        }
        public SpawnableOutsideObjectPair[] SpawnableOutsideObjects()
        {
            return serializedSpawnableOutsideObjects.Split(';').Select(s => s.Split('|')).Where(split => split.Length == 2).Select(split => new SpawnableOutsideObjectPair(split[0], CurveContainer.DeserializeCurve(split[1]))).ToArray();
        }
    }
}
