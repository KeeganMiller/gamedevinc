using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GameDevInc;

public enum EStaffSex
{
    SEX_Male,
    SEX_Female,
}

public enum ENameType
{
    NAME_First,
    NAME_Last,  
}

public abstract class StaffMember
{
    public const int c_MaxStatLevel = 25;
    public string Name { get; protected set; }                    // Name of the staff member
    public EModuleJobType JobType { get; protected set; }                 // Reference to the staff members job type
    public EStaffSex Sex { get; protected set; }
    

    // == Leveling Properties == //
    // XP Leveling
    public float XP { get; protected set; }
    protected float m_NextLevelXP = 125;
    protected const float c_NextLevelXPModifier = 1.4f;
    // Level properties
    public int Level { get; protected set; }
    public int SkillPoints { get; protected set; }
    public float XP_Modifier = 1f;

    // == Stats == //
    public GeneralStaffStats GeneralStats { get; protected set; }


    // == Generator Properties == //
    private const string c_NameResourcePath = "res://Data/Names.json";
    private static List<StaffNameData> Names = new List<StaffNameData>();
    private static List<PackedScene> _maleModels;
    private static List<PackedScene> _femaleModels;

    // == Current Task Data == //
    private List<BaseModule> m_CurrentWorkList = new List<BaseModule>();                    // List of current task queued
    private Timer m_WorkTimer;

    // Assigned Desk

    // Boss/Team
    // Burnout
    // Hours

    // == Model Properties == //
    protected StaffMemberModelColors _clothingColors;
    public int ModelIndex { get; protected set; }
    protected PackedScene _modelScene;

    public StaffMember(EModuleJobType jobType)
    {
        // Generate Random Name
        RandomNumberGenerator rand = new RandomNumberGenerator();
        rand.Randomize();
        Name = GetNewName(rand.Randi() > 0 ? EStaffSex.SEX_Male : EStaffSex.SEX_Female);

        JobType = jobType;              // Set the job type
        GenerateLevelDetails();                 // Generate the staff members level
        GenerateStats();                    // Call the generate stats method on the inheritied classes

        GeneralStats = new GeneralStaffStats();

        m_WorkTimer = new Timer(3f, WorkOnModules, true, true);
    }

    public void AddXP(float xp)
    {
        var lastXP = XP;
        XP += xp * XP_Modifier;
        while(XP < m_NextLevelXP)
        {
            Level += 1;
            SkillPoints += 1;
            m_NextLevelXP = lastXP * c_NextLevelXPModifier;
        }
    }

    protected abstract void GenerateStats();
    protected virtual void GenerateLevelDetails()
    {
        RandomNumberGenerator rand = new RandomNumberGenerator();
        rand.Randomize();
        AddXP(rand.RandfRange(1, 1000));
    }

    public virtual void _Update(float deltaTime)
    {
        if (m_WorkTimer != null)
            m_WorkTimer._Update(deltaTime);
    }

    public void WorkOnModules()
    {
        if (m_CurrentWorkList.Count <= 0)
            return;

        // Work on the module and remove it if it's complete
        if (m_CurrentWorkList[0].WorkOnModule(this))
            m_CurrentWorkList.RemoveAt(0);

        RandomNumberGenerator rand = new RandomNumberGenerator();
        rand.Randomize();
        if (m_WorkTimer != null)
            m_WorkTimer.TimerLength = rand.RandfRange(1, 7);
    }

    public void AddModule(BaseModule module)
        => m_CurrentWorkList.Add(module);
    public void RemoveModule(BaseModule module)
        => m_CurrentWorkList.Remove(module);

    public static void LoadNames()
    {
        if(FileAccess.FileExists(c_NameResourcePath))
        {
            var file = FileAccess.Open(c_NameResourcePath, FileAccess.ModeFlags.Read);
            if(file != null && file.IsOpen())
            {
                var data = file.GetAsText();
                var tokens = JArray.Parse(data);

                foreach(var token in tokens )
                {
                    var name = JsonConvert.DeserializeObject<StaffNameData>(token.ToString());
                    if (name != null)
                        Names.Add(name);
                    else
                        GD.PushError("StaffMember::LoadNames -> Failed to load name");
                }

                file.Close();
            }
        } else
        {
            GD.PushError("StafFMember::LoadNames -> Failed to locate name json file");
        }
    }

    public void SetCharacterModel(StaffMemberModelColors colors, int modelIndex)
    {
        _clothingColors = colors;
        ModelIndex = modelIndex;
        
        if(Sex == EStaffSex.SEX_Male)
        {
            _modelScene = _maleModels[ModelIndex];
        } else if(Sex == EStaffSex.SEX_Female)
        {
            _modelScene = _femaleModels[ModelIndex];
        }

        if(_modelScene == null)
        {
            GD.PushWarning("StaffMember::SetCharacterModel -> Failed to select a model");
        }
    }

    public void CreateController()
    {
        if(_modelScene != null)
        {
            var spawned = (StaffMemberController)GameController.Instance.SpawnToWorld(_modelScene, null);
            if (spawned != null)
                spawned.Setup(this);
        }
    }

    public static void GetCharacterModels()
    {
        // Load male models
        _maleModels.Add(GD.Load<PackedScene>("res://Meshes/Characters/Male_Casual.fbx"));
        _maleModels.Add(GD.Load<PackedScene>("res://Meshes/Characters/Male_LongSleeve.fbx"));
        _maleModels.Add(GD.Load<PackedScene>("res://Meshes/Characters/Male_Shirt.fbx"));
        _maleModels.Add(GD.Load<PackedScene>("res://Meshes/Characters/Male_Suit.fbx"));

        // Load female models
        _femaleModels.Add(GD.Load<PackedScene>("res://Meshes/Characters/Female_Alternative.fbx"));
        _femaleModels.Add(GD.Load<PackedScene>("res://Meshes/Characters/Female_Casual.fbx"));
        _femaleModels.Add(GD.Load<PackedScene>("res://Meshes/Characters/Female_Dress.fbx"));
        _femaleModels.Add(GD.Load<PackedScene>("res://Meshes/Characters/Female_TankTop.fbx"));
    }

    /// <summary>
    /// Generates a new name
    /// </summary>
    /// <param name="sex"></param>
    /// <returns></returns>
    public static string GetNewName(EStaffSex sex)
        => GetFirstName(sex) + " " + GetLastName();

    /// <summary>
    /// Finds a first name with the specified sex
    /// </summary>
    /// <param name="sex">Sex of the first name</param>
    /// <returns>First name</returns>
    private static string GetFirstName(EStaffSex sex)
    {
        RandomNumberGenerator rand = new RandomNumberGenerator();
        for(var i = 0; i < 100; ++i)
        {
            rand.Randomize();
            var name = Names[rand.RandiRange(0, Names.Count - 1)];
            if (sex == (EStaffSex)name.Sex && (ENameType)name.NameType != ENameType.NAME_Last) 
                return name.Name;
        }

        return "Error";
    }

    /// <summary>
    /// Finds a random last name
    /// </summary>
    /// <returns>Last name</returns>
    private static string GetLastName()
    {
        RandomNumberGenerator rand = new RandomNumberGenerator();
        for (var i = 0; i < 100; ++i)
        {
            rand.Randomize();
            var name = Names[rand.RandiRange(0, Names.Count - 1)];
            if ((ENameType)name.NameType == ENameType.NAME_Last)
                return name.Name;
        }

        return "Error";
    }
}

public class StaffNameData
{
    [JsonProperty]
    public int Sex;
    [JsonProperty]
    public string Name;
    [JsonProperty]
    public int NameType;
}

public class GeneralStaffStats
{
    public float WorkSpeed { get; private set; }
    public float WorkQuality { get; private set; }
    
    public GeneralStaffStats()
    {
        RandomNumberGenerator rand = new RandomNumberGenerator();
        rand.Randomize();

        WorkSpeed = rand.RandiRange(1, StaffMember.c_MaxStatLevel);
        WorkQuality = rand.RandiRange(1, StaffMember.c_MaxStatLevel);
    }

    public bool IncreaseWorkSpeed(int amount = 1)
    {
        if (WorkSpeed + amount <= StaffMember.c_MaxStatLevel)
        {
            WorkSpeed += amount;
            return true;
        }

        return false;
    }

    public bool IncreaseWorkQuality(int amount = 1)
    {
        if(WorkQuality + 1 <= StaffMember.c_MaxStatLevel)
        {
            WorkQuality += amount;
            return true;
        }

        var s = new StaffMemberModelColors
        {
            SkinColor = new Color()
        }

        return false;
    }
}

public class StaffMemberModelColors
{
    public Color SkinColor;
    public Color HairOne;
    public Color HairTwo;
    public Color ShirtOne;
    public Color ShirtTwo;
    public Color ShirtThree;
    public Color Pants;

}