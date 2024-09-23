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
}