using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBall : ResourcePatternBase
{
    private Vector3 _originScale;

    private float _scaleFactor = 0.5f;
    private Rigidbody _rigid;
    private Collider _col;
    
    public override bool Init(ResourceObjectPlayer current)
    {
        if (!base.Init(current)) return false;

        _originScale = renderers[4].transform.localScale;

        renderers[4].transform.localScale = _originScale * _scaleFactor;

        _col = renderers[4].GetComponent<Collider>();
        _col.enabled = true;

        _rigid = renderers[4].GetComponent<Rigidbody>();
        _rigid.isKinematic = false;
        
        float velocityX = Random.Range(3.0f, 6.0f);
        if (renderers[4].transform.position.x > 0.0f)
            velocityX *= -1.0f;

        _rigid.velocity = new Vector3(velocityX, 0.0f, 0.0f);

        return true;
    }
    
    public override void Update()
    {

    }

    public override void Release()
    {
        SpriteRenderer origin = renderers[4];
        origin.transform.rotation = Quaternion.identity;
        origin.transform.localScale = _originScale;
        origin.transform.position = _originPos;

        _rigid.isKinematic = true;

        _col.enabled = false;

        base.Release();
    }
}
