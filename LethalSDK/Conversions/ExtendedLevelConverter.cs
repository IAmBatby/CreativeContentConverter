using LethalLevelLoader;
using LethalSDK.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LethalSDK.Conversions
{
    public static class ExtendedLevelConverter
    {
        public static void ConvertAsset(Moon moon, SelectableLevel selectableLevel, ModManifest moonManifest, ref ExtendedLevel extendedLevel)
        {
            extendedLevel.selectableLevel = selectableLevel;
            extendedLevel.ForceSetRoutePrice(moon.RoutePrice);
            extendedLevel.isHidden = moon.IsHidden;
            extendedLevel.isLocked = moon.IsLocked;
            extendedLevel.overrideRouteConfirmNodeDescription = moon.BoughtComment;
            extendedLevel.overrideInfoNodeDescription = moon.PlanetLore;
            extendedLevel.contentSourceName = moonManifest.author;
        }
    }
}
