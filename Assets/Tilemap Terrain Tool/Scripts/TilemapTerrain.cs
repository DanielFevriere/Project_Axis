using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TilemapTerrainTools;
using UnityEngine;

namespace TilemapTerrainTools
{
    public class TilemapTerrain : MonoBehaviour
    {
        // Tile prefab
        public GameObject prefab;
        // The height at which we are painting our tiles
        //  Changing levels should create a new container if a new tile is placed for that level
        public int groundLevel = 1;
        public int brushSize = 1;
        
        // Dictionaries
        public Dictionary<int, TilemapTerrainLayer> Containers = new Dictionary<int, TilemapTerrainLayer>();
    
    
        // References
        public List<GameObject> objectPrefabs = new List<GameObject>();
        
        
        public void ClearLayer()
        {
            if (Containers.ContainsKey(groundLevel))
            {
                #if UNITY_EDITOR
                // Destroy game objects
                Transform container;
                if (Containers.TryGetValue(groundLevel, out TilemapTerrainLayer layer))
                {
                    if (layer != null)
                    {
                        for (int i = layer.transform.childCount - 1; i > -1; i--)
                        {
                            DestroyImmediate(layer.transform.GetChild(i).gameObject);
                        }
                        DestroyImmediate(layer.gameObject);
                    }
                    
                }
                #endif
                Containers.Remove(groundLevel);
                
            }
        }
    
        public void ClearAll()
        {
            for (int i = transform.childCount - 1; i > -1; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
    
        public void ReinitializeContainers()
        {
            Containers.Clear();
            
            //TODO: Create TilemapLayer class
            
            // Get layer children
            for (int i = 0; i < transform.childCount; i++)
            {
                TilemapTerrainLayer layer = transform.GetChild(i).GetComponent<TilemapTerrainLayer>();
                Containers.Add(layer.layerIndex, layer);
            }
        }
    }
}


