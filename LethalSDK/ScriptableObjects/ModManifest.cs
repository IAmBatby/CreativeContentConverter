using LethalSDK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LethalSDK.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ModManifest", menuName = "LethalSDK/Mod Manifest")]
    public class ModManifest : ScriptableObject
    {
        public string modName = "NewMod";
        [Space]
        [SerializeField]
        private SerializableVersion version = new SerializableVersion(0, 0, 0, 0);
        [HideInInspector]
        public string serializedVersion;
        [Space]
        public string author = "Author";
        [Space]
        [TextArea(5, 15)]
        public string description = "Mod Description";
        [Space]
        [HeaderAttribute("Content")]
        public Scrap[] scraps = new Scrap[0];
        public Moon[] moons = new Moon[0];
        [Space]
        public AssetBank assetBank;
        private void OnValidate()
        {
            serializedVersion = version.ToString();
        }
        public SerializableVersion GetVersion()
        {
            int[] version = serializedVersion != null ? serializedVersion.Split('.').Select(int.Parse).ToArray() : new int[]{0,0,0,0};
            return new SerializableVersion(version[0], version[1], version[2], version[3]);
        }
    }
}
