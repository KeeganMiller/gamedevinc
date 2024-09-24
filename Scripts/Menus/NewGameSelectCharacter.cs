using System.Collections.Generic;
using Godot;

namespace GameDevInc.Menus;

public partial class NewGameSelectCharacter : Control
{
    private ReferenceRect m_SelectedRect;                       // reference to the object that displays when selected
    public TextureButton ButtonRef { get; private set; }

    public override void _Ready()
    {
        base._Ready();

        m_SelectedRect = GetNode<ReferenceRect>("ReferenceRect");
        if (m_SelectedRect == null)
            GD.PushWarning("NewGameSelectCharacter::_Ready -> Failed to get reference to select rect");

        ButtonRef = GetNode<TextureButton>("TextureButton");
        if (ButtonRef == null)
            GD.PushWarning("NewGameSelectCharacter::_Ready -> Failed to get reference to the button");
    }

    public void ToggleRect(bool active)
        => m_SelectedRect.Visible = active;
}