using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class SnapObjectsToGridTool
{
    private const string UNDO_STR_SNAP = "snap objects";
    
    // Validation function
    [MenuItem("Extensions/Snap Selected Objects %&S", isValidateFunction: true)]
    public static bool SnapObjectsToGridValidate()
    {
        return Selection.gameObjects.Length > 0;
    }
    
    // MenuItem(path)
    //  to make hotkey, add space, symbols and hotkey
    //  https://docs.unity3d.com/ScriptReference/MenuItem.html
    //  % ctrl
    //  # shift
    //  & alt
    [MenuItem("Extensions/Snap Selected Objects %&S")]
    public static void SnapObjectsToGrid()
    {
        // Selection.gameObjects --> gameObjects in the scene that are selected
        foreach (GameObject go in Selection.gameObjects)
        {
            // Record for undo
            //   need to BE specific object that you're changing
            //      in this case we want to record the transform
            Undo.RecordObject(go.transform, UNDO_STR_SNAP);
            // Snap the transforms
            go.transform.position = go.transform.position.Round();
        }
    }
    
    // Extension method
    //  needs to be in static class and static function
    public static Vector3 Round(this Vector3 v)
    {
        v.x = Mathf.Round(v.x);
        v.y = Mathf.Round(v.y);
        v.z = Mathf.Round(v.z);
        return v;
    }
}
