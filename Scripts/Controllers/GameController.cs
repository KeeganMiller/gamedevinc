using System.Collections.Generic;
using Godot;

namespace GameDevInc;

public partial class GameController : Node3D
{
    public ModuleDatabase ModuleDatabase { get; private set; }

    public override void _Ready()
    {
        base._Ready();

        CompanyDatabase.LoadCompanyDetails();

        StaffMember.LoadNames();
        // Load All Modules
        ModuleDatabase = GetNode<ModuleDatabase>("/root/ModuleDatabase");
        if (ModuleDatabase != null)
            ModuleDatabase.LoadModules();
        else
            GD.PushError("ModuleDatabase::_Ready -> Failed to get reference to the module database");

        Genre.LoadGenres();
        Theme.LoadThemes();
    }
}