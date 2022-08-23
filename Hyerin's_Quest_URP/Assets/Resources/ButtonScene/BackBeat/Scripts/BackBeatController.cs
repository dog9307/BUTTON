using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public enum BackBeatPlayMode
{
    None = -1,
    Both,
    AudioOnly,
    VideoOnly,
    End
}

public class BackBeatController : MonoBehaviour
{
    private AudioSource _audio;
    private VideoPlayer _video;

    [SerializeField]
    private Image _blackmask;

    [SerializeField]
    private ResourceSetController _currentSet;
    public ResourceSetController currentSet { get { return _currentSet; } }

    void Awake()
    {
        _audio = GetComponentInChildren<AudioSource>();
        _video = GetComponentInChildren<VideoPlayer>();

        _video.targetCamera = Camera.main;
    }

    public void Play(BackBeatPlayMode mode = BackBeatPlayMode.Both)
    {
        if (mode != BackBeatPlayMode.AudioOnly)
            _video.Play();

        if (mode != BackBeatPlayMode.VideoOnly)
            _audio.Play();
    }

    public void Stop(BackBeatPlayMode mode = BackBeatPlayMode.Both)
    {
        if (mode != BackBeatPlayMode.AudioOnly)
            _video.Stop();

        if (mode != BackBeatPlayMode.VideoOnly)
            _audio.Stop();
    }

    public void PlayFade(float fadeTime, BackBeatPlayMode mode = BackBeatPlayMode.Both)
    {
        StartCoroutine(FadeIn(fadeTime, mode));
    }

    public void StopFade(float fadeTime, BackBeatPlayMode mode = BackBeatPlayMode.Both)
    {
        StartCoroutine(FadeOut(fadeTime, mode));
    }

    void SetRatio(float ratio, BackBeatPlayMode mode = BackBeatPlayMode.Both)
    {
        switch (mode)
        {
            case BackBeatPlayMode.Both:
                _audio.volume = ratio;

                if (_blackmask)
                {
                    Color color = _blackmask.color;
                    color.a = (1.0f - ratio);
                    _blackmask.color = color;
                }
            break;

            case BackBeatPlayMode.AudioOnly:
                _audio.volume = ratio;
            break;

            case BackBeatPlayMode.VideoOnly:
                if (_blackmask)
                {
                    Color color = _blackmask.color;
                    color.a = (1.0f - ratio);
                    _blackmask.color = color;
                }
            break;
        }
    }

    IEnumerator FadeIn(float fadeTime, BackBeatPlayMode mode = BackBeatPlayMode.Both)
    {
        if (mode != BackBeatPlayMode.AudioOnly)
            _video.Play();

        if (mode != BackBeatPlayMode.VideoOnly)
            _audio.Play();

        float currentTime = 0.0f;
        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            currentTime = Mathf.Clamp(currentTime, 0.0f, fadeTime);

            float ratio = currentTime / fadeTime;

            SetRatio(ratio, mode);

            yield return null;
        }
    }

    IEnumerator FadeOut(float fadeTime, BackBeatPlayMode mode = BackBeatPlayMode.Both)
    {
        float currentTime = fadeTime;
        while (currentTime > 0.0f)
        {
            currentTime -= Time.deltaTime;
            currentTime = Mathf.Clamp(currentTime, 0.0f, fadeTime);

            float ratio = currentTime / fadeTime;

            SetRatio(ratio, mode);

            yield return null;
        }
        
        if (mode != BackBeatPlayMode.AudioOnly)
            _video.Stop();

        if (mode != BackBeatPlayMode.VideoOnly)
            _audio.Stop();
    }
}