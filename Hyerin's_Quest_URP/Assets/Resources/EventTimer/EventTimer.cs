using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTimer : MonoBehaviour
{
    [SerializeField]
    private float _totalTime = 60.0f;
    private float _currentTime;

    private bool _isTimerStart;

    [SerializeField]
    private bool _isStartInStart = true;

    public UnityEvent OnTime;

    void Start()
    {
        if (_isStartInStart)
            TimerStart();
    }

    void Update()
    {
        if (!_isTimerStart) return;

        if (Input.anyKey)
        {
            _currentTime = 0.0f;
            return;
        }

        _currentTime += Time.deltaTime;
        if (_currentTime >= _totalTime)
            TimeEnd();
    }

    public void TimerStart()
    {
        _isTimerStart = true;
        _currentTime = 0.0f;
    }

    public void TimeEnd()
    {
        _isTimerStart = false;

        if (OnTime != null)
            OnTime.Invoke();
    }
}
