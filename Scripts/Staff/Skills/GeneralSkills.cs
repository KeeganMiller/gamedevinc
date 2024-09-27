﻿using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GameDevInc;

public enum ESkillType
{
    Programming = 0,
    Artist2D = 1,
    Artist3D = 2,
    Marketing = 3,
    Actor = 4, 
    Production = 5,
    QA = 6,
    Writer = 7,
    Designer = 8,
    Sound = 9,
    General = 10
}

public class GeneralSkills
{
    public const int MAX_SKILL_POINTS = 25;                           // Max skill points any category can have

    private List<Skill> _skills = new List<Skill>();

    // == Common General Skills == //
    public float WorkSpeed => GetSkill("Speed").SkillValue;
    public float WorkQuality => GetSkill("Quality").SkillValue;
    public float AttentionToDetails => GetSkill("AttentionToDetails").SkillValue;
    public float Creativity => GetSkill(("Creativity")).SkillValue;

    public GeneralSkills(EModuleJobType jobType)
    {

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
            if ((ESkillType)skill.SkillType == type)
                skills.Add(skill);
        }

        return skills;
    }

    /// <summary>
    /// Generates the skills for a character
    /// </summary>
    /// <param name="characterSkillType">Type of skills to generate</param>
    private void GenerateSkills(ESkillType characterSkillType)
    {

        return relatedSkills;
    }
}

public class Skill
{
    [JsonProperty]
    public string SkillName;
    [JsonProperty]
    public int SkillValue;
    [JsonProperty]
    public int SkillType;
    
    public ESkillType SkillTypeAsEnum => (ESkillType)SkillType;

    public Skill()
    {
        
    }

    public Skill(Skill skill)
    {
        SkillName = skill.SkillName;
        SkillValue = skill.SkillValue;
        SkillType = skill.SkillType;
    }
}