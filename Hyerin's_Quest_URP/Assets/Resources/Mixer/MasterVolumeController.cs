using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MasterVolumeController : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;

    [SerializeField]
    private Text _text;

    [SerializeField]
    private AudioMixer _mixer;

    // Update is called once per frame
    void Update()
    {
        float currentValue = _slider.value;
        if (KeyManager.instance.IsOnceKeyDown("button_a"))
        {
            currentValue -= 10.0f;
            if (currentValue < _slider.minValue)
                currentValue = _slider.minValue;
        }

        if (KeyManager.instance.IsOnceKeyDown("button_d"))
        {
            currentValue += 10.0f;
            if (currentValue > _slider.maxValue)
                currentValue = _slider.maxValue;
        }

        _slider.value = currentValue;
        int percent = (int)((currentValue + _slider.minValue) / (_slider.maxValue - _slider.minValue) * 100.0f);
        percent += 200;
        _text.text = string.Format("VOLUME : {0}%", percent);

        ApplyMasterVolume(currentValue);
    }

    void ApplyMasterVolume(float volume)
    {
        _mixer.SetFloat("Master_Volume", volume);
    }
}
