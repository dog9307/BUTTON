using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartButtonObject : MonoBehaviour
{
    private Animator _anim;

    [SerializeField]
    private string _key;

    public bool isPressed { get; set; }
    [SerializeField]
    private StartButtonController _prevButton;

    public UnityEvent OnButtonPressed;

    void Start()
    {
        _anim = GetComponent<Animator>();

        isPressed = false;
    }

    void Update()
    {
        if (_prevButton)
        {
            if (!_prevButton.isPressed)
                return;
        }

        if (KeyManager.instance.IsOnceKeyDown(_key))
            ButtonPress();
    }

    public void ButtonPress()
    {
        _anim.SetTrigger("press");
        isPressed = true;

        if (OnButtonPressed != null)
            OnButtonPressed.Invoke();
    }

    public void PlayNext()
    {
        ButtonSFXManager.instance.PlayNext();
    }
}
