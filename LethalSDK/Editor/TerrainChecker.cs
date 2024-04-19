using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace LethalSDK.Editor
{
    [RequireComponent(typeof(Terrain))]
    public class TerrainChecker : MonoBehaviour
    {
        [HideInInspector]
        public Terrain terrain;
        void OnDrawGizmosSelected()
        {
            terrain = this.GetComponent<Terrain>();
        }
        void Awake()
        {
            Destroy(this);
        }
    }
}
