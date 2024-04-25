using DunGen.Adapters;
using DunGen;
using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using LethalSDK.Utils;
using UnityEditor.SearchService;
using UnityEditor;
using Unity.Netcode;
using UnityEngine.Events;
using LethalSDK.ScriptableObjects;
using System.Collections;
using UnityEditor.Events;

namespace LethalSDK.Component
{
    public class ScriptImporter : MonoBehaviour
    {
        public bool hadErrors = false;

        public virtual void Awake()
        {
            DestroyImmediate(this);
        }

        public void ForceAwake()
        {
            hadErrors = false;
            try
            {
                string oldName = this.gameObject.name;
                this.Awake();
                if (hadErrors == false)
                    Debug.Log("Successfully Converted: " + oldName);
                else
                    Debug.Log("Converted: " + oldName + " With Errors");
            }
            catch (Exception e)
            {
                Debug.LogError("Failed To Converted: " + this.gameObject.name + "\n" + e, this.transform);
            }
            hadErrors = false;
        }

        public void ConvertUnityEvent(InteractEvent interactEvent, UnityEvent<object> unityEvent)
        {
            

            for (int i = 0; i < unityEvent.GetPersistentEventCount(); i++)
            {
                try
                {
                    UnityAction newUnityAction = Delegate.CreateDelegate(typeof(UnityAction), unityEvent.GetPersistentTarget(i), unityEvent.GetPersistentMethodName(i)) as UnityAction;
                    UnityEventTools.AddVoidPersistentListener(interactEvent, newUnityAction);
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed To Convert UnityEvent Action: " + unityEvent.GetPersistentMethodName(i) + " Due To Parameter Incompatability." + "\n" + e);
                    hadErrors = true;
                }
            }
        }
    }
    [AddComponentMenu("LethalSDK/MatchLocalPlayerPosition")]
    public class SI_MatchLocalPlayerPosition : ScriptImporter
    {
        public override void Awake()
        {
            this.gameObject.AddComponent<MatchLocalPlayerPosition>();
            base.Awake();
        }
    }
    [AddComponentMenu("LethalSDK/AnimatedSun")]
    public class SI_AnimatedSun : ScriptImporter
    {
        public Light indirectLight;
        public Light directLight;
        public override void Awake()
        {
            animatedSun tmp = this.gameObject.AddComponent<animatedSun>();
            tmp.indirectLight = indirectLight;
            tmp.directLight = directLight;
            base.Awake();
        }
    }
    [AddComponentMenu("LethalSDK/ScanNode")]
    public class SI_ScanNode : ScriptImporter
    {
        public int MaxRange;
        public int MinRange;
        public bool RequiresLineOfSight;
        public string HeaderText;
        public string SubText;
        public int ScrapValue;
        public int CreatureScanID;
        public NodeType NodeType;
        public override void Awake()
        {
            ScanNodeProperties tmp = this.gameObject.AddComponent<ScanNodeProperties>();
            tmp.minRange = MinRange;
            tmp.maxRange = MaxRange;
            tmp.requiresLineOfSight = RequiresLineOfSight;
            tmp.headerText = HeaderText;
            tmp.subText = SubText;
            tmp.scrapValue = ScrapValue;
            tmp.creatureScanID = CreatureScanID;
            tmp.nodeType = (int)NodeType;
            base.Awake();
        }
    }
    public enum NodeType
    {
        Information = 0,
        Danger = 0,
        Ressource = 0
    }
    [AddComponentMenu("LethalSDK/AudioReverbPresets")]
    public class SI_AudioReverbPresets : ScriptImporter
    {
        public GameObject[] presets;
        public override void Awake()
        {
            return;
        }
        public void Update()
        {
            int i = 0;
            foreach (GameObject preset in presets)
            {
                if(preset.GetComponent<SI_AudioReverbTrigger>() != null)
                {
                    i++;
                }
            }
            if(i == 0)
            {
                List<AudioReverbTrigger> list = new List<AudioReverbTrigger>();
                foreach (GameObject preset in presets)
                {
                    if (preset.GetComponent<AudioReverbTrigger>() != null)
                    {
                        list.Add(preset.GetComponent<AudioReverbTrigger>());
                    }
                }
                AudioReverbPresets tmp = this.gameObject.AddComponent<AudioReverbPresets>();
                tmp.audioPresets = list.ToArray();
                Destroy(this);
            }
        }
    }
    [AddComponentMenu("LethalSDK/AudioReverbTrigger")]
    public class SI_AudioReverbTrigger : ScriptImporter
    {
        [Header("Reverb Preset")]
        public bool ChangeDryLevel = false;
        [Range(-10000f, 0f)]
        public float DryLevel = 0f;
        public bool ChangeHighFreq = false;
        [Range(-10000f, 0f)]
        public float HighFreq = -270f;
        public bool ChangeLowFreq = false;
        [Range(-10000f, 0f)]
        public float LowFreq = -244f;
        public bool ChangeDecayTime = false;
        [Range(0f, 35f)]
        public float DecayTime = 1.4f;
        public bool ChangeRoom = false;
        [Range(-10000f, 0f)]
        public float Room = -600f;
        [Header("MISC")]
        public bool ElevatorTriggerForProps = false;
        public bool SetInElevatorTrigger = false;
        public bool IsShipRoom = false;
        public bool ToggleLocalFog = false;
        public float FogEnabledAmount = 10f;
        [Header("Weather and effects")]
        public bool SetInsideAtmosphere = false;
        public bool InsideLighting = false;
        public int WeatherEffect = -1;
        public bool EffectEnabled = true;
        public bool DisableAllWeather = false;
        public bool EnableCurrentLevelWeather = true;

        public override void Awake()
        {
            AudioReverbTrigger tmp = this.gameObject.AddComponent<AudioReverbTrigger>();
            ReverbPreset tmppreset = ScriptableObject.CreateInstance<ReverbPreset>();
            tmppreset.changeDryLevel = ChangeDryLevel;
            tmppreset.dryLevel = DryLevel;
            tmppreset.changeHighFreq = ChangeHighFreq;
            tmppreset.highFreq = HighFreq;
            tmppreset.changeLowFreq = ChangeLowFreq;
            tmppreset.lowFreq = LowFreq;
            tmppreset.changeDecayTime = ChangeDecayTime;
            tmppreset.decayTime = DecayTime;
            tmppreset.changeRoom = ChangeRoom;
            tmppreset.room = Room;
            tmp.reverbPreset = tmppreset;
            tmp.usePreset = -1;
            tmp.audioChanges = new switchToAudio[0];
            tmp.elevatorTriggerForProps = ElevatorTriggerForProps;
            tmp.setInElevatorTrigger = SetInElevatorTrigger;
            tmp.isShipRoom = IsShipRoom;
            tmp.toggleLocalFog = ToggleLocalFog;
            tmp.fogEnabledAmount = FogEnabledAmount;
            tmp.setInsideAtmosphere = SetInsideAtmosphere;
            tmp.insideLighting = InsideLighting;
            tmp.weatherEffect = WeatherEffect;
            tmp.effectEnabled = EffectEnabled;
            tmp.disableAllWeather = DisableAllWeather;
            tmp.enableCurrentLevelWeather = EnableCurrentLevelWeather;

            base.Awake();
        }
    }
    [AddComponentMenu("LethalSDK/DungeonGenerator")]
    public class SI_DungeonGenerator : ScriptImporter
    {
        public GameObject DungeonRoot;

        public override void Awake()
        {
            if (this.tag != "DungeonGenerator")
            {
                this.tag = "DungeonGenerator";
            }
            RuntimeDungeon runtimeDungeon = this.gameObject.AddComponent<RuntimeDungeon>();
            //runtimeDungeon.Generator.DungeonFlow = RoundManager.Instance.dungeonFlowTypes[0];
            runtimeDungeon.Generator.LengthMultiplier = 0.8f;
            runtimeDungeon.Generator.PauseBetweenRooms = 0.2f;
            runtimeDungeon.GenerateOnStart = false;
            if(DungeonRoot != null && DungeonRoot.scene == null)
            {
                DungeonRoot = new GameObject();
                DungeonRoot.name = "DungeonRoot";
                DungeonRoot.transform.position = new Vector3(0, -200, 0);
            }
            runtimeDungeon.Root = DungeonRoot;
            //runtimeDungeon.Generator.DungeonFlow = RoundManager.Instance.dungeonFlowTypes[0];
            UnityNavMeshAdapter dungeonNavMesh = this.gameObject.AddComponent<UnityNavMeshAdapter>();
            dungeonNavMesh.BakeMode = UnityNavMeshAdapter.RuntimeNavMeshBakeMode.FullDungeonBake;
            dungeonNavMesh.LayerMask = 35072; //256 + 2048 + 32768 = 35072

            base.Awake();
        }
    }
    [AddComponentMenu("LethalSDK/EntranceTeleport")]
    public class SI_EntranceTeleport : ScriptImporter
    {
        public int EntranceID = 0;
        public Transform EntrancePoint;
        public int AudioReverbPreset = 2;
        public AudioClip[] DoorAudios = new AudioClip[0];

        public override void Awake()
        {
            AudioSource audioSource = this.gameObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = AssetGatherDialog.audioMixers["Diagetic"].Item2.First(g => g.name == "Master");
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f;
            EntranceTeleport entranceTeleport = this.gameObject.AddComponent<EntranceTeleport>();
            entranceTeleport.isEntranceToBuilding = true;
            entranceTeleport.entrancePoint = EntrancePoint;
            entranceTeleport.entranceId = EntranceID;
            entranceTeleport.audioReverbPreset = AudioReverbPreset;
            entranceTeleport.entrancePointAudio = audioSource;
            entranceTeleport.doorAudios = DoorAudios;
            InteractTrigger trigger = this.gameObject.AddComponent<InteractTrigger>();
            trigger.hoverIcon = AssetGatherDialog.sprites.ContainsKey("HandIcon") ? AssetGatherDialog.sprites["HandIcon"] : AssetGatherDialog.sprites.First().Value;
            trigger.hoverTip = "Enter : [LMB]";
            trigger.disabledHoverTip = string.Empty;
            trigger.holdTip = string.Empty;
            trigger.animationString = string.Empty;
            trigger.interactable = true;
            trigger.oneHandedItemAllowed = true;
            trigger.twoHandedItemAllowed = true;
            trigger.holdInteraction = true;
            trigger.timeToHold = 1.5f;
            trigger.timeToHoldSpeedMultiplier = 1f;
            trigger.holdingInteractEvent = new InteractEventFloat();
            trigger.onInteract = new InteractEvent();
            trigger.onInteractEarly = new InteractEvent();
            trigger.onStopInteract = new InteractEvent();
            trigger.onCancelAnimation = new InteractEvent();
            trigger.onInteract.AddListener((player) => entranceTeleport.TeleportPlayer());

            base.Awake();
        }
    }
    [AddComponentMenu("LethalSDK/DoorLock")]
    public class SI_DoorLock : ScriptImporter
    {
        public override void Awake()
        {
            base.Awake();
        }
    }
    [AddComponentMenu("LethalSDK/WaterSurface")]
    public class SI_WaterSurface : ScriptImporter
    {
        GameObject obj;
        public int soundMaxDistance = 50;
        public override void Awake()
        {
            obj = Instantiate(SpawnPrefab.Instance.waterSurface);
            SceneManager.MoveGameObjectToScene(obj, this.gameObject.scene);
            obj.transform.parent = this.transform;
            obj.transform.localPosition = Vector3.zero;
            Transform WaterMesh = obj.transform.Find("Water");
            WaterMesh.GetComponent<MeshFilter>().sharedMesh = this.GetComponent<MeshFilter>().sharedMesh;
            WaterMesh.position = this.transform.position;
            WaterMesh.rotation = this.transform.rotation;
            WaterMesh.localScale = this.transform.localScale;
            SI_SoundYDistance soundYDistance = WaterMesh.gameObject.AddComponent<SI_SoundYDistance>();
            soundYDistance.audioSource = obj.transform.Find("WaterAudio").GetComponent<AudioSource>();
            soundYDistance.maxDistance = soundMaxDistance;
            obj.SetActive(true);

            base.Awake();
        }
    }
    [AddComponentMenu("LethalSDK/Ladder")]
    public class SI_Ladder : ScriptImporter
    {
        public Transform BottomPosition;
        public Transform TopPosition;
        public Transform HorizontalPosition;
        public Transform PlayerNodePosition;
        public bool UseRaycastToGetTopPosition = false;
        public override void Awake()
        {
            InteractTrigger trigger = this.gameObject.AddComponent<InteractTrigger>();
            trigger.hoverIcon = AssetGatherDialog.sprites.ContainsKey("HandLadderIcon") ? AssetGatherDialog.sprites["HandLadderIcon"] : AssetGatherDialog.sprites.First().Value;
            trigger.hoverTip = "Climb : [LMB]";
            trigger.disabledHoverTip = string.Empty;
            trigger.holdTip = string.Empty;
            trigger.animationString = string.Empty;
            trigger.specialCharacterAnimation = true;
            trigger.animationWaitTime = 0.5f;
            trigger.animationString = "SA_PullLever";
            trigger.isLadder = true;
            trigger.lockPlayerPosition = true;
            trigger.playerPositionNode = BottomPosition;
            trigger.bottomOfLadderPosition = BottomPosition;
            trigger.bottomOfLadderPosition = BottomPosition;
            trigger.topOfLadderPosition = TopPosition;
            trigger.ladderHorizontalPosition = HorizontalPosition;
            trigger.ladderPlayerPositionNode = PlayerNodePosition;
            trigger.useRaycastToGetTopPosition = UseRaycastToGetTopPosition;
            trigger.holdingInteractEvent = new InteractEventFloat();
            trigger.onCancelAnimation = new InteractEvent();
            trigger.onInteract = new InteractEvent();
            trigger.onInteractEarly = new InteractEvent();
            trigger.onStopInteract = new InteractEvent();
            base.Awake();
        }
    }
    [AddComponentMenu("LethalSDK/ItemDropship")]
    public class SI_ItemDropship : ScriptImporter
    {
        public Animator ShipAnimator;
        public Transform[] ItemSpawnPositions;
        public GameObject OpenTriggerObject;
        public GameObject KillTriggerObject;
        public AudioClip ShipThrusterCloseSound;
        public AudioClip ShipLandSound;
        public AudioClip ShipOpenDoorsSound;
        public override void Awake()
        {
            ItemDropship ItemDropship = this.gameObject.AddComponent<ItemDropship>();
            ItemDropship.shipAnimator = ShipAnimator;
            ItemDropship.itemSpawnPositions = ItemSpawnPositions;

            PlayAudioAnimationEvent _PlayAudioAnimationEvent = this.gameObject.AddComponent<PlayAudioAnimationEvent>();
            _PlayAudioAnimationEvent.audioToPlay = this.GetComponent<AudioSource>();
            _PlayAudioAnimationEvent.audioClip = ShipLandSound;
            _PlayAudioAnimationEvent.audioClip2 = ShipOpenDoorsSound;

            InteractTrigger OpenTriggerScript = OpenTriggerObject.AddComponent<InteractTrigger>();
            OpenTriggerScript.hoverIcon = AssetGatherDialog.sprites.ContainsKey("HandIcon") ? AssetGatherDialog.sprites["HandIcon"] : AssetGatherDialog.sprites.First().Value;
            OpenTriggerScript.hoverTip = "Open : [LMB]";
            OpenTriggerScript.disabledHoverTip = string.Empty;
            OpenTriggerScript.holdTip = string.Empty;
            OpenTriggerScript.animationString = string.Empty;
            OpenTriggerScript.twoHandedItemAllowed = true;
            OpenTriggerScript.onInteract = new InteractEvent();
            OpenTriggerScript.onInteract.AddListener((player) => ItemDropship.TryOpeningShip());

            ItemDropship.triggerScript = OpenTriggerScript;

            if(ItemDropship.transform.Find("Music") != null)
            {
                ItemDropship.transform.Find("Music").gameObject.AddComponent<OccludeAudio>();
                if(this.transform.Find("ThrusterContainer/Thruster") != null)
                {
                    facePlayerOnAxis FacePlayerOnAxis = this.transform.Find("ThrusterContainer/Thruster").gameObject.AddComponent<facePlayerOnAxis>();
                    FacePlayerOnAxis.turnAxis = this.transform.Find("ThrusterContainer/flameAxis");
                }
            }

            KillLocalPlayer KillLocalPlayerScript = KillTriggerObject.AddComponent<KillLocalPlayer>();

            InteractTrigger KillTriggerScript = KillTriggerObject.AddComponent<InteractTrigger>();
            KillTriggerScript.hoverTip = string.Empty;
            KillTriggerScript.disabledHoverTip = string.Empty;
            KillTriggerScript.holdTip = string.Empty;
            KillTriggerScript.animationString = string.Empty;
            KillTriggerScript.touchTrigger = true;
            KillTriggerScript.triggerOnce = true;
            KillTriggerScript.onInteract = new InteractEvent();
            KillTriggerScript.onInteract.AddListener((player) => KillLocalPlayerScript.KillPlayer(player));

            base.Awake();
        }

    }
    [AddComponentMenu("LethalSDK/InteractTrigger")]
    public class SI_InteractTrigger : ScriptImporter
    {
        [Header("Aesthetics")]
        public string hoverIcon = "HandIcon";
        public string hoverTip = "Interact";
        public string disabledHoverIcon = string.Empty;
        public string disabledHoverTip = string.Empty;
        [Header("Interaction")]
        public bool interactable = true;
        public bool oneHandedItemAllowed = true;
        public bool twoHandedItemAllowed = false;
        public bool holdInteraction = false;
        public float timeToHold = 0.5f;
        public float timeToHoldSpeedMultiplier = 1f;
        public string holdTip = string.Empty;
        public UnityEvent<float> holdingInteractEvent = new UnityEvent<float>();
        public bool touchTrigger = false;
        public bool triggerOnce = false;
        [Header("Misc")]
        public bool interactCooldown = true;
        public float cooldownTime = 1f;
        public bool disableTriggerMesh = true;
        public bool RandomChanceTrigger = false;
        public int randomChancePercentage = 0;
        [Header("Events")]
        public UnityEvent<object> onInteract = new UnityEvent<object>();
        public UnityEvent<object> onInteractEarly = new UnityEvent<object>();
        public UnityEvent<object> onStopInteract = new UnityEvent<object>();
        public UnityEvent<object> onCancelAnimation = new UnityEvent<object>();
        [Header("Special Animation")]
        public bool specialCharacterAnimation = false;
        public bool stopAnimationManually = false;
        public string stopAnimationString = "SA_stopAnimation";
        public bool hidePlayerItem = false;
        public bool isPlayingSpecialAnimation = false;
        public float animationWaitTime = 2f;
        public string animationString = string.Empty;
        public bool lockPlayerPosition = false;
        public Transform playerPositionNode;
        [Header("Ladders")]
        public bool isLadder = false;
        public Transform topOfLadderPosition;
        public bool useRaycastToGetTopPosition = false;
        public Transform bottomOfLadderPosition;
        public Transform ladderHorizontalPosition;
        public Transform ladderPlayerPositionNode;

        public override void Awake()
        {
            InteractTrigger trigger = this.gameObject.AddComponent<InteractTrigger>();
            trigger.hoverTip = hoverTip;
            trigger.disabledHoverTip = disabledHoverTip;
            trigger.interactable = interactable;
            trigger.oneHandedItemAllowed = oneHandedItemAllowed;
            trigger.twoHandedItemAllowed = twoHandedItemAllowed;
            trigger.holdInteraction = holdInteraction;
            trigger.timeToHold = timeToHold;
            trigger.timeToHoldSpeedMultiplier = timeToHoldSpeedMultiplier;
            trigger.holdTip = holdTip;
            //trigger.holdingInteractEvent = new InteractEventFloat();
            //trigger.holdingInteractEvent.AddListener((single) => holdingInteractEvent.Invoke(single));
            trigger.touchTrigger = touchTrigger;
            trigger.triggerOnce = triggerOnce;
            trigger.interactCooldown = interactCooldown;
            trigger.cooldownTime = cooldownTime;
            trigger.disableTriggerMesh = disableTriggerMesh;
            trigger.RandomChanceTrigger = RandomChanceTrigger;
            trigger.randomChancePercentage = randomChancePercentage;
            ConvertUnityEvent(trigger.onInteract, onInteract);
            ConvertUnityEvent(trigger.onInteractEarly, onInteractEarly);
            ConvertUnityEvent(trigger.onStopInteract, onStopInteract);
            ConvertUnityEvent(trigger.onCancelAnimation, onCancelAnimation);
            trigger.specialCharacterAnimation = specialCharacterAnimation;
            trigger.stopAnimationManually = stopAnimationManually;
            trigger.stopAnimationString = stopAnimationString;
            trigger.hidePlayerItem = hidePlayerItem;
            trigger.isPlayingSpecialAnimation = isPlayingSpecialAnimation;
            trigger.animationWaitTime = animationWaitTime;
            trigger.animationString = animationString;
            trigger.lockPlayerPosition = lockPlayerPosition;
            trigger.playerPositionNode = playerPositionNode;
            trigger.isLadder = isLadder;
            trigger.topOfLadderPosition = topOfLadderPosition;
            trigger.useRaycastToGetTopPosition = useRaycastToGetTopPosition;
            trigger.bottomOfLadderPosition = bottomOfLadderPosition;
            trigger.ladderHorizontalPosition = ladderHorizontalPosition;
            trigger.ladderPlayerPositionNode = ladderPlayerPositionNode;
        }
    }
    public enum SI_CauseOfDeath
    {
        Unknown = 0,
        Bludgeoning = 1,
        Gravity = 2,
        Blast = 3,
        Strangulation = 4,
        Suffocation = 5,
        Mauling = 6,
        Gunshots = 7,
        Crushing = 8,
        Drowning = 9,
        Abandoned = 10,
        Electrocution = 11,
        Kicking = 12
    }
}
