using System.Collections.Generic;
using Godot;

namespace GameDevInc;

public enum EDataCategory
{
    Module = 0,
    Skill = 1,
    Asset = 2,
}

public partial class DataLoader : Node
{
    public static DataLoader Instance { get; private set; }

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
    }
}

public class DataContent
{
    public int Category;
}