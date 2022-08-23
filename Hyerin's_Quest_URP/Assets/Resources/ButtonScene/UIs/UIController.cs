using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("BackBeat")]
    [SerializeField]
    private string _backBeatUIKey;
    [SerializeField]
    private string _backBeatUISelectKey;
    [SerializeField]
    private GameObject _backBeatUIPanel;
    [SerializeField]
    private AudioSource _backbeatBGM;

    [SerializeField]
    private Animator[] _backbeatUIs;

    [Header("Result")]
    [SerializeField]
    private string _resultUIKey;
    [SerializeField]
    private string _resultUIOutKey;
    [SerializeField]
    private GameObject _resultUIPanel;

    [SerializeField]
    private Animator[] _resultUIs;

    private GameObject _currentUI;

    void Start()
    {
        StartCoroutine(ChangeUI(_backBeatUIPanel));
        _resultUIPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (KeyManager.instance.IsOnceKeyDown(_backBeatUIKey))
        //{
        //    if (!_backBeatUIPanel.activeSelf)
        //        StartCoroutine(ChangeUI(_backBeatUIPanel));
        //    else
        //        StartCoroutine(ChangeUI(null));
        //}

        //if (KeyManager.instance.IsOnceKeyDown(_resultUIKey))
        //{
        //    if (!_resultUIPanel.activeSelf)
        //        StartCoroutine(ChangeUI(_resultUIPanel));
        //    else
        //        StartCoroutine(ChangeUI(null));
        //}

        //if (_currentUI == _backBeatUIPanel &&
        //    _backBeatUIPanel.activeSelf)
        //{
        //    if (KeyManager.instance.IsOnceKeyDown(_backBeatUISelectKey))
        //        StartCoroutine(ChangeUI(null));
        //}

        if (KeyManager.instance.IsOnceKeyDown(_backBeatUIKey))
        {
            if (!_backBeatUIPanel.activeSelf)
                StartCoroutine(ChangeUI(_backBeatUIPanel));
        }
        if (KeyManager.instance.IsOnceKeyDown(_backBeatUISelectKey))
        {
            if (_currentUI == _backBeatUIPanel)
            {
                StartCoroutine(ChangeUI(null));
                BackBeatManager manager = FindObjectOfType<BackBeatManager>();
                manager.Retry();
            }
        }

        if (KeyManager.instance.IsOnceKeyDown(_resultUIKey))
        {
            if (!_resultUIPanel.activeSelf)
                StartCoroutine(ChangeUI(_resultUIPanel));
        }
        if (KeyManager.instance.IsOnceKeyDown(_resultUIOutKey))
        {
            if (_currentUI == _resultUIPanel)
                StartCoroutine(ChangeUI(null));
        }
    }

    public IEnumerator ChangeUI(GameObject next, bool isQuit = false)
    {
        GameObject prev = _currentUI;
        if (!_currentUI)
        {
            _currentUI = next;
            _currentUI.SetActive(true);
        }
        else
        {
            if (_currentUI != next)
            {
                Animator[] currentAnim = null;
                AudioSource audio = null;
                float currentVolume = 0.0f;
                if (_currentUI == _backBeatUIPanel)
                {
                    currentAnim = _backbeatUIs;
                    audio = _backbeatBGM;
                    audio.Play();
                    audio.volume = currentVolume;
                }
                else if (_currentUI == _resultUIPanel)
                    currentAnim = _resultUIs;


                if (currentAnim != null)
                {
                    for (int i = 0; i < currentAnim.Length; ++i)
                    {
                        BackBeatSelectIcon icon = currentAnim[i].GetComponent<BackBeatSelectIcon>();
                        if (icon)
                        {
                            if (icon.isSelected)
                            {
                                Animator root = icon.transform.parent.GetComponent<Animator>();
                                if (root)
                                    root.SetTrigger("disappear");
                            }
                        }
                        else
                            currentAnim[i].SetTrigger("disappear");
                    }
                }

                if (!isQuit)
                {
                    BackBeatManager manager = FindObjectOfType<BackBeatManager>();
                    manager.SoundOn(1.0f);
                }

                if (audio != null)
                {
                    while (currentVolume < 1.0f)
                    {
                        currentVolume += Time.deltaTime;
                        audio.volume = currentVolume;
                        yield return null;
                    }
                }
                else
                    yield return new WaitForSeconds(1.0f);

                if (isQuit)
                    Quit();
                else
                {
                    _currentUI.SetActive(false);
                    if (next)
                        next.SetActive(true);

                    _currentUI = next;
                }
            }
        }

        if (_currentUI == null)
        {
            KeyManager.instance.Enable(_backBeatUIKey);
            KeyManager.instance.Enable(_resultUIKey);
            KeyManager.instance.Disable(_backBeatUISelectKey);
            KeyManager.instance.Disable(_resultUIOutKey);

            KeyManager.instance.Enable("button_1");
            KeyManager.instance.Enable("button_2");
            KeyManager.instance.Enable("button_3");
            KeyManager.instance.Enable("button_4");

            if (prev)
            {
                if (prev == _backBeatUIPanel)
                {
                    BackBeatSelector selector = _backBeatUIPanel.GetComponentInChildren<BackBeatSelector>();
                    if (selector.currentSelectedIndex == 1)
                        KeyManager.instance.Disable(_resultUIKey);
                }
            }
        }
        else if (_currentUI == _backBeatUIPanel)
        {
            KeyManager.instance.Disable(_backBeatUIKey);
            KeyManager.instance.Disable(_resultUIKey);
            KeyManager.instance.Enable(_backBeatUISelectKey);
            KeyManager.instance.Disable(_resultUIOutKey);

            KeyManager.instance.Disable("button_1");
            KeyManager.instance.Disable("button_2");
            KeyManager.instance.Disable("button_3");
            KeyManager.instance.Disable("button_4");
        }
        else if (_currentUI == _resultUIPanel)
        {
            KeyManager.instance.Disable(_backBeatUIKey);
            KeyManager.instance.Disable(_resultUIKey);
            KeyManager.instance.Disable(_backBeatUISelectKey);
            KeyManager.instance.Enable(_resultUIOutKey);

            KeyManager.instance.Disable("button_1");
            KeyManager.instance.Disable("button_2");
            KeyManager.instance.Disable("button_3");
            KeyManager.instance.Disable("button_4");
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
