using UnityEngine;
using System.Collections.Generic;

public class SquaresManager : MonoBehaviour
{
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private GameObject gameManagerObject;
    
    private GameManager _gameManager;
    private Square[,] _squaresArray;

    private int _width;
    private int _height;
    private float _squareSize;
    
    private static float _timer;

    private List<Square> _targetedSquaresList;
    private void Start()
    {
        _gameManager = gameManagerObject.GetComponent<GameManager>();

        _width = _gameManager.GetWidth();
        _height = _gameManager.GetHeight();
        _squareSize = _gameManager.GetCellSize();
        
        _squaresArray = new Square[_width, _height];
        _targetedSquaresList = new List<Square>();
        
        for (var i = 0; i < _squaresArray.GetLength(0); i++)
        {
            for (var j = 0; j < _squaresArray.GetLength(1); j++)
            {
                var position = Utils.GetWorldCellPosition(i, j) + new Vector3(_squareSize, _squareSize) / 2f;
                _squaresArray[i, j] = new Square(SpawnSquare(position, _squareSize));
            }
        }
        
    }
    private void UpdateSquares()
    {
        for (var x = 0; x < _squaresArray.GetLength(0); ++x)
        {
            for (var y = 0; y < _squaresArray.GetLength(1); ++y)
            {
                var neighbours = CountOfNeighbours(x, y);
                if (IsActiveSquare(x, y) && neighbours != 2 && neighbours != 3)
                {
                    _squaresArray[x, y].Deactivate();
                }

                if (!IsActiveSquare(x, y) && neighbours == 3)
                {
                    _squaresArray[x, y].Activate();
                }
                
            }
        }

        foreach (var square in _squaresArray)
        {
            square.Update();
        }
    }
    private void UpdateSquareOnClick(List<Vector2Int> squaresToUpdatePositions, bool activate)
    {
        foreach (var squarePositions in squaresToUpdatePositions)
        {
            var x = squarePositions.x;
            var y = squarePositions.y;

            if (!Utils.IsAllowableSquare(x, y)) continue;

            if (activate)
            {
                _squaresArray[x, y].Activate();
            }
            else
            {
                _squaresArray[x, y].Deactivate();
            }

            _squaresArray[x, y].Update();
        }
    }
    private void ClearGrid()
    {
        for (var x = 0; x < _squaresArray.GetLength(0); ++x)
        {
            for (var y = 0; y < _squaresArray.GetLength(1); ++y)
            {
                _squaresArray[x, y].Deactivate();
                _squaresArray[x, y].Update();
            }
        }
    }
    private void RandomFillGrid()
    {
        var random = new System.Random();
        for (var x = 0; x < _squaresArray.GetLength(0); ++x)
        {
            for (var y = 0; y < _squaresArray.GetLength(1); ++y)
            {
                if (random.Next(2) == 0)
                {
                    _squaresArray[x, y].Activate();
                }
                else
                {
                    _squaresArray[x, y].Deactivate();
                }
                _squaresArray[x, y].Update();
            }
        }
    }
    private void UpdateTargetSquare(List<Vector2Int> squaresToTargetPositions)
    {
        foreach (var square in _targetedSquaresList)
        {
            square.Untarget();
        }
        _targetedSquaresList.Clear();

        foreach (var squarePosition in squaresToTargetPositions)
        {
            var x = squarePosition.x;
            var y = squarePosition.y;

            if (!Utils.IsAllowableSquare(x, y)) continue;

            _squaresArray[x, y].Target();
            _targetedSquaresList.Add(_squaresArray[x, y]);
        }
    }
    private int CountOfNeighbours(int x, int y)
    {
        var neighbours = 0;

        for (var i = 0; i < Utils.Dx.Length; i++)
        {
            neighbours += IsActiveSquare(x + Utils.Dx[i], y + Utils.Dy[i]) ? 1 : 0;
        }
        
        return neighbours;
    } 
    private bool IsActiveSquare(int x, int y)
    {
        return Utils.IsAllowableSquare(x, y) && _squaresArray[x, y].IsActive();
    }
    public void UpdateOnTrigger()
    {
        if (!GameManager.IsRunning())
        {
            var curMousePosition = GameManager.GetCamera().ScreenToWorldPoint(Input.mousePosition);
            
            UpdateTargetSquare(GetSquaresByBrush(curMousePosition));
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                RandomFillGrid();
            }
            
            if (Input.GetMouseButton(0))
            {
                UpdateSquareOnClick(GetSquaresByBrush(curMousePosition), true);
            }
            
            if (Input.GetMouseButton(1))
            {
                UpdateSquareOnClick(GetSquaresByBrush(curMousePosition), false);
            }
            
            if (Input.GetKeyDown(KeyCode.C))
            {
                ClearGrid();
            }
            return;
        }

        if (_timer >= _gameManager.GetTimeToIteration())
        {
            _timer = 0f;
            UpdateSquares();
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }
    private List<Vector2Int> GetSquaresByBrush(Vector3 center)
    {
        var squares = new List<Vector2Int>();
        
        var x = Utils.GetCellCoordinates(center).x;
        var y = Utils.GetCellCoordinates(center).y;

        for (var dx = -_gameManager.GetBrushSize(); dx <= _gameManager.GetBrushSize(); dx++)
        {
            for (var dy = -_gameManager.GetBrushSize(); dy <= _gameManager.GetBrushSize(); dy++)
            {
                if (Mathf.Abs(dy) + Mathf.Abs(dx) <= _gameManager.GetBrushSize())
                {
                    squares.Add(new Vector2Int(x + dx, y + dy));
                }
            }
        }
        return squares;
    }
    private GameObject SpawnSquare(Vector3 position, float squareSize)
    {
        var square = Instantiate(squarePrefab, position, Quaternion.identity);
        square.transform.localScale = new Vector3(squareSize, squareSize, 1);
        return square;
    }
}
