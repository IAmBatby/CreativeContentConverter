using LethalSDK.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace LethalSDK.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AssetBank", menuName = "LethalSDK/Asset Bank")]
    public class AssetBank : ScriptableObject
    {
        [HeaderAttribute("Audio Clips")]
        [SerializeField]
        private AudioClipInfoPair[] _audioClips = new AudioClipInfoPair[0];
        [SerializeField]
        public PlanetPrefabInfoPair[] _planetPrefabs = new PlanetPrefabInfoPair[0];
        [SerializeField]
        private PrefabInfoPair[] _networkPrefabs = new PrefabInfoPair[0];
        [HideInInspector]
        public string serializedAudioClips;
        [HideInInspector]
        public string serializedPlanetPrefabs;
        [HideInInspector]
        public string serializedNetworkPrefabs;
        private void OnValidate()
        {
            for (int i = 0; i < _audioClips.Length; i++)
            {
                _audioClips[i].AudioClipName = _audioClips[i].AudioClipName.RemoveNonAlphanumeric(1);
                _audioClips[i].AudioClipPath = _audioClips[i].AudioClipPath.RemoveNonAlphanumeric(4);
            }
            for (int i = 0; i < _planetPrefabs.Length; i++)
            {
                _planetPrefabs[i].PlanetPrefabName = _planetPrefabs[i].PlanetPrefabName.RemoveNonAlphanumeric(1);
                _planetPrefabs[i].PlanetPrefabPath = _planetPrefabs[i].PlanetPrefabPath.RemoveNonAlphanumeric(4);
            }
            for (int i = 0; i < _networkPrefabs.Length; i++)
            {
                _networkPrefabs[i].PrefabName = _networkPrefabs[i].PrefabName.RemoveNonAlphanumeric(1);
                _networkPrefabs[i].PrefabPath = _networkPrefabs[i].PrefabPath.RemoveNonAlphanumeric(4);
            }
            serializedAudioClips = string.Join(";", _audioClips.Select(p => $"{(p.AudioClipName.Length == 0 ? (p.AudioClip != null ? p.AudioClip.name : "") : p.AudioClipName)},{AssetDatabase.GetAssetPath(p.AudioClip)}"));
            serializedPlanetPrefabs = string.Join(";", _planetPrefabs.Select(p => $"{(p.PlanetPrefabName.Length == 0 ? (p.PlanetPrefab != null ? p.PlanetPrefab.name : "") : p.PlanetPrefabName)},{AssetDatabase.GetAssetPath(p.PlanetPrefab)}"));
            serializedNetworkPrefabs = string.Join(";", _networkPrefabs.Select(p => $"{(p.PrefabName.Length == 0 ? (p.Prefab != null ? p.Prefab.name : "") : p.PrefabName)},{AssetDatabase.GetAssetPath(p.Prefab)}"));
        }
        public AudioClipInfoPair[] AudioClips()
        {
            if (serializedAudioClips != null)
            {
                return serializedAudioClips.Split(';').Select(s => s.Split(',')).Where(split => split.Length == 2).Select(split => new AudioClipInfoPair(split[0], split[1])).ToArray();
            }
            return new AudioClipInfoPair[0];
        }
        public bool HaveAudioClip(string audioClipName)
        {
            if (serializedAudioClips != null)
            {
                return AudioClips().Any(a => a.AudioClipName == audioClipName);
            }
            return false;
        }
        public string AudioClipPath(string audioClipName)
        {
            if (serializedAudioClips != null)
            {
                return AudioClips().First(c => c.AudioClipName == audioClipName).AudioClipPath;
            }
            return string.Empty;
        }
        public Dictionary<string, string> AudioClipsDictionary()
        {
            if (serializedAudioClips != null)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                foreach (var pair in _audioClips)
                {
                    dictionary.Add(pair.AudioClipName, pair.AudioClipPath);
                }
                return dictionary;
            }
            return new Dictionary<string, string>();
        }
        public PlanetPrefabInfoPair[] PlanetPrefabs()
        {
            if (serializedPlanetPrefabs != null)
            {
                return serializedPlanetPrefabs.Split(';').Select(s => s.Split(',')).Where(split => split.Length == 2).Select(split => new PlanetPrefabInfoPair(split[0], split[1])).ToArray();
            }
            return new PlanetPrefabInfoPair[0];
        }
        public bool HavePlanetPrefabs(string planetPrefabName)
        {
            if (serializedPlanetPrefabs != null)
            {
                return PlanetPrefabs().Any(a => a.PlanetPrefabName == planetPrefabName);
            }
            return false;
        }
        public string PlanetPrefabsPath(string planetPrefabName)
        {
            if (serializedPlanetPrefabs != null)
            {
                return PlanetPrefabs().First(c => c.PlanetPrefabName == planetPrefabName).PlanetPrefabPath;
            }
            return string.Empty;
        }
        public Dictionary<string, string> PlanetPrefabsDictionary()
        {
            if (serializedPlanetPrefabs != null)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                foreach (var pair in _planetPrefabs)
                {
                    dictionary.Add(pair.PlanetPrefabName, pair.PlanetPrefabPath);
                }
                return dictionary;
            }
            return new Dictionary<string, string>();
        }
        public PrefabInfoPair[] NetworkPrefabs()
        {
            if (serializedNetworkPrefabs != null)
            {
                return serializedNetworkPrefabs.Split(';').Select(s => s.Split(',')).Where(split => split.Length == 2).Select(split => new PrefabInfoPair(split[0], split[1])).ToArray();
            }
            return new PrefabInfoPair[0];
        }
        public bool HaveNetworkPrefabs(string networkPrefabName)
        {
            if (serializedNetworkPrefabs != null)
            {
                return NetworkPrefabs().Any(a => a.PrefabName == networkPrefabName);
            }
            return false;
        }
        public string NetworkPrefabsPath(string networkPrefabName)
        {
            if (serializedNetworkPrefabs != null)
            {
                return NetworkPrefabs().First(c => c.PrefabName == networkPrefabName).PrefabPath;
            }
            return string.Empty;
        }
        public Dictionary<string, string> NetworkPrefabsDictionary()
        {
            if (serializedNetworkPrefabs != null)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                foreach (var pair in _networkPrefabs)
                {
                    dictionary.Add(pair.PrefabName, pair.PrefabPath);
                }
                return dictionary;
            }
            return new Dictionary<string, string>();
        }
    }
}
