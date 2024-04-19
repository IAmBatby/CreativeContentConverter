using GameNetcodeStuff;
using LethalSDK.Component;
using LethalSDK.Converter;
using LethalSDK.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;

namespace LethalSDK.Conversions
{
    public class EntranceTeleportConverter : ComponentConverter<SI_EntranceTeleport, EntranceTeleport>
    {
        public override bool ConvertComponent(SI_EntranceTeleport siEntranceTeleport)
        {
            AudioSource audioSource = siEntranceTeleport.gameObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = LEConverterWindow.WindowSettings.diageticMasterMixer;
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f;
            EntranceTeleport entranceTeleport = siEntranceTeleport.gameObject.AddComponent<EntranceTeleport>();
            entranceTeleport.isEntranceToBuilding = true;
            entranceTeleport.entrancePoint = siEntranceTeleport.EntrancePoint;
            entranceTeleport.entranceId = siEntranceTeleport.EntranceID;
            entranceTeleport.audioReverbPreset = siEntranceTeleport.AudioReverbPreset;
            entranceTeleport.entrancePointAudio = audioSource;
            entranceTeleport.doorAudios = siEntranceTeleport.DoorAudios;
            InteractTrigger trigger = siEntranceTeleport.gameObject.AddComponent<InteractTrigger>();
            trigger.hoverIcon = LEConverterWindow.WindowSettings.handIcon;
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
            ConvertEventData.AddTeleportPlayerEvent(trigger, entranceTeleport);

            return (true);
        }
    }
}
