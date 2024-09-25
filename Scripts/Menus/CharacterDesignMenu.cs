using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace GameDevInc.Menus;

public partial class CharacterDesignMenu : Control
{
    private CharacterColorSelect _currentlySelectedColorEdit;

    [Export] private ColorPicker _colorPicker;
    [Export] private Array<CharacterColorSelect> _selectedCharacterColors;

    public StaffMemberController SelectedCharacterController { get; private set; }
    [Export] private SubViewport _viewport;

    private StaffMemberModelColors _modelColors;
   

    public override void _Ready()
    {
        base._Ready();

        if (_colorPicker != null)
            _colorPicker.Connect("color_changed", new Callable(this, "UpdateColor"));
    }

    public void Setup()
    {
        if (CompanyDatabase.Instance.PlayersStaffMember != null)
        {
            var index = CompanyDatabase.Instance.PlayersStaffMember.ModelIndex;
            var sex = CompanyDatabase.Instance.PlayersStaffMember.Sex;

            var scene = StaffMember.GetCharacterModel(sex, index);
            if (scene != null)
            {
                SelectedCharacterController = (StaffMemberController)scene.Instantiate();
                if (SelectedCharacterController != null)
                {
                    _modelColors = new StaffMemberModelColors();
                    _viewport.AddChild(SelectedCharacterController);
                }

            }

            foreach (var color in _selectedCharacterColors)
            {
                color.Setup();
                color.UpdateColorsWithModelData();
            }


        }
    }

    private void UpdateColor(Color color)
    {
        if (_currentlySelectedColorEdit != null)
            _currentlySelectedColorEdit.SetColor(color);
    }

    public void SetSelectedColorEdit(CharacterColorSelect color)
    {
        if (_currentlySelectedColorEdit != null)
            _currentlySelectedColorEdit.Deselect();

        _currentlySelectedColorEdit = color;
    }
}