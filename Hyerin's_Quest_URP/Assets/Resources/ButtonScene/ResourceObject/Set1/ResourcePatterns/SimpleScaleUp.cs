using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleScaleUp : ResourcePatternBase
{
    private Vector3 _originScale;

    private float _startFactor = 0.0f;
    private float _endFactor = 10.0f;

    private float _totalTime = 10.0f;
    private float _currentTime = 0.0f;

    public override bool Init(ResourceObjectPlayer current)
    {
        if (!base.Init(current)) return false;
        _originScale = renderers[4].transform.localScale;

        _currentTime = 0.0f;

        opacity = 1.0f;

        return true;
    }

    public override void Update()
    {
        SpriteRenderer origin = renderers[4];

        _currentTime += Time.deltaTime;
        if (_currentTime > _totalTime)
            _currentTime = 0.0f;

        float ratio = _currentTime / _totalTime;
        float factor = Mathf.Lerp(_startFactor, _endFactor, ratio);

        Vector3 newScale = new Vector3(_originScale.x * factor, _originScale.y * factor, 1.0f);
        origin.transform.localScale = newScale;

        ratio *= 2.0f;
        if (ratio > 1.0f)
            ratio = 1.0f;
        opacity = Mathf.Lerp(1.0f, 0.0f, ratio);
    }

    public override void Release()
    {
        SpriteRenderer origin = renderers[4];
        origin.transform.localScale = _originScale;

        base.Release();
    }
}
