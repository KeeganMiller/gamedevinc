using System.Collections.Generic;
using Godot;

namespace GameDevInc.BuildingSystem;

public enum EFacingDirection
{
    FACING_Left,
    FACING_Right,
    FACING_Up,
    FACING_Down,
}

public partial class OfficeObject : Node3D
{
    // == Placing Settings == //
    [Export]
    protected int m_CellsX;
    [Export]
    protected int m_CellsY;
    protected EFacingDirection m_FacingDirection = EFacingDirection.FACING_Up;

    public bool IsPlacing = false;
    public List<GridCell> PlacedInCells { get; protected set; }

    // == Purchase Settings == //
    [Export]
    public int Cost { get; protected set; }                       // How much this item cost
    public float Quality { get; protected set; } = 1.0f;                             // Reference to the quality of this item (Wears down over time)
    protected Date m_PurchaseDate;                  // Date the item was purchased

    // TODO: Where down quality over time


    public override void _Process(double delta)
    {
        base._Process(delta);
        if(IsPlacing)
        {
            if(Input.IsActionJustPressed("RotateObject"))
            {
                this.Rotate(Vector3.Up, Mathf.DegToRad(90f));                   // Rotate the object

                // Update the facing direction
                switch(m_FacingDirection)
                {
                    case EFacingDirection.FACING_Up:
                        m_FacingDirection = EFacingDirection.FACING_Left;
                        break;
                    case EFacingDirection.FACING_Left:
                        m_FacingDirection = EFacingDirection.FACING_Down;
                        break;
                    case EFacingDirection.FACING_Down:
                        m_FacingDirection = EFacingDirection.FACING_Right;
                        break;
                    case EFacingDirection.FACING_Right:
                        m_FacingDirection = EFacingDirection.FACING_Up;
                        break;
                }
            }
        }
    }

    public void PlaceObject(List<GridCell> cells)
    {
        IsPlacing = false;
        PlacedInCells = cells;
    }

    public void GetCellSize(out int x, out int y)
    {
        switch(m_FacingDirection)
        {
            case EFacingDirection.FACING_Up:
                x = m_CellsX;
                y = m_CellsY;
                break;
            case EFacingDirection.FACING_Left:
                x = m_CellsY;
                y = -m_CellsX;
                break;
            case EFacingDirection.FACING_Right:
                x = -m_CellsY;
                y = m_CellsX;
                break;
            case EFacingDirection.FACING_Down:
                x = -m_CellsX;
                y = -m_CellsY;
                break;
            default:
                x = 0;
                y = 0;
                break;
        }
    }

    public void GetGridPlacementModifiers(out int up, out int down, out int left, out int right)
    {
        switch (m_FacingDirection)
        {
            case EFacingDirection.FACING_Up:
                up = 0;
                down = 0;
                left = 0;
                right = 0;
                break;
            case EFacingDirection.FACING_Left:
                up = 0;
                left = 0;
                down = 0;
                right = 0;
                break;
            case EFacingDirection.FACING_Down:
                up = 0;
                left = 0;
                down = 0;
                right = 0;
                break;
            case EFacingDirection.FACING_Right:
                left = 0;
                right = 0;
                up = 0;
                down = 0;
                break;
            default:
                up = 0;
                down = 0;
                left = 0;
                right = 0;
                break;
        }
    }

}