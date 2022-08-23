using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBeatManager : MonoBehaviour
{
    [SerializeField]
    private List<BackBeatController> _backbeats;
    public BackBeatController currentBackbeat { get; set; }

    public static bool isChanging;

    [SerializeField]
    private float _fadeTime = 0.3f;
    public static float FadeTime;

    [SerializeField]
    private ResultDescManager _resultUI;

    private void Awake()
    {
        FadeTime = _fadeTime;
    }

    void Start()
    {
        isChanging = false;
    }

    void Update()
    {
        FadeTime = _fadeTime;
    }

    public void SoundOff()
    {
        if (currentBackbeat)
            currentBackbeat.StopFade(_fadeTime, BackBeatPlayMode.AudioOnly);
    }

    public void SoundOff(float fadeTime)
    {
        if (currentBackbeat)
            currentBackbeat.StopFade(fadeTime, BackBeatPlayMode.AudioOnly);
    }

    public void SoundOn()
    {
        if (currentBackbeat)
            currentBackbeat.PlayFade(_fadeTime, BackBeatPlayMode.AudioOnly);
    }
    
    public void SoundOn(float fadeTime)
    {
        if (currentBackbeat)
            currentBackbeat.PlayFade(fadeTime, BackBeatPlayMode.AudioOnly);
    }

    public void ChangeBackbeat(int next, BackBeatPlayMode mode = BackBeatPlayMode.Both)
    {
        ChangeBackbeat(_fadeTime, currentBackbeat, _backbeats[next], mode);
    }

    public void ChangeBackbeat(BackBeatController next, BackBeatPlayMode mode = BackBeatPlayMode.Both)
    {
        ChangeBackbeat(_fadeTime, currentBackbeat, next, mode);
    }

    public void ChangeBackbeat(float fadeTime, BackBeatController prev, BackBeatController next, BackBeatPlayMode mode = BackBeatPlayMode.Both)
    {
        if (prev)
        {
            prev.Stop(mode);
            prev.gameObject.SetActive(false);
        }

        if (next)
        {
            next.gameObject.SetActive(true);
            next.Play(mode);
            currentBackbeat = next;
            
            if (_resultUI)
                _resultUI.SetDesc(currentBackbeat);
        }
        //StartCoroutine(Change(fadeTime, prev, next, mode));
    }

    IEnumerator Change(float fadeTime, BackBeatController prev, BackBeatController next, BackBeatPlayMode mode = BackBeatPlayMode.Both)
    {
        isChanging = true;
        KeyManager.instance.Disable("backbeat_ui");
        KeyManager.instance.Disable("backbeat_select");
        KeyManager.instance.Disable("backbeat_left");
        KeyManager.instance.Disable("backbeat_right");

        if (prev)
        {
            prev.StopFade(fadeTime, mode);

            yield return new WaitForSeconds(fadeTime);

            prev.gameObject.SetActive(false);
        }

        if (next)
        {
            next.gameObject.SetActive(true);
            next.PlayFade(fadeTime, mode);

            yield return new WaitForSeconds(fadeTime);

            currentBackbeat = next;

            if (_resultUI)
                _resultUI.SetDesc(currentBackbeat);
        }

        isChanging = false;
        KeyManager.instance.Enable("backbeat_ui");
        KeyManager.instance.Enable("backbeat_select");
        KeyManager.instance.Enable("backbeat_left");
        KeyManager.instance.Enable("backbeat_right");
    }

    public void Retry()
    {
        if (!currentBackbeat) return;
        if (!currentBackbeat.currentSet) return;

        currentBackbeat.currentSet.Retry();
    }
}
