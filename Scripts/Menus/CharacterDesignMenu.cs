using System.Collections.Generic;
using Godot;

namespace GameDevInc.Menus;

public partial class CharacterDesignMenu : Control
{
    private CharacterColorSelect _currentlySelectedColorEdit;

    [Export] private ColorPicker _colorPicker;

    public override void _Ready()
    {
        base._Ready();

        if (_colorPicker != null)
            _colorPicker.Connect("color_changed", new Callable(this, "UpdateColor"));
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