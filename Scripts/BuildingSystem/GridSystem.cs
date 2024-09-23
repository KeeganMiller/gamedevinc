using System.Collections.Generic;
using Godot;

namespace GameDevInc.BuildingSystem;

public partial class GridSystem : Node3D
{
    [Export]
    private PackedScene PointIndicator;

    [Export]
    private int m_GridCellsX;
    [Export]
    private int m_GridCellsY;
    [Export]
    private float m_GridCellSize;

    private GridCell[,] m_Grid;

    [Export]
    private bool d_DrawGrid = false;
    private List<Node3D> d_SpawnedDebugCells = new List<Node3D>();

    public override void _Ready()
    {
        base._Ready();
        CreateGrid();
        if(d_DrawGrid)
            DrawDebugPoints();
    }

    private void CreateGrid()
    {
        if(m_GridCellsX > 0 && m_GridCellsY > 0 && m_GridCellSize > 0)
        {
            m_Grid = new GridCell[m_GridCellsX, m_GridCellsY];

            var startPos = new Vector2(GlobalPosition.X, GlobalPosition.Z);
            var currentPos = new Vector2(GlobalPosition.X, GlobalPosition.Z);

            for(var y = 0; y < m_GridCellsY; ++y)
            {
                for(var x = 0; x < m_GridCellsX; ++x)
                {
                    m_Grid[x, y] = new GridCell(this, x, y, currentPos);
                    currentPos.X += m_GridCellSize;
                }

                currentPos.X = startPos.X;
                currentPos.Y += m_GridCellSize;
            }
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if(d_DrawGrid)
        {
            DrawDebugPoints();
        }
    }

    private void DrawDebugPoints()
    {
        foreach(var d in d_SpawnedDebugCells)
        {
            d.QueueFree();
        }
        d_SpawnedDebugCells.Clear();

        if(m_Grid != null)
        {
            var startPos = Vector2.Zero;
            var currentPos = Vector2.Zero;
            for(var y = 0; y < m_GridCellsY; ++y)
            {
                for(var x = 0; x < m_GridCellsX; ++x)
                {
                    var cell = m_Grid[x, y];
                    if(!cell.HasObject)
                    {
                        var point = PointIndicator.Instantiate<Node3D>();
                        AddChild(point);
                        point.Position = new Vector3(currentPos.X, this.Position.Y, currentPos.Y);
                        d_SpawnedDebugCells.Add(point);
                    }

                    currentPos.X += m_GridCellSize;
                }

                currentPos.X = 0;
                currentPos.Y += m_GridCellSize;
            }
        }
    }

    public GridCell GetCell(Vector2 position)
    {
        for(var y = 0; y < m_GridCellsY; ++y)
        {
            for(var x = 0; x < m_GridCellsX; ++x)
            {
                var cell = m_Grid[x, y];
                if(position.X >= cell.GridCellPosition.X && position.X < (cell.GridCellPosition.X + m_GridCellSize))
                {
                    if(position.Y >= cell.GridCellPosition.Y && position.Y < (cell.GridCellPosition.Y + m_GridCellSize))
                    {
                        return cell;
                    }
                }
            }
        }

        return null;
    }

    public bool IsWithinGrid(int x, int y)
        => x >= 0 && x < m_Grid.GetLength(0) && y >= 0 && y < m_Grid.GetLength(1);

    public bool IsWithinGrid(int x, int y, int up, int down, int left, int right)
    {
        var minX = 0 - left;
        var maxX = m_Grid.GetLength(0) + right;
        var minY = 0 - up;
        var maxY = m_Grid.GetLength(1) + down;

        return x >= minX && x < maxX && y >= minY && y < maxY;
    }

    public GridCell GetCell(int x, int y)
    {
        if(x >= 0 && x < m_Grid.GetLength(0) && y >= 0 && y < m_Grid.GetLength(1))
            return m_Grid[x, y];

        return null;
    }

    public bool CanPlaceObject(int currentX, int currentY, int cellCountX, int cellCountY)
    {
        int xIndex = 0, yIndex = 0;
        var cellsX = Mathf.Abs(cellCountX);
        var cellsY = Mathf.Abs(cellCountY);
        for(var y = 0; y < cellsY; ++y)
        {
            for(var x = 0; x < cellsX; ++x)
            {
                if (IsWithinGrid(currentX + xIndex, currentY + yIndex))
                {
                    var cell = m_Grid[currentX + xIndex, currentY + yIndex];
                    xIndex = cellCountX > 0 ? xIndex + 1 : xIndex - 1;
                    if (cell == null)
                        return false;
                    if (cell != null && cell.HasObject)
                        return false;
                }

                yIndex = cellCountY > 0 ? yIndex + 1 : yIndex - 1;
            }
        }

        return true;
    }

    public void UpdatePlacedCells(int currentX, int currentY, int cellCountX, int cellCountY, out List<GridCell> updatedCells)
    {
        updatedCells = new List<GridCell>();                    // List to store the grid cells
        int xIndex = 0, yIndex = 0;                                 // current index to update
        // Get the amount of time we need to loop
        var cellsX = Mathf.Abs(cellCountX);
        var cellsY = Mathf.Abs(cellCountY);

        for(var y = 0; y < cellsY; ++y)
        {
            for(var x = 0; x < cellsX; ++x)
            {
                // Get next cell
                var cell = m_Grid[currentX + xIndex, currentY + yIndex];
                // Update the cell with the object
                if (cell != null)
                    cell.HasObject = true;
                // Update the index
                xIndex = cellCountX > 0 ? xIndex + 1 : xIndex - 1;
                updatedCells.Add(cell);                 // Add the cell to the list of updated cells
            }
            xIndex = 0;                 // Reset the x index
            // Update the y index
            yIndex = cellCountY > 0 ? yIndex + 1 : yIndex - 1;
        }
    }
}

public class GridCell
{
    private GridSystem m_Owner;
    public int GridCellX { get; }
    public int GridCellY { get; }
    public Vector2 GridCellPosition { get; }

    public bool HasObject = false;

    public GridCell(GridSystem owner, int x, int y, Vector2 pos)
    {
        m_Owner = owner;
        GridCellX = x;
        GridCellY = y;
        GridCellPosition = pos;
    }
}