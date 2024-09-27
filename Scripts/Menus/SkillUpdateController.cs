using System.Collections.Generic;
using System.Xml.Serialization;
using Godot;

namespace GameDevInc;

public partial class SkillUpdateController : Control
{
    // Label for the skill
    private Label _skillName;
    // Buttons to update the skill points
    private Button _decreaseBtn;
    private Button _increaseBtn;
    // Value of the skills
    private Label _skillValue;

    private Skill _skillRef;                        // Store reference to the skill
    public override void _Ready()
    {
        _skillName = GetNode<Label>("SkillName");
        _decreaseBtn = GetNode<Button>("DecreaseBtn");  
        _increaseBtn = GetNode<Button>("IncreaseBtn");
        _skillValue = GetNode<Label>("SkillValue");

        if (_decreaseBtn != null)
            _decreaseBtn.Pressed += OnDecreasePressed;

        if (_increaseBtn != null)
            _increaseBtn.Pressed += OnIncreasePressed;
    }

    public void Setup(Skill skill)
    {
        _skillRef = skill;
        if (_skillName != null)
            _skillName.Text = skill.SkillName;
        if (_skillValue != null)
            _skillValue.Text = skill.SkillValue.ToString();
    }

    private void OnDecreasePressed()
    {

    }

    private void OnIncreasePressed()
    {

    }
}