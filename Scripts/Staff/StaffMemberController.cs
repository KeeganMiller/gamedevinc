using System.Collections.Generic;
using Godot;

namespace GameDevInc;

public partial class StaffMemberController : Node3D
{
    public MeshInstance3D Mesh { get; private set; }                   // Reference to the mesh of the staff member
    private StaffMember _memberRef;                     // Reference to the staff member class
    private StaffMemberModelColors _colors;                     // Reference to the color of the model

    public override void _Ready()
    {
        base._Ready();
        Mesh = GetNode<MeshInstance3D>("Model/HumanArmature/Skeleton3D/Mesh");
        if (Mesh == null)
            GD.PushWarning("StaffMemberController::_Ready -> Failed to get reference to the mesh");
    }

    public void Setup(StaffMember member, StaffMemberModelColors colors)
    {
        _memberRef = member;
        _colors = colors;
        UpdateModelColors();
    }

    public void UpdateModelColors()
    {
        if(_memberRef.Sex == EStaffSex.SEX_Male)
        {
            var skin = Mesh.GetActiveMaterial(0) as OrmMaterial3D;
            if (skin != null) skin.AlbedoColor = _colors.SkinColor;

            if (_memberRef.ModelIndex == 0 || _memberRef.ModelIndex == 1)
            {
                var hair = Mesh.GetActiveMaterial(2) as OrmMaterial3D;
                if (hair != null) skin.AlbedoColor = _colors.HairOne;

                var shirt = Mesh.GetActiveMaterial(3) as OrmMaterial3D;
                if(shirt != null) shirt.AlbedoColor = _colors.ShirtOne;

                var pants = Mesh.GetActiveMaterial(4) as OrmMaterial3D;
                if (pants != null) pants.AlbedoColor = _colors.Pants;
            } else if(_memberRef.ModelIndex == 3)
            {
                var hairOne = Mesh.GetActiveMaterial(2) as OrmMaterial3D;
                if(hairOne != null) hairOne.AlbedoColor = _colors.HairOne;

                var hairTwo = Mesh.GetActiveMaterial(3) as OrmMaterial3D;
                if(hairTwo != null) hairTwo.AlbedoColor = _colors.HairTwo;

                var shirtOne = Mesh.GetActiveMaterial(4) as OrmMaterial3D;
                if(shirtOne != null) shirtOne.AlbedoColor = _colors.ShirtOne;

                var shirtTwo = Mesh.GetActiveMaterial(5) as OrmMaterial3D;
                if(shirtTwo != null) shirtTwo.AlbedoColor = _colors.ShirtTwo;

                var pants = Mesh.GetActiveMaterial(6) as OrmMaterial3D;
                if (pants != null) pants.AlbedoColor = _colors.Pants;
            } else if(_memberRef.ModelIndex == 4)
            {
                var hair = Mesh.GetActiveMaterial(3) as OrmMaterial3D;
                if (hair != null) hair.AlbedoColor = _colors.HairOne;

                var shirtOne = Mesh.GetActiveMaterial(1) as OrmMaterial3D;
                if (shirtOne != null) shirtOne.AlbedoColor = _colors.ShirtOne;

                var shirtTwo = Mesh.GetActiveMaterial(4) as OrmMaterial3D;
                if (shirtTwo != null) shirtTwo.AlbedoColor = _colors.ShirtTwo;

                var pants = Mesh.GetActiveMaterial(5) as OrmMaterial3D;
                if (pants != null) pants.AlbedoColor = _colors.ShirtThree;
            }
        }
    }
}