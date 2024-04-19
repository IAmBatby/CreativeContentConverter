using LethalSDK.Component;
using System;
using System.Collections.Generic;
using System.Text;

namespace LethalSDK.Conversions
{
    public class MatchLocalPlayerPositionConverter : ComponentConverter<SI_MatchLocalPlayerPosition, MatchLocalPlayerPosition>
    {
        public override bool ConvertComponent(SI_MatchLocalPlayerPosition siMatchLocalPlayerPosition)
        {
            MatchLocalPlayerPosition matchLocalPlayerPosition = siMatchLocalPlayerPosition.gameObject.AddComponent<MatchLocalPlayerPosition>();
            return (true);
        }
    }
}
