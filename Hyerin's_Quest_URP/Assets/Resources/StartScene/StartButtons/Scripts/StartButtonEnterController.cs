using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class StartButtonEnterController : MonoBehaviour
{
    [SerializeField]
    private float _sfxDelayTime = 0.15f;

    [SerializeField]
    private string _key = "press";

    public bool isPressed { get; set; }

    [SerializeField]
    private int[] _playList;

    [Header("End Event")]
    public UnityEvent OnEffectEnd;

    void Update()
    {
        if (isPressed) return;

        if (KeyManager.instance.IsOnceKeyDown(_key))
            StartCoroutine(Effect());
    }

    IEnumerator Effect()
    {
        isPressed = true;
        GetComponent<Animator>().SetTrigger("press");

        ButtonSFXManager.instance.ResetIndex();
        for (int i = 0; i < _playList.Length; ++i)
        {
            ButtonSFXManager.instance.PlayIndex(_playList[i]);
            yield return new WaitForSeconds(_sfxDelayTime);
        }

        if (OnEffectEnd != null)
            OnEffectEnd.Invoke();
    }

    public void GoToButtonScene()
    {
        ScreenFader.instance.StartSceneFader("ButtonScene");
    }
}
