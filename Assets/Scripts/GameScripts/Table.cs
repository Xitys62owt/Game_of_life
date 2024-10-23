using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table {
    private readonly Line[] _horizontalLines;
    private readonly Line[] _verticalLines;
    public Table(int width, int height)
    {
        _horizontalLines = new Line[width + 1];
        _verticalLines = new Line[height + 1];
        
        for (var y = 0; y < _horizontalLines.GetLength(0); ++y)
        {
            _horizontalLines[y] = new Line(Utils.GetWorldCellPosition(0, y), Utils.GetWorldCellPosition(width, y));
        }
        for (var x = 0; x < _verticalLines.GetLength(0); ++x)
        {
            _verticalLines[x] = new Line(Utils.GetWorldCellPosition(x, 0), Utils.GetWorldCellPosition(x, height));
        }
    }
    public void Render()
    {
        foreach (var line in _horizontalLines)
        {
            line.Render();
        }
        foreach (var line in _verticalLines)
        {
            line.Render();
        }
    }
}
