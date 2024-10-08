using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Diagnostics;

namespace GameDevInc;

public enum EModuleJobType
{
    JOB_Programmer = 0,
    JOB_2DArtist = 1,
    JOB_3DArtist = 2,
    JOB_SoundEngineer = 3,
    JOB_Marketing = 4,
    JOB_Design = 5,
    JOB_UIUX = 6,
    JOB_QA = 7,
    JOB_HR = 8,
    JOB_Production = 9,
    JOB_Actor = 10,
    JOB_Writer = 11,
    JOB_All = 12
}

public enum EGameSize
{
    GAME_Indie,
    GAME_III,
    GAME_AAA
}

public class BaseModule
{
    // == Production Min/Max Values == //
    private const float c_MinPointsProduced = 0.33f;
    private const float c_MaxPointsProduced = 1.33f;
    private const float c_MinQualityProduced = 0.012f;
    private const float c_MaxQualityProduced = 0.634f;


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

    public float CurrentPoints { get; protected set; }
    public float RequiredPoints { get; protected set; }
    public float ModuleQuality { get; protected set; }

    public BaseModule(string name, EModuleJobType jobType, int indieMin, int indieMax, int iiiMin, int iiiMax, int aaaMin, int aaaMax, string pathToIcon)
    {
        ModuleName = name;
        m_MinAAAPoints = aaaMin;
        m_MaxAAAPoints = aaaMax;
        m_MinIIIPoints = iiiMin;
        m_MaxIIIPoints = iiiMax;
        m_MinIndiePoints = indieMin;
        m_MaxIndiePoints = indieMax; 
        ModuleJobType = jobType;

        if(!string.IsNullOrEmpty(pathToIcon))
            ModuleIcon = GD.Load<Image>(pathToIcon);

    }

    public BaseModule(string name, EModuleJobType jobType, int indieMin, int indieMax, int iiiMin, int iiiMax, int aaaMin, int aaaMax, Image icon)
    {
        ModuleName = name;
        m_MinAAAPoints = aaaMin;
        m_MaxAAAPoints = aaaMax;
        m_MinIIIPoints = iiiMin;
        m_MaxIIIPoints = iiiMax;
        m_MinIndiePoints = indieMin;
        m_MaxIndiePoints = indieMax;
        ModuleJobType = jobType;


        ModuleIcon = icon;

    }

    /// <summary>
    /// Determine how many Programing points are required
    /// </summary>
    /// <param name="gameSize">Size of the game we are creating</param>
    /// <param name="pointModifier">Modifier</param>
    /// <returns></returns>
    public int SetRequiredPoints(EGameSize gameSize, float pointModifier = 1f)
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
                return Mathf.FloorToInt(rand.RandiRange(m_MinAAAPoints, m_MaxAAAPoints) * pointModifier);
            default: return 0; 
        }
    }

    public bool WorkOnModule(StaffMember worker)
    {
        RandomNumberGenerator rand = new RandomNumberGenerator();
        rand.Randomize();
        CurrentPoints += rand.RandfRange(c_MinPointsProduced, c_MaxPointsProduced) * ((worker.GeneralStats.WorkSpeed / 100) + 1);
        ModuleQuality += rand.RandfRange(c_MinQualityProduced, c_MaxQualityProduced) * ((worker.GeneralStats.WorkQuality / 100) + 1);

        // TODO: Add module to the company inventory
        /*if(CurrentPoints > RequiredPoints)
        {
            ModuleDatabase.ModuleInventory.Add(this);
            return true;
        }*/

        return false;
    }

    public BaseModule CreateModule(EGameSize size)
    {
        var module = new BaseModule(ModuleName, ModuleJobType, m_MinIndiePoints, m_MaxIndiePoints, m_MinIIIPoints, m_MaxIIIPoints, m_MinAAAPoints, m_MaxIIIPoints, ModuleIcon);
        module.SetRequiredPoints(size);
        return module;
    }
}

public class BaseModuleJsonData
{
    [JsonProperty]
    public string ModuleName;
    [JsonProperty]
    public int ModuleJobType;
    [JsonProperty]
    public string IconPath;
    [JsonProperty]
    public int MinAAA;
    [JsonProperty]
    public int MaxAAA;
    [JsonProperty]
    public int MinIII;
    [JsonProperty]
    public int MaxIII;
    [JsonProperty]
    public int MinIndie;
    [JsonProperty]
    public int MaxIndie;    
}
