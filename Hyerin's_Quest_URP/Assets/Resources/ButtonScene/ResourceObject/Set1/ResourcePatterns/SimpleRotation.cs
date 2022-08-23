using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : ResourcePatternBase
{
    private Vector3 _originScale;

    private float _angleSpeed = 1.0f;
    private float _angleAccel = 0.05f;
    private float _currentAngle = 0.0f;

    private float _scaleFactor = 1.0f;
    private float _scaleMax = 1.0f;
    private float _scaleMin = 0.0f;
    
    public override bool Init(ResourceObjectPlayer current)
    {
        if (!base.Init(current)) return false;

        _originScale = renderers[4].transform.localScale;

        _currentAngle = 0.0f;
        _angleSpeed = 0.0f;
        _scaleFactor = 1.0f;

        return true;
    }

    public override void Update()
    {
        _currentAngle += _angleSpeed;
        if (_currentAngle >= 360.0f)
            _currentAngle -= 360.0f;

        _angleSpeed += _angleAccel;

        renderers[4].transform.rotation = Quaternion.identity;
        renderers[4].transform.Rotate(new Vector3(0.0f, 0.0f, _currentAngle));

        float ratio = _angleSpeed / 100.0f;
        if (ratio > 1.0f)
            ratio = 1.0f;

        _scaleFactor = Mathf.Lerp(_scaleMax, _scaleMin, ratio);

        Vector3 newScale = new Vector3(_originScale.x * _scaleFactor, _originScale.y * _scaleFactor, 1.0f);
        renderers[4].transform.localScale = newScale;
    }

    public override void Release()
    {
        SpriteRenderer origin = renderers[4];
        origin.transform.rotation = Quaternion.identity;
        origin.transform.localScale = _originScale;

        base.Release();
    }
}
