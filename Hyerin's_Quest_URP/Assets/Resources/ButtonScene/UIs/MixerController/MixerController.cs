using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MixerController : MonoBehaviour
{
    [SerializeField]
    private string _mixerField;

    [SerializeField]
    private Slider _slider;

    [SerializeField]
    private Text _text;
    [SerializeField]
    private string _header = "";
    [SerializeField]
    private string _tail = "";
    [SerializeField]
    private bool _isPercetage;
    [SerializeField]
    private bool _isSpeed = false;

    [SerializeField]
    private AudioMixer _mixer;

    void Start()
    {
        ApplyValue();
    }

    public void ApplyValue()
    {
        _mixer.SetFloat(_mixerField, _slider.value);
        if (_isSpeed)
            _mixer.SetFloat("Back_Ptich_Shifter", 1.0f / _slider.value);
        
        float value = _slider.value;
        if (_isPercetage)
            value = (int)((value + _slider.minValue) / (_slider.maxValue - _slider.minValue) * 100.0f) + 200.0f;

        if (_isSpeed)
            _text.text = string.Format("SPEED : x {0:0.0}", value);
        else
            _text.text = string.Format("VOLUME : {0}%", (int)value);
    }
}
