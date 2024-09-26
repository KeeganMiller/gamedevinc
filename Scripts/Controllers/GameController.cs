using System.Collections.Generic;
using Godot;

namespace GameDevInc;

public partial class GameController : Node3D
{

    public static GameController Instance { get; private set; }
    public const string PATH_TO_DATA_FILE = "res://Data/Data.json";

    public override void _Ready()
    {
        base._Ready();

        Instance = this;
        StaffDatabase.Instance.GetCharacterModels();

        Genre.LoadGenres();
        Theme.LoadThemes();
    }

    /// <summary>
    /// Handles spawning something into the world
    /// </summary>
    /// <param name="scene">Packed scene to spawn</param>
    /// <param name="parent">Parent of this spawned object</param>
    /// <returns>Spawned object</returns>
    public Node3D SpawnToWorld(PackedScene scene, Node3D parent = null)
    {
        if(scene != null)
        {
            var spawned = scene.Instantiate<Node3D>();
            if(parent != null)
            {
                parent.AddChild(spawned);
            } else
            {
                GetNode<Node3D>("/root/SceneRoot").AddChild(spawned);
            }

            return spawned;
        }

        return null;
    }
}