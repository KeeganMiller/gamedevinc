using System.Collections.Generic;
using Godot;

namespace GameDevInc;

public class Programmer : StaffMember
{
    public float Proficiency { get; private set; }
    public float Mathematics { get; private set; }
    public float Debugging { get; private set; }
    public float GraphicsProgramming { get; private set; }
    public float ArtificalIntelligence { get; private set; }
    public float Networking { get; private set; }

    public Programmer(string name, EModuleJobType jobType) : base(name, jobType)
    {
    }
}