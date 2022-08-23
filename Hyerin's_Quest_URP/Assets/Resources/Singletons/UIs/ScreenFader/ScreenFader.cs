using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum FaderEventType
{
    NONE = -1,
    Pre,
    Middle,
    Post,
    END
}

public class ScreenFader : MonoBehaviour
{
    #region SINGLETON
    static private ScreenFader _instance;
    static public ScreenFader instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<ScreenFader>();
                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "ScreenFader";
                    _instance = container.AddComponent<ScreenFader>();
                }

                DontDestroyOnLoad(_instance);
            }

            return _instance;
        }
    }

    void Awake()
    {
        if (ScreenFader.instance)
        {
            ScreenFader[] managers = FindObjectsOfType<ScreenFader>();
            if (managers.Length >= 2)
            {
                for (int i = 1; i < managers.Length; ++i)
                    Destroy(managers[i].gameObject);
            }
        }
    }
    #endregion

    [SerializeField]
    private Image _image;
    private Color _startColor;

    [SerializeField]
    private AnimationCurve _curve;

    [SerializeField]
    private float _fadeTime = 2.0f;

    public delegate void FaderEvent();

    private Queue<FaderEvent> _preFader = new Queue<FaderEvent>();
    private Queue<FaderEvent> _middleFader = new Queue<FaderEvent>();
    private Queue<FaderEvent> _postFader = new Queue<FaderEvent>();

    void Start()
    {
        _startColor = _image.color;

        StartCoroutine(FadeIn(null));
    }

    public void StartSceneFader(string sceneName)
    {
        InvokeEvent(_preFader);
        StartCoroutine(FadeOut(sceneName));
    }

    void InvokeEvent(Queue<FaderEvent> current)
    {
        while (current.Count != 0)
        {
            FaderEvent evt = current.Dequeue();
            evt();
        }
    }

    IEnumerator FadeOut(string sceneName)
    {
        float ratio = 0.0f;
        while (ratio < 1.0f)
        {
            ratio += Time.deltaTime / _fadeTime;

            float alpha = _curve.Evaluate(ratio);

            Color newColor = _startColor;
            newColor.a = alpha;

            _image.color = newColor;

            yield return null;
        }

        InvokeEvent(_middleFader);
        StartCoroutine(FadeIn(sceneName));
    }

    IEnumerator FadeIn(string sceneName)
    {
        if (sceneName != null)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            while (!op.isDone)
                yield return null;
        }

        float time = 1.0f;
        while (time > 0.0f)
        {
            time -= Time.deltaTime / _fadeTime;

            float alpha = _curve.Evaluate(time);

            Color newColor = _startColor;
            newColor.a = alpha;

            _image.color = newColor;

            yield return null;
        }

        InvokeEvent(_postFader);
    }

    public void AddEvent(FaderEvent evt, FaderEventType type = FaderEventType.Middle)
    {
        if (evt == null) return;

        switch (type)
        {
            case FaderEventType.Pre:
                _preFader.Enqueue(evt);
            break;

            case FaderEventType.Middle:
                _middleFader.Enqueue(evt);
            break;

            case FaderEventType.Post:
                _postFader.Enqueue(evt);
            break;
        }
    }
}