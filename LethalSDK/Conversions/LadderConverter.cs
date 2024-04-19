using LethalSDK.Component;
using LethalSDK.Converter;
using LethalSDK.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace LethalSDK.Conversions
{
    public class LadderConverter : ComponentConverter<SI_Ladder, InteractTrigger>
    {
        public override bool ConvertComponent(SI_Ladder siLadder)
        {
            InteractTrigger trigger = siLadder.gameObject.AddComponent<InteractTrigger>();
            trigger.hoverIcon = LEConverterWindow.WindowSettings.handIcon;
            trigger.hoverTip = "Climb : [LMB]";
            trigger.disabledHoverTip = string.Empty;
            trigger.holdTip = string.Empty;
            trigger.animationString = string.Empty;
            trigger.specialCharacterAnimation = true;
            trigger.animationWaitTime = 0.5f;
            trigger.animationString = "SA_PullLever";
            trigger.isLadder = true;
            trigger.lockPlayerPosition = true;
            trigger.playerPositionNode = siLadder.BottomPosition;
            trigger.bottomOfLadderPosition = siLadder.BottomPosition;
            trigger.bottomOfLadderPosition = siLadder.BottomPosition;
            trigger.topOfLadderPosition = siLadder.TopPosition;
            trigger.ladderHorizontalPosition = siLadder.HorizontalPosition;
            trigger.ladderPlayerPositionNode = siLadder.PlayerNodePosition;
            trigger.useRaycastToGetTopPosition = siLadder.UseRaycastToGetTopPosition;
            trigger.holdingInteractEvent = new InteractEventFloat();
            trigger.onCancelAnimation = new InteractEvent();
            trigger.onInteract = new InteractEvent();
            trigger.onInteractEarly = new InteractEvent();
            trigger.onStopInteract = new InteractEvent();

            return (true);
        }
    }
}
