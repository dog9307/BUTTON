using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossTrain : ResourcePatternBase
{
    private TrainCreator _creator;
    private List<GameObject> _trains = new List<GameObject>();
    
    private float _currentTime;
    
    public override bool Init(ResourceObjectPlayer current)
    {
        if (!base.Init(current)) return false;

        currentPlayer.isCanPressButton = false;

        if (!_creator)
            _creator = GameObject.FindObjectOfType<TrainCreator>();
        _creator.Init(current.trainScale);

        _currentTime = 0.0f;

        renderers[4].enabled = false;

        return true;
    }

    public override void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= currentPlayer.trainsCreateTime)
        {
            _creator.CreateObject(_trains);
            _currentTime = 0.0f;
        }
        
        for (int i = 0; i < _trains.Count; i++)
        {
            if (!_trains[i])
            {
                _trains.RemoveAt(i);
                i--;
                continue;
            }

            GameObject train = _trains[i];
            SpriteRenderer sprite = train.GetComponent<SpriteRenderer>();
            sprite.sprite = renderers[4].sprite;
            sprite.color = renderers[4].color;
        }
    }
    
    public override void Release()
    {
        while (_trains.Count != 0)
        {
            if (_trains[0])
                GameObject.Destroy(_trains[0]);
            _trains.RemoveAt(0);
        }
        _trains.Clear();

        currentPlayer.isCanPressButton = true;

        base.Release();
    }
}
