using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderButtonColorChanger : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _renderer;
    private Material _mat;

    private int _eventCount = 0;
    [SerializeField]
    private float _selectIntensity = 3.5f;
    [SerializeField]
    private float _deselectIntensity = -1.4f;
    private float _changeTime = 0.3f;
    private float _currentIntensity;

    private bool _isActivate;

    private Color _color;
    [SerializeField]
    private float _currentH = 0.0f;
    [SerializeField]
    private float _currentS = 0.0f;
    [SerializeField]
    private float _currentV = 0.0f;

    [SerializeField]
    private bool _isChangingColor = true;
    [SerializeField]
    private float _frequency = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        _mat = _renderer.material;

        Deactivate();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isActivate) return;
        if (!_isChangingColor) return;

        _currentH += Time.deltaTime * _frequency * 360.0f;
        if (_currentH > 360.0f)
            _currentH -= 360.0f;

        float h = _currentH / 360.0f;
        float s = _currentS / 100.0f;
        float v = _currentV / 100.0f;
        _color = Color.HSVToRGB(h, s, v, true) * _currentIntensity;

        _mat.SetColor("_EmissionColor", _color);
    }

    public void Activate()
    {
        _isActivate = true;
        StartCoroutine(ChangeIntensity(_selectIntensity));
    }

    public void Deactivate()
    {
        _isActivate = false;
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

            float h = _currentH / 360.0f;
            float s = _currentS / 100.0f;
            float v = _currentV / 100.0f;
            _color = Color.HSVToRGB(h, s, v, true) * _currentIntensity;

            _mat.SetColor("_EmissionColor", _color);

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
}
