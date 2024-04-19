using LethalSDK.Component;
using System;
using System.Collections.Generic;
using System.Text;

namespace LethalSDK.Conversions
{
    public class AnimatedSunConverter : ComponentConverter<SI_AnimatedSun, animatedSun>
    {
        public override bool ConvertComponent(SI_AnimatedSun aiAnimatedSun)
        {
            animatedSun animatedSun = aiAnimatedSun.gameObject.AddComponent<animatedSun>();
            animatedSun.indirectLight = aiAnimatedSun.indirectLight;
            animatedSun.directLight = aiAnimatedSun.directLight;
            return (true);
        }
    }
}
