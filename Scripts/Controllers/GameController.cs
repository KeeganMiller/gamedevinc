using System.Collections.Generic;
using Godot;

namespace GameDevInc;

public partial class GameController : Node3D
{
    public ModuleDatabase ModuleDatabase { get; private set; }
    public override void _Ready()
    {
        base._Ready();
        // Load All Modules
        ModuleDatabase = GetNode<ModuleDatabase>("/root/ModuleDatabase");
        if(ModuleDatabase != null)
        {
            ModuleDatabase.LoadModules();
            var modules = ModuleDatabase.GetAllModules();
        } else
        {
            GD.PushError("ModuleDatabase::_Ready -> Failed to get reference to the module database");
        }
    }
}