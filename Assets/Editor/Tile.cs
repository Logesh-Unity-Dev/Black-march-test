
using UnityEngine;
// this class holds the datas which are assinged , from the CUSTOM EDITOR tool to create obstacles...
public class Tile 
{
    Rect rect;
    public GUIStyle style;
    public bool hasClicked;
    Color color;
    public Tile(Vector2 position,float width,float height,GUIStyle _style)
    {
        rect = new Rect(position.x, position.y, width, height);
        style = _style;
    }

    public void DrawOnCall()
    {
        GUI.color = color;
        GUI.Box(rect, "", style);
    }

    public void SetStyle(GUIStyle _style, Color _color)
    {
        color = _color;
        style = _style;
        //DrawOnCall();
    }
}
