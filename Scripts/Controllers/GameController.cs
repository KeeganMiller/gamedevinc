using System.Collections.Generic;
using Godot;

namespace GameDevInc;

public partial class GameController : Node3D
{
    public override void _Ready()
    {
        base._Ready();
        BaseModule.LoadModules();
        var modules = BaseModule.GetAllModules();
        foreach (var m in modules)
            GD.Print(m.ModuleName);
    }
}