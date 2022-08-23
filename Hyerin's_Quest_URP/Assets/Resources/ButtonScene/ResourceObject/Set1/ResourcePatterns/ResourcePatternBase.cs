using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResourcePatternBase
{
    public ResourceObjectPlayer currentPlayer { get; set; }
    public SpriteRenderer[] renderers { get { if (currentPlayer) return currentPlayer.renderers; return null; } }

    public bool isAlreadyUsing { get; set; }
    public float opacity { get; set; } = 1.0f;

    protected Vector3 _originPos;

    public virtual bool Init(ResourceObjectPlayer current)
    {
        if (isAlreadyUsing) return false;

        currentPlayer = current;
        isAlreadyUsing = true;
        
        _originPos = renderers[4].transform.position;

        Vector3 startPos;
        startPos.x = Random.Range(-7.0f, 7.0f);
        startPos.y = Random.Range(-4.0f, 4.0f);
        startPos.z = 0.0f;

        renderers[4].transform.position = startPos;
        renderers[4].enabled = true;

        return true;
    }

    public abstract void Update();

    public virtual void Release()
    {
        SpriteRenderer origin = renderers[4];
        origin.transform.position = _originPos;

        currentPlayer = null;
        isAlreadyUsing = false;
    }
}
