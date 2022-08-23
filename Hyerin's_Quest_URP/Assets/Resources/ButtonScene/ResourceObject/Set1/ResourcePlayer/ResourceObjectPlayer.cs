using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceObjectPlayer : ResourceObjectPlayerBase
{
    [SerializeField]
    private SpriteRenderer[] _renderers;
    public SpriteRenderer[] renderers { get { return _renderers; } }
    private SpriteRenderer _sprite;

    public ResourceSetController manager { get; set; }
    private ResourcePatternBase _currentPattern;
    public ResourcePatternBase currentPattern { get { return _currentPattern; } }
    
    public float _opacity = 1.0f;

    [SerializeField]
    private float[] _sequenceScale;

    [SerializeField]
    private Transform _secondaryPos;
    private Vector3 _startPos;

    [SerializeField]
    private float _trainScale = 0.5f;
    public float trainScale { get { return _trainScale; } set { _trainScale = value; } }
    [SerializeField]
    private float _trainsCreateTime = 0.3f;
    public float trainsCreateTime { get { return _trainsCreateTime; } }

    [SerializeField]
    private float _zSpeed = 5.0f;
    [SerializeField]
    private float _zMax = 15.0f;
    [SerializeField]
    private float _zMin = -5.0f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _sprite = GetComponent<SpriteRenderer>();

        _startPos = transform.position;

        manager = FindObjectOfType<ResourceSetController>();

        isCanPressButton = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (_currentPattern != null)
            _currentPattern.Update();

        RendererSync();

        Vector3 pos = transform.position;
        pos.z -= _zSpeed * Time.deltaTime;
        if (pos.z < _zMin)
            pos.z = _zMax;
        transform.position = pos;
    }

    protected override void Play()
    {
        base.Play();

        //int index = (_inputCount - 1) % _sequenceScale.Length;

        //Vector3 scale = new Vector3(_sequenceScale[index], _sequenceScale[index], 1.0f);
        //transform.localScale = scale;

        //if (index == 0)
        //{
        //    for (int i = 0; i < _renderers.Length; ++i)
        //    {
        //        if (i == 4)
        //            _renderers[i].enabled = true;
        //        else
        //            _renderers[i].enabled = false;
        //    }

        //    transform.position = _startPos;
        //}
        //else if (index == 1)
        //{
        //    for (int i = 0; i < _renderers.Length; ++i)
        //    {
        //        if (i == 4 || i == 3 || i == 5)
        //            _renderers[i].enabled = true;
        //        else
        //            _renderers[i].enabled = false;
        //    }

        //    if (_secondaryPos)
        //        transform.position = _secondaryPos.position;
        //}
        //else if (index == 2)
        //{
        //    for (int i = 0; i < _renderers.Length; ++i)
        //        _renderers[i].enabled = true;

        //    transform.position = _startPos;
        //}

        float x = Random.Range(0.0f, Screen.width);
        float y = Random.Range(0.0f, Screen.height);
        float z = Random.Range(_zMin, _zMax);

        Vector3 startPos = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0.0f));
        startPos.z = z;

        if (_inputCount == 1)
        {
            for (int i = 0; i < _renderers.Length; ++i)
            {
                if (i == 4)
                    _renderers[i].enabled = true;
                else
                    _renderers[i].enabled = false;
            }

            transform.position = startPos;
        }
        else
        {
            if (_currentPattern != null)
            {
                _currentPattern.Release();
                _currentPattern = null;
            }

            if (_currentPattern == null)
            {
                _currentPattern = manager.GetPattern();
                if (_currentPattern == null)
                {
                    for (int i = 0; i < _renderers.Length; ++i)
                    {
                        if (i == 4)
                            _renderers[i].enabled = true;
                        else
                            _renderers[i].enabled = false;
                    }
                    transform.position = startPos;
                }
                else
                    _currentPattern.Init(this);
            }
        }
    }

    protected override void SetRatio(float alpha)
    {
        Color color = _sprite.color;
        color.a = alpha * _opacity;
        if (_currentPattern != null)
            color.a *= _currentPattern.opacity;

        _sprite.color = color;

        base.SetRatio(alpha);

        if (alpha <= 0.0f)
        {
            if (_currentPattern != null)
            {
                _currentPattern.Release();
                _currentPattern = null;
            }
        }
    }

    void RendererSync()
    {
        if (!_sprite) return;

        foreach (var ren in _renderers)
        {
            if (!ren) continue;

            ren.sprite = _sprite.sprite;
            ren.color = _sprite.color;
        }
    }
}
