using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

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
    public const string c_ModuleDataPath = "res://Data/Modules.json";
    public string ModuleName { get; private set;  }
    public EModuleJobType ModuleJobType { get; private set; }
    public Image ModuleIcon { get; private set; }

    private static List<BaseModule> m_Modules = new List<BaseModule>();

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

        ModuleIcon = GD.Load<Image>(pathToIcon);
    }

    /// <summary>
    /// Determine how many Programing points are required
    /// </summary>
    /// <param name="gameSize">Size of the game we are creating</param>
    /// <param name="pointModifier">Modifier</param>
    /// <returns></returns>
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
                return Mathf.FloorToInt(rand.RandiRange(m_MinAAAPoints, m_MaxAAAPoints) * pointModifier);
            default: return 0; 
        }
    }

    /// <summary>
    /// Handles loading all the available modules
    /// </summary>
    public static void LoadModules()
    {
        // check the file exist
        if(FileAccess.FileExists(c_ModuleDataPath))
        {
            // Open the file
            var file = FileAccess.Open(c_ModuleDataPath, FileAccess.ModeFlags.Read);
            var data = file.GetAsText();                    // Read the file data

            var tokens = JArray.Parse(data);                    // Create tokens
            foreach(var token in tokens)
            {
                // Create the json data
                var tokenData = JsonConvert.DeserializeObject<BaseModuleJsonData>(token.ToString());     
                // Validate the token data
                if(tokenData != null)
                {
                    // Create and add the newly created module
                    var module = new BaseModule(tokenData.ModuleName, (EModuleJobType)tokenData.ModuleJobType,
                        tokenData.MinIndie, tokenData.MaxIndie,
                        tokenData.MinIII, tokenData.MaxIII,
                        tokenData.MinAAA, tokenData.MaxAAA, tokenData.IconPath);
                    m_Modules.Add(module);
                } else
                {
                    GD.PushError("BaseModule::LoadModules -> Token data not valid");
                }
            }
        } else
        {
            GD.PushError("BaseModule::LoadModules -> Module files don't exist");
        }
    }

    public static List<BaseModule> GetAllModules()
        => m_Modules;

    /// <summary>
    /// Gets the module by a specific name
    /// </summary>
    /// <param name="name">Name of the module</param>
    /// <returns>Base Module</returns>
    public static BaseModule GetModule(string name)
    {
        foreach(var module in m_Modules)
        {
            if (module.ModuleName == name)
                return module;
        }

        return null;
    }

    /// <summary>
    /// Gets all modules related to the passed in job type
    /// </summary>
    /// <param name="jobType">Modules job type</param>
    /// <returns>List of related modules</returns>
    public static List<BaseModule> GetModulesByJob(EModuleJobType jobType)
    {
        List<BaseModule> modules = new List<BaseModule>();
        foreach (var module in m_Modules)
            if (module.ModuleJobType == jobType)
                modules.Add(module);

        return modules;
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
