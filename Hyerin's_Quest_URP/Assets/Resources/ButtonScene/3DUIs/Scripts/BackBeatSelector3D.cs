using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBeatSelector3D : MonoBehaviour
{
    [SerializeField]
    private BackBeatManager _manager;

    [SerializeField]
    private BackBeatButtonController[] _buttons;
    private int _currentSelectedIndex;
    public int currentSelectedIndex { get { return _currentSelectedIndex; } }
    public BackBeatButtonController currentButton { get { return _buttons[currentSelectedIndex]; } }

    [SerializeField]
    private Slider3D _volumeSlider;
    [SerializeField]
    private Slider3D _speedSlider;
    private Slider3D _prevSlider;
    private Slider3D _currentSlider;
    public Slider3D currentSlider
    {
        get { return _currentSlider; }
        set
        {
            if (value != _currentSlider)
            {
                if (_currentSlider)
                    _currentSlider.Deactivate();

                _currentSlider = value;

                if (_currentSlider)
                    _currentSlider.Activate();
            }
        }
    }


    private bool _isBackbeatMode = true;

    private void OnEnable()
    {
        if (!_manager.currentBackbeat)
        {
            _currentSelectedIndex = 0;
            _buttons[_currentSelectedIndex].Select(false, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_isBackbeatMode)
        {
            int prevIndex = _currentSelectedIndex;
            if (KeyManager.instance.IsOnceKeyDown("backbeat_left"))
                _currentSelectedIndex = (_currentSelectedIndex - 1 + _buttons.Length) % _buttons.Length;

            if (KeyManager.instance.IsOnceKeyDown("backbeat_right"))
                _currentSelectedIndex = (_currentSelectedIndex + 1) % _buttons.Length;

            if (KeyManager.instance.IsOnceKeyDown("backbeat_down"))
            {
                _isBackbeatMode = false;
                currentSlider = _volumeSlider;
                currentButton.Deselect();

                return;
            }
            if (KeyManager.instance.IsOnceKeyDown("backbeat_up"))
            {
                _isBackbeatMode = false;
                currentSlider = _speedSlider;
                currentButton.Deselect();

                return;
            }

            if (prevIndex != _currentSelectedIndex)
                _buttons[_currentSelectedIndex].Select();
        }
        else
        {
            if (KeyManager.instance.IsOnceKeyDown("backbeat_left"))
            {
                if (currentSlider == _volumeSlider)
                {
                    float value = _volumeSlider.value - 4.0f;

                    if (value < _volumeSlider.minValue)
                        value = _volumeSlider.minValue;

                    _volumeSlider.value = value;
                }
                else if (currentSlider == _speedSlider)
                {
                    float value = _speedSlider.value - 0.2f;

                    if (value < _speedSlider.minValue)
                        value = _speedSlider.minValue;

                    _speedSlider.value = value;
                }
            }

            if (KeyManager.instance.IsOnceKeyDown("backbeat_right"))
            {
                if (currentSlider == _volumeSlider)
                {
                    float value = _volumeSlider.value + 4.0f;

                    if (value > _volumeSlider.maxValue)
                        value = _volumeSlider.maxValue;

                    _volumeSlider.value = value;
                }
                else if (currentSlider == _speedSlider)
                {
                    float value = _speedSlider.value + 0.2f;

                    if (value > _speedSlider.maxValue)
                        value = _speedSlider.maxValue;

                    _speedSlider.value = value;
                }
            }

            if (KeyManager.instance.IsOnceKeyDown("backbeat_down"))
            {
                if (currentSlider == _volumeSlider)
                {
                    currentSlider = _speedSlider;
                }
                else if (currentSlider == _speedSlider)
                {
                    _isBackbeatMode = true;
                    currentSlider = null;
                    currentButton.Select();
                }
            }
            if (KeyManager.instance.IsOnceKeyDown("backbeat_up"))
            {
                if (currentSlider == _speedSlider)
                {
                    currentSlider = _volumeSlider;
                }
                else if (currentSlider == _volumeSlider)
                {
                    _isBackbeatMode = true;
                    currentSlider = null;
                    currentButton.Select();
                }
            }
        }

        if (KeyManager.instance.IsOnceKeyDown("exit"))
        {
            UIController3D ui = FindObjectOfType<UIController3D>();
            StartCoroutine(ui.ChangeUI(null, true));
        }
    }

    public void ChangeVideo(BackBeatButtonController button)
    {
        for (int i = 0; i < _buttons.Length; ++i)
        {
            if (_buttons[i] == button)
            {
                _currentSelectedIndex = i;
                continue;
            }

            _buttons[i].Deselect();
        }

        _manager.ChangeBackbeat(button.index, BackBeatPlayMode.VideoOnly);
    }

    public void PressButton()
    {
        _buttons[_currentSelectedIndex].PressButton();
    }
}
