using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Slider3D : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    private Transform _top;
    [HideInInspector]
    [SerializeField]
    private Transform _bottom;
    [HideInInspector]
    [SerializeField]
    private Transform _button;

    [SerializeField]
    private float _min = 0.0f;
    public float minValue { get { return _min; } }
    [SerializeField]
    private float _max = 1.0f;
    public float maxValue { get { return _max; } }

    [SerializeField]
    private float _value;
    public float value { get { return _value; } set { this._value = value; } }
    private float _prevValue;

    private float _startScaleFactor;

    public UnityEvent OnActivated;
    public UnityEvent OnDeactivated;
    public UnityEvent OnValueChanged;

    // Start is called before the first frame update
    void Start()
    {
        _prevValue = _value;
        _startScaleFactor = _top.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        value = Mathf.Clamp(value, _min, _max);
        float length = _max - _min;

        float current = 1.0f;
        if (length != 0.0f)
            current = (value - _min) / (_max - _min);

        Vector3 topScale = _top.localScale;
        topScale.x = current * _startScaleFactor;
        _top.localScale = topScale;

        Vector2 buttonPos = _button.localPosition;
        buttonPos.x = _top.localScale.x;
        _button.localPosition = buttonPos;
    }

    void LateUpdate()
    {
        if (_prevValue != _value)
        {
            if (OnValueChanged != null)
                OnValueChanged.Invoke();
        }

        _prevValue = _value;
    }

    public void Activate()
    {
        if (OnActivated != null)
            OnActivated.Invoke();
    }

    public void Deactivate()
    {
        if (OnDeactivated != null)
            OnDeactivated.Invoke();
    }
}
