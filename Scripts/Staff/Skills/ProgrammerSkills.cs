using System.Collections.Generic;
using Godot;

namespace GameDevInc;

public class ProgrammerSkills
{
    public int Mathematics { get; private set; }
    public int Debugging { get; private set; }
    public int Graphics { get; private set; }
    public int AI { get; private set; }
    public int Networking { get; private set; }

    public ProgrammerSkills()
    {
        RandomNumberGenerator rand = new RandomNumberGenerator();
        rand.Randomize();
        Mathematics = rand.RandiRange(1, GeneralSkills.MAX_SKILL_POINTS);
        Debugging = rand.RandiRange(1, GeneralSkills.MAX_SKILL_POINTS);
        Graphics = rand.RandiRange(1, GeneralSkills.MAX_SKILL_POINTS);
        AI = rand.RandiRange(1, GeneralSkills.MAX_SKILL_POINTS);
        Networking = rand.RandiRange(1, GeneralSkills.MAX_SKILL_POINTS);
    }

    public ProgrammerSkills(int math, int debug, int graphics, int ai, int networking)
    {
        Mathematics = math;
        Debugging = debug;
        Graphics = graphics;
        AI = ai;
        Networking = networking;
    }
}