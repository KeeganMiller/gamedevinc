using Godot;
using System;

namespace GameDevInc;

public enum EModuleJobType
{
    JOB_Programmer,
    JOB_2DArtist,
    JOB_3DArtist,
    JOB_SoundEngineer,
    JOB_Marketing,
    JOB_Design,
    JOB_UIUX
}

public enum EGameSize
{
    GAME_Indie,
    GAME_III,
    GAME_AAA
}

public class BaseModule
{
    public string ModuleName { get; private set;  }
    public EModuleJobType ModuleJobType { get; private set; }
    public Image ModuleIcon { get; private set; }

    // == Number Sets == //
    private int m_MinIndiePoints;
    private int m_MaxIndiePoints;

    private int m_MinIIIPoints;
    private int m_MaxIIIPoints;

    private int m_MinAAAPoints;
    private int m_MaxAAAPoints;

    public BaseModule(string name, EModuleJobType jobType, int indieMin, int indieMax, int iiiMin, int iiiMax, int aaaMin, int aaaMax, string pathToIcon)
    {
        m_MinAAAPoints = aaaMin;
        m_MaxAAAPoints = aaaMax;
        m_MinIIIPoints = iiiMin;
        m_MaxIIIPoints = iiiMax;
        m_MinIndiePoints = indieMin;
        m_MaxIndiePoints = indieMax; 
        ModuleJobType = jobType;

        // TODO: Load Path to Icon
    }

    public int GetRequiredProgrammingPoints(EGameSize gameSize, float pointModifier = 1f)
    {
        RandomNumberGenerator rand = new RandomNumberGenerator();
        rand.Randomize();  
        switch(gameSize)
        {
            case EGameSize.GAME_Indie:
                return Mathf.FloorToInt(rand.RandiRange(m_MinIndiePoints, m_MaxIndiePoints) * pointModifier);
            case EGameSize.GAME_III:
                return Mathf.FloorToInt(rand.RandiRange(m_MinIIIPoints, m_MaxIIIPoints) * pointModifier);
            case EGameSize.GAME_AAA:
                return Mathf.FloorToInt(rand.RandfRange(m_MinAAAPoints, m_MaxAAAPoints) * pointModifier);
            default: return 0; 
        }
    }
    
}
