using System.Collections.Generic;
using Godot;

namespace GameDevInc;

public enum ESkillType
{
    Programming,
    Artist2D,
    Artist3D,
    Marketing,
    Actor, 
    Production,
    QA,
    Writer,
    Designer,
    Sound,
    General
}

public class GeneralSkills
{
    public const int MAX_SKILL_POINTS = 25;                           // Max skill points any category can have

    private List<Skill> _skills = new List<Skill>();

    public GeneralSkills()
    {
        var rand = new RandomNumberGenerator();
        rand.Randomize();
        _skills = new List<Skill>
        {
            new Skill("Speed", rand.RandiRange(1, MAX_SKILL_POINTS), ESkillType.General),
            new Skill("Quality", rand.RandiRange(1, MAX_SKILL_POINTS), ESkillType.General),
            new Skill("AttentionToDetails", rand.RandiRange(1, MAX_SKILL_POINTS), ESkillType.General),
            new Skill("Creativity", rand.RandiRange(1, MAX_SKILL_POINTS), ESkillType.General)
        };
    }

    public GeneralSkills(List<Skill> skills)
    {
        _skills = skills;
    }

    /// <summary>
    /// Finds the skill with a specific name
    /// </summary>
    /// <param name="skillName">Name of the skill</param>
    /// <returns>Reference to the skill</returns>
    public Skill GetSkill(string skillName)
    {
        foreach(var skill in _skills)
        {
            if (skill.SkillName == skillName)
                return skill;
        }

        return null;
    }

    /// <summary>
    /// Gets all the skills in a category
    /// </summary>
    /// <param name="type">Category</param>
    /// <returns>List of skills</returns>
    public List<Skill> GetAllSkillsInCategory(ESkillType type)
    {
        var skills = new List<Skill>();

        foreach(var skill in _skills)
        {
            if (skill.SkillType == type)
                skills.Add(skill);
        }

        return skills;
    }
}

public class Skill
{
    public string SkillName { get; private set; }
    public int SkillValue;
    public ESkillType SkillType { get; private set; }

    public Skill(string name, int value = 1, ESkillType type)
    {
        SkillName = name;
        SkillValue = value;
        SkillType = type;
    }
}