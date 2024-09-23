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
    private int m_CellsX;
    [Export]
    private int m_CellsY;
    private EFacingDirection m_FacingDirection;

    public bool IsPlacing = false;

    public override void _Process(double delta)
    {
        base._Process(delta);
        if(IsPlacing)
        {
            if(Input.IsActionJustPressed("RotateObject"))
            {
                this.Rotate(Vector3.Up, Mathf.DegToRad(90f));
            }
        }
    }
}