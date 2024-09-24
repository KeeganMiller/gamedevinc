using System.Collections.Generic;
using Godot;

namespace GameDevInc.Menus;

public partial class NewGameSpecialitySelect : Control
{
    private ReferenceRect m_SelectionRect;
    public TextureButton ButtonRef { get; private set; }

    [Export] private bool m_IsConnectedToBtn = false;

    public override void _Ready()
    {
        base._Ready();
        m_SelectionRect = GetNode<ReferenceRect>("ReferenceRect");
        if (m_SelectionRect == null)
            GD.PushWarning("NewGameSpecialitySelect::_Ready -> Failed to get reference to the ref rect");

        ButtonRef = GetNode<TextureButton>("TextureButton");
        if (ButtonRef == null)
            GD.PushWarning("NewGameSpecialitySelect::_Ready -> Failed to get reference to the texture button");
    }

    public void ToggleSelectionRect(bool active)
        => m_SelectionRect.Visible = active;
}