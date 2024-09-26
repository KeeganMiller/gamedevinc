using GameDevInc;
using Godot;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GameDevInc;

public partial class ModuleDatabase : Node3D
{
    public static ModuleDatabase Instance { get; private set; } 
    public List<BaseModule> Modules = new List<BaseModule>();

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
    }
    
    public List<BaseModule> GetAllModules()
        => Modules;

    /// <summary>
    /// Gets the module by a specific name
    /// </summary>
    /// <param name="name">Name of the module</param>
    /// <returns>Base Module</returns>
    public BaseModule GetModule(string name)
    {
        foreach (var module in Modules)
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
        foreach (var module in Modules)
            if (module.ModuleJobType == jobType)
                modules.Add(module);

        return modules;
    }
}