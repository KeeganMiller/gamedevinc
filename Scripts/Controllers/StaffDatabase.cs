using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;

namespace GameDevInc;

public partial class StaffDatabase : Node
{
    
    public static StaffDatabase Instance { get; private set; }

    public List<Skill> Skills = new List<Skill>();
    
    // == Generator Properties == //
    private List<StaffNameData> Names = new List<StaffNameData>();
    public List<PackedScene> MaleModels = new List<PackedScene>();
    public List<PackedScene> FemaleModels = new List<PackedScene>();

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
    }
    
    /// <summary>
    /// Generates a new name
    /// </summary>
    /// <param name="sex"></param>
    /// <returns></returns>
    public string GetNewName(EStaffSex sex)
        => GetFirstName(sex) + " " + GetLastName();

    /// <summary>
    /// Finds a first name with the specified sex
    /// </summary>
    /// <param name="sex">Sex of the first name</param>
    /// <returns>First name</returns>
    private string GetFirstName(EStaffSex sex)
    {
        RandomNumberGenerator rand = new RandomNumberGenerator();
        for(var i = 0; i < 100; ++i)
        {
            rand.Randomize();
            var name = StaffDatabase.Instance.Names[rand.RandiRange(0, StaffDatabase.Instance.Names.Count - 1)];
            if (sex == (EStaffSex)name.Sex && (ENameType)name.NameType != ENameType.NAME_Last) 
                return name.Name;
        }

        return "Error";
    }

    /// <summary>
    /// Finds a random last name
    /// </summary>
    /// <returns>Last name</returns>
    private string GetLastName()
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
    
    /// <summary>
    /// Retrieves all the character models
    /// </summary>
    public void GetCharacterModels()
    {
        // Load male models
        MaleModels.Add(GD.Load<PackedScene>("res://Prefabs/Characters/male_one.tscn"));
        MaleModels.Add(GD.Load<PackedScene>("res://Prefabs/Characters/male_two.tscn"));
        MaleModels.Add(GD.Load<PackedScene>("res://Prefabs/Characters/male_three.tscn"));
        MaleModels.Add(GD.Load<PackedScene>("res://Prefabs/Characters/male_four.tscn"));

        // Load female models
        FemaleModels.Add(GD.Load<PackedScene>("res://Prefabs/Characters/female_one.tscn"));
        FemaleModels.Add(GD.Load<PackedScene>("res://Prefabs/Characters/female_two.tscn"));
        FemaleModels.Add(GD.Load<PackedScene>("res://Prefabs/Characters/female_three.tscn"));
        FemaleModels.Add(GD.Load<PackedScene>("res://Prefabs/Characters/female_four.tscn"));
    }

    public List<Skill> GetAllSkillsOfJob(ESkillType jobType)
    {
        var skills = new List<Skill>(); 
        foreach (var skill in Skills)
        {
            if (skill.SkillTypeAsEnum == jobType)
            {
                skills.Add(new Skill(skill));
            }
        }

        return skills;
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