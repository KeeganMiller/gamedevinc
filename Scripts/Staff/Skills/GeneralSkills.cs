﻿using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

    public GeneralSkills(EModuleJobType jobType)
    {
        var rand = new RandomNumberGenerator();
        rand.Randomize();
        _skills = new List<Skill>
        {
            new Skill("Speed", rand.RandiRange(1, MAX_SKILL_POINTS), ESkillType.General),
            new Skill("Quality", rand.RandiRange(1, MAX_SKILL_POINTS), ESkillType.General),
            new Skill("AttentionToDetails", rand.RandiRange(1, MAX_SKILL_POINTS), ESkillType.General),
            new Skill("Creativity", rand.RandiRange(1, MAX_SKILL_POINTS), ESkillType.General),
            new Skill("Leadership", rand.RandiRange(1, MAX_SKILL_POINTS), ESkillType.General)
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

    /// <summary>
    /// Gets the list of skills in the data file, related to the passed in job type
    /// Will also generate random stat values if required
    /// </summary>
    /// <param name="jobType">Type of job to get the skills for</param>
    /// <param name="randomSkillValues">If we want to use the current values otherwise</param>
    /// <returns></returns>
    public List<Skill> CreateRelatedSkills(EModuleJobType jobType)
    {
        var rand = new RandomNumberGenerator();                 // Create random
        var skillPath = "res://Data/Data.json";                     // reference to the data path
        var relatedSkills = new List<Skill>();                  // Pre-define list of skills to return
        // Check the file exist
        if(FileAccess.FileExists(skillPath))
        {
            var file = FileAccess.Open(skillPath, FileAccess.ModeFlags.Read);
            if(file != null && file.IsOpen())
            {
                var txt = file.GetAsText();
                var tokens = JArray.Parse(txt);

                foreach(var token in tokens)
                {
                    var dataCat = JsonConvert.DeserializeObject<DataContent>(token.ToString());
                    if(jobType != EModuleJobType.JOB_All)
                    {
                        if (dataCat != null && (EDataCategory)dataCat.Category == EDataCategory.Skill)
                        {
                            var skill = JsonConvert.DeserializeObject<Skill>(token.ToString());
                            if (skill != null)
                            {
                                rand.Randomize();
                                skill.SkillValue = rand.RandiRange(1, MAX_SKILL_POINTS);
                                relatedSkills.Add(skill);
                            }
                        }
                    } else
                    {
                        var skill = JsonConvert.DeserializeObject<Skill>(token.ToString());
                        if(skill != null)
                        {
                            rand.Randomize();
                            skill.SkillValue = rand.RandiRange(1, MAX_SKILL_POINTS);
                            relatedSkills.Add(skill);
                        }
                    }

                }

                file.Close();
            }
        }

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
    public ESkillType SkillType;
}