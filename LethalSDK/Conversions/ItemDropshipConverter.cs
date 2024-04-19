using LethalSDK.Component;
using LethalSDK.Converter;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LethalSDK.Conversions
{
    public class ItemDropshipConverter : ComponentConverter<SI_NetworkPrefabInstancier, ItemDropship>
    {
        public override bool ConvertComponent(SI_NetworkPrefabInstancier siNetworkPrefabInstancier)
        {
            if (siNetworkPrefabInstancier.prefab != null)
            {
                SI_ItemDropship siItemDropship = siNetworkPrefabInstancier.prefab.GetComponentInChildren<SI_ItemDropship>();
                if (siItemDropship != null)
                {
                    GameObject itemDropshipPrefab = GameObject.Instantiate(LEConverterWindow.WindowSettings.itemDropshipPrefab, siNetworkPrefabInstancier.transform.parent);
                    itemDropshipPrefab.transform.position = siNetworkPrefabInstancier.transform.position;
                    itemDropshipPrefab.transform.rotation = siNetworkPrefabInstancier.transform.rotation;
                    GameObject.DestroyImmediate(siNetworkPrefabInstancier.gameObject);
                }
            }

            return (true);
        }
    }
}
