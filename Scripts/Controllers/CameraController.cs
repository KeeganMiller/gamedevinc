using System.Collections.Generic;
using Godot;

namespace GameDevInc;

public partial class CameraController : Camera3D
{
    private const float c_ZoomSpeed = 100.0f;
    // == Zoom Settings == //
    [Export]
    private float m_MinCameraZoom;                      // The minimum amount we can zoom this camera
    [Export]
    private float m_MaxCameraZoom;                              // the maximum zoom we can have on the camera
        
    private float m_CurrentZoom;                            // reference to the current zoom

    public override void _Process(double delta)
    {
        base._Process(delta);

        if(Input.IsActionJustPressed("ZoomOut"))
        {
            m_CurrentZoom += c_ZoomSpeed * (float)delta;
            m_CurrentZoom = Mathf.Clamp(m_CurrentZoom, m_MinCameraZoom, m_MaxCameraZoom);
            Size = m_CurrentZoom;
        }

        if(Input.IsActionJustPressed("ZoomIn"))
        {
            m_CurrentZoom -= c_ZoomSpeed * (float)delta;
            m_CurrentZoom = Mathf.Clamp(m_CurrentZoom, m_MinCameraZoom, m_MaxCameraZoom);
            Size = m_CurrentZoom;
        }
    }
}