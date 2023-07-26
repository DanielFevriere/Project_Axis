using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TilemapTerrainTools
{
    public class TileObject : MonoBehaviour
    {
        public Vector2Int position;
        
        public TilePrefabs prefabs;
        public TileNeighbors neighbors = new TileNeighbors();
        
        public Vector2Int Direction;
        public int immediateNeighborCount;

        [SerializeField] private GameObject tilePrefab;

        private MeshRenderer Renderer;

        private Vector2Int[] ImmediateOffsets = new Vector2Int[]
        {
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(0, 1)
        };

        private void OnEnable()
        {
            Renderer = GetComponentInChildren<MeshRenderer>();
        }

        public void Initialize(Vector2Int pos, Dictionary<Vector2Int, TileObject> layer)
        {
            position = pos;

            neighbors = new TileNeighbors();
            Direction = Vector2Int.zero;
            
            // Immediate neighbors
            foreach (var offset in ImmediateOffsets)
            {
                TileObject neighbor = GetNeighbor(pos + offset, layer);
                if (SetImmediateNeighbor(pos + offset, neighbor))
                {
                    Direction -= offset;
                }
            }
            
            UpdateTilePrefab();
        }

        public void UpdateTilePrefab()
        {
            if (Renderer != null)
            {
                DestroyImmediate(Renderer.gameObject);
            }
            
            // By default, set to top
            GameObject prefab;
            immediateNeighborCount = neighbors.ImmediateNeighbors.Count;

            switch (immediateNeighborCount)
            {
                case 0:
                    prefab = prefabs.SinglePiece;
                    break;
                case 1:
                    prefab = prefabs.TripleEdge;
                    break;
                case 2:
                    prefab = Direction == Vector2Int.zero ? prefabs.DoubleEdge : prefabs.Corner;
                    break;
                case 3:
                    prefab = prefabs.SingleEdge;
                    break;
                case 4:
                default:
                    prefab = prefabs.Top;
                    break;
            }
            
            // Only allow path drawing for top tiles
            
            // Spawn new prefab
            GameObject spawnedPrefab = Instantiate(prefab, transform);
            Undo.RegisterCreatedObjectUndo(spawnedPrefab, "update tile Prefab");
            Renderer = spawnedPrefab.GetComponent<MeshRenderer>();
            Renderer.transform.localEulerAngles = new Vector3(0, GetYawRotation(), 0);
        }

        public bool SetImmediateNeighbor(Vector2Int pos, TileObject neighborToSet)
        {
            if (neighborToSet == null)
            {
                return false;
            }

            if (!neighbors.ImmediateNeighbors.ContainsKey(pos))
            {
                // Register adjacent tile as neighbor
                neighbors.ImmediateNeighbors.Add(pos, neighborToSet);
                //neighborToSet.SetImmediateNeighbor(position, this);
                return true;
            }

            return false;
        }

        TileObject GetNeighbor(Vector2Int pos, Dictionary<Vector2Int, TileObject> layer)
        {
            if (layer.TryGetValue(pos, out var n))
            {
                return n;
            }

            return null;
        }

        float GetYawRotation()
        {
            switch (immediateNeighborCount)
            {
                case 0:
                    return 0;
                case 1:
                    if (Direction.y > 0) return 0;
                    if (Direction.y < 0) return 180;
                    if (Direction.x > 0) return 90;
                    if (Direction.x < 0) return 270;
                    break;
                case 2:
                    // Connector
                    if (Direction == Vector2Int.zero)
                    {
                        Vector2Int[] neighborCoords = neighbors.ImmediateNeighbors.Keys.ToArray();
                        Vector2Int delta = neighborCoords[0] - neighborCoords[1];
                        return Mathf.Abs(delta.x) > Mathf.Abs(delta.y) ? 90 : 0;
                    }
                    // Corner
                    else
                    {
                        if (Direction.x > 0 && Direction.y > 0) return 0;
                        if (Direction.x < 0 && Direction.y > 0) return 270;
                        
                        if (Direction.x < 0 && Direction.y < 0) return 180;
                        if (Direction.x > 0 && Direction.y < 0) return 90;
                    }
                    break;
                case 3:
                    if (Direction.x > 0) return 90;
                    if (Direction.x < 0) return 270;
                    if (Direction.y > 0) return 0;
                    if (Direction.y < 0) return 180;
                    break;
            }
            
            return 0;
        }
    }

    [System.Serializable]
    public class TileNeighbors
    {
        public Dictionary<Vector2Int, TileObject> ImmediateNeighbors;
        public Dictionary<Vector2Int, TileObject> CornerNeighbors;

        public TileNeighbors()
        {
            ImmediateNeighbors = new Dictionary<Vector2Int, TileObject>();
            CornerNeighbors = new Dictionary<Vector2Int, TileObject>();
        }
    }


    [System.Serializable]
    public struct TilePrefabs
    {
        // 0 neighbors
        public GameObject SinglePiece;
        // 1 neighbor
        public GameObject TripleEdge;
        // 2 neighbors
        public GameObject DoubleEdge;
        // 3 neighbors
        public GameObject SingleEdge;
        // 4 neighbors
        public GameObject Top;
        public GameObject Corner;
    }
}