using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSetController : MonoBehaviour
{
    [SerializeField]
    private ResourceObjectPlayerBase[] _resources;
    public ResourceObjectPlayerBase[] resources { get { return _resources; } }

    private List<ResourcePatternBase> _patterns = new List<ResourcePatternBase>();

    void Start()
    {
        ResourcePatternBase temp = null;

        temp = new SimpleScaleUp();
        _patterns.Add(temp);

        temp = new SimpleRotation();
        _patterns.Add(temp);

        temp = new BounceBall();
        _patterns.Add(temp);

        temp = new CrossTrain();
        _patterns.Add(temp);

        temp = new DoodleEffect();
        _patterns.Add(temp);

        SuffleList();
    }

    void SuffleList()
    {
        for (int i = 0; i < 777; ++i)
        {
            int sour = Random.Range(0, _patterns.Count);
            int dest = Random.Range(0, _patterns.Count);

            if (sour == dest) continue;

            ResourcePatternBase temp = _patterns[sour];
            _patterns[sour] = _patterns[dest];
            _patterns[dest] = temp;
        }
    }

    public ResourcePatternBase GetPattern()
    {
        ResourcePatternBase temp = null;

        SuffleList();
        for (int i = 0; i < _patterns.Count; ++i)
        {
            if (_patterns[i].isAlreadyUsing) continue;

            temp = _patterns[i];
            break;
        }

        return temp;
    }

    public void Retry()
    {
        foreach (var re in _resources)
            re.Retry();
    }
}
