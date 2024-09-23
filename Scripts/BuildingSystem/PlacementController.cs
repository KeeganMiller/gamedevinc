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

    private GridCell m_MouseOverCell;                       // Reference to the cell the mouse is currently over

    private Company m_PlayersCompany;                       // Reference to the players company

    public override void _Ready()
    {
        base._Ready();

        m_Camera = GetNode<Camera3D>("/root/SceneRoot/Camera3D");
        if (m_Camera == null)
            GD.PushWarning("PlacementController::_Ready -> Failed to get reference to the camera 3D");

        m_GridSystem = (GridSystem)GetTree().GetFirstNodeInGroup("BuildingSystem");
        if (m_GridSystem == null)
            GD.PushWarning("PlacementController::_Ready -> Failed to find reference to the grid system");

        // Get the players company
        var companyDb = GetNode<CompanyDatabase>("/root/CompanyDatabase");
        if (companyDb != null)
            m_PlayersCompany = companyDb.PlayersCompany;

        // Validate the players company
        if (m_PlayersCompany == null)
            GD.PushWarning("PlacementController::_Ready -> Failed to find reference to the players company");

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
                m_MouseOverCell = m_GridSystem.GetCell(gridPoint);
                
                if (m_MouseOverCell != null)
                {

                    if(IsPlacementValid(m_MouseOverCell.GridCellX, m_MouseOverCell.GridCellY))
                    {
                        PlacingObject.GlobalPosition = new Vector3(m_MouseOverCell.GridCellPosition.X, m_GridSystem.Position.Y, m_MouseOverCell.GridCellPosition.Y);
                    }
                }  
            }
        }

        if(IsPlacing && Input.IsActionJustPressed("LeftMouseClick"))
        {
            PlaceObject();
        }
    }

    private void PlaceObject()
    {
        if (m_GridSystem == null || m_MouseOverCell == null || PlacingObject == null)
            return;

        if (m_PlayersCompany == null || m_PlayersCompany.CompanyFans < PlacingObject.Cost)
            return;

        PlacingObject.GetCellSize(out int sizeX, out int sizeY);
        if(m_GridSystem.CanPlaceObject(m_MouseOverCell.GridCellX, m_MouseOverCell.GridCellY, sizeX, sizeY))
        {
            m_GridSystem.UpdatePlacedCells(m_MouseOverCell.GridCellX, m_MouseOverCell.GridCellY, sizeX, sizeY, out List<GridCell> cells);
            PlacingObject.PlaceObject(cells);
            SetPlacingObject(GD.Load<PackedScene>("res://Prefabs/Desk_LevelOne_01.tscn"));
        }
    }

    private bool IsPlacementValid(int currentX, int currentY)
    {
        // Get the Cell Size
        PlacingObject.GetCellSize(out int x, out int y);

        if(m_GridSystem != null && m_GridSystem.IsWithinGrid(currentX + x, currentY + y))
        {
            return true;
        }

        return false;
    }

    public void SetPlacingObject(PackedScene obj)
    {
        if(obj != null)
        {
            var spawned = (OfficeObject)obj.Instantiate();
            AddChild(spawned);
            PlacingObject = spawned;
            PlacingObject.IsPlacing = true;
        }
    }
}