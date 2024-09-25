using System.Collections.Generic;
using Godot;

namespace GameDevInc.Menus;

public enum EFadeState
{
    FADE_In,
    FADE_Out,
    FADE_Idle
}

public partial class NewGameSelectCharacter : Control
{
    private ReferenceRect _selectionRect;                       // reference to the object that displays when selected
    public TextureButton ButtonRef { get; private set; }

    public EFadeState FadeState = EFadeState.FADE_Idle;

    private float _currentFadeAmount = 1;                      // The current amount of the fade
    private float _fadeTime = 5f;                               // How long it takes to fade out

    public override void _Ready()
    {
        base._Ready();

        _selectionRect = GetNode<ReferenceRect>("ReferenceRect");
        if (_selectionRect == null)
            GD.PushWarning("NewGameSelectCharacter::_Ready -> Failed to get reference to select rect");

        ButtonRef = GetNode<TextureButton>("TextureButton");
        if (ButtonRef == null)
            GD.PushWarning("NewGameSelectCharacter::_Ready -> Failed to get reference to the button");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if(FadeState == EFadeState.FADE_Out)
        {
            _currentFadeAmount = Mathf.Lerp(_currentFadeAmount, 0, (float)delta);
            SelfModulate = new Color(SelfModulate.R, SelfModulate.G, SelfModulate.B, _currentFadeAmount);

            if (_currentFadeAmount < 0.05f)
                FadeState = EFadeState.FADE_Idle;
        } else if(FadeState == EFadeState.FADE_In)
        {
            _currentFadeAmount = Mathf.Lerp(_currentFadeAmount, 1, _fadeTime * (float)delta);
            SelfModulate = new Color(SelfModulate.R, SelfModulate.G, _currentFadeAmount, (float)delta);
            if (_currentFadeAmount > 0.95)
                FadeState = EFadeState.FADE_Idle;
        }
    }

    public void ToggleRect(bool active)
        => _selectionRect.Visible = active;

    public void HideWithSelfMod()
    {
        SelfModulate = new Color(SelfModulate.R, SelfModulate.G, SelfModulate.B, 0);
        _currentFadeAmount = 0f;
    }
}