using DunGen;
using DunGen.Adapters;
using JetBrains.Annotations;
using LethalSDK.Component;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LethalSDK.Conversions
{
    public class DungeonGeneratorConverter : ComponentConverter<SI_DungeonGenerator, DungeonGenerator>
    {
        public override bool ConvertComponent(SI_DungeonGenerator siDungeonGenerator)
        {
            if (siDungeonGenerator.tag != "DungeonGenerator")
                siDungeonGenerator.tag = "DungeonGenerator";
            GameObject DungeonRoot = siDungeonGenerator.DungeonRoot;

            RuntimeDungeon runtimeDungeon = siDungeonGenerator.gameObject.AddComponent<RuntimeDungeon>();
            runtimeDungeon.Generator.LengthMultiplier = 0.8f;
            runtimeDungeon.Generator.PauseBetweenRooms = 0.2f;
            runtimeDungeon.GenerateOnStart = false;
            if (DungeonRoot != null && DungeonRoot.scene == null)
            {
                DungeonRoot = new GameObject();
                DungeonRoot.name = "DungeonRoot";
                DungeonRoot.transform.position = new Vector3(0, -200, 0);
            }
            runtimeDungeon.Root = DungeonRoot;
            UnityNavMeshAdapter dungeonNavMesh = siDungeonGenerator.gameObject.AddComponent<UnityNavMeshAdapter>();
            dungeonNavMesh.BakeMode = UnityNavMeshAdapter.RuntimeNavMeshBakeMode.FullDungeonBake;
            dungeonNavMesh.LayerMask = 35072; //256 + 2048 + 32768 = 35072

            return (true);
        }
    }
}
