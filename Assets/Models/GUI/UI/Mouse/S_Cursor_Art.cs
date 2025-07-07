using UnityEngine;

public class S_Cursor_Art : MonoBehaviour
{
    public Texture2D cursorDefault;
    public Texture2D cursorClick;
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        // Set default cursor al iniciar
        Cursor.SetCursor(cursorDefault, hotspot, cursorMode);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(cursorClick, hotspot, cursorMode);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorDefault, hotspot, cursorMode);
        }
    }
}
