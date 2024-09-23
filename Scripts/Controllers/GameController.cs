using System.Collections.Generic;
using Godot;

namespace GameDevInc;

public partial class GameController : Node3D
{
    public ModuleDatabase ModuleDatabase { get; private set; }

    private StaffMember staff;
    private BaseModule module;
    public override void _Ready()
    {
        base._Ready();
        // Load All Modules
        ModuleDatabase = GetNode<ModuleDatabase>("/root/ModuleDatabase");
        if(ModuleDatabase != null)
            ModuleDatabase.LoadModules();
        else
            GD.PushError("ModuleDatabase::_Ready -> Failed to get reference to the module database");

        Genre.LoadGenres();

        // Load all names
        StaffMember.LoadNames();
        staff = new Programmer();
        GD.Print($"GameController::_Ready -> {staff.Name}");

        // Test out the module work
        module = ModuleDatabase.GetAllModules()[0].CreateModule(EGameSize.GAME_Indie);
        staff.AddModule(module);
        
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (staff != null)
            staff._Update((float)delta);
    }
}