using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleGravityHelper : MonoBehaviour
{
    [SerializeField]
    private Transform _gravityPoint;

    [SerializeField]
    private float _angleSpeed = 10.0f;
    private float _currentAngle;

    public Vector3 dir
    {
        get
        {
            if (!_gravityPoint)
                return -transform.up;

            Vector3 dir = (_gravityPoint.position - transform.position);
            return dir.normalized;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentAngle = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gravityPoint) return;

        _currentAngle += _angleSpeed * Time.deltaTime;
        if (_currentAngle >= 360.0f)
            _currentAngle -= 360.0f;

        float x = Mathf.Cos(_currentAngle * Mathf.Deg2Rad);
        float y = -Mathf.Sin(_currentAngle * Mathf.Deg2Rad);

        _gravityPoint.localPosition = new Vector3(x, y, 0.0f);
    }
}
