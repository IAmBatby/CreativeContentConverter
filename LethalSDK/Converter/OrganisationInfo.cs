using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

namespace LethalSDK.Converter
{
    public class OrganisationInfo
    {
        public List<UnityEngine.Object> AllAssets { get; set; } = new List<UnityEngine.Object>();
        public List<string> AllDirectories { get; set; } = new List<string>();

        public List<AssetDirectoryInfo> AssetDirectoryInfos { get; set; } = new List<AssetDirectoryInfo>();

        public OrganisationInfo(Dictionary<string, List<UnityEngine.Object>> collectedAssets = null)
        {
        }

        public void AddInfo(Dictionary<string, List<UnityEngine.Object>> collectedAssets, AssetType assetType)
        {
            AssetDirectoryInfos.Add(new AssetDirectoryInfo(collectedAssets, assetType));
        }
    }

    public class AssetDirectoryInfo
    {
        public Dictionary<string, List<UnityEngine.Object>> DirectoriesWithAssets = new Dictionary<string, List<UnityEngine.Object>>();
        public Dictionary<UnityEngine.Object, string> AssetsWithDirectory = new Dictionary<UnityEngine.Object, string>();
        public List<string> Directories { get; set; } = new List<string>();
        public List<UnityEngine.Object> Assets { get; set; } = new List<UnityEngine.Object>();

        public AssetType AssetType { get; set; }

        public AssetDirectoryInfo(Dictionary<string, List<UnityEngine.Object>> collectedAssets, AssetType assetType)
        {
            AssetType = assetType;

            foreach (KeyValuePair<string, List<UnityEngine.Object>> directoryWithAssets in collectedAssets)
            {
                foreach (UnityEngine.Object asset in directoryWithAssets.Value)
                    if (!Assets.Contains(asset) && !AssetsWithDirectory.ContainsKey(asset))
                    {
                        Assets.Add(asset);
                        AssetsWithDirectory.Add(asset, directoryWithAssets.Key);
                    }

                if (!Directories.Contains(directoryWithAssets.Key) && !DirectoriesWithAssets.ContainsKey(directoryWithAssets.Key))
                {
                    Directories.Add(directoryWithAssets.Key);
                    DirectoriesWithAssets.Add(directoryWithAssets.Key, directoryWithAssets.Value);
                }
            }
        }

        public List<string> GetDirectoriesWithSingleAsset()
        {
            List<string> returnList = new List<string>();
            foreach (KeyValuePair<string, List<UnityEngine.Object>> directoryWithAssets in DirectoriesWithAssets)
                if (directoryWithAssets.Value.Count < 2)
                    returnList.Add(directoryWithAssets.Key);
            return (returnList);
        }

        public List<UnityEngine.Object> GetOrphanAssets()
        {
            List<UnityEngine.Object> returnList = new List<UnityEngine.Object>();
            List<string> singleAssetDirectories = GetDirectoriesWithSingleAsset();
            foreach (KeyValuePair<string, List<UnityEngine.Object>> directoryWithAssets in DirectoriesWithAssets)
                if (singleAssetDirectories.Contains(directoryWithAssets.Key))
                    foreach (UnityEngine.Object orphanAsset in directoryWithAssets.Value)
                        returnList.Add(orphanAsset);

            return (returnList);
        }
    }
}
