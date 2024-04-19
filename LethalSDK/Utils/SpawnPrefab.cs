using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LethalSDK.Utils
{
    public class SpawnPrefab
    {
        private static SpawnPrefab _instance;
        public static SpawnPrefab Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SpawnPrefab();
                }
                return _instance;
            }
        }
        public GameObject waterSurface;
    }
}
