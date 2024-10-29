using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TilemapTerrainTools
{
    [CreateAssetMenu(menuName = "Tilemap/Tile Prefab Data")]
    [System.Serializable]
    public class TilePrefabsData : ScriptableObject
    {
        [Header("Mesh prefabs")]
        public TilePrefabs prefabs;

        [Space]
        [Header("Materials")]
        public Material baseMaterial;
        public Material blendMaterial;
    }
}

