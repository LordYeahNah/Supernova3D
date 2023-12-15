using System.Collections.Generic;
using System;
using UnityEngine;

public class Timer
{
    public float TimerLength;
    private float _CurrentTime;
    public bool IsActive = true;
    public bool Loop;
    public event Action TimerCompleteActions;

    public float TimeRemaining => TimerLength - _CurrentTime;

    public Timer(float length, Action completeAction, bool loop = false, bool isActive = true)
    {
        TimerLength = length;
        TimerCompleteActions += completeAction;
        Loop = loop;
        IsActive = isActive;
    }

    public void OnUpdate()
    {
        _CurrentTime += 1 * Time.deltaTime;
        if(_CurrentTime > TimerLength)
            _CompleteTimer();
    }

    private void _CompleteTimer()
    {
        TimerCompleteActions?.Invoke();
        _CurrentTime = 0f;
        IsActive = Loop;
    }

    public void ResetTimer(bool isActive = true)
    {
        _CurrentTime = 0f;
        IsActive = isActive;
    }
}