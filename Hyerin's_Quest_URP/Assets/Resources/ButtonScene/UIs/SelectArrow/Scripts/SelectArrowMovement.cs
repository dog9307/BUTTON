using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectArrowMovement : MonoBehaviour
{
    [SerializeField]
    private float _bounceOffsetX = 0.0f;

    private RectTransform _rc;
    private RectTransform _target;
    private RectTransform _prevTarget;

    private Vector3 _start;
    private Vector3 _end;
    private Vector3 _via;
    private bool _isMove;
    private float _currentTime;

    [SerializeField]
    private float _totalTime = 0.1f;

    private bool _isBackBeatTarget;

    [SerializeField]
    private RectTransform _posY;

    private bool _isIgnore = false;

    void Start()
    {
        if (!_rc)
            _rc = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (_prevTarget == _target)
        {
            if (_isBackBeatTarget)
            {
                Vector3 pos = _target.position;
                pos.y = _posY.position.y;
                _rc.position = pos;
            }
        }
        else
        {
            if (_isMove)
            {
                _currentTime += Time.deltaTime;
                if (_currentTime > _totalTime)
                {
                    _isMove = false;
                    _currentTime = _totalTime;
                    _prevTarget = _target;
                    _isIgnore = false;
                }

                float ratio = _currentTime / _totalTime;
                Vector3 newPos = _start * (1 - ratio) * (1 - ratio) + 2 * (1 - ratio) * ratio * _via + _end * ratio * ratio;
                if (_isBackBeatTarget && !_isIgnore)
                    newPos.y = _posY.position.y;
                _rc.position = newPos;
            }
            else
                _prevTarget = _target;
        }
    }

    public void MoveStart(RectTransform target, bool isMove = true, bool isBackBeat = true)
    {
        if (!_rc)
            _rc = GetComponent<RectTransform>();

        if (_isBackBeatTarget && !isBackBeat)
            StartCoroutine(ChangeRotate(90.0f));
        else if (!_isBackBeatTarget && isBackBeat)
        {
            _isIgnore = true;
            StartCoroutine(ChangeRotate(0.0f));
        }

        _isBackBeatTarget = isBackBeat;

        _target = target;

        _start = _rc.position;
        _end = target.position;
        _via = target.position;

        if (_isIgnore)
        {
            _end.y = _posY.position.y;
            _via.y = _posY.position.y;
        }
        
        if (_isBackBeatTarget)
        {
            if (_start.x < _end.x)
                _via.x += _bounceOffsetX;
            else
                _via.x -= _bounceOffsetX;
        }
        else
        {
            if (_start.y < _end.y)
                _via.y += _bounceOffsetX;
            else
                _via.y -= _bounceOffsetX;
        }

        _currentTime = 0.0f;
        _isMove = isMove;
    }

    IEnumerator ChangeRotate(float targetRotate)
    {
        float currentTime = 0.0f;
        while (currentTime < _totalTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime > _totalTime)
                currentTime = _totalTime;

            float start = _rc.rotation.eulerAngles.z;
            float end = targetRotate;
            float ratio = currentTime / _totalTime;
            float angle = Mathf.Lerp(start, end, ratio);

            _rc.rotation = Quaternion.identity;
            _rc.Rotate(new Vector3(0.0f, 0.0f, angle));

            yield return null;
        }
    }
}
