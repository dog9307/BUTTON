using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackBeatSelector : MonoBehaviour
{
    [SerializeField]
    private BackBeatManager _manager;

    [SerializeField]
    private BackBeatSelectIcon[] _icons;
    private int _currentSelectedIndex;
    public int currentSelectedIndex { get { return _currentSelectedIndex; } }

    [SerializeField]
    private RectTransform _volumeTarget;
    [SerializeField]
    private RectTransform _speedTarget;
    [SerializeField]
    private SelectArrowMovement _arrow;
    private RectTransform _currentTarget;

    [SerializeField]
    private Slider _volumeSlider;
    [SerializeField]
    private Slider _speedSlider;
    private Slider _prevSlider;

    private bool _isBackbeatMode = true;

    private void OnEnable()
    {
        if (!_manager.currentBackbeat)
        {
            _currentSelectedIndex = 0;
            _icons[_currentSelectedIndex].Select(false, false);
        }

        _manager.SoundOff();
    }

    void Update()
    {
        if (_isBackbeatMode)
        {
            int prevIndex = _currentSelectedIndex;
            if (KeyManager.instance.IsOnceKeyDown("backbeat_left"))
                _currentSelectedIndex = (_currentSelectedIndex - 1 + _icons.Length) % _icons.Length;

            if (KeyManager.instance.IsOnceKeyDown("backbeat_right"))
                _currentSelectedIndex = (_currentSelectedIndex + 1) % _icons.Length;

            if (KeyManager.instance.IsOnceKeyDown("backbeat_down"))
            {
                _isBackbeatMode = false;
                _currentTarget = _volumeTarget;
                StartCoroutine(SliderTargetChange(_volumeSlider));
                StartCoroutine(ArrowDisappear());
                return;
            }
            if (KeyManager.instance.IsOnceKeyDown("backbeat_up"))
            {
                _isBackbeatMode = false;
                _currentTarget = _speedTarget;
                StartCoroutine(SliderTargetChange(_speedSlider));
                StartCoroutine(ArrowDisappear());
                return;
            }

            if (prevIndex != _currentSelectedIndex)
                _icons[_currentSelectedIndex].Select();
        }
        else
        {
            if (KeyManager.instance.IsOnceKeyDown("backbeat_left"))
            {
                if (_currentTarget == _volumeTarget)
                {
                    float value = _volumeSlider.value - 4.0f;

                    if (value < _volumeSlider.minValue)
                        value = _volumeSlider.minValue;

                    _volumeSlider.value = value;
                }
                else if (_currentTarget == _speedTarget)
                {
                    float value = _speedSlider.value - 0.2f;

                    if (value < _speedSlider.minValue)
                        value = _speedSlider.minValue;

                    _speedSlider.value = value;
                }
            }
            
            if (KeyManager.instance.IsOnceKeyDown("backbeat_right"))
            {
                if (_currentTarget == _volumeTarget)
                {
                    float value = _volumeSlider.value + 4.0f;

                    if (value > _volumeSlider.maxValue)
                        value = _volumeSlider.maxValue;

                    _volumeSlider.value = value;
                }
                else if (_currentTarget == _speedTarget)
                {
                    float value = _speedSlider.value + 0.2f;

                    if (value > _speedSlider.maxValue)
                        value = _speedSlider.maxValue;

                    _speedSlider.value = value;
                }
            }

            if (KeyManager.instance.IsOnceKeyDown("backbeat_down"))
            {
                if (_currentTarget == _volumeTarget)
                {
                    _currentTarget = _speedTarget;
                    StartCoroutine(SliderTargetChange(_speedSlider));
                }
                else if (_currentTarget == _speedTarget)
                {
                    _isBackbeatMode = true;
                    StartCoroutine(ArrowAppear());
                    StartCoroutine(SliderTargetChange(null));
                }

            }
            if (KeyManager.instance.IsOnceKeyDown("backbeat_up"))
            {
                if (_currentTarget == _speedTarget)
                {
                    _currentTarget = _volumeTarget;
                    StartCoroutine(SliderTargetChange(_volumeSlider));
                }
                else if (_currentTarget == _volumeTarget)
                {
                    _isBackbeatMode = true;
                    StartCoroutine(ArrowAppear());
                    StartCoroutine(SliderTargetChange(null));
                }
            }
        }

        if (KeyManager.instance.IsOnceKeyDown("exit"))
        {
            UIController ui = FindObjectOfType<UIController>();
            StartCoroutine(ui.ChangeUI(null, true));
        }
    }

    IEnumerator ArrowDisappear()
    {
        _arrow.GetComponent<Animator>().enabled = false;

        Image arrow = _arrow.GetComponentsInChildren<Image>()[0];
        float totalTime = 0.3f;
        float currentTime = 0.0f;
        while (currentTime < totalTime)
        {
            currentTime += Time.deltaTime;
            float ratio = currentTime / totalTime;
            if (ratio > 1.0f)
                ratio = 1.0f;

            Color newColor = arrow.color;
            newColor.a = Mathf.Lerp(1.0f, 0.0f, ratio);
            arrow.color = newColor;

            yield return null;
        }
    }

    IEnumerator ArrowAppear()
    {
        Image arrow = _arrow.GetComponentsInChildren<Image>()[0];
        float totalTime = 0.3f;
        float currentTime = 0.0f;
        while (currentTime < totalTime)
        {
            currentTime += Time.deltaTime;
            float ratio = currentTime / totalTime;
            if (ratio > 1.0f)
                ratio = 1.0f;

            Color newColor = arrow.color;
            newColor.a = Mathf.Lerp(0.0f, 1.0f, ratio);
            arrow.color = newColor;

            yield return null;
        }

        _arrow.GetComponent<Animator>().enabled = true;
    }

    IEnumerator SliderTargetChange(Slider target)
    {
        ColorBlock volumeColor = _volumeSlider.colors;
        ColorBlock speedColor = _speedSlider.colors;

        float totalTime = 0.3f;
        float currentTime = 0.0f;
        while (currentTime < totalTime)
        {
            currentTime += Time.deltaTime;
            float ratio = currentTime / totalTime;
            if (ratio > 1.0f)
                ratio = 1.0f;

            float color = Mathf.Lerp(0.0f, 1.0f, ratio);
            if (target == _volumeSlider)
                volumeColor.normalColor = new Color(1.0f - color, 1.0f - color, 1.0f - color, 1.0f);
            else
            {
                if (_prevSlider == _volumeSlider)
                    volumeColor.normalColor = new Color(color, color, color, 1.0f);
            }

            if (target == _speedSlider)
                speedColor.normalColor = new Color(1.0f - color, 1.0f - color, 1.0f - color, 1.0f);
            else
            {
                if (_prevSlider == _speedSlider)
                    speedColor.normalColor = new Color(color, color, color, 1.0f);
            }

            _volumeSlider.colors = volumeColor;
            _speedSlider.colors = speedColor;

            yield return null;
        }

        _prevSlider = target;
    }

    public void ChangeVideo(BackBeatSelectIcon icon)
    {
        for (int i = 0; i < _icons.Length; ++i)
        {
            if (_icons[i] == icon)
            {
                _currentSelectedIndex = i;
                continue;
            }

            _icons[i].Deselect();
        }

        _manager.ChangeBackbeat(icon.index, BackBeatPlayMode.VideoOnly);
    }
}
