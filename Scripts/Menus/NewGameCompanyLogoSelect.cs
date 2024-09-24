using System.Collections.Generic;
using Godot;

namespace GameDevInc.Menus;

public partial class NewGameCompanyLogoSelect : TextureButton
{
    private ReferenceRect m_SelectionRect;

    public override void _Ready()
    {
        base._Ready();
        m_SelectionRect = GetNode<ReferenceRect>("ReferenceRect");
        if (m_SelectionRect == null)
            GD.PushWarning("NewGameCompanyLogoSelect::_Ready -> Failed to get reference to the selection rect");
    }

    public void ToggleSelectionRect(bool active)
        => m_SelectionRect.Visible = active;
}