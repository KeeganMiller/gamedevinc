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
    public string Name { get; protected set; }                    // Name of the staff member
    public EModuleJobType JobType { get; protected set; }                 // Reference to the staff members job type

    // == Leveling Properties == //
    // XP Leveling
    public float XP { get; protected set; }
    protected float m_NextLevelXP = 125;
    protected const float c_NextLevelXPModifier = 1.4f;
    // Level properties
    public int Level { get; protected set; }
    public int SkillPoints { get; protected set; }
    public float XP_Modifier = 1f;


    // == Generator Properties == //
    private const string c_NameResourcePath = "res://Data/Names.json";
    private static List<StaffNameData> Names = new List<StaffNameData>();


    // Assigned Desk
    // Task Queue
    // Boss/Team
    // Burnout
    // Hours

    public StaffMember(EModuleJobType jobType)
    {
        // Generate Random Name
        RandomNumberGenerator rand = new RandomNumberGenerator();
        rand.Randomize();
        Name = GetNewName(rand.Randi() > 0 ? EStaffSex.SEX_Male : EStaffSex.SEX_Female);

        JobType = jobType;              // Set the job type
        GenerateLevelDetails();                 // Generate the staff members level
        GenerateStats();                    // Call the generate stats method on the inheritied classes
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