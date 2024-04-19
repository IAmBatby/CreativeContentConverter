using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Audio;
using UnityEngine;

namespace LethalSDK.Utils
{
    public static class AssetGatherDialog
    {
        //Audio Clips
        public static Dictionary<String, AudioClip> audioClips = new Dictionary<String, AudioClip>();
        //Audio Mixers
        public static Dictionary<String, (AudioMixer, AudioMixerGroup[])> audioMixers = new Dictionary<String, (AudioMixer, AudioMixerGroup[])>();
        //Sprites
        public static Dictionary<String, Sprite> sprites = new Dictionary<String, Sprite>();
    }
}
