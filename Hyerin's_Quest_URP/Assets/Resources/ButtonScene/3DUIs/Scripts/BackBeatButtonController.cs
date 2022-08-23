using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackBeatButtonController : MonoBehaviour
{
    [SerializeField]
    private BackBeatSelector3D _selector;
    [SerializeField]
    private int _index = 0;
    public int index { get { return _index; } }

    private bool _isSelected;
    public bool isSelected { get { return _isSelected; } }

    [SerializeField]
    private Material _mat;
    [SerializeField]
    private float _selectIntensity = 3.5f;
    [SerializeField]
    private float _deselectIntensity = -1.4f;
    private float _changeTime = 0.3f;
    private float _currentIntensity;

    [SerializeField]
    private Color _color;

    private int _eventCount = 0;

    public void Select(bool isSound = true, bool isArrowMove = true, bool isIgnoreSelected = false)
    {
        if (BackBeatManager.isChanging) return;
        if (!isIgnoreSelected)
        {
            if (_isSelected)
                return;
        }

        _isSelected = true;
        _selector.ChangeVideo(this);

        if (isSound)
            ButtonSFXManager.instance.PlayNext();

        StartCoroutine(ChangeIntensity(_selectIntensity));
    }

    public void Deselect()
    {
        _isSelected = false;
        StartCoroutine(ChangeIntensity(_deselectIntensity));
    }

    IEnumerator ChangeIntensity(float toIntensity)
    {
        _eventCount++;
        float currentTime = 0.0f;
        float startIntensity = _currentIntensity;
        while (currentTime < _changeTime)
        {
            currentTime += Time.deltaTime;
            float ratio = currentTime / _changeTime;
            _currentIntensity = Mathf.Lerp(startIntensity, toIntensity, ratio);

            _mat.SetColor("_EmissionColor", _color * _currentIntensity);

            yield return null;

            if (_eventCount > 1)
            {
                _eventCount--;
                yield break;
            }
        }
        _currentIntensity = Mathf.Lerp(startIntensity, toIntensity, 1.0f);
        _eventCount--;
    }

    public void PressButton()
    {
        Animator anim = GetComponent<Animator>();
        if (anim)
            anim.SetTrigger("press");
    }
}
