using LethalSDK.Component;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LethalSDK.Conversions
{
    public class AudioReverbPresetsConverter : ComponentConverter<SI_AudioReverbPresets, AudioReverbPresets>
    {
        public override bool ConvertComponent(SI_AudioReverbPresets siAudioReverbPresets)
        {
            AudioReverbPresets audioReverbPresets = siAudioReverbPresets.gameObject.AddComponent<AudioReverbPresets>();
            List<AudioReverbTrigger> audioReverbTriggers = new List<AudioReverbTrigger>();
            foreach (GameObject gameObject in siAudioReverbPresets.presets)
                audioReverbTriggers.Add(gameObject.GetComponent<AudioReverbTrigger>());
            audioReverbPresets.audioPresets = audioReverbTriggers.ToArray();

            return (true);
        }
    }
}
