using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace LethalSDK.Editor
{
    internal class CopyrightsWindow : EditorWindow
    {
        private Vector2 scrollPosition;
        private readonly Dictionary<string, string> assetAuthorList = new Dictionary<string, string>
        {
            {"Drop Ship assets, Sun cycle animations, ScrapItem sprite, ScavengerSuit Textures/Arms Mesh and MonitorWall mesh", "Zeekerss"},
            {"SDK Scripts, Sun Texture, CrossButton Sprite (Inspired of vanilla), OldSeaPort planet prefab texture", "HolographicWings"},
            {"Old Sea Port asset package", "VIVID Arts"},
            {"Survival Game Tools asset package", "cookiepopworks.com"},
        };

        [MenuItem("LethalSDK/Copyrights", false, 999)]
        public static void ShowWindow()
        {
            GetWindow<CopyrightsWindow>("Copyrights");
        }

        void OnGUI()
        {
            GUILayout.Label("List of Copyrights", EditorStyles.boldLabel);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            
            EditorGUILayout.Space(5);
            foreach (KeyValuePair<string, string> asset in assetAuthorList)
            {
                GUILayout.Label($"Asset: {asset.Key} - By: {asset.Value}", EditorStyles.wordWrappedLabel);
                EditorGUILayout.Space(2);
            }

            EditorGUILayout.Space(5);
            GUILayout.Label("This SDK do not embed any Vanilla script.");

            GUILayout.EndScrollView();
        }
    }
}
