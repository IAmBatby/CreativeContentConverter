using LethalSDK.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using LethalSDK.Converter;
using HarmonyLib;
using System.Linq;

namespace LethalSDK.Conversions
{
    public class ScrapConverter
    {
        public static AudioClip defaultDropSound;
        public static AudioClip defaultGrabSound;

        public static AudioClip defaultShovelReelUp;
        public static AudioClip defaultShovelSwing;
        public static AudioClip defaultShovelHitSFX1;
        public static AudioClip defaultShovelHitSFX2;

        public static AudioClip defaultFlashlight;
        public static AudioClip defaultFlashlightOutOfBatteriesClip;
        public static AudioClip defaultFlashlightFlicker;

        public static AudioClip defaultNoisemakerNoiseSFX;
        public static AudioClip defaultNoisemakerNoiseSFXFar;

        public static AudioClip defaultWhoopieCushionFartSFX1;
        public static AudioClip defaultWhoopieCushionFartSFX2;
        public static AudioClip defaultWhoopieCushionFartSFX3;
        public static AudioClip defaultWhoopieCushionFartSFX4;
        public static AudioClip defaultWhoopieCushionFartSFX5;

        public static void GetDefaultAudioClips()
        {
            defaultDropSound = GetAudioClip("DropCan");
            defaultGrabSound = GetAudioClip("ShovelPickup");

            defaultShovelReelUp = GetAudioClip("ShovelReelUp");
            defaultShovelSwing = GetAudioClip("ShovelSwing");
            defaultShovelHitSFX1 = GetAudioClip("ShovelHitDefault");
            defaultShovelHitSFX2 = GetAudioClip("ShovelHitDefault2");

            defaultFlashlight = GetAudioClip("FlashlightClick");
            defaultFlashlightOutOfBatteriesClip = GetAudioClip("FlashlightOutOfBatteries");
            defaultFlashlightFlicker = GetAudioClip("FlashlightFlicker");

            defaultNoisemakerNoiseSFX = GetAudioClip("ClownHorn1");
            defaultNoisemakerNoiseSFXFar = GetAudioClip("ClownHornFar");

            defaultWhoopieCushionFartSFX1 = GetAudioClip("Fart1");
            defaultWhoopieCushionFartSFX2 = GetAudioClip("Fart2");
            defaultWhoopieCushionFartSFX3 = GetAudioClip("Fart3");
            defaultWhoopieCushionFartSFX4 = GetAudioClip("Fart4");
            defaultWhoopieCushionFartSFX5 = GetAudioClip("Fart5");
        }

        public static AudioClip GetAudioClip(string assetName)
        {
            foreach (AudioClip audioClip in SelectableLevelConverter.audioClips)
                if (audioClip.name.SkipToLetters().RemoveWhitespace().StripSpecialCharacters().ToLower() == assetName.SkipToLetters().RemoveWhitespace().StripSpecialCharacters().ToLower())
                    return (audioClip);

            LEConverterWindow.LogInfo(LEConverterWindow.currentExtendedItem, "Failed To Get AudioClip: " + assetName);
            //Debug.LogError("Failed To Get AudioClip: " + assetName);
            return (null);
        }

        public static void ConvertComponent(Item item, Scrap scrap)
        {
            item.itemName = scrap.itemName;

            item.weight = (float)scrap.weight / 100 + 1;
            item.isConductiveMetal = scrap.isConductiveMetal;
            item.maxValue = scrap.maxValue;
            item.minValue = scrap.minValue;
            item.requiresBattery = scrap.requiresBattery;
            item.restingRotation = scrap.restingRotation;
            item.rotationOffset = scrap.rotationOffset;
            item.positionOffset = scrap.positionOffset;
            item.meshOffset = false;
            item.meshVariants = scrap.meshVariants;
            item.materialVariants = scrap.materialVariants;
            item.verticalOffset = scrap.verticalOffset;
            item.canBeInspected = false;
            item.canBeGrabbedBeforeGameStart = true;
            item.isScrap = true;
            item.twoHanded = scrap.twoHanded;

            item.itemId = 0;
            item.holdButtonUse = false;
            item.highestSalePercentage = 80;
            item.floorYOffset = 0;
            item.allowDroppingAheadOfPlayer = true;
            item.grabAnimationTime = 0;
            item.spawnPositionTypes = new List<ItemGroup>();
            if (scrap.scrapType == ScrapType.Noisemaker)
                item.itemIsTrigger = true;
            else
                item.itemIsTrigger = false;
            item.itemSpawnsOnGround = true;
            item.creditsWorth = 0;
            item.lockedInDemo = false;
            item.batteryUsage = 5;
            item.automaticallySetUsingPower = !item.requiresBattery;

            item.useAnim = string.Empty;
            item.pocketAnim = string.Empty;
            item.throwAnim = string.Empty;
            item.saveItemVariable = false;

            if (scrap.scrapType == ScrapType.Shovel)
                item.isDefensiveWeapon = true;
            else
                item.isDefensiveWeapon = false;

            item.usableInSpecialAnimations = false;



            AudioSource audioSource = item.spawnPrefab.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f;
            audioSource.outputAudioMixerGroup = LEConverterWindow.WindowSettings.diageticMasterMixer;

            item.itemIcon = LEConverterWindow.WindowSettings.scrapItemIcon;

            if (!string.IsNullOrEmpty(scrap.grabSFX))
                item.grabSFX = GetAudioClip(scrap.grabSFX);
            else
                item.grabSFX = defaultGrabSound;

            if (!string.IsNullOrEmpty(scrap.dropSFX))
                item.dropSFX = GetAudioClip(scrap.dropSFX);
            else
                item.dropSFX = defaultDropSound;


            if (scrap.HandedAnimation == GrabAnim.OneHanded)
            { item.twoHandedAnimation = true; item.grabAnim = string.Empty; }
            else if (scrap.HandedAnimation == GrabAnim.TwoHanded)
            { item.twoHandedAnimation = true; item.grabAnim = "HoldLung"; }
            else if (scrap.HandedAnimation == GrabAnim.Shotgun)
            { item.twoHandedAnimation = true; item.grabAnim = "HoldShotgun"; }
            else if (scrap.HandedAnimation == GrabAnim.Jetpack)
            { item.twoHandedAnimation = false; item.grabAnim = "HoldJetpack"; }
            else if (scrap.HandedAnimation == GrabAnim.Clipboard)
            { item.twoHandedAnimation = false; item.grabAnim = string.Empty; }
            else
            { item.twoHandedAnimation = false; item.grabAnim = string.Empty; }

            if (scrap.scrapType == ScrapType.Normal)
                ConvertNormalScrapData(item, scrap);
            else if (scrap.scrapType == ScrapType.Shovel)
                ConvertNormalScrapData(item, scrap);
            else if (scrap.scrapType == ScrapType.Flashlight)
                ConvertFlashlightScrapData(item, scrap);
            else if (scrap.scrapType == ScrapType.Noisemaker)
                ConvertNoisemakerScrapData(item, scrap);
            else if (scrap.scrapType == ScrapType.WhoopieCushion)
                ConvertWhoopieCushionScrapData(item, scrap);

            Transform scanNodeObject = item.spawnPrefab.transform.Find("ScanNode");
            if (scanNodeObject != null)
            {
                ScanNodeProperties scanNode = scanNodeObject.gameObject.AddComponent<ScanNodeProperties>();
                scanNode.maxRange = 13;
                scanNode.minRange = 1;
                scanNode.headerText = item.itemName;
                scanNode.subText = "Value: ";
                scanNode.nodeType = 2;
            }

            //EditorUtility.SetDirty(item);
        }

        public static void ConvertNormalScrapData(Item item, Scrap scrap)
        {
            PhysicsProp newPhysicsProp = item.spawnPrefab.AddComponent<PhysicsProp>();
            if (newPhysicsProp == null)
                newPhysicsProp = item.spawnPrefab.AddComponent<PhysicsProp>();

            newPhysicsProp.grabbable = true;
            newPhysicsProp.itemProperties = item;
            newPhysicsProp.mainObjectRenderer = item.spawnPrefab.GetComponent<MeshRenderer>();
            newPhysicsProp.useCooldown = scrap.useCooldown;

            item.syncUseFunction = false;
            item.syncDiscardFunction = false;
            item.syncInteractLRFunction = false;
            item.syncGrabFunction = false;
        }

        public static void ConvertShovelScrapData(Item item, Scrap scrap)
        {
            Shovel shovel = item.spawnPrefab.GetComponent<Shovel>();
            if (shovel == null)
                shovel = item.spawnPrefab.AddComponent<Shovel>();


            item.holdButtonUse = true;

            shovel.grabbable = true;
            shovel.itemProperties = item;
            shovel.mainObjectRenderer = item.spawnPrefab.GetComponent<MeshRenderer>();

            shovel.useCooldown = scrap.useCooldown;

            item.toolTips = new string[1] { "Use " + item.itemName + " : [RMB" };

            item.syncUseFunction = false;
            item.syncDiscardFunction = false;
            item.syncInteractLRFunction = false;
            item.syncGrabFunction = false;

            shovel.shovelHitForce = scrap.shovelHitForce;
            if (shovel.shovelAudio == null)
                shovel.shovelAudio = item.spawnPrefab.GetComponent<AudioSource>();
            if (shovel.shovelAudio == null)
                shovel.shovelAudio = item.spawnPrefab.GetComponent<AudioSource>();            

            if (item.spawnPrefab.GetComponent<OccludeAudio>() == null)
                item.spawnPrefab.AddComponent<OccludeAudio>();

            //////////

            if (!string.IsNullOrEmpty(scrap.reelUp))
                shovel.reelUp = GetAudioClip(scrap.reelUp);
            else
                shovel.reelUp = defaultShovelReelUp;

            if (!string.IsNullOrEmpty(scrap.swing))
                shovel.swing = GetAudioClip(scrap.swing);
            else
                shovel.swing = defaultShovelSwing;

            if (scrap.hitSFX.Length == 0)
                shovel.hitSFX = new AudioClip[2] { defaultShovelHitSFX1, defaultShovelHitSFX2 };
            else
                foreach (string hitString in scrap.hitSFX)
                    shovel.hitSFX = shovel.hitSFX.AddItem(GetAudioClip(hitString)).ToArray();
        }

        public static void ConvertFlashlightScrapData(Item item, Scrap scrap)
        {
            FlashlightItem flashlight = item.spawnPrefab.GetComponent<FlashlightItem>();
            if (flashlight == null)
                flashlight = item.spawnPrefab.AddComponent<FlashlightItem>();

            flashlight.grabbable = true;
            flashlight.itemProperties = item;
            flashlight.mainObjectRenderer = item.spawnPrefab.GetComponent<MeshRenderer>();

            flashlight.useCooldown = scrap.useCooldown;

            item.toolTips = new string[1] { "Use " + item.itemName + " : [RMB" };

            item.syncUseFunction = true;
            item.syncDiscardFunction = true;
            item.syncInteractLRFunction = true;
            item.syncGrabFunction = false;

            flashlight.usingPlayerHelmetLight = scrap.usingPlayerHelmetLight;
            flashlight.flashlightInterferenceLevel = scrap.flashlightInterferenceLevel;

            flashlight.flashlightBulb = scrap.flashlightBulb;
            if (flashlight.flashlightBulb == null)
                flashlight.flashlightBulb = item.spawnPrefab.AddComponent<Light>();
            flashlight.flashlightBulb.intensity = 0;

            flashlight.flashlightBulbGlow = scrap.flashlightBulbGlow;
            if (flashlight.flashlightBulbGlow == null)
                flashlight.flashlightBulbGlow = item.spawnPrefab.AddComponent<Light>();
            flashlight.flashlightBulbGlow.intensity = 0;

            flashlight.flashlightAudio = scrap.flashlightAudio;
            if (flashlight.flashlightAudio == null)
                flashlight.flashlightAudio = item.spawnPrefab.AddComponent<AudioSource>();

            if (item.spawnPrefab.GetComponent<OccludeAudio>() == null)
                item.spawnPrefab.AddComponent<OccludeAudio>();

            flashlight.bulbLight = scrap.bulbLight;
            if (flashlight.bulbLight == null)
                flashlight.bulbLight = new Material(Shader.Find("HDRP/Lit"));

            flashlight.bulbDark = scrap.bulbDark;
            if (flashlight.bulbDark == null)
                flashlight.bulbDark = new Material(Shader.Find("HDRP/Lit"));

            flashlight.flashlightMesh = scrap.flashlightMesh;
            if (flashlight.flashlightMesh == null)
                flashlight.flashlightMesh = flashlight.mainObjectRenderer;

            flashlight.flashlightTypeID = scrap.flashlightTypeID;
            flashlight.changeMaterial = scrap.changeMaterial;

            //////////

            if (!string.IsNullOrEmpty(scrap.flashlightFlicker))
                flashlight.flashlightFlicker = GetAudioClip(scrap.flashlightFlicker);
            else
                flashlight.flashlightFlicker = defaultFlashlightFlicker;

            if (!string.IsNullOrEmpty(scrap.outOfBatteriesClip))
                flashlight.outOfBatteriesClip = GetAudioClip(scrap.outOfBatteriesClip);
            else
                flashlight.outOfBatteriesClip = defaultFlashlightOutOfBatteriesClip;

            if (scrap.flashlightClips.Length == 0)
                flashlight.flashlightClips = new AudioClip[1] { defaultFlashlight };
            else
                foreach (string flashlightString in scrap.flashlightClips)
                    flashlight.flashlightClips = flashlight.flashlightClips.AddItem(GetAudioClip(flashlightString)).ToArray();
        }

        public static void ConvertNoisemakerScrapData(Item item, Scrap scrap)
        {
            NoisemakerProp noisemaker = item.spawnPrefab.AddComponent<NoisemakerProp>();
            noisemaker.grabbable = true;
            noisemaker.itemProperties = item;
            noisemaker.mainObjectRenderer = item.spawnPrefab.GetComponent<MeshRenderer>();

            noisemaker.useCooldown = scrap.useCooldown;

            item.toolTips = new string[1] { "Use " + item.itemName + " : [RMB" };

            item.syncUseFunction = true;
            item.syncDiscardFunction = false;
            item.syncInteractLRFunction = false;
            item.syncGrabFunction = false;

            noisemaker.noiseAudio = scrap.noiseAudio;
            if (noisemaker.noiseAudio == null)
            {
                noisemaker.noiseAudio = item.spawnPrefab.AddComponent<AudioSource>();
                noisemaker.noiseAudio.playOnAwake = false;
                noisemaker.noiseAudio.priority = 128;
                noisemaker.noiseAudio.pitch = 1f;
                noisemaker.noiseAudio.panStereo = 0f;
                noisemaker.noiseAudio.spatialBlend = 1f;
                noisemaker.noiseAudio.reverbZoneMix = 0f;

                noisemaker.noiseAudio.dopplerLevel = 4f;
                noisemaker.noiseAudio.spread = 26f;
                noisemaker.noiseAudio.minDistance = 4f;
                noisemaker.noiseAudio.maxDistance = 21f;
                noisemaker.noiseAudio.rolloffMode = AudioRolloffMode.Linear;

                noisemaker.noiseAudio.outputAudioMixerGroup = LEConverterWindow.WindowSettings.diageticMasterMixer;
            }

            noisemaker.noiseAudioFar = scrap.noiseAudioFar;
            if (noisemaker.noiseAudioFar == null)
            {
                noisemaker.noiseAudioFar = item.spawnPrefab.AddComponent<AudioSource>();

                // Configure AudioSource
                noisemaker.noiseAudioFar.playOnAwake = true;
                noisemaker.noiseAudioFar.priority = 128;
                noisemaker.noiseAudioFar.pitch = 1f;
                noisemaker.noiseAudioFar.panStereo = 0f;
                noisemaker.noiseAudioFar.spatialBlend = 1f;
                noisemaker.noiseAudioFar.reverbZoneMix = 1f;

                // Configure 3D Sound Settings
                noisemaker.noiseAudioFar.dopplerLevel = 1.4f;
                noisemaker.noiseAudioFar.spread = 87;
                noisemaker.noiseAudioFar.rolloffMode = AudioRolloffMode.Custom;
                noisemaker.noiseAudioFar.maxDistance = 75;
                noisemaker.noiseAudioFar.SetCustomCurve(AudioSourceCurveType.CustomRolloff, new AnimationCurve(new Keyframe[] {
                                                        new Keyframe(18f, 0f, 0f, 0.065f),
                                                        new Keyframe(25.59f, 0.866f, -0.01f, -0.01f),
                                                        new Keyframe(87f, 0f, -0.018f, 0f)
                                                    }));
            }

            noisemaker.noiseRange = scrap.noiseRange;
            noisemaker.maxLoudness = scrap.maxLoudness;
            noisemaker.minLoudness = scrap.minLoudness;
            noisemaker.minPitch = scrap.minPitch;
            noisemaker.maxPitch = scrap.maxPitch;
            noisemaker.triggerAnimator = scrap.triggerAnimator;

            //////////

            if (scrap.noiseSFX.Length == 0)
                noisemaker.noiseSFX = new AudioClip[1] { defaultNoisemakerNoiseSFX };
            else
                foreach (string noisemakerString in scrap.noiseSFX)
                    noisemaker.noiseSFX = noisemaker.noiseSFX.AddItem(GetAudioClip(noisemakerString)).ToArray();

            if (scrap.noiseSFXFar.Length == 0)
                noisemaker.noiseSFXFar = new AudioClip[1] { defaultNoisemakerNoiseSFXFar };
            else
                foreach (string noisemakerString in scrap.noiseSFXFar)
                    noisemaker.noiseSFXFar = noisemaker.noiseSFXFar.AddItem(GetAudioClip(noisemakerString)).ToArray();
        }

        public static void ConvertWhoopieCushionScrapData(Item item, Scrap scrap)
        {
            WhoopieCushionItem whoopieCushion = item.spawnPrefab.AddComponent<WhoopieCushionItem>();

            whoopieCushion.grabbable = true;
            whoopieCushion.itemProperties = item;
            whoopieCushion.mainObjectRenderer = item.spawnPrefab.GetComponent<MeshRenderer>();

            whoopieCushion.useCooldown = scrap.useCooldown;

            item.syncUseFunction = false;
            item.syncDiscardFunction = false;
            item.syncInteractLRFunction = false;
            item.syncGrabFunction = false;

            whoopieCushion.whoopieCushionAudio = scrap.whoopieCushionAudio;
            if (whoopieCushion.whoopieCushionAudio == null)
            {
                whoopieCushion.whoopieCushionAudio = item.spawnPrefab.AddComponent<AudioSource>();
            }
            Transform triggerObject = item.spawnPrefab.transform.Find("Trigger");
            if (triggerObject == null)
            {
                Debug.LogWarning($"{whoopieCushion.itemProperties.name} Whoopie Cushion Trigger not found, please add one");
            }
            else if (triggerObject.gameObject.GetComponent<WhoopieCushionTrigger>() == null)
            {
                WhoopieCushionTrigger trigger = triggerObject.gameObject.AddComponent<WhoopieCushionTrigger>();
                trigger.itemScript = whoopieCushion;
            }

            //////////

            if (scrap.fartAudios.Length == 0)
                whoopieCushion.fartAudios = new AudioClip[5] { defaultWhoopieCushionFartSFX1, defaultWhoopieCushionFartSFX2, defaultWhoopieCushionFartSFX3, defaultWhoopieCushionFartSFX4, defaultWhoopieCushionFartSFX5 };
            else
                foreach (string fartString in scrap.fartAudios)
                    whoopieCushion.fartAudios = whoopieCushion.fartAudios.AddItem(GetAudioClip(fartString)).ToArray();
        }
    }
}
