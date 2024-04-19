using GameNetcodeStuff;
using LethalSDK.Component;
using LethalSDK.Conversions;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LethalSDK.Converter
{
    public static class ConvertComponents
    {
        [MenuItem("LethalSDK/Convert Scripts/Interact Triggers")]
        public static void ConvertInteractTriggers()
        {
            InteractTriggerConverter interactTriggerConverter = new InteractTriggerConverter();
            interactTriggerConverter.InitializeConverter();
        }

        [MenuItem("LethalSDK/Convert Scripts/Match Local Player Position")]
        public static void ConvertMatchLocalPlayerPosition()
        {
            MatchLocalPlayerPositionConverter matchLocalPlayerPositionConverter = new MatchLocalPlayerPositionConverter();
            matchLocalPlayerPositionConverter.InitializeConverter();
        }

        [MenuItem("LethalSDK/Convert Scripts/Animated Sun")]
        public static void ConvertAnimatedSun()
        {
            AnimatedSunConverter animatedSunConverter = new AnimatedSunConverter();
            animatedSunConverter.InitializeConverter();
        }

        [MenuItem("LethalSDK/Convert Scripts/Scan Nodes")]
        public static void ConvertScanNodes()
        {
            ScanNodeConverter scanNodeConverter = new ScanNodeConverter();
            scanNodeConverter.InitializeConverter();
        }

        [MenuItem("LethalSDK/Convert Scripts/Audio Reverb Presets")]
        public static void ConvertAudioReverbPresets()
        {
            AudioReverbPresetsConverter audioReverbPresetsConverter = new AudioReverbPresetsConverter();
            audioReverbPresetsConverter.InitializeConverter();
        }

        [MenuItem("LethalSDK/Convert Scripts/Audio Reverb Triggers")]
        public static void ConvertAudioReverbTriggers()
        {
            AudioReverbTriggerConverter audioReverbTriggerConverter = new AudioReverbTriggerConverter();
            audioReverbTriggerConverter.InitializeConverter();
        }

        [MenuItem("LethalSDK/Convert Scripts/Item Dropship")]
        public static void ConvertItemDropship()
        {
            ItemDropshipConverter itemDropshipConverter = new ItemDropshipConverter();
            itemDropshipConverter.InitializeConverter();
        }

        [MenuItem("LethalSDK/Convert Scripts/Dungeon Generator")]
        public static void ConvertDungeonGenerator()
        {
            DungeonGeneratorConverter dungeonGeneratorConverter = new DungeonGeneratorConverter();
            dungeonGeneratorConverter.InitializeConverter();
        }

        [MenuItem("LethalSDK/Convert Scripts/Entrance Teleports")]
        public static void ConvertEntranceTeleports()
        {
            EntranceTeleportConverter entranceTeleportConverter = new EntranceTeleportConverter();
            entranceTeleportConverter.InitializeConverter();
        }

        [MenuItem("LethalSDK/Convert Scripts/Water Surfaces")]
        public static void ConvertWaterSurfaces()
        {
            WaterSurfaceConverter waterSurfaceConverter = new WaterSurfaceConverter();
            waterSurfaceConverter.InitializeConverter();
        }

        [MenuItem("LethalSDK/Convert Scripts/Ladders")]
        public static void ConvertLadders()
        {
            LadderConverter ladderConverter = new LadderConverter();
            ladderConverter.InitializeConverter();
        }

        public static List<GameObject> GetSceneRootGameObjects()
        {
            return (SceneManager.GetActiveScene().GetRootGameObjects().ToList());
        }
    }
}
