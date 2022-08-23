using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HSVControlType
{
    NONE = -1,
    H,
    S,
    V,
    Intensity,
    END
}


public class HSVController : ResourceObjectPlayerBase
{
    [SerializeField]
    private HSVControlType _type;

    [SerializeField]
    private HSVCube _cube;
    [SerializeField]
    private float _totalHSVTime = 4.0f;
    private float _currentHSVTime = 0.0f;

    [SerializeField]
    private int _min;
    [SerializeField]
    private int _max;

    private bool _isUp = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _currentHSVTime = _totalHSVTime - 0.1f;

        float ratio = _currentHSVTime / _totalHSVTime;
        SetHSV(ratio);

        _isUp = true;
    }

    protected override void Update()
    {
        base.Update();

        if (isPlaying)
        {
            if (_type == HSVControlType.H)
            {
                _currentHSVTime += Time.deltaTime;
                if (_currentHSVTime > _totalHSVTime)
                    _currentHSVTime = 0.0f;
            }
            else
            {
                if (_isUp)
                {
                    _currentHSVTime += Time.deltaTime;
                    if (_currentHSVTime > _totalHSVTime)
                    {
                        _isUp = false;
                        _currentHSVTime = _totalHSVTime;
                    }
                }
                else
                {
                    _currentHSVTime -= Time.deltaTime;
                    if (_currentHSVTime < 0.0f)
                    {
                        _isUp = true;
                        _currentHSVTime = 0.0f;
                    }
                }
            }

            float ratio = _currentHSVTime / _totalHSVTime;
            SetHSV(ratio);
        }
    }

    void SetHSV(float ratio)
    {
        float figure = Mathf.Lerp(_min, _max, ratio);
        _cube.SetHSV(figure, _type);
    }
}
