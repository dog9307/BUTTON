using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFXManager : MonoBehaviour
{
    #region SINGLETON
    static private ButtonSFXManager _instance;
    static public ButtonSFXManager instance { get { return _instance; } }

    private void Awake()
    {
        if (!_instance)
        {
            _instance = FindObjectOfType<ButtonSFXManager>();
            if (!_instance)
            {
                GameObject container = new GameObject();
                container.name = "ButtonSFXManager";
                _instance = container.AddComponent<ButtonSFXManager>();
            }
        }

        DontDestroyOnLoad(ButtonSFXManager.instance);
    }
    #endregion

    [SerializeField]
    private List<AudioSource> _buttonSfxes;
    private int _currentIndex;

    public int sfxCount { get { if (_buttonSfxes != null) return _buttonSfxes.Count; return 0; } }

    // Start is called before the first frame update
    void Start()
    {
        if (instance)
        {
            if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }

        ResetIndex();
    }

    public void PlayNext()
    {
        _buttonSfxes[_currentIndex].Play();
        _currentIndex = (_currentIndex + 1) % _buttonSfxes.Count;
    }

    public void PlayIndex(int index)
    {
        _buttonSfxes[index].Play();
    }

    public void ResetIndex()
    {
        _currentIndex = 0;
    }
}
