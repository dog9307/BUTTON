using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    #region SINGLETON
    static private KeyManager _instance;
    static public KeyManager instance { get { return _instance; } }
    
    private void Awake()
    {
        if (!_instance)
        {
            _instance = FindObjectOfType<KeyManager>();
            if (!_instance)
            {
                GameObject container = new GameObject();
                container.name = "KeyManager";
                _instance = container.AddComponent<KeyManager>();
            }
        }

        DontDestroyOnLoad(KeyManager.instance);
        _isEnable = true;
    }
    #endregion

    void Start()
    {
        if (instance)
        {
            if (instance != this)
                Destroy(gameObject);
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

//#if UNITY_EDITOR
//    private void OnGUI()
//    {
//        if (Event.current != null)
//            print(Event.current.keyCode.ToString());
//    }
//#endif

    [SerializeField]
    private KeyDictionary _keys = new KeyDictionary();

    public void KeyChange(string name, KeyCode key)
    {
        KeySet find = _keys[name];
        if (find != null)
            find.key = key;
    }

    [SerializeField]
    private bool _isEnable;
    public bool isEnable { get { return _isEnable; } set { _isEnable = value; } }

    public bool IsOnceKeyDown(string name)
    {
        if (!_keys.ContainsKey(name)) return false;

        return _keys[name].IsOnceKeyDown() && isEnable;
    }

    public bool IsOnceKeyUp(string name)
    {
        if (!_keys.ContainsKey(name)) return false;

        return _keys[name].IsOnceKeyUp() && isEnable;
    }

    public bool IsStayKeyDown(string name)
    {
        if (!_keys.ContainsKey(name)) return false;

        return _keys[name].IsStayKeyDown() && isEnable;
    }

    public void Enable(string name) { if (_keys.ContainsKey(name)) _keys[name].Enable(); }
    public void Disable(string name) { if (_keys.ContainsKey(name)) _keys[name].Disable(); }
}
