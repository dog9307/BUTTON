using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodleEffect : ResourcePatternBase
{
    private Material _originMat;

    private Material _doodle;
    private string _doodlePath = "ButtonScene/ResourceObject/Set1/Doodle";

    public override bool Init(ResourceObjectPlayer current)
    {
        if (!base.Init(current)) return false;

        if (_doodle == null)
            _doodle = Resources.Load<Material>(_doodlePath);

        _originMat = renderers[4].material;
        renderers[4].material = _doodle;

        return true;
    }

    public override void Update()
    {
    }

    public override void Release()
    {
        renderers[4].material = _originMat;

        base.Release();
    }
}
