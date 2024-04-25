using System;
using System.Collections.Generic;
using System.Text;

namespace LethalSDK.Converter
{
    public enum AssetType { Textures, Materials, Meshes, Audio, Animation, Prefabs, Scenes, Terrain, Other}
    [System.Serializable]
    public struct AssetTypePair
    {
        public AssetType assetInfoType;
        public string fileExtension;
    }
}
