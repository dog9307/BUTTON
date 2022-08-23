using UnityEngine;

[System.Serializable]
public class KeySet
{
    [SerializeField]
    private KeyCode _key;
    public KeyCode key { get { return _key; } set { _key = value; } }

    [SerializeField]
    private bool enabled;

    [SerializeField]
    private KeyCode[] _overrides;

    public KeySet(KeyCode key)
    {
        _key = key;
        Enable();
    }

    public bool IsOnceKeyDown()
    {
        if (!enabled) return false;
        if (Input.GetKeyDown(_key)) return true;

        bool isOverride = false;
        if (_overrides != null)
        {
            foreach (var key in _overrides)
            {
                if (Input.GetKeyDown(key))
                {
                    isOverride = true;
                    break;
                }
            }
        }

        return isOverride;
    }

    public bool IsOnceKeyUp()
    {
        if (!enabled) return false;
        if (Input.GetKeyUp(_key)) return true;

        bool isOverride = false;
        if (_overrides != null)
        {
            foreach (var key in _overrides)
            {
                if (Input.GetKeyUp(key))
                {
                    isOverride = true;
                    break;
                }
            }
        }

        return isOverride;
    }

    public bool IsStayKeyDown()
    {
        if (!enabled) return false;
        if (Input.GetKey(_key)) return true;

        bool isOverride = false;
        if (_overrides != null)
        {
            foreach (var key in _overrides)
            {
                if (Input.GetKey(key))
                {
                    isOverride = true;
                    break;
                }
            }
        }

        return isOverride;
    }

    public void Enable() { enabled = true; }
    public void Disable() { enabled = false; }
}

[System.Serializable]
public class KeyDictionary : SerializableDictionary<string, KeySet> { };
