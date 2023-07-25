using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TilemapTerrainTools
{
    public class TilemapTerrainLayer : MonoBehaviour
    {
        public int layerIndex = -1;

        public Dictionary<Vector2Int, TilemapTileObject> Tiles;
    }
}
