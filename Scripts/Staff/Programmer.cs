using System.Collections.Generic;
using Godot;

namespace GameDevInc;

public class Programmer : StaffMember
{
    public int Proficiency { get; protected set; }
    public int Mathematics { get; protected set; }
    public int Debugging { get; protected set; }
    public int GraphicsProgramming { get; protected set; }
    public int ArtificalIntelligence { get; protected set; }
    public int Networking { get; protected set; }

    public Programmer() : base(EModuleJobType.JOB_Programmer)
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

    public bool IncreaseProficency(int amount = 1)
    {
        if(Proficiency + amount <= StaffMember.c_MaxStatLevel)
        {
            Proficiency += 1;
            return true;
        }

        return false;
    }

    public bool IncreaseMathematics(int amount = 1)
    {
        if(Mathematics + amount <= StaffMember.c_MaxStatLevel)
        {
            Mathematics += 1;
            return true;
        }

        return false;
    }

    public bool IncreaseDebugging(int amount = 1)
    {
        if(Debugging + amount <= StaffMember.c_MaxStatLevel)
        {
            Debugging += 1;
            return true;
        }

        return false;
    }

    public bool IncreaseGraphicsProgramming(int amount = 1)
    {
        if(GraphicsProgramming + amount <= StaffMember.c_MaxStatLevel)
        {
            GraphicsProgramming += 1;
            return true;
        }

        return false;
    }

    public bool IncreaseArtificalIntelligence(int amount = 1)
    { 
        if(ArtificalIntelligence + amount <= StaffMember.c_MaxStatLevel)
        {
            ArtificalIntelligence += 1;
            return true;
        }

        return false;
    }

    public bool IncreaseNetworking(int amount = 1)
    {
        if(Networking + amount <= StaffMember.c_MaxStatLevel)
        {
            Networking += 1;
            return true;
        }

        return false;
    }
}