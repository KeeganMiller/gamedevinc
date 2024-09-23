using System.Collections.Generic;
using System.Diagnostics;
using Godot;

namespace GameDevInc.BuildingSystem;

public partial class PlacementController : Node3D
{
    public bool IsPlacing = true;

    public OfficeObject PlacingObject { get; private set; }                        // Object that we are placing
    private GridSystem m_GridSystem;

    private Camera3D? m_Camera;

    public override void _Ready()
    {
        base._Ready();

        m_Camera = GetNode<Camera3D>("/root/SceneRoot/Camera3D");
        if (m_Camera == null)
            GD.PushWarning("PlacementController::_Ready -> Failed to get reference to the camera 3D");

        m_GridSystem = (GridSystem)GetTree().GetFirstNodeInGroup("BuildingSystem");
        if (m_GridSystem == null)
            GD.PushWarning("PlacementController::_Ready -> Failed to find reference to the grid system");

        // Testing placing object
        SetPlacingObject(GD.Load<PackedScene>("res://Prefabs/Desk_LevelOne_01.tscn"));
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (!IsPlacing || m_Camera == null || PlacingObject == null)
            return;

        var mousePos = GetViewport().GetMousePosition();
        var rayOrigin = m_Camera.ProjectRayOrigin(mousePos);
        var rayDirection = m_Camera.ProjectRayNormal(mousePos);
        var rayLength = 5000f;
        var rayEnd = rayOrigin + rayDirection * rayLength;

        var space = GetWorld3D().DirectSpaceState;

        var rayParams = new PhysicsRayQueryParameters3D
        {
            From = rayOrigin,
            To = rayEnd,
        };

        var result = space.IntersectRay(rayParams);

        if(result.Count > 0)
        {
            var hitPos = (Vector3)result["position"];
            var gridPoint = new Vector2(hitPos.X, hitPos.Z);

            if(m_GridSystem != null)
            {
                var cell = m_GridSystem.GetCell(gridPoint);
                
                if (cell != null)
                {
                    GD.Print($"X: {cell.GridCellX}, Y: {cell.GridCellY}");
                    PlacingObject.GlobalPosition = new Vector3(cell.GridCellPosition.X, m_GridSystem.Position.Y, cell.GridCellPosition.Y);
                }
                    
            }
        }
    }

    public void SetPlacingObject(PackedScene obj)
    {
        if(obj != null)
        {
            var spawned = (OfficeObject)obj.Instantiate();
            AddChild(spawned);
            PlacingObject = spawned;
        }
    }
}