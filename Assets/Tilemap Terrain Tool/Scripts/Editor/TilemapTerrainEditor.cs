using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering;

namespace TilemapTerrainTools
{
    internal enum TilemapTool
    {
        SpawnTile = 0,
        PaintPath = 1,
        Trees,
        Foliage,
        COUNT
    }
    
    
    [CustomEditor(typeof(TilemapTerrain))]
    public class TilemapTerrainEditor : Editor
    {
        internal class Styles
        {
            public Texture settingsIcon = EditorGUIUtility.IconContent("SettingsIcon").image;

            public GUIStyle command = "Command";
            
            // List of tools supported by the editor
            public readonly GUIContent[] toolIcons =
            {
                EditorGUIUtility.TrIconContent("TerrainInspector.TerrainToolAdd", "Spawn Tiles"),
                EditorGUIUtility.TrIconContent("TerrainInspector.TerrainToolSplat", "Paint Path"),
                EditorGUIUtility.TrIconContent("TerrainInspector.TerrainToolTrees", "Paint Trees"),
                EditorGUIUtility.TrIconContent("TerrainInspector.TerrainToolPlants", "Paint Foliage")
            };
        
            public readonly GUIContent[] toolNames =
            {
                EditorGUIUtility.TrTextContent("Spawn Tiles", "Click to spawn a tile"),
                EditorGUIUtility.TrTextContent("Paint Path", "Click or hold to paint paths"),
                EditorGUIUtility.TrTextContent("Paint Foliage", "Click to paint foliage."),
                EditorGUIUtility.TrTextContent("Terrain Settings")
            };
        }

        internal static Styles styles;

        private TilemapTerrain tilemap;
        GameObject prefab { get
        {
            if (tilemap != null) return tilemap.prefab;
            return null;
        }}

        public GameObject cursorPrefab;
        
        // Selected category tool
        private TilemapTool m_selectedCategory;
        private TilemapTool selectedCategory
        {
            get => m_selectedCategory;
            set => m_selectedCategory = value;
        }

        private Vector3 mouseWorld;
        private Plane ground = new Plane(Vector3.up, Vector3.zero);

        private Vector3 prefabObjectPosition;
        private static bool snapObjectToGrid;
        
        #region Properties

        // Props
        private SerializedObject so;
        private SerializedProperty propLevel;
        private SerializedProperty propBrushSize;

        private static bool drawGrid;
        private float gridExtent = 4;

        private SerializedProperty propPrefab;
        public Material meshPreviewMaterial;

        private SerializedProperty objectPrefabs;
        private GameObject selectedPrefab = null;
        
        #endregion
        
        private void OnEnable()
        {
            Tools.hidden = true;
            
            tilemap = target as TilemapTerrain;
            if (tilemap != null)
            {
                tilemap.ReinitializeContainers();
            }
            
            // Initialize
            propLevel = serializedObject.FindProperty("groundLevel");
            propBrushSize = serializedObject.FindProperty("brushSize");

            propPrefab = serializedObject.FindProperty("prefab");
            objectPrefabs = serializedObject.FindProperty("objectPrefabs");
            
            SceneView.duringSceneGui += DuringSceneGUI;
        }

        private void OnDisable()
        {
            Tools.hidden = false;
            SceneView.duringSceneGui -= DuringSceneGUI;
        }

        void DuringSceneGUI(SceneView sceneView)
        {
            // Only allows this if tilemap is selected
            if (Selection.Contains(tilemap.gameObject))
            {
                // Prevents deselection
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                
                Event e = Event.current;
                
                Handles.zTest = CompareFunction.LessEqual;
            
                // Repaint scene view everytime mouse moves
                if (Event.current.type == EventType.MouseMove)
                {
                    sceneView.Repaint();
                    Repaint();
                }
            
                // Get world-space mouse position
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                // Find distance to current level (plane)
                ground.Raycast(ray, out float distToGround);
                Vector3 point = ray.GetPoint(distToGround);
                point = point.Round(1);
                mouseWorld = point; // where to place prefab

                DrawCursor();
            
                // Try to spawn tile (left mouse button)
                if ((e.type == EventType.MouseDrag || e.type == EventType.MouseDown) &&
                    e.button == 0)
                {
                    RequestSpawn();
                    
                    Repaint();
                    // Consume event
                    e.Use();
                }
            }
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (styles == null)
            {
                styles = new Styles();
            }
            
            GUILayout.Label("Terrain Tools", EditorStyles.boldLabel);
            GUILayout.Space(2);
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                int tool = (int)selectedCategory;
                GUILayout.Label("Selected mode: " + selectedCategory.ToString());

                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    GUI.changed = false;
                    int desiredTool = GUILayout.Toolbar(tool, styles.toolIcons, styles.command);

                    // Tool changed
                    if (desiredTool != tool)
                    {
                        // Change category
                        selectedCategory = (TilemapTool)desiredTool;
                        
                        // Repaint
                        Repaint();
                    }
                    
                    GUILayout.FlexibleSpace();
                }
                
                GUILayout.Space(5);
                // Draw based on category
                switch (selectedCategory)
                {
                    case TilemapTool.SpawnTile:
                        Draw_SpawnTiles();
                        break;
                    
                    case TilemapTool.Trees:
                        Draw_SpawnTrees();
                        break;
                    default:
                        break;
                }
            }
            
            // Level & tile size
            EditorGUILayout.IntSlider(propBrushSize, 1, 3, new GUIContent("Brush Size"));
            EditorGUILayout.PropertyField(propLevel, new GUIContent("Level"));
            propLevel.intValue = propLevel.intValue.AtLeast(0);
            ground.SetNormalAndPosition(Vector3.up, new Vector3(0, propLevel.intValue, 0));
            
            GUILayout.Label("Mouse world position: " + mouseWorld);

            // Functionality buttons
            if (GUILayout.Button("Clear Layer"))
            {
                tilemap.ClearLayer();
                Repaint();
            }
            
            if (GUILayout.Button("Clear All"))
            {
                tilemap.ClearAll();
                Repaint();
            }
            
            serializedObject.ApplyModifiedProperties();
        }

        void DrawCursor()
        {
            switch (selectedCategory)
            {
                case TilemapTool.SpawnTile:
                    // Draw cursor preview mesh
                    if (cursorPrefab != null)
                    {
                        Mesh mesh = cursorPrefab.GetComponent<MeshFilter>().sharedMesh;
                        Material mat = meshPreviewMaterial;
                        mat.SetPass(0);

                        int brushSize = propBrushSize.intValue;
                        Vector3 scaleOffset = new Vector3(brushSize - 1, 0, brushSize - 1);
                    
                        Matrix4x4 meshMatrix = Matrix4x4.TRS(mouseWorld - scaleOffset * 0.5f, Quaternion.identity,
                            Vector3.one * brushSize);
                        Graphics.DrawMeshNow(mesh, meshMatrix);
                    }
                    break;
                case TilemapTool.Trees:
                    // Do additional raycast to make sure we're on a tile
                    Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        if (selectedPrefab != null)
                        {
                            prefabObjectPosition = hit.point;
                            if (snapObjectToGrid)
                            {
                                prefabObjectPosition = prefabObjectPosition.Round(1f);
                            }
                            
                            Matrix4x4 poseToWorld = Matrix4x4.TRS(prefabObjectPosition, Quaternion.identity, Vector3.one);
                            MeshFilter[] filters = selectedPrefab.GetComponentsInChildren<MeshFilter>();
                            foreach (MeshFilter filter in filters)
                            {
                                Matrix4x4 childToPose = filter.transform.localToWorldMatrix;
                                Matrix4x4 childToWorld = poseToWorld * childToPose;
                            
                                Mesh mesh = filter.sharedMesh;
                                if (mesh == null) continue;
                                Material mat = meshPreviewMaterial;
                                mat.SetPass(0);
                                Graphics.DrawMeshNow(mesh, childToWorld);
                            }
                        }
                    }
                    
                    break;
                default:
                    break;
            }
        }
        
        void DrawGrid(float gridDrawExtent, float tileSize)
        {
            float gridHalfSize = tileSize * 0.5f;

            int lineCount = Mathf.RoundToInt(gridDrawExtent / tileSize * 2);
            if (lineCount % 2 != 0) lineCount++;
            int halfLineCount = lineCount / 2;
        
            float drawAlpha = 0.35f;
            
            for (int i = -halfLineCount; i <= halfLineCount; i++)
            {
                Vector3 origin = mouseWorld;
                
                Handles.color = new Color(1, 1, 1, Mathf.Lerp(0f, drawAlpha, 1 - Mathf.Abs(i / (float)halfLineCount)));

                // Horizontal
                Vector3 ys = origin + new Vector3(i * tileSize, 0, halfLineCount * tileSize);
                Vector3 ye = origin + new Vector3(i * tileSize, 0, -halfLineCount * tileSize);
                Handles.DrawAAPolyLine(ys, ye);
                
                // Vertical
                Vector3 xs = origin + new Vector3(halfLineCount * tileSize, 0, i * tileSize);
                Vector3 xe = origin + new Vector3(-halfLineCount * tileSize, 0, i * tileSize);
                Handles.DrawAAPolyLine(xs, xe);
            }
            
            Handles.color = Color.white;

        }

        void Draw_SpawnTiles()
        {
            using (new GUILayout.VerticalScope())
            {
                EditorGUILayout.PropertyField(propPrefab);
            }
        }

        void Draw_SpawnTrees()
        {
            using (new GUILayout.VerticalScope())
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(20);
                EditorGUILayout.PropertyField(objectPrefabs);
                EditorGUILayout.EndHorizontal();
                
                // Draw grid
                snapObjectToGrid = EditorGUILayout.Toggle("Snap to Grid", snapObjectToGrid);
                
                // Draw palette buttons
                using (new GUILayout.HorizontalScope())
                {
                    Rect rect = new Rect(8, 8, 64, 64);
                    for (int i = 0; i < tilemap.objectPrefabs.Count; i++)
                    {
                        if (tilemap.objectPrefabs[i] == null) { continue;}
                        
                        // Get reference texture to preview icon
                        Texture icon = AssetPreview.GetAssetPreview(tilemap.objectPrefabs[i]);
            
                        if (GUILayout.Button(icon, GUILayout.Width(64), GUILayout.Height(64)))
                        {
                            selectedPrefab = tilemap.objectPrefabs[i];
                        }

                        rect.x += rect.width + 2;
                    }
                }
            }
        }

        void RequestSpawnTiles(Vector3 cursorWorldPos, int brushSize)
        {
            List<Vector3> tilePositions = new List<Vector3>();

            for (int x = 0; x < brushSize; x++)
            {
                for (int y = 0; y < brushSize; y++)
                {
                    tilePositions.Add(cursorWorldPos - new Vector3(x, 0, y));
                }
            }

            for (int i = 0; i < tilePositions.Count; i++)
            {
                TrySpawnTile(tilePositions[i]);
            }
        }
        
        void TrySpawnTile(Vector3 position)
        {
            if (prefab == null)
            {
                return;
            }
            
            Debug.Log("Spawning a tile at: " + mouseWorld);

            // Try to get container
            Transform currentLayerContainer = TryGetContainer();
            
            // Tile spawning
            bool spawnNewTile = true;
            Vector3 heightOffset = Vector3.up * 0.1f;
            // Raycast downward to check for existing tiles,
            if (Physics.Raycast(position + heightOffset, Vector3.down, out RaycastHit hit, 0.2f))
            {
                spawnNewTile = false;
            }

            if (currentLayerContainer == null)
            {
                Debug.LogError("No valid container for tile, skipping spawn...");
                return;
            }
            
            // Spawn tile
            if (spawnNewTile)
            {
                GameObject spawnedTile = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                Undo.RegisterCreatedObjectUndo(spawnedTile, "Spawn Tile");
                spawnedTile.transform.position = position;
                Undo.SetTransformParent(spawnedTile.transform, currentLayerContainer.transform, "Set tile's parent to container");
                Undo.RegisterFullObjectHierarchyUndo(currentLayerContainer.GameObject(), "Update layer container");
                Undo.SetCurrentGroupName("Create and set parent for new tile");
            }
            // If we detected a previous tile, update the prefab
            else
            {
                Debug.Log("Detected: " + hit.transform.gameObject);
            }
        }

        Transform TryGetContainer()
        {
            int layer = tilemap.groundLevel;

            TilemapTerrainLayer tilemapLayer;
            bool layerIndexCheck = tilemap.Containers.TryGetValue(layer, out tilemapLayer);
            bool createNewLayer = !layerIndexCheck || tilemapLayer == null;
            
            // Dictionary doesn't contain layer, create new layer
            if (createNewLayer)
            {
                // New container and layer
                GameObject newContainer = new GameObject("Layer " + layer);
                TilemapTerrainLayer newTilemapLayer = newContainer.AddComponent<TilemapTerrainLayer>();
                newTilemapLayer.layerIndex = layer;
                if (layerIndexCheck)
                {
                    // If somehow a key already exists, we replace the container value
                    tilemap.Containers.Remove(layer);
                }
                tilemap.Containers.Add(layer, newTilemapLayer);
                
                // Transforms
                Undo.RegisterCreatedObjectUndo(newContainer, "Create new container");
                Undo.SetTransformParent(newContainer.transform, tilemap.transform, "Set parent to tilemap");
                Undo.RegisterFullObjectHierarchyUndo(tilemap.GameObject(), "Update tilemap hierarchy");
                return newContainer.transform;
            }
            else
            {
                // We still need to check container
                Debug.Log("Container: " + tilemapLayer);
                return tilemapLayer.transform;
            }
            
            return default;
        }

        void TrySpawnObject(Vector3 position)
        {
            if (selectedPrefab == null)
            {
                return;
            }
            
            // Try to get container
            Transform currentLayerContainer = TryGetContainer();
            
            // Spawn prefab
            GameObject spawnedObject = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);
            Undo.RegisterCreatedObjectUndo(spawnedObject, "Spawned Objects");
            spawnedObject.transform.position = position;
            spawnedObject.transform.rotation = Quaternion.identity;
            Undo.SetTransformParent(spawnedObject.transform, currentLayerContainer.transform, "Set tile's parent to container");
        }
        
        void RequestSpawn()
        {
            switch (selectedCategory)
            {
                case TilemapTool.SpawnTile:
                    RequestSpawnTiles(mouseWorld, propBrushSize.intValue);
                    break;
                case TilemapTool.Trees:
                    TrySpawnObject(prefabObjectPosition);
                    break;
                default:
                    break;
            }
        }
    }
}
