using LethalSDK.Component;
using LethalSDK.Converter;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LethalSDK.Conversions
{
    public class InteractTriggerConverter : ComponentConverter<SI_InteractTrigger, InteractTrigger>
    {
        public override bool ConvertComponent(SI_InteractTrigger siInteractTrigger)
        {
            InteractTrigger trigger = siInteractTrigger.gameObject.AddComponent<InteractTrigger>();
            trigger.hoverTip = siInteractTrigger.hoverTip;
            trigger.disabledHoverTip = siInteractTrigger.disabledHoverTip;
            trigger.interactable = siInteractTrigger.interactable;
            trigger.oneHandedItemAllowed = siInteractTrigger.oneHandedItemAllowed;
            trigger.twoHandedItemAllowed = siInteractTrigger.twoHandedItemAllowed;
            trigger.holdInteraction = siInteractTrigger.holdInteraction;
            trigger.timeToHold = siInteractTrigger.timeToHold;
            trigger.timeToHoldSpeedMultiplier = siInteractTrigger.timeToHoldSpeedMultiplier;
            trigger.holdTip = siInteractTrigger.holdTip;
            trigger.touchTrigger = siInteractTrigger.touchTrigger;
            trigger.triggerOnce = siInteractTrigger.triggerOnce;
            trigger.interactCooldown = siInteractTrigger.interactCooldown;
            trigger.cooldownTime = siInteractTrigger.cooldownTime;
            trigger.disableTriggerMesh = siInteractTrigger.disableTriggerMesh;
            trigger.RandomChanceTrigger = siInteractTrigger.RandomChanceTrigger;
            trigger.randomChancePercentage = siInteractTrigger.randomChancePercentage;
            trigger.specialCharacterAnimation = siInteractTrigger.specialCharacterAnimation;
            trigger.stopAnimationManually = siInteractTrigger.stopAnimationManually;
            trigger.stopAnimationString = siInteractTrigger.stopAnimationString;
            trigger.hidePlayerItem = siInteractTrigger.hidePlayerItem;
            trigger.isPlayingSpecialAnimation = siInteractTrigger.isPlayingSpecialAnimation;
            trigger.animationWaitTime = siInteractTrigger.animationWaitTime;
            trigger.animationString = siInteractTrigger.animationString;
            trigger.lockPlayerPosition = siInteractTrigger.lockPlayerPosition;
            trigger.playerPositionNode = siInteractTrigger.playerPositionNode;
            trigger.isLadder = siInteractTrigger.isLadder;
            trigger.topOfLadderPosition = siInteractTrigger.topOfLadderPosition;
            trigger.useRaycastToGetTopPosition = siInteractTrigger.useRaycastToGetTopPosition;
            trigger.bottomOfLadderPosition = siInteractTrigger.bottomOfLadderPosition;
            trigger.ladderHorizontalPosition = siInteractTrigger.ladderHorizontalPosition;
            trigger.ladderPlayerPositionNode = siInteractTrigger.ladderPlayerPositionNode;
            string result = ConvertEventData.ConvertInteractEventData(siInteractTrigger, trigger);
            if (result.Contains("Error"))
            {
                GameObject.DestroyImmediate(trigger);
                componentLogDictionary[siInteractTrigger] = result;
                return (false);
            }
            else
                return (true);
        }
    }
}
