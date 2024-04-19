using LethalSDK.Component;
using LethalSDK.Converter;
using LethalSDK.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LethalSDK.Conversions
{
    public class WaterSurfaceConverter : ComponentConverter<SI_WaterSurface, MeshRenderer>
    {
        public override bool ConvertComponent(SI_WaterSurface siWaterSurface)
        {
            GameObject WaterMesh = GameObject.Instantiate(LEConverterWindow.WindowSettings.waterSurfacePrefab);
            WaterMesh.transform.parent = siWaterSurface.transform;
            WaterMesh.transform.localPosition = Vector3.zero;
            WaterMesh.GetComponent<MeshFilter>().sharedMesh = siWaterSurface.GetComponent<MeshFilter>().sharedMesh;
            WaterMesh.transform.position = siWaterSurface.transform.position;
            WaterMesh.transform.rotation = siWaterSurface.transform.rotation;
            WaterMesh.transform.localScale = siWaterSurface.transform.localScale;
            WaterMesh.SetActive(true);


            return (true);
        }
    }
}
