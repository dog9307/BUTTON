using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum SHAPE
{
    NONE = -1,
    Clover,
    Heart,
    Dia,
    Spade,
    END
}

public class ShapeCreator : ResourceObjectPlayerBase
{
    [Header("Button Short Touch")]
    [SerializeField]
    [Range(0.001f, 0.5f)] private float _shortButtonTime = 0.1f;
    private float _buttonPressTime;

    [Header("Create Settings")]
    [SerializeField]
    private GameObject _prefab;
    
    [SerializeField]
    private Vector3 _center;
    [SerializeField]
    private float _width;
    [SerializeField]
    private float _height;

    [SerializeField]
    private float _createDelay = 0.2f;
    private float _createCheckTime;

    private List<GameObject> _objList = new List<GameObject>();

    public UnityEvent OnShort;

    [SerializeField]
    private bool _isSpade = false;

    [SerializeField]
    private SHAPE _shape;

    private Theme4RewardHelper _theme4;

    public string key { get { return _key; } set { _key = value; } }

    protected override void Update()
    {
        base.Update();

        if (!isPlaying) return;

        _buttonPressTime += Time.deltaTime;
        _createCheckTime += Time.deltaTime;
        if (_createCheckTime >= _createDelay)
        {
            CreateObject();

            _createCheckTime = 0.0f;
        }
    }

    public override void Retry()
    {
        foreach (var obj in _objList)
            Destroy(obj);

        _objList.Clear();
    }

    protected override void Play()
    {
        base.Play();

        _createCheckTime = 0.0f;
        _buttonPressTime = 0.0f;

        CreateObject();

        if (!_theme4)
            _theme4 = FindObjectOfType<Theme4RewardHelper>();

        if (_theme4)
            _theme4.CardOn((int)_shape);
    }

    protected override void Stop()
    {
        base.Stop();

        _key = "";
        
        if (_buttonPressTime < _shortButtonTime)
            DoingShort();
    }

    void DoingShort()
    {
        if (OnShort != null)
            OnShort.Invoke();
    }

    public void CreateObject()
    {
        if (!_prefab) return;

        GameObject newObj = Instantiate(_prefab);
        _objList.Add(newObj);

        float min = 0.0f;
        float max = 0.0f;
        float origin = 0.0f;

        // x
        float x = 0.0f;
        origin = _center.x;
        min = origin - _width / 2.0f;
        max = origin + _width / 2.0f;
        x = Random.Range(min, max);

        // y
        float y = 0.0f;
        origin = _center.y;
        min = origin - _height / 2.0f;
        max = origin + _height / 2.0f;
        y = Random.Range(min, max);

        float z = transform.position.z;

        newObj.transform.position = new Vector3(x, y, z);

        if (_isSpade)
        {
            for (int i = 0; i < _objList.Count;)
            {
                GameObject obj = _objList[i];
                if (obj == newObj)
                {
                    i++;
                    continue;
                }

                SpadeDissolve dis = obj.GetComponent<SpadeDissolve>();
                if (dis)
                {
                    dis.DissolveOut();
                    _objList.Remove(obj);
                }
            }
        }

        newObj.transform.parent = transform;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(_center, new Vector3(_width, _height, 0.1f));
    }
}
