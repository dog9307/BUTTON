using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBButtonManager : MonoBehaviour
{
    [SerializeField]
    private MBButtonColorController _okButton;
    [SerializeField]
    private MBButtonColorController _cancelButton;

    private MBButtonColorController _currentButton;

    [SerializeField]
    private GameObject _mb;

    // Start is called before the first frame update
    void OnEnable()
    {
        _currentButton = _okButton;
        _cancelButton.Deselect();
        _currentButton.Select();
    }

    // Update is called once per frame
    void Update()
    {
        if (KeyManager.instance.IsOnceKeyDown("mb_left") ||
            KeyManager.instance.IsOnceKeyDown("mb_right"))
        {
            _currentButton.Deselect();
            _currentButton = (_currentButton == _okButton ? _cancelButton : _okButton);
            _currentButton.Select();
        }

        if (KeyManager.instance.IsOnceKeyDown("mb_select"))
            _currentButton.PressButton();
    }

    public void MessageboxClose()
    {
        Invoke("Close", 1.0f / 3.0f);
    }

    void Close()
    {
        _mb.SetActive(false);
    }

    public void GoToStartScene()
    {
        ScreenFader.instance.StartSceneFader("StartScene");
    }
}
