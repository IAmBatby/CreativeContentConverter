using LethalSDK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace LethalSDK.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Scrap", menuName = "LethalSDK/Scrap")]
    public class Scrap : ScriptableObject
    {
        public string[] RequiredBundles;
        public string[] IncompatibleBundles;
        [Header("Base")]
        public ScrapType scrapType = ScrapType.Normal;
        public string itemName = "NewScrap";
        public int minValue = 0;
        public int maxValue = 0;
        public bool twoHanded = false;
        public GrabAnim HandedAnimation = GrabAnim.OneHanded;
        public bool requiresBattery = false;
        public bool isConductiveMetal = false;
        public int weight = 0;
        public GameObject prefab;
        public float useCooldown = 0f;
        [Header("Sounds")]
        public string grabSFX = string.Empty;
        public string dropSFX = string.Empty;
        [Header("Offsets")]
        public float verticalOffset = 0f;
        public Vector3 restingRotation = Vector3.zero;
        public Vector3 positionOffset = Vector3.zero;
        public Vector3 rotationOffset = Vector3.zero;
        [Header("Variants")]
        public Mesh[] meshVariants = new Mesh[0];
        public Material[] materialVariants = new Material[0];
        [Header("Spawn rate")]
        public bool useGlobalSpawnWeight = true;
        [Range(0, 100)]
        public int globalSpawnWeight = 10;
        [SerializeField]
        private ScrapSpawnChancePerScene[] _perPlanetSpawnWeight = new ScrapSpawnChancePerScene[]
        {
            new ScrapSpawnChancePerScene("41 Experimentation", 10),
            new ScrapSpawnChancePerScene("220 Assurance", 10),
            new ScrapSpawnChancePerScene("56 Vow", 10),
            new ScrapSpawnChancePerScene("21 Offense", 10),
            new ScrapSpawnChancePerScene("61 March", 10),
            new ScrapSpawnChancePerScene("85 Rend", 10),
            new ScrapSpawnChancePerScene("7 Dine", 10),
            new ScrapSpawnChancePerScene("8 Titan", 10),
            new ScrapSpawnChancePerScene("Others", 10)
        };
        public string[] planetSpawnBlacklist = new string[0];
        [Header("Shovel")]
        public int shovelHitForce = 1;
        public AudioSource shovelAudio;
        public string reelUp = "ShovelReelUp";
        public string swing = "ShovelSwing";
        public string[] hitSFX = new string[]{ "ShovelHitDefault", "ShovelHitDefault2" };
        [Header("Flashlight")]
        public bool usingPlayerHelmetLight = false;
        public int flashlightInterferenceLevel = 0;
        public Light flashlightBulb;
        public Light flashlightBulbGlow;
        public AudioSource flashlightAudio;
        public string[] flashlightClips = new string[] { "FlashlightClick" };
        public string outOfBatteriesClip = "FlashlightOutOfBatteries";
        public string flashlightFlicker = "FlashlightFlicker";
        public Material bulbLight;
        public Material bulbDark;
        public MeshRenderer flashlightMesh;
        public int flashlightTypeID = 0;
        public bool changeMaterial = true;
        [Header("Noisemaker")]
        public AudioSource noiseAudio;
        public AudioSource noiseAudioFar;
        public string[] noiseSFX = new string[] { "ClownHorn1" };
        public string[] noiseSFXFar = new string[] { "ClownHornFar" };
        public float noiseRange = 60f;
        public float maxLoudness = 1f;
        public float minLoudness = 0.6f;
        public float minPitch = 0.93f;
        public float maxPitch = 1f;
        public Animator triggerAnimator;
        [Header("WhoopieCushion")]
        public AudioSource whoopieCushionAudio;
        public string[] fartAudios = new string[] { "Fart1" , "Fart2", "Fart3", "Fart5" };
        [HideInInspector]
        public string serializedData;
        private void OnValidate()
        {
            RequiredBundles = RequiredBundles.RemoveNonAlphanumeric(1);
            IncompatibleBundles = IncompatibleBundles.RemoveNonAlphanumeric(1);
            itemName = itemName.RemoveNonAlphanumeric(1);
            grabSFX = grabSFX.RemoveNonAlphanumeric(1);
            dropSFX = dropSFX.RemoveNonAlphanumeric(1);
            for(int i = 0; i < _perPlanetSpawnWeight.Length; i++)
            {
                _perPlanetSpawnWeight[i].SceneName = _perPlanetSpawnWeight[i].SceneName.RemoveNonAlphanumeric(1);
            }
            serializedData = string.Join(";", _perPlanetSpawnWeight.Select(p => $"{p.SceneName},{p.SpawnWeight}"));
        }
        public ScrapSpawnChancePerScene[] perPlanetSpawnWeight()
        {
            return serializedData.Split(';').Select(s => s.Split(',')).Where(split => split.Length == 2).Select(split => new ScrapSpawnChancePerScene(split[0], int.Parse(split[1]))).ToArray();
        }
    }
    public enum ScrapType
    {
        Normal = 0,
        Shovel = 1,
        Flashlight = 2,
        Noisemaker = 3,
        WhoopieCushion = 4,
    }
    public enum GrabAnim
    {
        OneHanded = 0,
        TwoHanded = 1,
        Shotgun = 2,
        Jetpack = 3,
        Clipboard = 4,
    }
}
