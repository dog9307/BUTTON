using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UIController3D : MonoBehaviour
{
    [Header("BackBeat")]
    [SerializeField]
    private string _backBeatUIKey = "backbeat_ui";
    [SerializeField]
    private string _backBeatUISelectKey = "backbeat_ui_select";
    [SerializeField]
    private GameObject _backBeatUIPanel;
    [SerializeField]
    private AudioSource _backbeatBGM;

    private GameObject _currentUI;
    private bool _isOpen;

    [SerializeField]
    private BackBeatSelector3D _selector;

    void Start()
    {
        StartCoroutine(ChangeUI(_backBeatUIPanel));
    }

    void Update()
    {
        if (KeyManager.instance.IsOnceKeyDown(_backBeatUIKey))
        {
            if (_uiCam.Priority != 10)
            {
                StartCoroutine(ChangeUI(_backBeatUIPanel));

                FindObjectOfType<BackBeatManager>().SoundOff();
            }
        }
        if (KeyManager.instance.IsOnceKeyDown(_backBeatUISelectKey))
        {
            if (_currentUI == _backBeatUIPanel)
            {
                if (_selector)
                    _selector.PressButton();

                StartCoroutine(ChangeUI(null));
                BackBeatManager manager = FindObjectOfType<BackBeatManager>();
                manager.Retry();
            }
        }
    }

    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private CinemachineVirtualCamera _backBeatCam;
    [SerializeField]
    private CinemachineVirtualCamera _uiCam;

    public IEnumerator ChangeUI(GameObject next, bool isQuit = false)
    {
        if (next == null)
        {
            _isOpen = false;

            if (!isQuit)
                yield return new WaitForSeconds(1.0f);
        }
        else
            _isOpen = true;
        _anim.SetBool("isOpen", _isOpen);

        if (_currentUI != next)
        {
            AudioSource audio = null;
            float currentVolume = 0.0f;
            if (next == _backBeatUIPanel)
            {
                audio = _backbeatBGM;
                audio.Play();
                audio.volume = currentVolume;

                BackBeatBGMOn();

                if (!isQuit)
                {
                    BackBeatManager manager = FindObjectOfType<BackBeatManager>();
                    manager.SoundOff(1.0f);
                }
            }
            else
            {
                BackBeatBGMOff();

                if (!isQuit)
                {
                    BackBeatManager manager = FindObjectOfType<BackBeatManager>();
                    manager.SoundOn(1.0f);
                }
            }

            if (audio == null)
                yield return new WaitForSeconds(1.0f);

            if (isQuit)
                Quit();
            else
            {
                _currentUI = next;
            }
        }

        if (_currentUI == null)
        {
            KeyManager.instance.Enable(_backBeatUIKey);
            KeyManager.instance.Disable(_backBeatUISelectKey);

            KeyManager.instance.Enable("button_1");
            KeyManager.instance.Enable("button_2");
            KeyManager.instance.Enable("button_3");
            KeyManager.instance.Enable("button_4");

            KeyManager.instance.Disable("bubble_one");
            KeyManager.instance.Disable("bubble_two");
            KeyManager.instance.Disable("bubble_three");
            KeyManager.instance.Disable("bubble_four");

            KeyManager.instance.Disable("backbeat_left");
            KeyManager.instance.Disable("backbeat_right");

            _backBeatCam.Priority = 10;
            _uiCam.Priority = 1;
        }
        else if (_currentUI == _backBeatUIPanel)
        {
            KeyManager.instance.Disable(_backBeatUIKey);
            KeyManager.instance.Enable(_backBeatUISelectKey);

            KeyManager.instance.Disable("button_1");
            KeyManager.instance.Disable("button_2");
            KeyManager.instance.Disable("button_3");
            KeyManager.instance.Disable("button_4");

            KeyManager.instance.Enable("bubble_one");
            KeyManager.instance.Enable("bubble_two");
            KeyManager.instance.Enable("bubble_three");
            KeyManager.instance.Enable("bubble_four");

            KeyManager.instance.Enable("backbeat_left");
            KeyManager.instance.Enable("backbeat_right");

            _backBeatCam.Priority = 1;
            _uiCam.Priority = 10;
        }
    }

    public void BackBeatBGMOn()
    {
        StartCoroutine(BGMOn());
    }

    IEnumerator BGMOn()
    {
        float currentVolume = 0.0f;
        while (currentVolume < 1.0f)
        {
            currentVolume += Time.deltaTime;
            if (currentVolume > 1.0f)
                currentVolume = 1.0f;

            _backbeatBGM.volume = currentVolume;
            yield return null;
        }
    }

    public void BackBeatBGMOff()
    {
        StartCoroutine(BGMOff());
    }

    IEnumerator BGMOff()
    {
        float currentVolume = 1.0f;
        while (currentVolume > 0.0f)
        {
            currentVolume -= Time.deltaTime;
            if (currentVolume < 0.0f)
                currentVolume = 0.0f;

            _backbeatBGM.volume = currentVolume;
            yield return null;
        }
    }

    public void Quit()
    {
        BackBeatBGMOff();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
