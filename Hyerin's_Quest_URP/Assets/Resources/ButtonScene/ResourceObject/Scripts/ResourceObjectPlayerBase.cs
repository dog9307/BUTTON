using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ResourceObjectPlayerBase : MonoBehaviour
{
    [SerializeField]
    protected AudioSource _audio;

    [SerializeField]
    protected string _key;

    protected int _inputCount = 0;
    public int inputCount { get { return _inputCount; } }

    [SerializeField]
    protected float _fadeTime = 0.3f;
    private float _currentTime;
    public bool isPlaying { get; set; }

    [SerializeField]
    protected float _resultScaleFactor = 1.0f;
    public float resultScaleFactor { get { return _resultScaleFactor; } }
    
    public bool isCanPressButton { get; set; } = true;

    public UnityEvent OnRetry;

    protected virtual void Start()
    {
        _currentTime = _fadeTime;
    }

    protected virtual void Update()
    {
        if (KeyManager.instance.IsOnceKeyDown(_key) && isCanPressButton)
            Play();

        if (!isCanPressButton)
            isCanPressButton = !KeyManager.instance.IsStayKeyDown(_key);

        if (KeyManager.instance.IsOnceKeyUp(_key) && isCanPressButton)
            Stop();

        float ratio = 1.0f;
        if (!isPlaying)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime > _fadeTime)
                _currentTime = _fadeTime;

            ratio = _currentTime / _fadeTime;
            ratio = 1.0f - ratio;
        }

        SetRatio(ratio);
    }

    protected virtual void Play()
    {
        isPlaying = true;
        _audio.Play();

        _inputCount++;
    }
    protected virtual void Stop()
    {
        isPlaying = false;

        _currentTime = 0.0f;
    }

    public virtual void Retry()
    {
        Stop();
        _inputCount = 0;

        if (OnRetry != null)
            OnRetry.Invoke();
    }
    
    protected virtual void SetRatio(float alpha)
    {
        _audio.volume = alpha;
    }
}
