using System.Collections.Generic;
using System;
using Godot;

namespace GameDevInc;

public class Timer
{
    public float TimerLength;
    private float m_CurrentTime;
    public Action TimerCompleteAction;
    public bool Loop;
    public bool IsActive;

    public Timer(float timerLength, Action timerCompleteAction, bool loop = false, bool isActive = true)
    {
        TimerLength = timerLength;
        TimerCompleteAction = timerCompleteAction;
        Loop = loop;
        IsActive = isActive;
    }

    public void Pause(bool reset = true)
    {
        IsActive = false;
        m_CurrentTime = reset ? 0 : m_CurrentTime;
    }

    public void Start(bool reset = true)
    {
        IsActive = true;
        m_CurrentTime = reset ? 0 : m_CurrentTime;
    }

    public void _Update(float deltaTime)
    {
        if(!IsActive)
            return;

        m_CurrentTime += 1 * deltaTime;
        if (m_CurrentTime > TimerLength)
            CompleteTimer();
    }

    private void CompleteTimer()
    {
        TimerCompleteAction?.Invoke();
        m_CurrentTime = 0;
        IsActive = Loop;
    }
}