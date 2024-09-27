using System.Collections.Generic;
using System.IO;
using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GameDevInc;

public enum EDataCategory
{
    Module = 0,
    Skill = 1,
    Asset = 2,
}

public partial class DataLoader : Node
{
    public string PathToDataFile { get; private set; } = "C:/Users/Keegan/Documents/GitHub/gamedevinc/Data/Data.json";
    public static DataLoader Instance { get; private set; }

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
        LoadAllData();
    }

    private void LoadAllData()
    {
        if (File.Exists(PathToDataFile))
        {
            var fileData = File.ReadAllText(PathToDataFile);
            var tokens = JArray.Parse(fileData);
            foreach (var token in tokens)
            {
                var dataContent = JsonConvert.DeserializeObject<DataContent>(token.ToString());
                if (dataContent != null)
                {
                    switch ((EDataCategory)dataContent.Category)
                    {
                        case EDataCategory.Skill:
                            var skill = JsonConvert.DeserializeObject<Skill>(token.ToString());
                            if(skill != null)
                                StaffDatabase.Instance.Skills.Add(skill);
                            break;
                    }
                }
            }
        }
    }
}

public class DataContent
{
    public int Category;
    public int JobType;
}