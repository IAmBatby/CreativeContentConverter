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
        public static void ConvertInteractTriggers()
        {
            InteractTriggerConverter interactTriggerConverter = new InteractTriggerConverter();
            interactTriggerConverter.InitializeConverter();
        }

        public static void ConvertMatchLocalPlayerPosition()
        {
            MatchLocalPlayerPositionConverter matchLocalPlayerPositionConverter = new MatchLocalPlayerPositionConverter();
            matchLocalPlayerPositionConverter.InitializeConverter();
        }

        public static void ConvertAnimatedSun()
        {
            AnimatedSunConverter animatedSunConverter = new AnimatedSunConverter();
            animatedSunConverter.InitializeConverter();
        }

        public static void ConvertScanNodes()
        {
            ScanNodeConverter scanNodeConverter = new ScanNodeConverter();
            scanNodeConverter.InitializeConverter();
        }

        public static void ConvertAudioReverbPresets()
        {
            AudioReverbPresetsConverter audioReverbPresetsConverter = new AudioReverbPresetsConverter();
            audioReverbPresetsConverter.InitializeConverter();
        }

        public static void ConvertAudioReverbTriggers()
        {
            AudioReverbTriggerConverter audioReverbTriggerConverter = new AudioReverbTriggerConverter();
            audioReverbTriggerConverter.InitializeConverter();
        }


        public static void ConvertItemDropship()
        {
            ItemDropshipConverter itemDropshipConverter = new ItemDropshipConverter();
            itemDropshipConverter.InitializeConverter();
        }

        public static void ConvertDungeonGenerator()
        {
            DungeonGeneratorConverter dungeonGeneratorConverter = new DungeonGeneratorConverter();
            dungeonGeneratorConverter.InitializeConverter();
        }

        public static void ConvertEntranceTeleports()
        {
            EntranceTeleportConverter entranceTeleportConverter = new EntranceTeleportConverter();
            entranceTeleportConverter.InitializeConverter();
        }

        public static void ConvertWaterSurfaces()
        {
            WaterSurfaceConverter waterSurfaceConverter = new WaterSurfaceConverter();
            waterSurfaceConverter.InitializeConverter();
        }

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
