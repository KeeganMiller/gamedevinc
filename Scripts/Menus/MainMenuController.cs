using System.Collections.Generic;
using Godot;

namespace GameDevInc.Menus;

public partial class MainMenuController : Control
{
    [ExportCategory("Buttons")]
    [Export] private TextureButton m_NewGameBtn;
    [Export] private TextureButton m_LoadGameBtn;
    [Export] private TextureButton m_SettingsBtn;
    [Export] private TextureButton m_ExitBtn;

    [ExportCategory("Menus")]
    [Export] private PackedScene m_NewGamePopup;
    private bool b_IsMenuOpen = false;

    public override void _Ready()
    {
        base._Ready();
        if(m_NewGameBtn != null)
            m_NewGameBtn.Connect("pressed", new Callable(this, "OnSelectNewGame"));

        if (m_ExitBtn != null)
            m_ExitBtn.Connect("pressed", new Callable(this, "ExitGame"));
      
    }

    private void OnSelectNewGame()
    {
        if (b_IsMenuOpen)
            return;


        if(m_NewGameBtn != null)
        {
            b_IsMenuOpen = true;
            var newGameMenu = m_NewGamePopup.Instantiate();
            AddChild(newGameMenu);
        }
    }

    private void ExitGame()
        => GetTree().Quit();
}