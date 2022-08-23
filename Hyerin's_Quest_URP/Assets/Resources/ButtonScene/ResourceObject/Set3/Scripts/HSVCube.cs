using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSVCube : MonoBehaviour
{
    [SerializeField]
    private int _startH;
    [SerializeField]
    private int _startS;
    [SerializeField]
    private int _startV;
    [SerializeField]
    private float _startIntensity;

    private int _currentH;
    private int _currentS;
    private int _currentV;
    private float _currentIntensity;

    [SerializeField]
    private Material _mat;

    private Color _currentColor;

    private Theme4RewardHelper _theme4;

    [SerializeField]
    private float _moveTime = 3.0f;
    [SerializeField]
    private float _moveStartZ = 10.0f;

    public void MoveStart()
    {
        StartCoroutine(Moving());
    }

    IEnumerator Moving()
    {
        float currentTime = 0.0f;
        Vector3 newPos = Vector3.zero;
        while (currentTime < _moveTime)
        {
            float ratio = currentTime / _moveTime;
            float z = Mathf.Lerp(_moveStartZ, 0.0f, ratio);

            newPos = transform.position;
            newPos.z = z;
            transform.position = newPos;

            yield return null;

            currentTime += Time.deltaTime;
        }
        newPos = transform.position;
        newPos.z = 0.0f;
        transform.position = newPos;
    }

    void Update()
    {
        if (!_mat)
            _mat = GetComponent<Renderer>().material;

        float h = (float)_currentH / 360.0f;
        float s = (float)_currentS / 100.0f;
        float v = (float)_currentV / 100.0f;
        _currentColor = Color.HSVToRGB(h, s, v, true) * _currentIntensity;

        _mat.SetColor("_EmissionColor", _currentColor);

        if (!_theme4)
            _theme4 = FindObjectOfType<Theme4RewardHelper>();

        if (_theme4)
            _theme4.SetHSV(_currentH, _currentS, _currentV, _currentIntensity);
    }

    public void SetHSV(float figure, HSVControlType type)
    {
        switch (type)
        {
            case HSVControlType.H:
                _currentH = (int)figure;
            break;

            case HSVControlType.S:
                _currentV = (int)figure;
            break;

            case HSVControlType.V:
                _currentS = (int)figure;
            break;

            case HSVControlType.Intensity:
                _currentIntensity = figure;
            break;
        }
    }
}