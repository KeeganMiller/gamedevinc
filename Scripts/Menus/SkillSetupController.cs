using System.Collections.Generic;
using Godot;

namespace GameDevInc;

public partial class SkillSetupController : Control
{
    private GeneralSkills _generalSkills;                       // Reference to the players skills
    private int SkillPoints = 8;
    public bool CanUseSkillPoint => SkillPoints > 0;

    private GeneralSkills _playerSkills;
    private PackedScene _skillContainer;
    private VBoxContainer _skillsDisplayContainer;
    private Label _skillPointsLabel;

    public override void _Ready()
    {
        // Get required properties
        _skillContainer = GD.Load<PackedScene>("res://UserInterface/skill_container.tscn");
        _skillsDisplayContainer =
            GetNode<VBoxContainer>("VScrollBar/ScrollContainer/VBoxContainer");
        _skillPointsLabel = GetNode<Label>("SkillPointLabel");
        _skillPointsLabel.Text = "Skill Points: " + SkillPoints;
    }

    public void Setup()
    {
        var player = CompanyDatabase.Instance.PlayersStaffMember;
        if (player != null)
        {
            _playerSkills = player.GeneralStats;
            if (_playerSkills != null)
            {
                foreach (var skill in _playerSkills.Skills)
                {
                    if (_skillContainer != null)
                    {
                        var spawnedSkill = (SkillUpdateController)_skillContainer.Instantiate();
                        if (spawnedSkill != null)
                        {
                            _skillsDisplayContainer.AddChild(spawnedSkill);
                            spawnedSkill.Setup(skill, this);
                        }
                    }
                }
            }
        }
    }

    public void UseSkillPoint(Skill skillName)
    {
        SkillPoints -= 1;
        _skillPointsLabel.Text = "Skill Points: " + SkillPoints;
    }

    public void AddSkillPoint(Skill skillName)
    {
        SkillPoints += 1;
        _skillPointsLabel.Text = "Skill Points: " + SkillPoints;
    }
}