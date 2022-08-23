using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theme4RewardController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _cards;

    [SerializeField]
    private Material _screenshot;

    [SerializeField]
    private Material _cubeMat;

    private int _currentH;
    private int _currentS;
    private int _currentV;
    private float _currentIntensity;

    private Color _currentColor;

    void Update()
    {
        float h = (float)_currentH / 360.0f;
        float s = (float)_currentS / 100.0f;
        float v = (float)_currentV / 100.0f;
        _currentColor = Color.HSVToRGB(h, s, v, true) * _currentIntensity;

        _cubeMat.SetColor("_EmissionColor", _currentColor);
    }

    public void SetMainTex(Texture tex)
    {
        _screenshot.SetTexture("_BaseMap", tex);
    }

    public void SetHSV(int h, int s, int v, float intensity)
    {
        _currentH = h;
        _currentS = s;
        _currentV = v;
        _currentIntensity = intensity;
    }

    public void CardOn(int index)
    {
        for (int i = 0; i < _cards.Length; ++i)
        {
            if (i == index)
                _cards[i].SetActive(true);
            else
                _cards[i].SetActive(false);
        }
    }
}
