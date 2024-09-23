using System.Collections.Generic;
using Godot;

namespace GameDevInc;

public class Programmer : StaffMember
{
    public float Proficiency { get; protected set; }
    public float Mathematics { get; protected set; }
    public float Debugging { get; protected set; }
    public float GraphicsProgramming { get; protected set; }
    public float ArtificalIntelligence { get; protected set; }
    public float Networking { get; protected set; }

    public Programmer(EModuleJobType jobType) : base(jobType)
    {
    }

    protected override void GenerateStats()
    {
        RandomNumberGenerator rand = new RandomNumberGenerator();
        rand.Randomize();
        for(var i = 0; i < Level; ++i)
        {
            var stat = rand.RandiRange(0, 5);
            switch(stat)
            {
                case 0:
                    Proficiency += 1;
                    break;
                case 1:
                    Mathematics += 1;
                    break;
                case 2:
                    Debugging += 1;
                    break;
                case 3:
                    GraphicsProgramming += 1;
                    break;
                case 4:
                    ArtificalIntelligence += 1;
                    break;
                case 5:
                    Networking += 1; 
                    break;
            }
        }
    }
}