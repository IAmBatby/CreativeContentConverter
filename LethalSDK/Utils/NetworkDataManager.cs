using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Profiling.Memory.Experimental;

namespace LethalSDK.Utils
{
    public static class NetworkDataManager
    {
        public static Dictionary<ulong, SI_NetworkData> NetworkData = new Dictionary<ulong, SI_NetworkData>();
    }
    public class SI_NetworkData : NetworkBehaviour
    {
        public StringStringPair[] data = new StringStringPair[0];
        [HideInInspector]
        public string serializedData = string.Empty;
        public string datacache = string.Empty;

        public UnityEvent dataChangeEvent = new UnityEvent();

        public void Start()
        {
            if (dataChangeEvent != null)
            {
                dataChangeEvent.AddListener(OnDataChanged);
            }
        }
        public void Update()
        {
            if (datacache != serializedData)
            {
                if(dataChangeEvent != null)
                {
                    dataChangeEvent.Invoke();
                }
            }
        }
        public override void OnNetworkSpawn()
        {
            NetworkDataManager.NetworkData.Add(this.NetworkObjectId, this);
        }
        public override void OnNetworkDespawn()
        {
            NetworkDataManager.NetworkData.Remove(this.NetworkObjectId);
        }
        public virtual void OnDataChanged()
        {
            datacache = serializedData;
        }
        public override void OnDestroy()
        {
            if (dataChangeEvent != null)
            {
                dataChangeEvent.RemoveAllListeners();
            }
        }
        public virtual StringStringPair[] getData()
        {
            return serializedData.Split(';').Select(s => s.Split(',')).Where(split => split.Length == 2).Select(split => new StringStringPair(split[0], split[1])).ToArray();
        }
        public virtual void setData(string datastring)
        {
            if (!datastring.Contains(','))
            {
                Debug.LogWarning("Invalid datastring format.");
                return;
            }

            serializedData = datastring;
        }
        public virtual void setData(StringStringPair[] dataarray)
        {
            string newdata = string.Join(";", dataarray.Select(p => $"{p._string1},{p._string2}"));

            if (!newdata.Contains(','))
            {
                Debug.LogWarning("Invalid datastring format.");
                return;
            }

            serializedData = newdata;
        }
        public virtual void addData(string datastring)
        {
            if (!datastring.Contains(','))
            {
                Debug.LogWarning("Invalid datastring format.");
                return;
            }

            serializedData += datastring;

            serializedData = serializedData.Replace(";;", string.Empty);
            if (serializedData.StartsWith(";"))
            {
                serializedData = serializedData.Substring(1);
            }
            if (serializedData.EndsWith(";"))
            {
                serializedData = serializedData.Substring(0, serializedData.Length - 1);
            }
        }
        public virtual void addData(StringStringPair[] dataarray)
        {
            string newdata = string.Join(";", dataarray.Select(p => $"{p._string1},{p._string2}")).Insert(0, ";");

            if (!newdata.Contains(','))
            {
                Debug.LogWarning("Invalid datastring format.");
                return;
            }

            serializedData += newdata;

            serializedData = serializedData.Replace(";;", string.Empty);
            if (serializedData.StartsWith(";"))
            {
                serializedData = serializedData.Substring(1);
            }
            if (serializedData.EndsWith(";"))
            {
                serializedData = serializedData.Substring(0, serializedData.Length - 1);
            }
        }
        public virtual void delData(string datastring)
        {
            if (!datastring.Contains(','))
            {
                Debug.LogWarning("Invalid datastring format.");
                return;
            }

            if (!serializedData.Contains(datastring))
            {
                Debug.Log("Datastring doesn't exist in serializedData.");
                return;
            }

            serializedData = serializedData.Replace(datastring, string.Empty);

            serializedData = serializedData.Replace(";;", string.Empty);
            if (serializedData.StartsWith(";"))
            {
                serializedData = serializedData.Substring(1);
            }
            if (serializedData.EndsWith(";"))
            {
                serializedData = serializedData.Substring(0, serializedData.Length - 1);
            }
        }
        public virtual void delData(StringStringPair[] dataarray)
        {
            string newdata = string.Join(";", dataarray.Select(p => $"{p._string1},{p._string2}")).Insert(0, ";");

            if (!newdata.Contains(','))
            {
                Debug.LogWarning("Invalid datastring format.");
                return;
            }

            if (!serializedData.Contains(newdata))
            {
                Debug.Log("Datastring doesn't exist in serializedData.");
                return;
            }

            serializedData = serializedData.Replace(newdata, string.Empty);

            serializedData = serializedData.Replace(";;", string.Empty);
            if (serializedData.StartsWith(";"))
            {
                serializedData = serializedData.Substring(1);
            }
            if (serializedData.EndsWith(";"))
            {
                serializedData = serializedData.Substring(0, serializedData.Length - 1);
            }
        }
    }
}
