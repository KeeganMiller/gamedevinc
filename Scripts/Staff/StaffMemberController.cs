using System.Collections.Generic;
using Godot;

namespace GameDevInc;

public partial class StaffMemberController : Node3D
{
    public MeshInstance3D Mesh { get; private set; }                   // Reference to the mesh of the staff member
    private StaffMember _memberRef;                     // Reference to the staff member class

    public override void _Ready()
    {
        base._Ready();
        Mesh = GetNode<MeshInstance3D>("Model/HumanArmature/Skeleton3D/BaseHuman");
        if (Mesh == null)
            GD.PushWarning("StaffMemberController::_Ready -> Failed to get reference to the mesh");
    }

    public void Setup(StaffMember member)
    {
        _memberRef = member;
    }
}