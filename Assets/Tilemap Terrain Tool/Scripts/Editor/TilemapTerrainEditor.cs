using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

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

    public enum ValueType
    {
        Fixed,
        Random,
    }

    public struct SpawnPoint
    {
        private GameObject prefab;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public bool isObstructed;
        public TileObject tile;
        
        public Vector3 Up => rotation * Vector3.up; // Transforms a direction by rotation

        public SpawnPoint(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.prefab = prefab;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            isObstructed = false;
            tile = null;
            
            // Check for obstruction, if true then allows overrides
            if (prefab == null)  { return; }

            //SpawnablePrefab spawnablePrefab = prefab.GetComponent<SpawnablePrefab>();
            //if (spawnablePrefab == null)
            //{
            //    // allows spawning on top
            //    isObstructed = false;
            //}
            //else
            //{
            //    isObstructed = Physics.BoxCast(spawnablePrefab.Center, spawnablePrefab.Size * 0.5f,
            //        spawnablePrefab.transform.forward, out RaycastHit hit, Quaternion.identity, 
            //        spawnablePrefab.Size.x, 0);
            //    if (isObstructed)
            //    {
            //        Debug.Log("Prefab hitting: " + hit.collider.gameObject.name);
            //    }
            //}
        }
        
    }

    public struct FoliageSpawnData
    {
        public GameObject foliagePrefab;
        public Vector2 pointInTile;

        public void SetRandomValues(List<GameObject> prefabs)
        {
            pointInTile = new Vector2(Random.value, Random.value) * 2f - Vector2.one; // remap to -1, 1
            foliagePrefab = prefabs.Count == 0 ? null : prefabs[Random.Range(0, prefabs.Count)];
        }
    }

    public struct FoliageSpawnPoint
    {
        public FoliageSpawnData spawnData;
        public Vector3 position;
        public Quaternion rotation;
        public TileObject tile;

        public Vector3 Up => rotation * Vector3.up; 
        
        public FoliageSpawnPoint(Vector3 position, Quaternion rotation, FoliageSpawnData spawnData, TileObject tileHit = null)
        {
            this.spawnData = spawnData;
            this.position = position;
            this.rotation = rotation;
            tile = tileHit;
        }
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
        private static bool snapObjectToGrid = true;

        private static bool deleting;
        static bool drawMode = true;
        
        #region Properties

        // Props
        private SerializedObject so;
        private SerializedProperty propLevel;
        private SerializedProperty propBrushSize;

        private static bool drawGrid;
        private float gridExtent = 4;

        public Material meshPreviewMaterial;
        private SerializedProperty propTileSize;
        private SerializedProperty propBuildTopOnlyMode;
        
        private SerializedProperty propPrefab;
        private SerializedProperty objectPrefabs;
        private GameObject selectedPrefab = null;

        private SerializedProperty propPathIndex;
        private SerializedProperty propPathPaintDebugging;
        private SerializedProperty propPathPaintDebugMaterial;

        SerializedProperty propLayerMask;

        private int previousLayer = -1;
        #endregion
        
        #region Trees/Object/Foliage

        public static ValueType rotationType;
        private static Vector2 randomRotationRange;
        private static float fixedRotation;

        public static ValueType scaleType;
        private static Vector2 randomScaleRange = new Vector2(1f, 1f);
        private static float fixedScale = 1f;

        private Material matObstructed;

        private SerializedProperty foliagePrefabs;
        public static int foliageDensity = 1;
        
        private List<GameObject> selectedFoliages = new List<GameObject>();
        [SerializeField] private bool[] foliageSelectionStates;
        public FoliageSpawnData[] foliageSpawnDatas;
        List<FoliageSpawnPoint> foliageSpawnPoints = new List<FoliageSpawnPoint>();

        #endregion

        private void OnEnable()
        {
            Tools.hidden = true;

            tilemap = target as TilemapTerrain;
            if (tilemap != null)
            {
                tilemap.ReinitializeContainers();
                tilemap.InitializeCurrentLayer();
            }
            
            // Initialize
            propLevel = serializedObject.FindProperty("LayerIndex");
            propBrushSize = serializedObject.FindProperty("brushSize");
            propTileSize = serializedObject.FindProperty("tileSize");

            propPrefab = serializedObject.FindProperty("prefab");
            objectPrefabs = serializedObject.FindProperty("objectPrefabs");

            propPathIndex = serializedObject.FindProperty("pathIndex");
            propPathPaintDebugging = serializedObject.FindProperty("pathPaintDebugging");
            propPathPaintDebugMaterial = serializedObject.FindProperty("pathPaintDebugMaterial");

            propBuildTopOnlyMode = serializedObject.FindProperty("buildTopOnly");
            previousLayer = propLevel.intValue;

            propLayerMask = serializedObject.FindProperty("layerMask");

            foliagePrefabs = serializedObject.FindProperty("foliagePrefabs");
            
            currentContainerLayer = TryGetContainer();
            
            SceneView.duringSceneGui += DuringSceneGUI;
            
            Shader obstructedShader = Shader.Find("Anthony/Unlit/ObstructedSpawn");
            matObstructed = new Material(obstructedShader);
            
            // Check if array is wrong length
            ResetFoliageStatesArray();
        }

        private void OnDisable()
        {
            Tools.hidden = false;
            SceneView.duringSceneGui -= DuringSceneGUI;
            
            DestroyImmediate(matObstructed);
            
        }

        void DuringSceneGUI(SceneView sceneView)
        {
            if (!drawMode) { return; }

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

                // Only redraws cursor preview on repaint
                if (e.type == EventType.Repaint)
                {
                    DrawCursorPreview(sceneView.camera);
                }
            
                bool holdingShift = (Event.current.modifiers & EventModifiers.Shift) != 0;
                // Try to spawn tile (left mouse button)
                if ((e.type == EventType.MouseDrag || e.type == EventType.MouseDown) &&
                    e.button == 0)
                {
                    OnMouseClick(holdingShift || deleting);
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

                if (GUILayout.Button(drawMode ? "Disable Draw Mode" : "Enable Draw Mode"))
                {
                    drawMode = !drawMode;
                }

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
                
                // Draw based on category
                switch (selectedCategory)
                {
                    case TilemapTool.SpawnTile:
                        GUIDraw_SpawnTiles();
                        break;
                    case TilemapTool.PaintPath:
                        GUIDraw_PaintPath();
                        break;
                    case TilemapTool.Trees:
                        GUIDraw_SpawnTrees();
                        break;
                    case TilemapTool.Foliage:
                        GUIDraw_Foliage();
                        break;
                    default:
                        break;
                }
            }
            
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(propLevel, new GUIContent("Level"));
            propLevel.intValue = propLevel.intValue.AtLeast(0);
            // Adjust ground plane
            ground.SetNormalAndPosition(Vector3.up, new Vector3(0, propLevel.intValue, 0));
            GUILayout.Label("Mouse world position: " + mouseWorld);

            // Functionality buttons
            if (GUILayout.Button("Clear Layer"))
            {
                tilemap.ClearLayer();
                ReinitializeLayer();
            }
            
            if (GUILayout.Button("Clear All"))
            {
                tilemap.ClearAll();
                ReinitializeLayer();
            }

            if (GUILayout.Button("Update all Tile Prefabs"))
            {
                tilemap.UpdateAllTilePrefabs();
            }

            // If value change
            if (serializedObject.ApplyModifiedProperties())
            {
                // If layer index changed
                if (previousLayer != tilemap.LayerIndex)
                {
                    // Reinitializes layer here
                    ReinitializeLayer();
                    previousLayer = tilemap.LayerIndex;
                }
                // Reset foliage states
                ResetFoliageStatesArray();
            }
        }

        void ReinitializeLayer()
        {
            // Try to get container
            currentContainerLayer = TryGetContainer();
            // Set new current layer
            tilemap.InitializeCurrentLayer();
        }

    #region Scene Drawing

        private Pose prefabPose;
        private Vector3 prefabScale;
        private SpawnPoint prefabSpawnPoint;
    
        void DrawCursorPreview(Camera cam)
        {
            switch (selectedCategory)
            {
                case TilemapTool.SpawnTile:
                    DrawTileBrush(propBrushSize.intValue);
                    break;
                case TilemapTool.PaintPath:
                    DrawPaintPathTileBrush();
                    break;
                case TilemapTool.Trees:
                    DrawPrefabs(cam);
                    break;
                case TilemapTool.Foliage:
                    DrawFoliagePreview(cam);
                    break;
                default:
                    break;
            }
        }

        void DrawTileBrush(int Size)
        {
            Mesh mesh = cursorPrefab.GetComponent<MeshFilter>().sharedMesh;
            Material mat = meshPreviewMaterial;
            mat.SetPass(0);

            int brushSize = Size;
            Vector3 scaleOffset = new Vector3(brushSize - 1, 0, brushSize - 1);
            Vector3 cursorPosition = mouseWorld - scaleOffset * 0.5f;
            cursorPosition.y += 0.01f;
                        
            Matrix4x4 meshMatrix = Matrix4x4.TRS( cursorPosition, Quaternion.identity,
                Vector3.one * brushSize);
            Graphics.DrawMeshNow(mesh, meshMatrix);
        }

        void DrawPaintPathTileBrush()
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                DrawTileBrush(propBrushSize.intValue);
            }
        }
        
         // Draw world grid
         void DrawWorldGrid(float gridDrawExtent, float tileSize, Vector3 origin)
         {
             float gridHalfSize = tileSize * 0.5f;

             int lineCount = Mathf.RoundToInt(gridDrawExtent / tileSize * 2);
             if (lineCount % 2 != 0) lineCount++;
             int halfLineCount = lineCount / 2;
        
             float drawAlpha = 0.35f;
            
             for (int i = -halfLineCount; i <= halfLineCount; i++)
             {
                 Handles.color = new Color(1, 1, 1, Mathf.Lerp(0f, drawAlpha, 1 - Mathf.Abs(i / (float)halfLineCount)));

                 // Horizontal
                 Vector3 ys = origin + new Vector3(i * tileSize + gridHalfSize, 0, halfLineCount * tileSize + gridHalfSize);
                 Vector3 ye = origin + new Vector3(i * tileSize + gridHalfSize, 0, -halfLineCount * tileSize - gridHalfSize);
                 Handles.DrawAAPolyLine(ys, ye);
                
                 // Vertical
                 Vector3 xs = origin + new Vector3(halfLineCount * tileSize + gridHalfSize, 0, i * tileSize + gridHalfSize);
                 Vector3 xe = origin + new Vector3(-halfLineCount * tileSize - gridHalfSize, 0, i * tileSize + gridHalfSize);
                 Handles.DrawAAPolyLine(xs, xe);
             }
            
             Handles.color = Color.white;

         }

         void DrawPrefabs(Camera cam)
         {
             if (TryRaycastToTilemap(cam.transform.up, tilemap.layerMask, out RaycastHit hit, out Vector3 hitTangent, out Vector3 hitBitangent))
             {
                 Pose pose = GetSpawnPoseForObjects(hit.point, hit.normal, hitTangent, hitBitangent);
                 prefabScale = (scaleType == ValueType.Fixed) ? new Vector3(fixedScale, fixedScale, fixedScale) : Vector3.one;
                        
                 // Make new spawn point
                 prefabSpawnPoint = new SpawnPoint(selectedPrefab, pose.position, pose.rotation, prefabScale);
                        
                 if (selectedPrefab != null)
                 {
                     // Check tile
                     TileObject hitTile = hit.collider.gameObject.GetComponentInParent<TileObject>();
                     prefabSpawnPoint.tile = hitTile;
                     prefabSpawnPoint.isObstructed  = (hitTile != null && hitTile.objectOccupied != null);

                     Matrix4x4 poseToWorld = Matrix4x4.TRS(prefabSpawnPoint.position, prefabSpawnPoint.rotation, prefabScale);
                     // Draw mesh prefab
                     DrawPrefabPreview(selectedPrefab, poseToWorld, cam, prefabSpawnPoint.isObstructed);
                 }
                 else
                 {
                     //  Draw sphere and normal on hit surface
                     Handles.SphereHandleCap(-1, hit.point, Quaternion.identity, 0.1f, EventType.Repaint);
                     Handles.DrawAAPolyLine(hit.point, hit.point + hit.normal);
                 }
                        
                 // Draw grid
                 if (drawGrid)
                 {
                     DrawWorldGrid(gridExtent, tilemap.tileSize, mouseWorld);
                 }
             }
         }
         
         // Draw prefab preview
         void DrawPrefabPreview(GameObject prefabToDraw, Matrix4x4 poseToWorld, Camera cam, bool isObstructed = false)
         {
             // Draw mesh prefab
             MeshFilter[] filters = prefabToDraw.GetComponentsInChildren<MeshFilter>();
             foreach (MeshFilter filter in filters)
             {
                 Matrix4x4 childToPose = filter.transform.localToWorldMatrix;
                 Matrix4x4 childToWorld = poseToWorld * childToPose;
                                
                 Mesh mesh = filter.sharedMesh;
                 if (mesh == null) continue;

                 MeshRenderer renderer = filter.GetComponent<MeshRenderer>();
                 for (int j = 0; j < mesh.subMeshCount; j++)
                 {
                     Material mat = isObstructed ? matObstructed : renderer.sharedMaterials[j];
                     Graphics.DrawMesh(mesh, childToWorld, mat, 0, cam, j);
                 }
             }
         }

         bool TryRaycastToTilemap(Vector3 cameraUp, int layerMask, out RaycastHit hit, out Vector3 hitTangent, out Vector3 hitBitangent)
         {
             Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
             if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
             {
                 if (snapObjectToGrid)
                 {
                     hit.point = hit.point.Round(tilemap.tileSize);
                 }
                 
                 // Set up tangent space
                 Vector3 hitNormal = hit.normal;
                 hitTangent = Vector3.Cross(hitNormal, cameraUp).normalized;
                 hitBitangent = Vector3.Cross(hitNormal, hitTangent);
                 //tangentToWorldMtx = Matrix4x4.TRS(hit.point, Quaternion.LookRotation(hitNormal, hitBitangent), Vector3.one);
                 return true;
             }

             hitTangent = hitBitangent = default;
             return false;
         }

         Pose GetSpawnPoseForObjects(Vector3 hitPoint, Vector3 hitNormal, Vector3 hitTangent, Vector3 hitBitangent)
         {
             Ray pointRay = GetTangentRay(hitPoint, hitNormal, hitTangent, hitBitangent, Random.insideUnitCircle);
             if (Physics.Raycast(pointRay, out RaycastHit hit))
             {
                 // Random rotation
                 Quaternion rot = Quaternion.LookRotation(hit.normal) * ( Quaternion.Euler(90f, 0f, 0f));
                 // If rotation mode is fixed (for preview)
                 if (rotationType == ValueType.Fixed)
                 {
                     // Only adjust the yaw rotation
                     rot *= Quaternion.Euler(0, fixedRotation, 0);
                 }
                 
                 Pose pose = new Pose(hit.point, rot);
                 return pose;
             }

             return default;
         }
         
         Ray GetTangentRay(Vector3 hitPoint, Vector3 hitNormal, Vector3 hitTangent, Vector3 hitBitangent, Vector2 tangentSpacePos, float radius = 0)
         {
             // Transform tangent space into world space
             Vector3 rayOrigin = hitPoint + (hitTangent * tangentSpacePos.x + hitBitangent * tangentSpacePos.y) * radius;
             rayOrigin += hitNormal * 2f; // offset margin
             Vector3 rayDirection = -hitNormal;
             return new Ray(rayOrigin, rayDirection);
         }

         void DrawFoliagePreview(Camera cam)
         {
             DrawTileBrush(1);

             if (selectedFoliages.Count == 0)
             {
                 foliageSpawnPoints.Clear();
                 return;
             }
             // Otherwise, raycast and draw preview points
             else
             {
                 foliageSpawnPoints = new List<FoliageSpawnPoint>();
                 
                 // Check if there is a tile
                 TileObject tile = tilemap.GetTile(new Vector2Int((int)mouseWorld.x, (int)mouseWorld.z));
                 
                 // If we hit something in world space
                 if (TryRaycastToTilemap(cam.transform.up, tilemap.layerMask, out RaycastHit hit,
                         out Vector3 hitTangent, out Vector3 hitBitangent))
                 {
                     foreach (var data in foliageSpawnDatas)
                     {
                         Ray pointRay = GetTangentRay(hit.point, hit.normal, hitTangent, hitBitangent, data.pointInTile, 0.5f);
                         if (Physics.Raycast(pointRay, out RaycastHit pointHit))
                         {
                             // Rotate 90 degrees around the X-axis (to fix rotation)
                             Quaternion rot = Quaternion.LookRotation(pointHit.normal) * (Quaternion.Euler(90f, 0f, 0f));
                             FoliageSpawnPoint spawnPoint = new FoliageSpawnPoint(pointHit.point, rot, data, tile);
                             foliageSpawnPoints.Add(spawnPoint);
                         }
                     }
                 }
                 // Otherwise
                 else
                 {
                     foreach (var data in foliageSpawnDatas)
                     {
                         Vector3 location = mouseWorld + new Vector3(data.pointInTile.x, 0, data.pointInTile.y) * 0.5f;
                         FoliageSpawnPoint spawnPoint = new FoliageSpawnPoint(location, Quaternion.identity, data, tile);
                         foliageSpawnPoints.Add(spawnPoint);
                     }
                 }
                 
                 // Draw preview points
                 foreach (var point in foliageSpawnPoints)
                 {
                     // Draw points
                     Handles.SphereHandleCap(-1, point.position, point.rotation, 0.1f, EventType.Repaint);
                     Handles.DrawAAPolyLine(point.position, point.position + point.Up);
                 }
             }
         }

    #endregion
         
    #region GUI Drawing
        void GUIDraw_SpawnTiles()
        {
            GUILayout.Space(3);
            deleting = EditorGUILayout.Toggle("Delete", deleting);
            GUILayout.Space(2);
            
            using (new GUILayout.VerticalScope())
            {
                EditorGUILayout.PropertyField(propPrefab);
                EditorGUILayout.PropertyField(propBuildTopOnlyMode);
                
                // Level & tile size
                EditorGUILayout.IntSlider(propBrushSize, 1, 3, new GUIContent("Brush Size"));
            }
        }

        void GUIDraw_PaintPath()
        {
            using (new GUILayout.VerticalScope())
            {
                EditorGUILayout.PropertyField(propPathIndex);
                EditorGUILayout.PropertyField(propPathPaintDebugging);
                if (propPathPaintDebugging.boolValue)
                {
                    EditorGUILayout.PropertyField(propPathPaintDebugMaterial);  
                }
                
                // Level & tile size
                EditorGUILayout.IntSlider(propBrushSize, 1, 3, new GUIContent("Brush Size"));
            }
        }
        
        void GUIDraw_SpawnTrees()
        {
            using (new GUILayout.VerticalScope())
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(20);
                EditorGUILayout.PropertyField(objectPrefabs);
                EditorGUILayout.EndHorizontal();
                
                // Draw grid
                snapObjectToGrid = EditorGUILayout.Toggle("Snap to Grid", snapObjectToGrid);
                
                // Layer mask
                EditorGUILayout.PropertyField(propLayerMask);

                // Draw palette buttons
                int count = tilemap.objectPrefabs.Count;
                int columns = Mathf.Min(4, count);
                int rows = (count / 4) + (count % columns == 0 ? 0 : 1);
                if (rows < 1) rows = 1;

                int currentIndex = 0;
                for (int i = 0; i < rows; i++)
                { 
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Space(25);
                        for (int j = 0; j < columns; j++)
                        {
                            if (tilemap.objectPrefabs[currentIndex] == null) { continue;}
                        
                            // Get reference texture to preview icon
                            Texture icon = AssetPreview.GetAssetPreview(tilemap.objectPrefabs[currentIndex]);
            
                            if (GUILayout.Button(icon, GUILayout.Width(64), GUILayout.Height(64)))
                            {
                                selectedPrefab = tilemap.objectPrefabs[currentIndex];
                            }

                            currentIndex++;
                            if (currentIndex >= count)
                            {
                                break;
                            }
                        }
                    }
                }
                
                
                // Rotation
                using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    rotationType = (ValueType)EditorGUILayout.EnumPopup("Rotation Mode", rotationType);

                    switch (rotationType)
                    {
                        case ValueType.Random:
                            EditorGUILayout.LabelField(string.Format("Rotation: [{0:0},{1:0}]", randomRotationRange.x, randomRotationRange.y));
                            EditorGUILayout.MinMaxSlider(ref randomRotationRange.x, ref randomRotationRange.y, 0f, 360f);
                            break;
                        case ValueType.Fixed:
                            fixedRotation = EditorGUILayout.FloatField("Rotation", fixedRotation);
                            if (GUILayout.Button("Rotate 90"))
                            {
                                fixedRotation += 90f;
                                if (fixedRotation > 360f) fixedRotation -= 360f;
                            }
                            break;
                    }
                }
                
                // Scale
                using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    scaleType = (ValueType)EditorGUILayout.EnumPopup("Scale Mode", scaleType);

                    switch (scaleType)
                    {
                        case ValueType.Random:
                            EditorGUILayout.LabelField(string.Format("Scale: [{0},{1}]", randomScaleRange.x.ToString("F1"), randomScaleRange.y.ToString("F1")));
                            EditorGUILayout.MinMaxSlider(ref randomScaleRange.x, ref randomScaleRange.y, 0.1f, 5f);
                            break;
                        case ValueType.Fixed:
                            fixedScale = EditorGUILayout.FloatField("Scale", fixedScale);
                            break;
                    }
                }
                
                // Draw grid and tile size
                drawGrid = EditorGUILayout.Toggle("Draw Grid", drawGrid);
                if (drawGrid)
                {
                    EditorGUILayout.PropertyField(propTileSize);
                }
                
                // Functions
                if (GUILayout.Button("Remove all Objects from Current Layer"))
                {
                    tilemap.ClearAllObjectsFromCurrentLayer();
                }

                if (GUILayout.Button("Extract all Objects from Current Layer"))
                {
                    tilemap.ExtractCurrentLayerObjects();
                }
            }
        }

        void GUIDraw_Foliage()
        {
            using (new GUILayout.VerticalScope())
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(20);
                EditorGUILayout.PropertyField(foliagePrefabs);
                EditorGUILayout.EndHorizontal();

                snapObjectToGrid = EditorGUILayout.Toggle("Snap to Grid", snapObjectToGrid);

                // Layer mask
                EditorGUILayout.PropertyField(propLayerMask);

                // Draw palette buttons
                using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                {
                    for (int i = 0; i < tilemap.foliagePrefabs.Count; i++)
                    {
                        GameObject foliagePrefab = tilemap.foliagePrefabs[i];
                        if (foliagePrefab == null)
                        {
                            continue;
                        }

                        // Get reference texture to preview icon
                        Texture icon = AssetPreview.GetAssetPreview(tilemap.foliagePrefabs[i]);

                        EditorGUI.BeginChangeCheck();
                        foliageSelectionStates[i] = GUILayout.Toggle(foliageSelectionStates[i], icon, GUILayout.Width(64), GUILayout.Height(64));
                        if (EditorGUI.EndChangeCheck())
                        {
                            selectedFoliages.Clear();
                            for (int j = 0; j < foliageSelectionStates.Length; j++)
                            {
                                if (foliageSelectionStates[j])
                                {
                                    selectedFoliages.Add(tilemap.foliagePrefabs[j]);
                                }
                            }

                            GenerateFoliageSpawnData();
                            Debug.Log("Selected a foliage");
                        }
                    }
                }
                
                // Density
                EditorGUI.BeginChangeCheck();
                foliageDensity = EditorGUILayout.IntSlider("Density ", foliageDensity, 1, 3);
                if (EditorGUI.EndChangeCheck())
                {
                    GenerateFoliageSpawnData();
                }
                
                // Functions
                if (GUILayout.Button("Extract Foliage"))
                {
                    tilemap.ExtractFoliageFromCurrentLayer();
                }
            }
        }

        void ResetFoliageStatesArray()
        {
            // Check if array is wrong length
            if (foliageSelectionStates == null || foliageSelectionStates.Length != foliagePrefabs.arraySize)
            {
                foliageSelectionStates = new bool[foliagePrefabs.arraySize];
            }

            GenerateFoliageSpawnData();
        }
        
    #endregion
        
    #region Mouse click
        void OnMouseClick(bool delete = false)
        {
            switch (selectedCategory)
            {
                case TilemapTool.SpawnTile:
                    RequestSpawnOrDeleteTiles(mouseWorld, propBrushSize.intValue, delete);
                    break;
                case TilemapTool.PaintPath:
                    TryPaintPathMulti(mouseWorld, propBrushSize.intValue);
                    break;
                case TilemapTool.Trees:
                    TrySpawnObject(prefabSpawnPoint);
                    break;
                case TilemapTool.Foliage:
                    TryPaintOrDeleteFoliage(mouseWorld, delete);
                    break;
                default:
                    break;
            }
        }
    
        private TilemapLayer currentContainerLayer;
        void RequestSpawnOrDeleteTiles(Vector3 cursorWorldPos, int brushSize, bool delete = false)
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
                TrySpawnOrDeleteTile(tilePositions[i], delete);
            }
        }

        void TrySpawnOrDeleteTile(Vector3 position, bool delete = false)
        {
            if (prefab == null)
            {
                return;
            }

            Vector2Int tileCoords = new Vector2Int((int)position.x, (int)position.z);

            // Tile spawning
            bool detectedTile = tilemap.CurrentLayer.TryGetValue(tileCoords, out var tile);

            // Check tile
            if (detectedTile)
            {
                // Delete Tile
                if (delete)
                {
                    tilemap.DeleteTile(tileCoords, currentContainerLayer);
                }
            }
            
            // Spawn Tile
            else
            {
                if (!delete)
                {
                    GameObject spawnedTile = (GameObject)PrefabUtility.InstantiatePrefab(prefab, currentContainerLayer.transform);
                    tile = spawnedTile.GetComponent<TileObject>();
                    spawnedTile.transform.position = position;

                    // Save tile and set neighbors
                    tilemap.AddTile(tileCoords, tile, currentContainerLayer);
                }
            }
        }

        TilemapLayer TryGetContainer()
        {
            int layer = tilemap.LayerIndex;

            TilemapLayer tilemapLayer;
            bool layerIndexCheck = tilemap.Containers.TryGetValue(layer, out tilemapLayer);
            bool createNewLayer = !layerIndexCheck || tilemapLayer == null;
            
            // Dictionary doesn't contain layer, create new layer
            if (createNewLayer)
            {
                // New container and layer
                GameObject newContainer = new GameObject("Layer " + layer);
                TilemapLayer newTilemapLayer = newContainer.AddComponent<TilemapLayer>();
                newTilemapLayer.layerIndex = layer;
                newTilemapLayer.CreateContainers();
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
                return newTilemapLayer;
            }
            else
            {
                // We still need to check container
                Debug.Log("Container: " + tilemapLayer);
                return tilemapLayer;
            }
            
            return default;
        }

        void TrySpawnObject(SpawnPoint prefabSpawnPoint)
        {
            if (selectedPrefab == null)
            {
                return;
            }

            // Don't allow spawn
            if (prefabSpawnPoint.isObstructed)
            {
                return;
            }
            
            // Try to get container
            TilemapLayer currentLayerContainer = TryGetContainer();
            
            // Modify rotation if rotation mode is set to random
            if (rotationType == ValueType.Random)
            {
                prefabSpawnPoint.rotation *= Quaternion.Euler(0, Random.Range(randomRotationRange.x, randomRotationRange.y), 0);
            }
            
            // Modify scale if scale mode is set to random
            Vector3 spawnScale = prefabSpawnPoint.scale;
            if (scaleType == ValueType.Random)
            {
                float randScale = Random.Range(randomScaleRange.x, randomScaleRange.y);
                spawnScale = new Vector3(randScale, randScale, randScale);
            }
            
            // Spawn prefab
            GameObject spawnedObject = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);
            Undo.RegisterCreatedObjectUndo(spawnedObject, "Spawned Objects");
            spawnedObject.transform.position = prefabSpawnPoint.position;
            spawnedObject.transform.rotation = prefabSpawnPoint.rotation;
            spawnedObject.transform.localScale = spawnScale;
            Undo.SetTransformParent(spawnedObject.transform, currentLayerContainer.objectsContainer.transform, "Set tile's parent to container");

            // Set tile to occupy object
            if (prefabSpawnPoint.tile != null)
            {
                prefabSpawnPoint.tile.objectOccupied = spawnedObject;
            }
        }
        
        
        void TryPaintPath()
        {
            Vector2Int mouseCoords = new Vector2Int((int)mouseWorld.x, (int)mouseWorld.z);
            tilemap.TryPathPaint(mouseCoords);
        }

        void TryPaintPathMulti(Vector3 cursorWorldPos, int brushSize)
        {
            List<Vector2Int> tilePositions = new List<Vector2Int>();

            for (int x = 0; x < brushSize; x++)
            {
                for (int y = 0; y < brushSize; y++)
                {
                    Vector3 p = cursorWorldPos - new Vector3(x, 0, y);
                    tilePositions.Add(new Vector2Int((int)p.x, (int)p.z));
                }
            }

            tilemap.TryPathPaintMulti(tilePositions);
        }

        void GenerateFoliageSpawnData()
        {
            foliageSpawnDatas = new FoliageSpawnData[foliageDensity];
            for (int i = 0; i < foliageDensity; i++)
            {
                foliageSpawnDatas[i].SetRandomValues(selectedFoliages);
            }
        }
        
        void TryPaintOrDeleteFoliage(Vector3 position, bool delete = false)
        {
            // No foliage selected
            if (selectedFoliages.Count == 0)
            {
                return;
            }

            foreach (var point in foliageSpawnPoints)
            {
                bool allowSpawn = true;
                
                // Spawn prefab
                GameObject spawnedFoliage = (GameObject)PrefabUtility.InstantiatePrefab(point.spawnData.foliagePrefab);
                Undo.RegisterCreatedObjectUndo(spawnedFoliage, "Spawned Foliage");
                spawnedFoliage.transform.position = point.position;
                spawnedFoliage.transform.rotation = point.rotation;
                spawnedFoliage.transform.localScale = Vector3.one;
                
                // Check for tile
                if (point.tile != null)
                {
                    point.tile.AddFoliage(spawnedFoliage, foliageDensity);
                }
                // If there is no tile, we just spawn freely
                else
                {
                    // Try to get container
                    TilemapLayer currentLayerContainer = TryGetContainer();
                    
                    // Parent new foliage to layer's foliage container
                    Undo.SetTransformParent(spawnedFoliage.transform, currentLayerContainer.objectsContainer.transform, "Set tile's parent to container");
                }
            }
            
            
            // Last step
            GenerateFoliageSpawnData();
        }
        
    #endregion
        
        
    }
}
