using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theme2RandomShape : MonoBehaviour
{
    [System.Serializable]
    private struct CreatorNode
    {
        public ShapeCreator creator;
        public bool isSelected;
    }

    [SerializeField]
    private ShapeCreator[] _creators;
    private List<CreatorNode> _nodes = new List<CreatorNode>();

    // Start is called before the first frame update
    void Start()
    {
        _nodes.Clear();
        foreach (var creator in _creators)
        {
            CreatorNode node = new CreatorNode();
            node.creator = creator;
            node.isSelected = false;

            _nodes.Add(node);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (KeyManager.instance.IsOnceKeyDown("button_1"))
            KeyBinding("button_1");
        if (KeyManager.instance.IsOnceKeyDown("button_2"))
            KeyBinding("button_2");
        if (KeyManager.instance.IsOnceKeyDown("button_3"))
            KeyBinding("button_3");
        if (KeyManager.instance.IsOnceKeyDown("button_4"))
            KeyBinding("button_4");

        if (KeyManager.instance.IsOnceKeyUp("button_1"))
            KeyUnbinding("button_1");
        if (KeyManager.instance.IsOnceKeyUp("button_2"))
            KeyUnbinding("button_2");
        if (KeyManager.instance.IsOnceKeyUp("button_3"))
            KeyUnbinding("button_3");
        if (KeyManager.instance.IsOnceKeyUp("button_4"))
            KeyUnbinding("button_4");
    }

    void KeyBinding(string key)
    {
        ShuffleList();
        int index = 0;
        for (int i = 0; i < _nodes.Count; ++i)
        {
            if (!_nodes[i].isSelected)
            {
                index = i;
                break;
            }
        }

        CreatorNode node = _nodes[index];
        node.creator.key = key;
        node.isSelected = true;

        _nodes[index] = node;
    }

    void KeyUnbinding(string key)
    {
        for (int i = 0; i < _nodes.Count; ++i)
        {
            CreatorNode node = _nodes[i];
            if (node.creator.key == key)
            {
                node.isSelected = false;
                _nodes[i] = node;

                break;
            }
        }
    }

    void ShuffleList()
    {
        for (int i = 0; i < 777; ++i)
        {
            int sour = Random.Range(0, _nodes.Count);
            int dest = Random.Range(0, _nodes.Count);

            CreatorNode temp = _nodes[sour];
            _nodes[sour] = _nodes[dest];
            _nodes[dest] = temp;
        }
    }
}
