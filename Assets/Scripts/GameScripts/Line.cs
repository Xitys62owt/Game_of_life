using UnityEngine;
using Unity.VisualScripting;

public class Line 
{
    private readonly Vector3 _start;
    private readonly Vector3 _end;
    private readonly LineRenderer _lineRenderer;
    public Line(Vector3 start, Vector3 end)
    {
        _start = start;
        _end = end;
        
        var lineObject = new GameObject("Line");
        
        _lineRenderer = lineObject.AddComponent<LineRenderer>();
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _lineRenderer.startColor = Color.gray;
        _lineRenderer.endColor = Color.gray;
        
        _lineRenderer.widthMultiplier = 0.1f;
        _lineRenderer.positionCount = 2;
    }
    public void Render()
    {
        _lineRenderer.SetPosition(0, _start);
        _lineRenderer.SetPosition(1, _end);
    }
}
