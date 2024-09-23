using GameDevInc;
using Godot;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GameDevInc;

public partial class ModuleDatabase : Node3D
{
    public const string c_ModuleDataPath = "res://Data/Modules.json";
    private List<BaseModule> m_Modules = new List<BaseModule>();

    /// <summary>
    /// Handles loading all the available modules
    /// </summary>
    public void LoadModules()
    {
        // check the file exist
        if (FileAccess.FileExists(c_ModuleDataPath))
        {
            // Open the file
            var file = FileAccess.Open(c_ModuleDataPath, FileAccess.ModeFlags.Read);
            var data = file.GetAsText();                    // Read the file data

            var tokens = JArray.Parse(data);                    // Create tokens
            foreach (var token in tokens)
            {
                // Create the json data
                var tokenData = JsonConvert.DeserializeObject<BaseModuleJsonData>(token.ToString());
                // Validate the token data
                if (tokenData != null)
                {
                    // Create and add the newly created module
                    var module = new BaseModule(tokenData.ModuleName, (EModuleJobType)tokenData.ModuleJobType,
                        tokenData.MinIndie, tokenData.MaxIndie,
                        tokenData.MinIII, tokenData.MaxIII,
                        tokenData.MinAAA, tokenData.MaxAAA, tokenData.IconPath);
                    m_Modules.Add(module);
                }
                else
                {
                    GD.PushError("BaseModule::LoadModules -> Token data not valid");
                }
            }
        }
        else
        {
            GD.PushError("BaseModule::LoadModules -> Module files don't exist");
        }
    }

    public List<BaseModule> GetAllModules()
        => m_Modules;

    /// <summary>
    /// Gets the module by a specific name
    /// </summary>
    /// <param name="name">Name of the module</param>
    /// <returns>Base Module</returns>
    public BaseModule GetModule(string name)
    {
        foreach (var module in m_Modules)
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
    public List<BaseModule> GetModulesByJob(EModuleJobType jobType)
    {
        List<BaseModule> modules = new List<BaseModule>();
        foreach (var module in m_Modules)
            if (module.ModuleJobType == jobType)
                modules.Add(module);

        return modules;
    }
}