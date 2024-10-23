using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private GameObject squaresManagerObject;
    
    private static Table _table;
    private static SquaresManager _squaresManager;
    private static Camera _camera;
    
    [SerializeField] private float timeToIteration;
    [SerializeField] private int width;
    [SerializeField] private int height;  
    [SerializeField] private float cellSize;
    [SerializeField] private int brushSize;
    
    private static bool _isRunning;
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        
        _isRunning = false;
        
        Utils.Instantiate(width, height, cellSize);
        
        _table = new Table(width, height);
        
        _squaresManager = squaresManagerObject.GetComponent<SquaresManager>();
        
        _camera = cameraObject.GetComponent<Camera>();
        
        cameraObject.transform.position = new Vector3(width * cellSize / 2f, height * cellSize / 2f, -10);
    }
    private void Update()
    {
        Debug.Log("FPS: " + 1f / Time.deltaTime);
        
        _table.Render();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isRunning = !_isRunning;
        }
        
        _squaresManager.UpdateOnTrigger();

        for (var i = 0; i <= 9; ++i)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                brushSize = i;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            if (timeToIteration > 0.1)
            {
                timeToIteration *= (float)(0.75);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            if (timeToIteration < 5)
            {
                timeToIteration *= (float)(1.25);
            }
        }
    }
    public static bool IsRunning()
    {
        return _isRunning;
    }
    public static Camera GetCamera()
    {
        return _camera;
    }
    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
    public float GetCellSize()
    {
        return cellSize;
    }
    public int GetBrushSize()
    {
        return brushSize;
    }
    public float GetTimeToIteration()
    {
        return timeToIteration;
    }

    public List<Vector2Int> GetSquaresByBrush(Vector3 center)
    {
        var squares = new List<Vector2Int>();
        
        var x = Utils.GetCellCoordinates(center).x;
        var y = Utils.GetCellCoordinates(center).y;

        for (var dx = -brushSize; dx <= brushSize; dx++)
        {
            for (var dy = -brushSize; dy <= brushSize; dy++)
            {
                if (Mathf.Abs(dy) + Mathf.Abs(dx) <= brushSize)
                {
                    squares.Add(new Vector2Int(x + dx, y + dy));
                }
            }
        }
        return squares;
    }
}
