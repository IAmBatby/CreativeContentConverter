using LethalSDK.Component;
using LethalSDK.Converter;
using LethalSDK.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace LethalSDK.Conversions
{
    public class AudioReverbTriggerConverter : ComponentConverter<SI_AudioReverbTrigger, AudioReverbTrigger>
    {
        public override void InitializeConverter()
        {
            string modContentDirectory = LEConverterWindow.currentModContentDirectory;

            if (AssetDatabase.IsValidFolder(modContentDirectory + "/AudioReverbPresets"))
                AssetDatabase.DeleteAsset(modContentDirectory + "/AudioReverbPresets");
            
            AssetDatabase.Refresh();
            AssetDatabase.CreateFolder(modContentDirectory, "AudioReverbPresets");

            base.InitializeConverter();
        }
        public override bool ConvertComponent(SI_AudioReverbTrigger siAudioReverbTrigger)
        {
            string audioReverbPresetDirectory = LEConverterWindow.currentModContentDirectory + "/AudioReverbPresets";

            AudioReverbTrigger audioReverbTrigger = siAudioReverbTrigger.gameObject.AddComponent<AudioReverbTrigger>();
            ReverbPreset newAudioReverbPreset = ScriptableObject.CreateInstance<ReverbPreset>();
            newAudioReverbPreset.changeDryLevel = siAudioReverbTrigger.ChangeDryLevel;
            newAudioReverbPreset.dryLevel = siAudioReverbTrigger.DryLevel;
            newAudioReverbPreset.changeHighFreq = siAudioReverbTrigger.ChangeHighFreq;
            newAudioReverbPreset.highFreq = siAudioReverbTrigger.HighFreq;
            newAudioReverbPreset.changeLowFreq = siAudioReverbTrigger.ChangeLowFreq;
            newAudioReverbPreset.lowFreq = siAudioReverbTrigger.LowFreq;
            newAudioReverbPreset.changeDecayTime = siAudioReverbTrigger.ChangeDecayTime;
            newAudioReverbPreset.decayTime = siAudioReverbTrigger.DecayTime;
            newAudioReverbPreset.changeRoom = siAudioReverbTrigger.ChangeRoom;
            newAudioReverbPreset.room = siAudioReverbTrigger.Room;

            AssetDatabase.CreateAsset(newAudioReverbPreset, audioReverbPresetDirectory + "/" + siAudioReverbTrigger.gameObject.name + ".asset");
            ReverbPreset audioReverbPreset = (ReverbPreset)AssetDatabase.LoadAssetAtPath(audioReverbPresetDirectory + "/" + siAudioReverbTrigger.name + ".asset", typeof(ReverbPreset));

            audioReverbTrigger.reverbPreset = audioReverbPreset;
            audioReverbTrigger.usePreset = -1;
            audioReverbTrigger.audioChanges = new switchToAudio[0];
            audioReverbTrigger.elevatorTriggerForProps = siAudioReverbTrigger.ElevatorTriggerForProps;
            audioReverbTrigger.setInElevatorTrigger = siAudioReverbTrigger.SetInElevatorTrigger;
            audioReverbTrigger.isShipRoom = siAudioReverbTrigger.IsShipRoom;
            audioReverbTrigger.toggleLocalFog = siAudioReverbTrigger.ToggleLocalFog;
            audioReverbTrigger.fogEnabledAmount = siAudioReverbTrigger.FogEnabledAmount;
            audioReverbTrigger.setInsideAtmosphere = siAudioReverbTrigger.SetInsideAtmosphere;
            audioReverbTrigger.insideLighting = siAudioReverbTrigger.InsideLighting;
            audioReverbTrigger.weatherEffect = siAudioReverbTrigger.WeatherEffect;
            audioReverbTrigger.effectEnabled = siAudioReverbTrigger.EffectEnabled;
            audioReverbTrigger.disableAllWeather = siAudioReverbTrigger.DisableAllWeather;
            audioReverbTrigger.enableCurrentLevelWeather = siAudioReverbTrigger.EnableCurrentLevelWeather;

            return (true);
        }
    }
}
