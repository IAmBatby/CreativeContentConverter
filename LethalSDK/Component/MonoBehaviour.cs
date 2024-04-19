using GameNetcodeStuff;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine;
using Unity.Netcode;
using LethalSDK.Utils;
using System.Text.RegularExpressions;
using LethalSDK.Component;

namespace LethalSDK.Component
{
    [AddComponentMenu("LethalSDK/DamagePlayer")]
    public class SI_DamagePlayer : MonoBehaviour
    {
        public bool kill = false;
        public bool dontSpawnBody = false;

        public SI_CauseOfDeath causeOfDeath = SI_CauseOfDeath.Gravity;

        public int damages = 25;

        public int numberIterations = 1;
        public int iterationCooldown = 1000;
        public int warmupCooldown = 0;

        public UnityEvent postEvent = new UnityEvent();
        public void Trigger(object player)
        {
            if (kill)
            {
                StartCoroutine(Kill(player));
            }
            else
            {
                StartCoroutine(Damage(player));
            }
        }
        public IEnumerator Kill(object player)
        {
            yield return new WaitForSeconds(warmupCooldown / 1000f);
            (player as PlayerControllerB).KillPlayer(Vector3.zero, !dontSpawnBody, (CauseOfDeath)(int)causeOfDeath, 0);
            postEvent.Invoke();
        }
        public IEnumerator Damage(object player)
        {
            yield return new WaitForSeconds(warmupCooldown / 1000f);
            int iteration = 0;
            while (iteration < numberIterations || numberIterations == -1)
            {
                (player as PlayerControllerB).DamagePlayer(damages, true, true, (CauseOfDeath)(int)causeOfDeath, 0, false, Vector3.zero);
                postEvent.Invoke();
                iteration++;
                yield return new WaitForSeconds(iterationCooldown / 1000f);
            }
        }
        public void StopCounter(object player)
        {
            StopAllCoroutines();
        }

    }
    [AddComponentMenu("LethalSDK/SoundYDistance")]
    public class SI_SoundYDistance : MonoBehaviour
    {
        public AudioSource audioSource;
        public int maxDistance = 50;

        public void Awake()
        {
            if (audioSource == null)
            {
                audioSource = this.gameObject.GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    audioSource = this.gameObject.AddComponent<AudioSource>();
                }
            }
        }
        public void Update()
        {
            if (RoundManager.Instance != null && StartOfRound.Instance != null)
            {
                audioSource.volume = 1 - (Mathf.Abs(this.transform.position.y - RoundManager.Instance.playersManager.allPlayerScripts[StartOfRound.Instance.ClientPlayerList[StartOfRound.Instance.NetworkManager.LocalClientId]].gameplayCamera.transform.position.y) / maxDistance);
            }
        }
    }
    [AddComponentMenu("LethalSDK/AudioOutputInterface")]
    public class SI_AudioOutputInterface : MonoBehaviour
    {
        public AudioSource audioSource;
        public string mixerName = "Diagetic";
        public string mixerGroupName = "Master";
        public void Awake()
        {
            if (audioSource == null)
            {
                audioSource = this.gameObject.GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    audioSource = this.gameObject.AddComponent<AudioSource>();
                }
            }
            if(mixerName != null && mixerName.Length > 0 && mixerGroupName != null && mixerGroupName.Length > 0)
            {
                audioSource.outputAudioMixerGroup = AssetGatherDialog.audioMixers[mixerName].Item2.First(g => g.name == mixerGroupName);
            }
            Destroy(this);
        }
    }
    [AddComponentMenu("LethalSDK/NetworkPrefabInstancier")]
    public class SI_NetworkPrefabInstancier : MonoBehaviour
    {
        public GameObject prefab;
        [HideInInspector]
        public GameObject instance;
        public void Awake()
        {
            if (prefab != null)
            {
                NetworkObject no = prefab.GetComponent<NetworkObject>();
                if (no != null && no.NetworkManager != null)
                {
                    SI_NetworkDataInterfacing NDI = this.GetComponent<SI_NetworkDataInterfacing>();
                    SI_NetworkData pNDI = null;
                    if (NDI != null)
                    {
                        pNDI = prefab.GetComponent<SI_NetworkData>();
                        if (pNDI == null)
                        {
                            prefab.AddComponent<SI_NetworkData>();
                        }
                    }
                    if (no.NetworkManager.IsHost)
                    {
                        instance = NetworkObject.Instantiate(prefab, this.transform.position, this.transform.rotation, this.transform.parent);
                        SI_NetworkData temppNDI = instance.GetComponent<SI_NetworkData>();
                        if (NDI != null && temppNDI != null)
                        {
                            temppNDI.setData(NDI.getData());
                        }
                        instance.GetComponent<NetworkObject>().Spawn();
                    }
                }
            }
            this.gameObject.SetActive(false);
        }
        public void OnDestroy()
        {
            if (instance != null)
            {
                NetworkObject no = prefab.GetComponent<NetworkObject>();
                if (no != null && no.NetworkManager != null && no.NetworkManager.IsHost)
                {
                    instance.GetComponent<NetworkObject>().Despawn();
                    Destroy(instance);
                }
            }
        }
    }
    [AddComponentMenu("LethalSDK/NetworkDataInterfacing")]
    public class SI_NetworkDataInterfacing : MonoBehaviour
    {
        public StringStringPair[] data = new StringStringPair[0];
        [HideInInspector]
        public string serializedData = string.Empty;
        private void OnValidate()
        {
            serializedData = string.Join(";", data.Select(p => $"{p._string1.RemoveNonAlphanumeric(1)},{p._string2.RemoveNonAlphanumeric(1)}"));
        }
        public virtual StringStringPair[] getData()
        {
            return serializedData.Split(';').Select(s => s.Split(',')).Where(split => split.Length == 2).Select(split => new StringStringPair(split[0], split[1])).ToArray();
        }
    }
}
