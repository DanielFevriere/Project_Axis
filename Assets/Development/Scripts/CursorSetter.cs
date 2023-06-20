using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSetter : MonoBehaviour
{
    public Texture2D CursorTexture;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(CursorTexture, new Vector2(0,0), CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
