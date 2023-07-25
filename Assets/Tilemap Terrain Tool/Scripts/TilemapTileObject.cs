using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TilemapTerrainTools
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class TilemapTileObject : MonoBehaviour
    {
        public MeshFilter meshFilter;
        public MeshRenderer renderer;
        
        private void OnEnable()
        {
            meshFilter = GetComponent<MeshFilter>();
            renderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            
        }
    }

    [System.Serializable]
    public class TileNeighbors
    {
        
    }
}