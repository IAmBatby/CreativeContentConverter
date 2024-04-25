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
            extendedLevel.IsRouteHidden = moon.IsHidden;
            extendedLevel.IsRouteLocked = moon.IsLocked;
            extendedLevel.OverrideRouteConfirmNodeDescription = moon.BoughtComment;
            extendedLevel.OverrideInfoNodeDescription = moon.PlanetLore;
            extendedLevel.contentSourceName = moonManifest.author;
        }
    }
}
