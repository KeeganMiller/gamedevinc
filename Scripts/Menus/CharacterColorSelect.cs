using System.Collections.Generic;
using Godot;

namespace GameDevInc.Menus;

public partial class CharacterColorSelect : Label
{
    private ColorRect _activeColorRect;
    private TextureButton _buttonRef;
    private ReferenceRect _selectedRef;

    [Export] private CharacterDesignMenu _designMenu;

    public override void _Ready()
    {
        base._Ready();

        _activeColorRect = GetNode<ColorRect>("ColorRect");
        _buttonRef = GetNode<TextureButton>("TextureRect");
        _selectedRef = GetNode<ReferenceRect>("ReferenceRect");

        if (_buttonRef != null)
            _buttonRef.Connect("pressed", new Callable(this, "OnSelectColor"));
    }

    public void SetColor(Color color)
        => _activeColorRect.Color = color;

    public void OnSelectColor()
    {
        if (_designMenu != null)
        {
            _designMenu.SetSelectedColorEdit(this);
            _selectedRef.Visible = true;
        }
    }

    public void Deselect()
        => _selectedRef.Visible = false;
}